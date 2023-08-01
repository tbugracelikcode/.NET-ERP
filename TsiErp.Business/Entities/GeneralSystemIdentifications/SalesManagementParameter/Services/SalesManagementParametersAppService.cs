using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services
{
    [ServiceRegistration(typeof(ISalesManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class SalesManagementParametersAppService : ApplicationService<SalesManagementParametersResource>, ISalesManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public SalesManagementParametersAppService(IStringLocalizer<SalesManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var SalesManagementParameter = queryFactory.Get<SelectSalesManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(SalesManagementParameter, SalesManagementParameter, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectSalesManagementParametersDto>(SalesManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesManagementParametersDto>>> GetListAsync(ListSalesManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(null, false, false, "");

                var SalesManagementParameters = queryFactory.GetList<ListSalesManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListSalesManagementParametersDto>>(SalesManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesManagementParametersDto>> UpdateAsync(UpdateSalesManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<SalesManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.SalesManagementParameters).Update(new UpdateSalesManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var SalesManagementParameters = queryFactory.Update<SelectSalesManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, SalesManagementParameters, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectSalesManagementParametersDto>(SalesManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectSalesManagementParametersDto>> CreateAsync(CreateSalesManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectSalesManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
