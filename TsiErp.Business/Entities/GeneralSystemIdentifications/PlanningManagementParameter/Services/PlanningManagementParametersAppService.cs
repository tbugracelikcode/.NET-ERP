using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PlanningManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Services
{
    [ServiceRegistration(typeof(IPlanningManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class PlanningManagementParametersAppService : ApplicationService<PlanningManagementParametersResource>, IPlanningManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public PlanningManagementParametersAppService(IStringLocalizer<PlanningManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectPlanningManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var PlanningManagementParameter = queryFactory.Get<SelectPlanningManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(PlanningManagementParameter, PlanningManagementParameter, LoginedUserService.UserId, Tables.PlanningManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectPlanningManagementParametersDto>(PlanningManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPlanningManagementParametersDto>>> GetListAsync(ListPlanningManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").Where(null, false, false, "");

                var PlanningManagementParameters = queryFactory.GetList<ListPlanningManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListPlanningManagementParametersDto>>(PlanningManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlanningManagementParametersDto>> UpdateAsync(UpdatePlanningManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<PlanningManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Update(new UpdatePlanningManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var PlanningManagementParameters = queryFactory.Update<SelectPlanningManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, PlanningManagementParameters, LoginedUserService.UserId, Tables.PlanningManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectPlanningManagementParametersDto>(PlanningManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectPlanningManagementParametersDto>> CreateAsync(CreatePlanningManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPlanningManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
