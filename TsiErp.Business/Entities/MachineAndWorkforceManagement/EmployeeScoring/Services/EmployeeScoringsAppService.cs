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
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeScoring.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EmployeeScorings.Page;

namespace TsiErp.Business.Entities.EmployeeScoring.Services
{
    [ServiceRegistration(typeof(IEmployeeScoringsAppService), DependencyInjectionType.Scoped)]
    public class EmployeeScoringsAppService : ApplicationService<EmployeeScoringsResource>, IEmployeeScoringsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public EmployeeScoringsAppService(IStringLocalizer<EmployeeScoringsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateEmployeeScoringsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeScoringsDto>> CreateAsync(CreateEmployeeScoringsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.EmployeeScorings).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<EmployeeScorings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EmployeeScorings).Insert(new CreateEmployeeScoringsDto
            {
                Code = input.Code,
                Year_ = input.Year_,
                ScoringState = input.ScoringState,
                Month_ = input.Month_,
                Description_ = input.Description_,
                EndDate = input.EndDate,
                StartDate = input.StartDate,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectEmployeeScoringLines)
            {
                var lineID = GuidGenerator.CreateGuid();
                var queryLine = queryFactory.Query().From(Tables.EmployeeScoringLines).Insert(new CreateEmployeeScoringLinesDto
                {
                    EmployeeScoringID = addedEntityId,
                    CreationTime =now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = lineID,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                    AbsencePeriod = item.AbsencePeriod,
                    AfterEvaluationSalary = item.AfterEvaluationSalary,
                    AttendanceRatio = item.AttendanceRatio,
                    DepartmentID = item.DepartmentID,
                    EducationLevelID = item.EducationLevelID,
                    ProductionPerformanceRatio = item.ProductionPerformanceRatio,
                    SeniorityRatio = item.SeniorityRatio,
                    EmployeeID = item.EmployeeID,
                    GeneralSkillRatio = item.GeneralSkillRatio,
                    ManagementImprovementRatio = item.ManagementImprovementRatio,
                    OfficialSeniorityID = item.OfficialSeniorityID,
                    PositionValuationRaiseRatio = item.PositionValuationRaiseRatio,
                    RaiseMonth = item.RaiseMonth,
                    ReevaluationRatio = item.ReevaluationRatio,
                    SeniorityValue = item.SeniorityValue,
                    ShiftValue = item.ShiftValue,
                    StartingSalaryofPosition = item.StartingSalaryofPosition,
                    TaskCapabilityRatio = item.TaskCapabilityRatio,
                    TaskCompetenceScore = item.TaskCompetenceScore,
                    TodaysDate = item.TodaysDate,
                    SeniorityID = item.SeniorityID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                foreach (var itemline in item.SelectEmployeeOperations)
                {
                    var queryLinesLine = queryFactory.Query().From(Tables.EmployeeOperations).Insert(new CreateEmployeeOperationsDto
                    {
                        EmployeeScoringID = addedEntityId,
                        EmployeeScoringLineID = lineID,
                        EmployeeID = item.EmployeeID,
                        IsCalculated = itemline.IsCalculated,
                        Score_ = itemline.Score_,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = itemline.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                }
            }

            var employeeScoring = queryFactory.Insert<SelectEmployeeScoringsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EmployeeScoreChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EmployeeScorings, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeScoreChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeScoringsDto>(employeeScoring);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.EmployeeScorings).Select("*").Where(new { Id = id }, "");

            var EmployeeScorings = queryFactory.Get<SelectEmployeeScoringsDto>(query);

            if (EmployeeScorings.Id != Guid.Empty && EmployeeScorings != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.EmployeeScorings).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.EmployeeScoringLines).Delete(LoginedUserService.UserId).Where(new { EmployeeScoringID = id }, "");

                var lineLineDeleteQuery = queryFactory.Query().From(Tables.EmployeeOperations).Delete(LoginedUserService.UserId).Where(new { EmployeeScoringID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineLineDeleteQuery.Sql + " where " + lineLineDeleteQuery.WhereSentence;

                var employeeScoring = queryFactory.Update<SelectEmployeeScoringsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeScorings, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeScoreChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains(","))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split(',');

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(user),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }
                        else
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(notTemplate.TargetUsersId),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }

                }

                #endregion

                await Task.CompletedTask;
                return new SuccessDataResult<SelectEmployeeScoringsDto>(employeeScoring);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.EmployeeScoringLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var lineLineDeleteQuery = queryFactory.Query().From(Tables.EmployeeOperations).Delete(LoginedUserService.UserId).Where(new { EmployeeScoringLineID = id }, "");
                queryLine.Sql = queryLine.Sql + QueryConstants.QueryConstant + lineLineDeleteQuery.Sql + " where " + lineLineDeleteQuery.WhereSentence;
                var employeeScoringLines = queryFactory.Update<SelectEmployeeScoringLinesDto>(queryLine, "Id", true);
                var employeeScoringLinesLine = queryFactory.Update<SelectEmployeeOperationsDto>(lineLineDeleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeScoringLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectEmployeeScoringLinesDto>(employeeScoringLines);
            }

        }

