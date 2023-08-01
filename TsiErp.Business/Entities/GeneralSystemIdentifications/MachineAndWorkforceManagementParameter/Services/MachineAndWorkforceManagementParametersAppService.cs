using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MachineAndWorkforceManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Services
{
    [ServiceRegistration(typeof(IMachineAndWorkforceManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class MachineAndWorkforceManagementParametersAppService : ApplicationService<MachineAndWorkforceManagementParametersResource>, IMachineAndWorkforceManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public MachineAndWorkforceManagementParametersAppService(IStringLocalizer<MachineAndWorkforceManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var MachineAndWorkforceManagementParameter = queryFactory.Get<SelectMachineAndWorkforceManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(MachineAndWorkforceManagementParameter, MachineAndWorkforceManagementParameter, LoginedUserService.UserId, Tables.MachineAndWorkforceManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectMachineAndWorkforceManagementParametersDto>(MachineAndWorkforceManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMachineAndWorkforceManagementParametersDto>>> GetListAsync(ListMachineAndWorkforceManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(null, false, false, "");

                var MachineAndWorkforceManagementParameters = queryFactory.GetList<ListMachineAndWorkforceManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListMachineAndWorkforceManagementParametersDto>>(MachineAndWorkforceManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> UpdateAsync(UpdateMachineAndWorkforceManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<MachineAndWorkforceManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Update(new UpdateMachineAndWorkforceManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var MachineAndWorkforceManagementParameters = queryFactory.Update<SelectMachineAndWorkforceManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, MachineAndWorkforceManagementParameters, LoginedUserService.UserId, Tables.MachineAndWorkforceManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectMachineAndWorkforceManagementParametersDto>(MachineAndWorkforceManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> CreateAsync(CreateMachineAndWorkforceManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
