using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ExchangeRate.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ExchangeRates.Page;

namespace TsiErp.Business.Entities.ExchangeRate.Services
{
    [ServiceRegistration(typeof(IExchangeRatesAppService), DependencyInjectionType.Scoped)]
    public class ExchangeRatesAppService : ApplicationService<ExchangeRatesResource>, IExchangeRatesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ExchangeRatesAppService(IStringLocalizer<ExchangeRatesResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> CreateAsync(CreateExchangeRatesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                Guid addedEntityId = GuidGenerator.CreateGuid();


                var query = queryFactory.Query().From(Tables.ExchangeRates).Insert(new CreateExchangeRatesDto
                {
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    BuyingRate = input.BuyingRate,
                    CurrencyID = input.CurrencyID,
                    Date = input.Date,
                    EffectiveBuyingRate = input.EffectiveBuyingRate,
                    EffectiveSaleRate = input.EffectiveSaleRate,
                    SaleRate = input.SaleRate,
                });

                var exchangeRates = queryFactory.Insert<SelectExchangeRatesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ExchangeRates).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Delete, id);

                return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);
            }

        }


        public async Task<IDataResult<SelectExchangeRatesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ExchangeRates).Select<ExchangeRates>(e => new { e.CurrencyID, e.BuyingRate, e.SaleRate, e.EffectiveBuyingRate, e.EffectiveSaleRate, e.DataOpenStatus, e.DataOpenStatusUserId, e.Date, e.Id })
                            .Join<Currencies>
                            (
                                c => new { CurrencyCode = c.Code, CurrencyID = c.Id },
                                nameof(ExchangeRates.CurrencyID),
                                nameof(Currencies.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.ExchangeRates);

                var exchangeRate = queryFactory.Get<SelectExchangeRatesDto>(query);

                LogsAppService.InsertLogToDatabase(exchangeRate, exchangeRate, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Get, id);

                return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRate);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListExchangeRatesDto>>> GetListAsync(ListExchangeRatesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.ExchangeRates)
                   .Select<ExchangeRates>(e => new { e.CurrencyID, e.BuyingRate, e.SaleRate, e.EffectiveBuyingRate, e.EffectiveSaleRate, e.DataOpenStatus, e.DataOpenStatusUserId, e.Date, e.Id })
                       .Join<Currencies>
                       (
                          c => new { CurrencyCode = c.Code },
                           nameof(ExchangeRates.CurrencyID),
                                nameof(Currencies.Id),
                           JoinType.Left
                       ).Where(null, false, false, Tables.ExchangeRates);

                var exchangeRates = queryFactory.GetList<ListExchangeRatesDto>(query).ToList();

                return new SuccessDataResult<IList<ListExchangeRatesDto>>(exchangeRates);
            }
        }


        [ValidationAspect(typeof(UpdateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateAsync(UpdateExchangeRatesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ExchangeRates).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ExchangeRates>(entityQuery);

                var query = queryFactory.Query().From(Tables.ExchangeRates).Update(new UpdateExchangeRatesDto
                {
                    Date = input.Date,
                    EffectiveSaleRate = input.EffectiveSaleRate,
                    EffectiveBuyingRate = input.EffectiveBuyingRate,
                    SaleRate = input.SaleRate,
                    BuyingRate = input.BuyingRate,
                    CurrencyID = input.CurrencyID,
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

                var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, exchangeRates, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);
            }
        }

        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ExchangeRates).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<ExchangeRates>(entityQuery);

                var query = queryFactory.Query().From(Tables.ExchangeRates).Update(new UpdateExchangeRatesDto
                {
                    BuyingRate = entity.BuyingRate,
                    CurrencyID = entity.CurrencyID,
                    SaleRate = entity.SaleRate,
                    EffectiveBuyingRate = entity.EffectiveBuyingRate,
                    EffectiveSaleRate = entity.EffectiveSaleRate,
                    Date = entity.Date,
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

                var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);

                return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);

            }
        }
    }
}
