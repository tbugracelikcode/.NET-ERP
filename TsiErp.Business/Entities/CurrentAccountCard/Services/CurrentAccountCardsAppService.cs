using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.CurrentAccountCards.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CurrentAccountCard.BusinessRules;
using TsiErp.Business.Entities.CurrentAccountCard.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.Entities.Entities.Currency;
using TSI.QueryBuilder.Constants.Join;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    [ServiceRegistration(typeof(ICurrentAccountCardsAppService), DependencyInjectionType.Scoped)]
    public class CurrentAccountCardsAppService : ApplicationService<CurrentAccountCardsResource>, ICurrentAccountCardsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public CurrentAccountCardsAppService(IStringLocalizer<CurrentAccountCardsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> CreateAsync(CreateCurrentAccountCardsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<CurrentAccountCards>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.CurrentAccountCards).Insert(new CreateCurrentAccountCardsDto
                {
                    Code = input.Code,
                    Address1 = input.Address1,
                    Address2 = input.Address2,
                    City = input.City,
                    CoatingCustomer = input.CoatingCustomer,
                    ContractSupplier = input.ContractSupplier,
                    Country = input.Country,
                    CurrencyID = input.CurrencyID,
                    District = input.District,
                    Email = input.Email,
                    Fax = input.Fax,
                    IDnumber = input.IDnumber,
                    PlusPercentage = input.PlusPercentage,
                    PostCode = input.PostCode,
                    PrivateCode1 = input.PrivateCode1,
                    PrivateCode2 = input.PrivateCode2,
                    PrivateCode3 = input.PrivateCode3,
                    PrivateCode4 = input.PrivateCode4,
                    PrivateCode5 = input.PrivateCode5,
                    Responsible = input.Responsible,
                    SaleContract = input.SaleContract,
                    ShippingAddress = input.ShippingAddress,
                    SoleProprietorship = input.SoleProprietorship,
                    Supplier = input.Supplier,
                    SupplierNo = input.SupplierNo,
                    TaxAdministration = input.TaxAdministration,
                    TaxNumber = input.TaxNumber,
                    Tel1 = input.Tel1,
                    Tel2 = input.Tel2,
                    Type = input.Type,
                    Web = input.Web,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var currentAccountCards = queryFactory.Insert<SelectCurrentAccountCardsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Insert, currentAccountCards.Id);

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.CurrentAccountCards).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Delete, id);

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);
            }
        }


        public async Task<IDataResult<SelectCurrentAccountCardsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.CurrentAccountCards).Select<CurrentAccountCards>(ca => new { ca.IDnumber, ca.Address2, ca.Address1, ca.City, ca.CoatingCustomer, ca.Code, ca.ContractProductionTrackings, ca.ContractSupplier, ca.Country, ca.CurrencyID, ca.DataOpenStatus, ca.DataOpenStatusUserId, ca.District, ca.Email, ca.Fax, ca.TaxNumber, ca.Tel1, ca.Tel2, ca.Type, ca.Web, ca.TaxAdministration, ca.SupplierNo, ca.Supplier, ca.SoleProprietorship, ca.ShippingAddress, ca.SaleContract, ca.Responsible, ca.PrivateCode5, ca.PrivateCode4, ca.PrivateCode3, ca.PrivateCode2, ca.PrivateCode1, ca.PostCode, ca.PlusPercentage, ca.Name })
                            .Join<Currencies>
                            (
                                c => new { Currency = c.Code, CurrencyID = c.Id },
                                nameof(CurrentAccountCards.CurrencyID),
                                nameof(Currencies.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.Periods);

                var currentAccountCard = queryFactory.Get<SelectCurrentAccountCardsDto>(query);

                LogsAppService.InsertLogToDatabase(currentAccountCard, currentAccountCard, LoginedUserService.UserId, Tables.Periods, LogType.Get, id);

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCard);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrentAccountCardsDto>>> GetListAsync(ListCurrentAccountCardsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                   .Query()
                   .From(Tables.CurrentAccountCards)
                   .Select<CurrentAccountCards>(ca => new { ca.IDnumber, ca.Address2, ca.Address1, ca.City, ca.CoatingCustomer, ca.Code, ca.ContractProductionTrackings, ca.ContractSupplier, ca.Country, ca.CurrencyID, ca.DataOpenStatus, ca.DataOpenStatusUserId, ca.District, ca.Email, ca.Fax, ca.TaxNumber, ca.Tel1, ca.Tel2, ca.Type, ca.Web, ca.TaxAdministration, ca.SupplierNo, ca.Supplier, ca.SoleProprietorship, ca.ShippingAddress, ca.SaleContract, ca.Responsible, ca.PrivateCode5, ca.PrivateCode4, ca.PrivateCode3, ca.PrivateCode2, ca.PrivateCode1, ca.PostCode, ca.PlusPercentage, ca.Name })
                       .Join<Currencies>
                       (
                           c => new { Currency = c.Code },
                           nameof(CurrentAccountCards.CurrencyID),
                           nameof(Currencies.Id),
                           JoinType.Left
                       ).Where(null, true, true, Tables.CurrentAccountCards);

                var currentAccountCards = queryFactory.GetList<ListCurrentAccountCardsDto>(query).ToList();

                return new SuccessDataResult<IList<ListCurrentAccountCardsDto>>(currentAccountCards);
            }
        }


        [ValidationAspect(typeof(UpdateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateAsync(UpdateCurrentAccountCardsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<CurrentAccountCards>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<CurrentAccountCards>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.CurrentAccountCards).Update(new UpdateCurrentAccountCardsDto
                {
                    Code = input.Code,
                    PlusPercentage = input.PlusPercentage,
                    PostCode = input.PostCode,
                    Address1 = input.Address1,
                    PrivateCode1 = input.PrivateCode1,
                    PrivateCode2 = input.PrivateCode2,
                    Address2 = input.Address2,
                    City = input.City,
                    CoatingCustomer = input.CoatingCustomer,
                    ContractSupplier = input.ContractSupplier,
                    Country = input.Country,
                    CurrencyID = input.CurrencyID,
                    District = input.District,
                    Email = input.Email,
                    Fax = input.Fax,
                    IDnumber = input.IDnumber,
                    PrivateCode3 = input.PrivateCode3,
                    PrivateCode4 = input.PrivateCode4,
                    PrivateCode5 = input.PrivateCode5,
                    Responsible = input.Responsible,
                    SaleContract = input.SaleContract,
                    ShippingAddress = input.ShippingAddress,
                    SoleProprietorship = input.SoleProprietorship,
                    Supplier = input.Supplier,
                    SupplierNo = input.SupplierNo,
                    TaxAdministration = input.TaxAdministration,
                    TaxNumber = input.TaxNumber,
                    Tel1 = input.Tel1,
                    Tel2 = input.Tel2,
                    Type = input.Type,
                    Web = input.Web,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, currentAccountCards, LoginedUserService.UserId, Tables.CurrentAccountCards, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);
            }
        }

        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.CurrentAccountCards).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<CurrentAccountCards>(entityQuery);

                var query = queryFactory.Query().From(Tables.CurrentAccountCards).Update(new UpdateCurrentAccountCardsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    PlusPercentage = entity.PlusPercentage,
                    PostCode = entity.PostCode,
                    Address1 = entity.Address1,
                    PrivateCode1 = entity.PrivateCode1,
                    PrivateCode2 = entity.PrivateCode2,
                    Address2 = entity.Address2,
                    City = entity.City,
                    CoatingCustomer = entity.CoatingCustomer,
                    ContractSupplier = entity.ContractSupplier,
                    Country = entity.Country,
                    CurrencyID = entity.CurrencyID,
                    District = entity.District,
                    Email = entity.Email,
                    Fax = entity.Fax,
                    IDnumber = entity.IDnumber,
                    PrivateCode3 = entity.PrivateCode3,
                    PrivateCode4 = entity.PrivateCode4,
                    PrivateCode5 = entity.PrivateCode5,
                    Responsible = entity.Responsible,
                    SaleContract = entity.SaleContract,
                    ShippingAddress = entity.ShippingAddress,
                    SoleProprietorship = entity.SoleProprietorship,
                    Supplier = entity.Supplier,
                    SupplierNo = entity.SupplierNo,
                    TaxAdministration = entity.TaxAdministration,
                    TaxNumber = entity.TaxNumber,
                    Tel1 = entity.Tel1,
                    Tel2 = entity.Tel2,
                    Type = entity.Type,
                    Web = entity.Web,
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

                }).Where(new { Id = id }, true, true, "");

                var currentAccountCards = queryFactory.Update<SelectCurrentAccountCardsDto>(query, "Id", true);

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(currentAccountCards);

            }
        }
    }
}
