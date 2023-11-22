using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.EmployeeAnnualSeniorityDifference.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EmployeeAnnualSeniorityDifferences.Page;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    [ServiceRegistration(typeof(IEmployeeAnnualSeniorityDifferencesAppService), DependencyInjectionType.Scoped)]
    public class EmployeeAnnualSeniorityDifferencesAppService : ApplicationService<EmployeeAnnualSeniorityDifferencesResource>, IEmployeeAnnualSeniorityDifferencesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public EmployeeAnnualSeniorityDifferencesAppService(IStringLocalizer<EmployeeAnnualSeniorityDifferencesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }


        [ValidationAspect(typeof(CreateEmployeeAnnualSeniorityDifferencesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> CreateAsync(CreateEmployeeAnnualSeniorityDifferencesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Insert(new CreateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = input.Description_,
                SeniorityID = input.SeniorityID,
                Difference = input.Difference,
                Year_ = input.Year_,
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


            var EmployeeAnnualSeniorityDifferences = queryFactory.Insert<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var EmployeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Delete, id);

            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }


        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.EmployeeAnnualSeniorityDifferences);
            var EmployeeSeniority = queryFactory.Get<SelectEmployeeAnnualSeniorityDifferencesDto>(query);

            LogsAppService.InsertLogToDatabase(EmployeeSeniority, EmployeeSeniority, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Get, id);

            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeSeniority);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeeAnnualSeniorityDifferencesDto>>> GetListAsync(ListEmployeeAnnualSeniorityDifferencesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.EmployeeAnnualSeniorityDifferences);

            var employeeAnnualSeniorityDifferences = queryFactory.GetList<ListEmployeeAnnualSeniorityDifferencesDto>(query).ToList();
            return new SuccessDataResult<IList<ListEmployeeAnnualSeniorityDifferencesDto>>(employeeAnnualSeniorityDifferences);
        }


        [ValidationAspect(typeof(UpdateEmployeeAnnualSeniorityDifferencesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> UpdateAsync(UpdateEmployeeAnnualSeniorityDifferencesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(new { Id = input.Id }, false, false, Tables.EmployeeAnnualSeniorityDifferences);
            var entity = queryFactory.Get<EmployeeAnnualSeniorityDifferences>(entityQuery);



            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Update(new UpdateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = input.Description_,
                Year_ = input.Year_,
                Difference = input.Difference,
                SeniorityID = input.SeniorityID,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, false, false, "");

            var employeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, employeeAnnualSeniorityDifferences, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Update, entity.Id);

            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(employeeAnnualSeniorityDifferences);
        }

        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<EmployeeAnnualSeniorityDifferences>(entityQuery);

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Update(new UpdateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = entity.Description_,
                SeniorityID = entity.SeniorityID,
                Difference = entity.Difference,
                Year_ = entity.Year_,
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

            }).Where(new { Id = id }, false, false, "");

            var EmployeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }
    }
}