        public async Task<IDataResult<SelectEmployeeScoringsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.EmployeeScorings)
                   .Select("*").Where(
            new
            {
                Id = id
            }, "");

            var EmployeeScorings = queryFactory.Get<SelectEmployeeScoringsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.EmployeeScoringLines)
                   .Select<EmployeeScoringLines>(null)
                   .Join<Employees>
                    (
                        p => new { EmployeeName = p.Name, EmployeeID = p.Id, EmployeeSurname = p.Surname, EmployeeHiringDate = p.HiringDate, EmployeeCurrentSalary = p.CurrentSalary, EmployeeTaskDefinition = p.TaskDefinition },
                        nameof(EmployeeScoringLines.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                    .Join<Departments>
                    (
                        p => new { DepartmentName = p.Name, DepartmentID = p.Id },
                        nameof(EmployeeScoringLines.DepartmentID),
                        nameof(Departments.Id),
                        JoinType.Left
                    )
                     .Join<EmployeeSeniorities>
                    (
                        p => new { OfficialSeniorityName = p.Name, OfficialSeniorityID = p.Id },
                        nameof(EmployeeScoringLines.OfficialSeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        "OfficialSeniority",
                        JoinType.Left
                    )
                   .Join<EmployeeSeniorities>
                    (
                        p => new { SeniorityName = p.Name, SeniorityID = p.Id },
                        nameof(EmployeeScoringLines.SeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        JoinType.Left
                    )
                    .Join<EducationLevelScores>
                    (
                        p => new { EducationLevelName = p.Name, EducationLevelID = p.Id, EducationLevelScore = p.Score },
                        nameof(EmployeeScoringLines.EducationLevelID),
                        nameof(EducationLevelScores.Id),
                        JoinType.Left
                    )
                    .Where(new { EmployeeScoringID = id }, Tables.EmployeeScoringLines);

            var EmployeeScoringLine = queryFactory.GetList<SelectEmployeeScoringLinesDto>(queryLines).ToList();

            foreach (var linesline in EmployeeScoringLine)
            {
                var queryLinesLine = queryFactory
                   .Query()
                   .From(Tables.EmployeeOperations)
                   .Select<EmployeeOperations>(null)

                    .Join<TemplateOperations>
                    (
                        p => new { TemplateOperationName = p.Name, TemplateOperationID = p.Id, TemplateOperationWorkScore = p.WorkScore },
                        nameof(EmployeeOperations.TemplateOperationID),
                        nameof(TemplateOperations.Id),
                        JoinType.Left
                    )

                    .Where(new { EmployeeScoringID = id, EmployeeScoringLineID = linesline.Id }, Tables.EmployeeOperations);

                var EmployeeScoringLinesLine = queryFactory.GetList<SelectEmployeeOperationsDto>(queryLines).ToList();

                int lineIndex = EmployeeScoringLine.IndexOf(linesline);
                EmployeeScoringLine[lineIndex].SelectEmployeeOperations = EmployeeScoringLinesLine;
            }

            EmployeeScorings.SelectEmployeeScoringLines = EmployeeScoringLine;

            LogsAppService.InsertLogToDatabase(EmployeeScorings, EmployeeScorings, LoginedUserService.UserId, Tables.EmployeeScorings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeScoringsDto>(EmployeeScorings);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeeScoringsDto>>> GetListAsync(ListEmployeeScoringsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.EmployeeScorings)
                   .Select<EmployeeScorings>(s => new { s.Code, s.StartDate, s.EndDate, s.Year_, s.Month_, s.Id }).Where(null, "");

            var EmployeeScorings = queryFactory.GetList<ListEmployeeScoringsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEmployeeScoringsDto>>(EmployeeScorings);

        }

        [ValidationAspect(typeof(UpdateEmployeeScoringsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeScoringsDto>> UpdateAsync(UpdateEmployeeScoringsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.EmployeeScorings)
                   .Select("*").Where(
            new
            {
                Id = input.Id
            }, "");

            var entity = queryFactory.Get<SelectEmployeeScoringsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.EmployeeScoringLines)
                   .Select<EmployeeScoringLines>(null)
                    .Join<Employees>
                    (
                        p => new { EmployeeName = p.Name, EmployeeID = p.Id, EmployeeSurname = p.Surname, EmployeeHiringDate = p.HiringDate, EmployeeCurrentSalary = p.CurrentSalary, EmployeeTaskDefinition = p.TaskDefinition },
                        nameof(EmployeeScoringLines.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                    .Join<Departments>
                    (
                        p => new { DepartmentName = p.Name, DepartmentID = p.Id },
                        nameof(EmployeeScoringLines.DepartmentID),
                        nameof(Departments.Id),
                        JoinType.Left
                    )
                     .Join<EmployeeSeniorities>
                    (
                        p => new { OfficialSeniorityName = p.Name, OfficialSeniorityID = p.Id },
                        nameof(EmployeeScoringLines.OfficialSeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        "OfficialSeniority",
                        JoinType.Left
                    )
                   .Join<EmployeeSeniorities>
                    (
                        p => new { SeniorityName = p.Name, SeniorityID = p.Id },
                        nameof(EmployeeScoringLines.SeniorityID),
                        nameof(EmployeeSeniorities.Id),
                        JoinType.Left
                    )
                    .Join<EducationLevelScores>
                    (
                        p => new { EducationLevelName = p.Name, EducationLevelID = p.Id, EducationLevelScore = p.Score },
                        nameof(EmployeeScoringLines.EducationLevelID),
                        nameof(EducationLevelScores.Id),
                        JoinType.Left
                    )
                    .Where(new { EmployeeScoringID = input.Id }, Tables.EmployeeScoringLines);

            var EmployeeScoringLine = queryFactory.GetList<SelectEmployeeScoringLinesDto>(queryLines).ToList();

            foreach (var line in EmployeeScoringLine)
            {
                var queryLinesLine = queryFactory
                 .Query()
                 .From(Tables.EmployeeOperations)
                 .Select<EmployeeOperations>(null)

                  .Join<TemplateOperations>
                  (
                      p => new { TemplateOperationName = p.Name, TemplateOperationID = p.Id, TemplateOperationWorkScore = p.WorkScore },
                      nameof(EmployeeOperations.TemplateOperationID),
                      nameof(TemplateOperations.Id),
                      JoinType.Left
                  )

                  .Where(new { EmployeeScoringID = input.Id, EmployeeScoringLineID = line.Id }, Tables.EmployeeOperations);

                var EmployeeScoringLinesLine = queryFactory.GetList<SelectEmployeeOperationsDto>(queryLines).ToList();

                int lineIndex = EmployeeScoringLine.IndexOf(line);
                EmployeeScoringLine[lineIndex].SelectEmployeeOperations = EmployeeScoringLinesLine;
            }

            entity.SelectEmployeeScoringLines = EmployeeScoringLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.EmployeeScorings)
                           .Select("*").Where(new { Code = input.Code }, "");

            var list = queryFactory.GetList<ListEmployeeScoringsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EmployeeScorings).Update(new UpdateEmployeeScoringsDto
            {
                Code = input.Code,
                Month_ = input.Month_,
                ScoringState = input.ScoringState,
                Description_ = input.Description_,
                EndDate = input.EndDate,
                StartDate = input.StartDate,
                Year_ = input.Year_,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectEmployeeScoringLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.EmployeeScoringLines).Insert(new CreateEmployeeScoringLinesDto
                    {
                        EmployeeScoringID = input.Id,
                        AbsencePeriod = item.AbsencePeriod,
                        AfterEvaluationSalary = item.AfterEvaluationSalary,
                        AttendanceRatio = item.AttendanceRatio,
                        DepartmentID = item.DepartmentID,
                        EducationLevelID = item.EducationLevelID,
                        EmployeeID = item.EmployeeID,
                        GeneralSkillRatio = item.GeneralSkillRatio,
                        ManagementImprovementRatio = item.ManagementImprovementRatio,
                        OfficialSeniorityID = item.OfficialSeniorityID,
                        PositionValuationRaiseRatio = item.PositionValuationRaiseRatio,
                        SeniorityRatio = item.SeniorityRatio,
                        RaiseMonth = item.RaiseMonth,
                        ReevaluationRatio = item.ReevaluationRatio,
                        SeniorityValue = item.SeniorityValue,
                        ShiftValue = item.ShiftValue,
                        StartingSalaryofPosition = item.StartingSalaryofPosition,
                        TaskCapabilityRatio = item.TaskCapabilityRatio,
                        TaskCompetenceScore = item.TaskCompetenceScore,
                        ProductionPerformanceRatio = item.ProductionPerformanceRatio,
                        TodaysDate = item.TodaysDate,
                        CreationTime = now,
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
                        SeniorityID = item.SeniorityID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                    foreach (var line in item.SelectEmployeeOperations)
                    {
                        var queryLinesLine = queryFactory.Query().From(Tables.EmployeeOperations).Insert(new CreateEmployeeOperationsDto
                        {
                            EmployeeScoringID = input.Id,
                            EmployeeScoringLineID = item.Id,
                            EmployeeID = item.EmployeeID,
                            IsCalculated = line.IsCalculated,
                            Score_ = line.Score_,
                            CreationTime = now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            LineNr = line.LineNr,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql;
                    }
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.EmployeeScoringLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectEmployeeScoringLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.EmployeeScoringLines).Update(new UpdateEmployeeScoringLinesDto
                        {
                            EmployeeScoringID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            AbsencePeriod = item.AbsencePeriod,
                            AfterEvaluationSalary = item.AfterEvaluationSalary,
                            AttendanceRatio = item.AttendanceRatio,
                            DepartmentID = item.DepartmentID,
                            EducationLevelID = item.EducationLevelID,
                            EmployeeID = item.EmployeeID,
                            GeneralSkillRatio = item.GeneralSkillRatio,
                            ManagementImprovementRatio = item.ManagementImprovementRatio,
                            OfficialSeniorityID = item.OfficialSeniorityID,
                            PositionValuationRaiseRatio = item.PositionValuationRaiseRatio,
                            ProductionPerformanceRatio = item.ProductionPerformanceRatio,
                            SeniorityRatio = item.SeniorityRatio,
                            RaiseMonth = item.RaiseMonth,
                            ReevaluationRatio = item.ReevaluationRatio,
                            SeniorityValue = item.SeniorityValue,
                            ShiftValue = item.ShiftValue,
                            StartingSalaryofPosition = item.StartingSalaryofPosition,
                            TaskCapabilityRatio = item.TaskCapabilityRatio,
                            TaskCompetenceScore = item.TaskCompetenceScore,
                            TodaysDate = item.TodaysDate,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            SeniorityID = item.SeniorityID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                        foreach (var empoprline in item.SelectEmployeeOperations)
                        {
                            var lineslineGetQuery = queryFactory.Query().From(Tables.EmployeeOperations).Select("*").Where(new { EmployeeScoringLineID = item.Id }, "");

                            var linesline = queryFactory.Get<SelectEmployeeOperationsDto>(lineslineGetQuery);

                            var queryLinesLine = queryFactory.Query().From(Tables.EmployeeOperations).Insert(new CreateEmployeeOperationsDto
                            {
                                EmployeeScoringID = input.Id,
                                EmployeeScoringLineID = item.Id,
                                EmployeeID = item.EmployeeID,
                                IsCalculated = empoprline.IsCalculated,
                                Score_ = empoprline.Score_,
                                TemplateOperationID = empoprline.TemplateOperationID,
                                CreationTime = now,
                                CreatorId = linesline.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = Guid.Empty,
                                DeletionTime = linesline.DeletionTime,
                                Id = linesline.Id,
                                IsDeleted = false,
                                LastModificationTime =now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = empoprline.LineNr,
                            }).Where(new { Id = empoprline.Id }, ""); ;

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLinesLine.Sql + " where " + queryLinesLine.WhereSentence;
                        }
                    }
                }
            }

            var employeeScorings = queryFactory.Update<SelectEmployeeScoringsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.EmployeeScorings, LogType.Update, employeeScorings.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeScoreChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeScoringsDto>(employeeScorings);

        }

        public async Task<IDataResult<SelectEmployeeScoringsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeScorings).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<EmployeeScorings>(entityQuery);

            var query = queryFactory.Query().From(Tables.EmployeeScorings).Update(new UpdateEmployeeScoringsDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                ScoringState = (int)entity.ScoringState,
                EndDate = entity.EndDate,
                StartDate = entity.StartDate,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Month_ = entity.Month_,
                Year_ = entity.Year_,
                Description_ = entity.Description_,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var EmployeeScoringsDto = queryFactory.Update<SelectEmployeeScoringsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeScoringsDto>(EmployeeScoringsDto);


        }
    }
}
