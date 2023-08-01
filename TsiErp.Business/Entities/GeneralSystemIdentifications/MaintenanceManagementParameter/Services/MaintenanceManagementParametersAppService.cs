using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenanceManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Services
{
    [ServiceRegistration(typeof(IMaintenanceManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceManagementParametersAppService : ApplicationService<MaintenanceManagementParametersResource>, IMaintenanceManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public MaintenanceManagementParametersAppService(IStringLocalizer<MaintenanceManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectMaintenanceManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.MaintenanceManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var MaintenanceManagementParameter = queryFactory.Get<SelectMaintenanceManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(MaintenanceManagementParameter, MaintenanceManagementParameter, LoginedUserService.UserId, Tables.MaintenanceManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectMaintenanceManagementParametersDto>(MaintenanceManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceManagementParametersDto>>> GetListAsync(ListMaintenanceManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MaintenanceManagementParameters).Select("*").Where(null, false, false, "");

                var MaintenanceManagementParameters = queryFactory.GetList<ListMaintenanceManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListMaintenanceManagementParametersDto>>(MaintenanceManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceManagementParametersDto>> UpdateAsync(UpdateMaintenanceManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MaintenanceManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<MaintenanceManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.MaintenanceManagementParameters).Update(new UpdateMaintenanceManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var MaintenanceManagementParameters = queryFactory.Update<SelectMaintenanceManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, MaintenanceManagementParameters, LoginedUserService.UserId, Tables.MaintenanceManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectMaintenanceManagementParametersDto>(MaintenanceManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectMaintenanceManagementParametersDto>> CreateAsync(CreateMaintenanceManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectMaintenanceManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
