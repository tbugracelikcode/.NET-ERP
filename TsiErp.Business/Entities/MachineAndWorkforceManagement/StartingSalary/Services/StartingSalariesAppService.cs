using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.StartingSalary.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.StartingSalary.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StartingSalaries.Page;

namespace TsiErp.Business.Entities.StartingSalary.Services
{
    [ServiceRegistration(typeof(IStartingSalariesAppService), DependencyInjectionType.Scoped)]
    public class StartingSalariesAppService : ApplicationService<StartingSalariesResource>, IStartingSalariesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StartingSalariesAppService(IStringLocalizer<StartingSalariesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateStartingSalariesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStartingSalariesDto>> CreateAsync(CreateStartingSalariesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StartingSalaries).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<StartingSalaries>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StartingSalaries).Insert(new CreateStartingSalariesDto
            {
                Code = input.Code,
                Year_ = input.Year_,
                Description_ = input.Description_,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
            });

            foreach (var item in input.SelectStartingSalaryLines)
            {
                var queryLine = queryFactory.Query().From(Tables.StartingSalaryLines).Insert(new CreateStartingSalaryLinesDto
                {
                    StartingSalaryID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    CurrentSalaryLowerLimit = item.CurrentSalaryLowerLimit,
                    CurrentSalaryUpperLimit = item.CurrentSalaryUpperLimit,
                    CurrentStartingSalary = item.CurrentStartingSalary,
                    Difference = item.Difference,
                    SeniorityID = item.SeniorityID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var billOfMaterial = queryFactory.Insert<SelectStartingSalariesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StartingSalariesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StartingSalaries, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStartingSalariesDto>(billOfMaterial);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StartingSalaries).Select("*").Where(new { Id = id }, false, false, "");

            var StartingSalaries = queryFactory.Get<SelectStartingSalariesDto>(query);

            if (StartingSalaries.Id != Guid.Empty && StartingSalaries != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.StartingSalaries).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.StartingSalaryLines).Delete(LoginedUserService.UserId).Where(new { StartingSalaryID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var billOfMaterial = queryFactory.Update<SelectStartingSalariesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StartingSalaries, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectStartingSalariesDto>(billOfMaterial);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.StartingSalaryLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var billOfMaterialLines = queryFactory.Update<SelectStartingSalaryLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StartingSalaryLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectStartingSalaryLinesDto>(billOfMaterialLines);
            }

        }

        public async Task<IDataResult<SelectStartingSalariesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StartingSalaries)
                   .Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");

            var StartingSalaries = queryFactory.Get<SelectStartingSalariesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StartingSalaryLines)
                   .Select<StartingSalaryLines>(null)
                   .Join<EmployeeSeniorities>
                    (
                        p => new { SeniorityName = p.Name, SeniorityID = p.Id },
                        nameof(StartingSalaryLines.SeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        JoinType.Left
                    )
                    .Where(new { StartingSalaryID = id }, false, false, Tables.StartingSalaryLines);

            var StartingSalaryLine = queryFactory.GetList<SelectStartingSalaryLinesDto>(queryLines).ToList();

            StartingSalaries.SelectStartingSalaryLines = StartingSalaryLine;

            LogsAppService.InsertLogToDatabase(StartingSalaries, StartingSalaries, LoginedUserService.UserId, Tables.StartingSalaries, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStartingSalariesDto>(StartingSalaries);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStartingSalariesDto>>> GetListAsync(ListStartingSalariesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.StartingSalaries)
                   .Select("*").Where(null, false, false, "");

            var StartingSalaries = queryFactory.GetList<ListStartingSalariesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStartingSalariesDto>>(StartingSalaries);

        }

        [ValidationAspect(typeof(UpdateStartingSalariesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStartingSalariesDto>> UpdateAsync(UpdateStartingSalariesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.StartingSalaries)
                   .Select("*").Where(
            new
            {
                Id = input.Id
            }, false, false, "");

            var entity = queryFactory.Get<SelectStartingSalariesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StartingSalaryLines)
                   .Select<StartingSalaryLines>(null)
                   .Join<EmployeeSeniorities>
                    (
                        p => new { SeniorityName = p.Name, SeniorityID = p.Id },
                        nameof(StartingSalaryLines.SeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        JoinType.Left
                    )
                    .Where(new { StartingSalaryID = input.Id }, false, false, Tables.StartingSalaryLines);

            var StartingSalaryLine = queryFactory.GetList<SelectStartingSalaryLinesDto>(queryLines).ToList();

            entity.SelectStartingSalaryLines = StartingSalaryLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.StartingSalaries)
                           .Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.GetList<ListStartingSalariesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.StartingSalaries).Update(new UpdateStartingSalariesDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Year_ = input.Year_,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectStartingSalaryLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.StartingSalaryLines).Insert(new CreateStartingSalaryLinesDto
                    {
                        StartingSalaryID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        SeniorityID = item.SeniorityID.GetValueOrDefault(),
                        CurrentSalaryLowerLimit = item.CurrentSalaryLowerLimit,
                        CurrentSalaryUpperLimit = item.CurrentSalaryUpperLimit,
                        CurrentStartingSalary = item.CurrentStartingSalary,
                        Difference = item.Difference,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.StartingSalaryLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectStartingSalaryLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StartingSalaryLines).Update(new UpdateStartingSalaryLinesDto
                        {
                            StartingSalaryID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            Difference = item.Difference,
                            CurrentStartingSalary = item.CurrentStartingSalary,
                            CurrentSalaryUpperLimit = item.CurrentSalaryUpperLimit,
                            CurrentSalaryLowerLimit = item.CurrentSalaryLowerLimit,
                            SeniorityID = item.SeniorityID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectStartingSalariesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.StartingSalaries, LogType.Update, billOfMaterial.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStartingSalariesDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectStartingSalariesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StartingSalaries).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<StartingSalaries>(entityQuery);

            var query = queryFactory.Query().From(Tables.StartingSalaries).Update(new UpdateStartingSalariesDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
                Year_ = entity.Year_,
                Description_ = entity.Description_,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, false, false, "");

            var StartingSalariesDto = queryFactory.Update<SelectStartingSalariesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStartingSalariesDto>(StartingSalariesDto);


        }
    }
}
