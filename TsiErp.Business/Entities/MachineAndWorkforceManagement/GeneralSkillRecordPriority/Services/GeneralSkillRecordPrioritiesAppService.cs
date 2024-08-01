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
using TsiErp.Business.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.GeneralSkillRecordPriorities.Page;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    [ServiceRegistration(typeof(IGeneralSkillRecordPrioritiesAppService), DependencyInjectionType.Scoped)]
    public class GeneralSkillRecordPrioritiesAppService : ApplicationService<GeneralSkillRecordPrioritiesResource>, IGeneralSkillRecordPrioritiesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public GeneralSkillRecordPrioritiesAppService(IStringLocalizer<GeneralSkillRecordPrioritiesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateGeneralSkillRecordPrioritiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGeneralSkillRecordPrioritiesDto>> CreateAsync(CreateGeneralSkillRecordPrioritiesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Insert(new CreateGeneralSkillRecordPrioritiesDto
            {
                Score = input.Score,
                Code = input.Code,
                GeneralSkillID = input.GeneralSkillID.GetValueOrDefault(),
                Description_ = input.Description_,
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


            var GeneralSkillRecordPriorities = queryFactory.Insert<SelectGeneralSkillRecordPrioritiesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("GeneralSkillRecordPrioritiesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.GeneralSkillRecordPriorities, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralSkillRecordPrioritiesDto>(GeneralSkillRecordPriorities);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var GeneralSkillRecordPriorities = queryFactory.Update<SelectGeneralSkillRecordPrioritiesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.GeneralSkillRecordPriorities, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralSkillRecordPrioritiesDto>(GeneralSkillRecordPriorities);
        }


        public async Task<IDataResult<SelectGeneralSkillRecordPrioritiesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Select<GeneralSkillRecordPriorities>(null)
                        .Join<EmployeeGeneralSkillRecords>
                        (
                            d => new { GeneralSkillName = d.Name, GeneralSkillID = d.Id },
                            nameof(GeneralSkillRecordPriorities.GeneralSkillID),
                            nameof(EmployeeGeneralSkillRecords.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.GeneralSkillRecordPriorities);
            var EmployeeSeniority = queryFactory.Get<SelectGeneralSkillRecordPrioritiesDto>(query);

            LogsAppService.InsertLogToDatabase(EmployeeSeniority, EmployeeSeniority, LoginedUserService.UserId, Tables.GeneralSkillRecordPriorities, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralSkillRecordPrioritiesDto>(EmployeeSeniority);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGeneralSkillRecordPrioritiesDto>>> GetListAsync(ListGeneralSkillRecordPrioritiesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Select<GeneralSkillRecordPriorities>(null)
                         .Join<EmployeeGeneralSkillRecords>
                        (
                            d => new { GeneralSkillName = d.Name, GeneralSkillID = d.Id },
                            nameof(GeneralSkillRecordPriorities.GeneralSkillID),
                            nameof(EmployeeGeneralSkillRecords.Id),
                            JoinType.Left
                        ).Where(null, false, false, Tables.GeneralSkillRecordPriorities);

            var generalSkillRecordPriorities = queryFactory.GetList<ListGeneralSkillRecordPrioritiesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListGeneralSkillRecordPrioritiesDto>>(generalSkillRecordPriorities);
        }


        [ValidationAspect(typeof(UpdateGeneralSkillRecordPrioritiesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGeneralSkillRecordPrioritiesDto>> UpdateAsync(UpdateGeneralSkillRecordPrioritiesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Select<GeneralSkillRecordPriorities>(null)
                        .Join<EmployeeGeneralSkillRecords>
                        (
                            d => new { GeneralSkillName = d.Name, GeneralSkillID = d.Id },
                            nameof(GeneralSkillRecordPriorities.GeneralSkillID),
                            nameof(EmployeeGeneralSkillRecords.Id),
                            JoinType.Left
                        ).Where(new { Id = input.Id }, false, false, Tables.GeneralSkillRecordPriorities);
            var entity = queryFactory.Get<GeneralSkillRecordPriorities>(entityQuery);



            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Update(new UpdateGeneralSkillRecordPrioritiesDto
            {

                GeneralSkillID = input.GeneralSkillID.GetValueOrDefault(),
                Description_ = input.Description_,
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

            var generalSkillRecordPriorities = queryFactory.Update<SelectGeneralSkillRecordPrioritiesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, generalSkillRecordPriorities, LoginedUserService.UserId, Tables.GeneralSkillRecordPriorities, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralSkillRecordPrioritiesDto>(generalSkillRecordPriorities);
        }

        public async Task<IDataResult<SelectGeneralSkillRecordPrioritiesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<GeneralSkillRecordPriorities>(entityQuery);

            var query = queryFactory.Query().From(Tables.GeneralSkillRecordPriorities).Update(new UpdateGeneralSkillRecordPrioritiesDto
            {

                GeneralSkillID = entity.GeneralSkillID,
                Description_ = entity.Description_,
                Score = entity.Score,
                Code = entity.Code,
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

            var GeneralSkillRecordPriorities = queryFactory.Update<SelectGeneralSkillRecordPrioritiesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralSkillRecordPrioritiesDto>(GeneralSkillRecordPriorities);
        }
    }
}
