using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services
{
    [ServiceRegistration(typeof(IStockManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class StockManagementParametersAppService : ApplicationService<StockManagementParametersResource>, IStockManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public StockManagementParametersAppService(IStringLocalizer<StockManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> CreateAsync(CreateStockManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.StockManagementParameters).Insert(new CreateStockManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                FutureDateParameter = input.FutureDateParameter
            }).UseIsDelete(false); ;


            var stockManagementParameter = queryFactory.Insert<SelectStockManagementParametersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Insert, stockManagementParameter.Id);

            return new SuccessDataResult<SelectStockManagementParametersDto>(stockManagementParameter);
        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> GetStockManagementParametersAsync()
        {
            SelectStockManagementParametersDto result = new SelectStockManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").UseIsDelete(false);

            SelectStockManagementParametersDto StockManagementParameter = queryFactory.Get<SelectStockManagementParametersDto>(controlQuery);

            if (StockManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(
                 new
                 {
                     Id = StockManagementParameter.Id
                 }, false, false, "").UseIsDelete(false);

                result = queryFactory.Get<SelectStockManagementParametersDto>(query);
            }

            LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Get, result.Id);

            return new SuccessDataResult<SelectStockManagementParametersDto>(result);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockManagementParametersDto>> UpdateAsync(UpdateStockManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);

            var entity = queryFactory.Get<StockManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockManagementParameters).Update(new UpdateStockManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                Id = input.Id
            }).Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);


            var StockManagementParameters = queryFactory.Update<SelectStockManagementParametersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StockManagementParameters, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Update, entity.Id);

            return new SuccessDataResult<SelectStockManagementParametersDto>(StockManagementParameters);
        }

        #region Unused Implemented Methods

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectStockManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockManagementParametersDto>>> GetListAsync(ListStockManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").UseIsDelete(false);

            var StockManagementParameters = queryFactory.GetList<ListStockManagementParametersDto>(query).ToList();

            return new SuccessDataResult<IList<ListStockManagementParametersDto>>(StockManagementParameters);
        }

        #endregion
    }
}
