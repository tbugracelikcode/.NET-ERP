using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Currencies.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Currency.BusinessRules;
using TsiErp.Business.Entities.Currency.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.Currency.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;

namespace TsiErp.Business.Entities.Currency.Services
{
    [ServiceRegistration(typeof(ICurrenciesAppService), DependencyInjectionType.Scoped)]
    public class CurrenciesAppService : ApplicationService<CurrenciesResource>, ICurrenciesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();


        public CurrenciesAppService(IStringLocalizer<CurrenciesResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> CreateAsync(CreateCurrenciesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Currencies>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();


                var query = queryFactory.Query().From(Tables.Currencies).Insert(new CreateCurrenciesDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    IsActive = true,
                    Id = addedEntityId,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var currencies = queryFactory.Insert<SelectCurrenciesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Currencies, LogType.Insert, addedEntityId);


                return new SuccessDataResult<SelectCurrenciesDto>(currencies);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                #region Delete Control

                #endregion

                var query = queryFactory.Query().From(Tables.Currencies).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Currencies, LogType.Delete, id);

                return new SuccessDataResult<SelectCurrenciesDto>(currencies);
            }
        }


        public async Task<IDataResult<SelectCurrenciesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Currencies).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var currency = queryFactory.Get<SelectCurrenciesDto>(query);


                LogsAppService.InsertLogToDatabase(currency, currency, LoginedUserService.UserId, Tables.Currencies, LogType.Get, id);

                return new SuccessDataResult<SelectCurrenciesDto>(currency);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrenciesDto>>> GetListAsync(ListCurrenciesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Currencies).Select("*").Where(null, true, true, "");
                var currencies = queryFactory.GetList<ListCurrenciesDto>(query).ToList();
                return new SuccessDataResult<IList<ListCurrenciesDto>>(currencies);
            }
        }


        [ValidationAspect(typeof(UpdateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> UpdateAsync(UpdateCurrenciesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Currencies>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Currencies>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Currencies).Update(new UpdateCurrenciesDto
                {
                    Code = input.Code,
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

                var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, currencies, LoginedUserService.UserId, Tables.Currencies, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectCurrenciesDto>(currencies);
            }
        }

        public async Task<IDataResult<SelectCurrenciesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Currencies).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<Currencies>(entityQuery);

                var query = queryFactory.Query().From(Tables.Currencies).Update(new UpdateCurrenciesDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var currencies = queryFactory.Update<SelectCurrenciesDto>(query, "Id", true);
                return new SuccessDataResult<SelectCurrenciesDto>(currencies);

            }
        }
    }
}
