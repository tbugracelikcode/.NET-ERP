using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ShippingAdress.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ShippingAdresses.Page;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    [ServiceRegistration(typeof(IShippingAdressesAppService), DependencyInjectionType.Scoped)]
    public class ShippingAdressesAppService : ApplicationService<ShippingAdressesResource>, IShippingAdressesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ShippingAdressesAppService(IStringLocalizer<ShippingAdressesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> CreateAsync(CreateShippingAdressesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<ShippingAdresses>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Insert(new CreateShippingAdressesDto
            {
                Adress1 = input.Adress1,
                Adress2 = input.Adress2,
                City = input.City,
                Country = input.Country,
                CustomerCardID = input.CustomerCardID,
                District = input.District,
                EMail = input.EMail,
                Fax = input.Fax,
                Phone = input.Phone,
                PostCode = input.PostCode,
                _Default = input._Default,
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name
            });

            var shippingAdresses = queryFactory.Insert<SelectShippingAdressesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ShippingAdressesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ShippingAdressID", new List<string>
            {
                Tables.PurchaseOrders,
                Tables.SalesOrders,
                Tables.SalesPropositions
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.ShippingAdresses).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);
            }
        }


        public async Task<IDataResult<SelectShippingAdressesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ShippingAdresses).Select<ShippingAdresses>(sh => new { sh._Default, sh.PostCode, sh.Phone, sh.Name, sh.Id, sh.Fax, sh.EMail, sh.District, sh.DataOpenStatusUserId, sh.DataOpenStatus, sh.CustomerCardID, sh.Country, sh.Code, sh.City, sh.Adress2, sh.Adress1 })
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CustomerCardID = ca.Id, CustomerCardCode = ca.Code, CustomerCardName = ca.Name },
                            nameof(ShippingAdresses.CustomerCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ShippingAdresses);

            var shippingAdress = queryFactory.Get<SelectShippingAdressesDto>(query);

            LogsAppService.InsertLogToDatabase(shippingAdress, shippingAdress, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdress);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShippingAdressesDto>>> GetListAsync(ListShippingAdressesParameterDto input)
        {

            var query = queryFactory
               .Query()
               .From(Tables.ShippingAdresses).Select<ShippingAdresses>(sh => new { sh._Default, sh.PostCode, sh.Phone, sh.Name, sh.Id, sh.Fax, sh.EMail, sh.District, sh.DataOpenStatusUserId, sh.DataOpenStatus, sh.CustomerCardID, sh.Country, sh.Code, sh.City, sh.Adress2, sh.Adress1 })
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CustomerCardCode = ca.Code, CustomerCardName = ca.Name },
                            nameof(ShippingAdresses.CustomerCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.ShippingAdresses);

            var shippingAdresses = queryFactory.GetList<ListShippingAdressesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShippingAdressesDto>>(shippingAdresses);


        }


        [ValidationAspect(typeof(UpdateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateAsync(UpdateShippingAdressesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<ShippingAdresses>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<ShippingAdresses>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Update(new UpdateShippingAdressesDto
            {
                Adress1 = input.Adress1,
                Adress2 = input.Adress2,
                City = input.City,
                Country = input.Country,
                CustomerCardID = input.CustomerCardID,
                District = input.District,
                EMail = input.EMail,
                Fax = input.Fax,
                Phone = input.Phone,
                PostCode = input.PostCode,
                _Default = input._Default,
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, shippingAdresses, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }

        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<ShippingAdresses>(entityQuery);

            var query = queryFactory.Query().From(Tables.ShippingAdresses).Update(new UpdateShippingAdressesDto
            {
                Adress1 = entity.Adress1,
                Adress2 = entity.Adress2,
                City = entity.City,
                Country = entity.Country,
                CustomerCardID = entity.CustomerCardID,
                District = entity.District,
                EMail = entity.EMail,
                Fax = entity.Fax,
                Phone = entity.Phone,
                PostCode = entity.PostCode,
                _Default = entity._Default,
                Code = entity.Code,
                Name = entity.Name,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,

            }).Where(new { Id = id }, false, false, "");

            var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

        }
    }
}
