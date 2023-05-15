using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.ShippingAdresses.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ShippingAdress.BusinessRules;
using TsiErp.Business.Entities.ShippingAdress.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TSI.QueryBuilder.Constants.Join;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    [ServiceRegistration(typeof(IShippingAdressesAppService), DependencyInjectionType.Scoped)]
    public class ShippingAdressesAppService : ApplicationService<ShippingAdressesResource>, IShippingAdressesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ShippingAdressesAppService(IStringLocalizer<ShippingAdressesResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> CreateAsync(CreateShippingAdressesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<ShippingAdresses>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

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
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var shippingAdresses = queryFactory.Insert<SelectShippingAdressesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Insert, shippingAdresses.Id);

                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ShippingAdresses).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Delete, id);

                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);
            }
        }


        public async Task<IDataResult<SelectShippingAdressesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ShippingAdresses).Select<ShippingAdresses>(sh => new { sh.Adress1, sh.Adress2,sh.City,sh.Name,sh.Code,sh.Country,sh.CustomerCardID,sh.DataOpenStatus,sh._Default,sh.Fax,sh.DataOpenStatusUserId,sh.District,sh.Phone,sh.PostCode,sh.Id })
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CustomerCardID = ca.Id, CustomerCardCode = ca.Code, CustomerCardName= ca.Name },
                                nameof(ShippingAdresses.CustomerCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.ShippingAdresses);

                var shippingAdress = queryFactory.Get<SelectShippingAdressesDto>(query);

                LogsAppService.InsertLogToDatabase(shippingAdress, shippingAdress, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Get, id);

                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdress);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShippingAdressesDto>>> GetListAsync(ListShippingAdressesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.ShippingAdresses).Select<ShippingAdresses>(sh => new { sh.Adress1, sh.Adress2, sh.City, sh.Name, sh.Code, sh.Country, sh.CustomerCardID, sh.DataOpenStatus, sh._Default, sh.Fax, sh.DataOpenStatusUserId, sh.District, sh.Phone, sh.PostCode, sh.Id })
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CustomerCardCode = ca.Code, CustomerCardName = ca.Name },
                                nameof(ShippingAdresses.CustomerCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            ).Where(null, false, false, Tables.ShippingAdresses);

                var shippingAdresses = queryFactory.GetList<ListShippingAdressesDto>(query).ToList();

                return new SuccessDataResult<IList<ListShippingAdressesDto>>(shippingAdresses);
            }

        }


        [ValidationAspect(typeof(UpdateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateAsync(UpdateShippingAdressesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ShippingAdresses>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ShippingAdresses).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<ShippingAdresses>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
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
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var shippingAdresses = queryFactory.Update<SelectShippingAdressesDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, shippingAdresses, LoginedUserService.UserId, Tables.ShippingAdresses, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);
            }

        }

        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

                return new SuccessDataResult<SelectShippingAdressesDto>(shippingAdresses);

            }

        }
    }
}
