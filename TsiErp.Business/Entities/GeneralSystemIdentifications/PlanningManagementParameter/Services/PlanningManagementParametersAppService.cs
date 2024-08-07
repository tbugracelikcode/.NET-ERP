using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PlanningManagementParametersAppService(IStringLocalizer<PlanningManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectPlanningManagementParametersDto>> CreateAsync(CreatePlanningManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Insert(new CreatePlanningManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                FutureDateParameter = input.FutureDateParameter,
                 MRPIISourceModule = input.MRPIISourceModule,
                MRPPurchaseTransaction = input.MRPPurchaseTransaction
            }).UseIsDelete(false); 


            var PlanningManagementParameter = queryFactory.Insert<SelectPlanningManagementParametersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PlanningManagementParameters, LogType.Insert, PlanningManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPlanningManagementParametersDto>(PlanningManagementParameter);
        }

        public async Task<IDataResult<SelectPlanningManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPlanningManagementParametersDto>>> GetListAsync(ListPlanningManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").UseIsDelete(false);

            var PlanningManagementParameters = queryFactory.GetList<ListPlanningManagementParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPlanningManagementParametersDto>>(PlanningManagementParameters);
        }

        public async Task<IDataResult<SelectPlanningManagementParametersDto>> GetPlanningManagementParametersAsync()
        {
            SelectPlanningManagementParametersDto result = new SelectPlanningManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").UseIsDelete(false);

            SelectPlanningManagementParametersDto PlanningManagementParameter = queryFactory.Get<SelectPlanningManagementParametersDto>(controlQuery);

            if (PlanningManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").Where(
                 new
                 {
                     Id = PlanningManagementParameter.Id
                 }, "").UseIsDelete(false);

                result = queryFactory.Get<SelectPlanningManagementParametersDto>(query);
            }

            LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.PlanningManagementParameters, LogType.Get, result.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPlanningManagementParametersDto>(result);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlanningManagementParametersDto>> UpdateAsync(UpdatePlanningManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PlanningManagementParameters).Select("*").Where(new { Id = input.Id }, "").UseIsDelete(false);
            var entity = queryFactory.Get<PlanningManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.PlanningManagementParameters).Update(new UpdatePlanningManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                 MRPIISourceModule = input.MRPIISourceModule,
                 MRPPurchaseTransaction = input.MRPPurchaseTransaction,
                Id = input.Id
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var PlanningManagementParameters = queryFactory.Update<SelectPlanningManagementParametersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, PlanningManagementParameters, LoginedUserService.UserId, Tables.PlanningManagementParameters, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectPlanningManagementParametersDto>(PlanningManagementParameters);
        }

        #region Unused Implemented Methods

        

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
