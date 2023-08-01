using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
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
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var StockManagementParameter = queryFactory.Get<SelectStockManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(StockManagementParameter, StockManagementParameter, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectStockManagementParametersDto>(StockManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockManagementParametersDto>>> GetListAsync(ListStockManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(null, false, false, "");

                var StockManagementParameters = queryFactory.GetList<ListStockManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListStockManagementParametersDto>>(StockManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockManagementParametersDto>> UpdateAsync(UpdateStockManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<StockManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.StockManagementParameters).Update(new UpdateStockManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var StockManagementParameters = queryFactory.Update<SelectStockManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, StockManagementParameters, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectStockManagementParametersDto>(StockManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectStockManagementParametersDto>> CreateAsync(CreateStockManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectStockManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
