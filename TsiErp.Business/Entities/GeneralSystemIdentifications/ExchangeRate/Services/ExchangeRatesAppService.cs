using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ExchangeRate.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ExchangeRatesAppService(IStringLocalizer<ExchangeRatesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> CreateAsync(CreateExchangeRatesDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();


            var query = queryFactory.Query().From(Tables.ExchangeRates).Insert(new CreateExchangeRatesDto
            {
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
                BuyingRate = input.BuyingRate,
                CurrencyID = input.CurrencyID,
                Date = input.Date,
                EffectiveBuyingRate = input.EffectiveBuyingRate,
                EffectiveSaleRate = input.EffectiveSaleRate,
                SaleRate = input.SaleRate,
            });

            var exchangeRates = queryFactory.Insert<SelectExchangeRatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ExchangeRates).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);

        }


        public async Task<IDataResult<SelectExchangeRatesDto>> GetAsync(Guid id)
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
                        .Where(new { Id = id }, Tables.ExchangeRates);

            var exchangeRate = queryFactory.Get<SelectExchangeRatesDto>(query);

            LogsAppService.InsertLogToDatabase(exchangeRate, exchangeRate, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRate);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListExchangeRatesDto>>> GetListAsync(ListExchangeRatesParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ExchangeRates)
               .Select<ExchangeRates>(e => new { e.CurrencyID, e.BuyingRate, e.SaleRate, e.EffectiveBuyingRate, e.EffectiveSaleRate, e.DataOpenStatus, e.DataOpenStatusUserId, e.Date, e.Id })
                   .Join<Currencies>
                   (
                      c => new { CurrencyCode = c.Code, CurrencyId=c.Id },
                       nameof(ExchangeRates.CurrencyID),
                            nameof(Currencies.Id),
                       JoinType.Left
                   ).Where(null, Tables.ExchangeRates);

            var exchangeRates = queryFactory.GetList<ListExchangeRatesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListExchangeRatesDto>>(exchangeRates);
        }


        [ValidationAspect(typeof(UpdateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateAsync(UpdateExchangeRatesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ExchangeRates).Select("*").Where(new { Id = input.Id }, "");
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
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, exchangeRates, LoginedUserService.UserId, Tables.ExchangeRates, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);

        }

        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ExchangeRates).Select("*").Where(new { Id = id }, "");
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var exchangeRates = queryFactory.Update<SelectExchangeRatesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectExchangeRatesDto>(exchangeRates);

        }
    }
}
