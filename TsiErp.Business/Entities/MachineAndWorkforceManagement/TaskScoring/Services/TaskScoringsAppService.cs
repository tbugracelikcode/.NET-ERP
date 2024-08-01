using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.TaskScoring.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.TaskScoring.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.TaskScorings.Page;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    [ServiceRegistration(typeof(ITaskScoringsAppService), DependencyInjectionType.Scoped)]
    public class TaskScoringsAppService : ApplicationService<TaskScoringsResource>, ITaskScoringsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public TaskScoringsAppService(IStringLocalizer<TaskScoringsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateTaskScoringsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTaskScoringsDto>> CreateAsync(CreateTaskScoringsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.TaskScorings).Insert(new CreateTaskScoringsDto
            {
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                Score = input.Score,
                Code = input.Code,
                IsAdjustment = input.IsAdjustment,
                IsDetectFault = input.IsDetectFault,
                IsDeveloperIdea = input.IsDeveloperIdea,
                IsTaskDone = input.IsTaskDone,
                IsTaskSharing = input.IsTaskSharing,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var TaskScorings = queryFactory.Insert<SelectTaskScoringsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("TaskScoringsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.TaskScorings, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTaskScoringsDto>(TaskScorings);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.TaskScorings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var TaskScorings = queryFactory.Update<SelectTaskScoringsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TaskScorings, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTaskScoringsDto>(TaskScorings);
        }


        public async Task<IDataResult<SelectTaskScoringsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.TaskScorings).Select<TaskScorings>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(TaskScorings.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.TaskScorings);
            var EmployeeSeniority = queryFactory.Get<SelectTaskScoringsDto>(query);

            LogsAppService.InsertLogToDatabase(EmployeeSeniority, EmployeeSeniority, LoginedUserService.UserId, Tables.TaskScorings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTaskScoringsDto>(EmployeeSeniority);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTaskScoringsDto>>> GetListAsync(ListTaskScoringsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.TaskScorings).Select<TaskScorings>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(TaskScorings.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.TaskScorings);

            var taskScorings = queryFactory.GetList<ListTaskScoringsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListTaskScoringsDto>>(taskScorings);
        }


        [ValidationAspect(typeof(UpdateTaskScoringsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTaskScoringsDto>> UpdateAsync(UpdateTaskScoringsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.TaskScorings).Select<TaskScorings>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(TaskScorings.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(new { Id = input.Id }, false, false, Tables.TaskScorings);
            var entity = queryFactory.Get<TaskScorings>(entityQuery);



            var query = queryFactory.Query().From(Tables.TaskScorings).Update(new UpdateTaskScoringsDto
            {
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                IsTaskSharing = input.IsTaskSharing,
                IsTaskDone = input.IsTaskDone,
                IsDeveloperIdea = input.IsDeveloperIdea,
                IsDetectFault = input.IsDetectFault,
                IsAdjustment = input.IsAdjustment,
                Code = input.Code,
                Score = input.Score,
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
            }).Where(new { Id = input.Id }, false, false, "");

            var taskScorings = queryFactory.Update<SelectTaskScoringsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, taskScorings, LoginedUserService.UserId, Tables.TaskScorings, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTaskScoringsDto>(taskScorings);
        }

        public async Task<IDataResult<SelectTaskScoringsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.TaskScorings).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<TaskScorings>(entityQuery);

            var query = queryFactory.Query().From(Tables.TaskScorings).Update(new UpdateTaskScoringsDto
            {
                SeniorityID = entity.SeniorityID,
                Score = entity.Score,
                Code = entity.Code,
                IsAdjustment = entity.IsAdjustment,
                IsDetectFault = entity.IsDetectFault,
                IsDeveloperIdea = entity.IsDeveloperIdea,
                IsTaskDone = entity.IsTaskDone,
                IsTaskSharing = entity.IsTaskSharing,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, false, false, "");

            var TaskScorings = queryFactory.Update<SelectTaskScoringsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectTaskScoringsDto>(TaskScorings);
        }
    }
}
