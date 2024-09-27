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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.TemplateOperation.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.TemplateOperations.Page;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    [ServiceRegistration(typeof(ITemplateOperationsAppService), DependencyInjectionType.Scoped)]
    public class TemplateOperationsAppService : ApplicationService<TemplateOperationsResource>, ITemplateOperationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public TemplateOperationsAppService(IStringLocalizer<TemplateOperationsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreateTemplateOperationsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectTemplateOperationsDto>> CreateAsync(CreateTemplateOperationsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<TemplateOperations>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.TemplateOperations).Insert(new CreateTemplateOperationsDto
            {
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                IsCanBeDetected = input.IsCanBeDetected,
                IsCauseExtraCost = input.IsCauseExtraCost,
                IsHighRepairCost = input.IsHighRepairCost,
                IsLongWorktimeforOperator = input.IsLongWorktimeforOperator,
                IsPhysicallyHard = input.IsPhysicallyHard,
                IsRequiresKnowledge = input.IsRequiresKnowledge,
                IsRequiresSkill = input.IsRequiresSkill,
                IsRiskyforOperator = input.IsRiskyforOperator,
                IsSensitive = input.IsSensitive,
                WorkScore = input.WorkScore,
                Code = input.Code,
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
                Name = input.Name,
            });

            foreach (var item in input.SelectTemplateOperationLines)
            {
                var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Insert(new CreateTemplateOperationLinesDto
                {
                    AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                    Alternative = item.Alternative,
                    OperationTime = item.OperationTime,
                    Priority = item.Priority,
                    ProcessQuantity = item.ProcessQuantity,
                    StationID = item.StationID.GetValueOrDefault(),
                    TemplateOperationID = addedEntityId,
                    CreationTime =now,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            foreach (var item in input.SelectTemplateOperationUnsuitabilityItems)
            {
                var queryLine = queryFactory.Query().From(Tables.TemplateOperationUnsuitabilityItems).Insert(new CreateTemplateOperationUnsuitabilityItemsDto
                {
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
                    TemplateOperationId = addedEntityId,
                    ToBeUsed = item.ToBeUsed,
                    UnsuitabilityItemsId = item.UnsuitabilityItemsId
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var templateOperation = queryFactory.Insert<SelectTemplateOperationsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("TempOperationsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TempOperationsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("TemplateOperationID", new List<string>
            {
                Tables.EmployeeOperations,
                Tables.ProductsOperations
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = id },  "");

                var templateOperations = queryFactory.Get<SelectTemplateOperationsDto>(query);

                if (templateOperations.Id != Guid.Empty && templateOperations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.TemplateOperations).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.TemplateOperationLines).Delete(LoginedUserService.UserId).Where(new { TemplateOperationID = id }, "");

                    var lineUnsuitabilityItemsQuery = queryFactory.Query().From(Tables.TemplateOperationUnsuitabilityItems).Delete(LoginedUserService.UserId).Where(new { TemplateOperationId = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql 
                        + QueryConstants.QueryConstant + lineDeleteQuery.Sql  + " where " + lineDeleteQuery.WhereSentence
                        + QueryConstants.QueryConstant + lineUnsuitabilityItemsQuery.Sql + " where " + lineUnsuitabilityItemsQuery.WhereSentence;

                    var templateOperation = queryFactory.Update<SelectTemplateOperationsDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TempOperationsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var templateOperationLines = queryFactory.Update<SelectTemplateOperationLinesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.TemplateOperationLines, LogType.Delete, id);
                    await Task.CompletedTask;

                    return new SuccessDataResult<SelectTemplateOperationLinesDto>(templateOperationLines);
                }
            }
        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.TemplateOperations).Select<TemplateOperations>(null)
                .Join<StationGroups>
                (
                    g => new { WorkCenterName = g.Name },
                    nameof(TemplateOperations.WorkCenterID),
                    nameof(StationGroups.Id), JoinType.Left
                )
                .Where
                (
                    new
                    {
                        Id = id
                    }, Tables.TemplateOperations
                );

            var templateOperations = queryFactory.Get<SelectTemplateOperationsDto>(query);

            #region TemplateOperationLines
            var queryLines = queryFactory
                           .Query()
                           .From(Tables.TemplateOperationLines)
                           .Select<TemplateOperationLines>(null)
                           .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(TemplateOperationLines.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Where(new { TemplateOperationID = id }, Tables.TemplateOperationLines);

            var templateOperationLine = queryFactory.GetList<SelectTemplateOperationLinesDto>(queryLines).ToList();

            templateOperations.SelectTemplateOperationLines = templateOperationLine;
            #endregion

            #region UnsuitabilityItems
            var queryUnsuitabilityItems = queryFactory
                        .Query()
                        .From(Tables.TemplateOperationUnsuitabilityItems)
                        .Select<TemplateOperationUnsuitabilityItems>(null)
                        .Join<UnsuitabilityItems>
                        (
                            s => new { UnsuitabilityItemsId = s.Id, UnsuitabilityItemsName = s.Name },
                            nameof(TemplateOperationUnsuitabilityItems.UnsuitabilityItemsId),
                            nameof(UnsuitabilityItems.Id),
                            JoinType.Left
                        )
                        .Where(new { TemplateOperationId = id }, Tables.TemplateOperationUnsuitabilityItems);

            var unsuitabilityItemsLine = queryFactory.GetList<SelectTemplateOperationUnsuitabilityItemsDto>(queryUnsuitabilityItems).ToList();

            templateOperations.SelectTemplateOperationUnsuitabilityItems = unsuitabilityItemsLine;

            #region UnsuitabilityItems Control

            var unsuitabilityItemsQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { StationGroupId = templateOperations.WorkCenterID }, "");

            var unsuitabilityItemsList = queryFactory.GetList<SelectUnsuitabilityItemsDto>(unsuitabilityItemsQuery).ToList();

            var lineNr = unsuitabilityItemsLine.Count > 1 ? unsuitabilityItemsLine.Max(s => s.LineNr) : 1;

            foreach (var item in unsuitabilityItemsList)
            {
                if (!unsuitabilityItemsLine.Any(t => t.UnsuitabilityItemsId == item.Id))
                {
                    lineNr++;

                    unsuitabilityItemsLine.Add(new SelectTemplateOperationUnsuitabilityItemsDto
                    {
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = Guid.Empty,
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = lineNr,
                        ToBeUsed = false,
                        UnsuitabilityItemsId = item.Id,
                        UnsuitabilityItemsName = item.Name
                    });

                }
            }
            #endregion

            templateOperations.SelectTemplateOperationUnsuitabilityItems = unsuitabilityItemsLine;

            #endregion

            LogsAppService.InsertLogToDatabase(templateOperations, templateOperations, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperations);

        }

        public async Task<IDataResult<IList<ListTemplateOperationsDto>>> GetListAsync(ListTemplateOperationsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.TemplateOperations)
                .Select<TemplateOperations>(s => new { s.Code, s.Name, s.Id , s.WorkScore})
                .Join<StationGroups>
                (
                    g => new { WorkCenterName = g.Name },
                    nameof(TemplateOperations.WorkCenterID),
                    nameof(StationGroups.Id), JoinType.Left
                ).Where(null,Tables.TemplateOperations);

            var templateOperations = queryFactory.GetList<ListTemplateOperationsDto>(query).ToList();

            #region TemplateOperationLines
            var queryLines = queryFactory
                         .Query()
                         .From(Tables.TemplateOperationLines)
                         .Select<TemplateOperationLines>(null)
                         .Join<Stations>
                          (
                              s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                              nameof(TemplateOperationLines.StationID),
                              nameof(Stations.Id),
                              JoinType.Left
                          )
                          .Where(null, Tables.TemplateOperationLines);

            var templateOperationLine = queryFactory.GetList<SelectTemplateOperationLinesDto>(queryLines).ToList();

            foreach (var item in templateOperations)
            {
                item.SelectTemplateOperationLines = templateOperationLine.Where(t => t.TemplateOperationID == item.Id).ToList();
            }
            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListTemplateOperationsDto>>(templateOperations);

        }

        [ValidationAspect(typeof(UpdateTemplateOperationsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateAsync(UpdateTemplateOperationsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = input.Id }, Tables.TemplateOperations);

            var entity = queryFactory.Get<SelectTemplateOperationsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.TemplateOperationLines)
                   .Select<TemplateOperationLines>(null)
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                        nameof(TemplateOperationLines.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Where(new { TemplateOperationID = input.Id }, Tables.TemplateOperationLines);

            var templateOperationLine = queryFactory.GetList<SelectTemplateOperationLinesDto>(queryLines).ToList();

            entity.SelectTemplateOperationLines = templateOperationLine;

            #region UnsuitabilityItems
            var queryUnsuitabilityItems = queryFactory
                        .Query()
                        .From(Tables.TemplateOperationUnsuitabilityItems)
                        .Select<TemplateOperationUnsuitabilityItems>(null)
                        .Join<UnsuitabilityItems>
                        (
                            s => new { UnsuitabilityItemsId = s.Id, UnsuitabilityItemsName = s.Name },
                            nameof(TemplateOperationUnsuitabilityItems.UnsuitabilityItemsId),
                            nameof(UnsuitabilityItems.Id),
                            JoinType.Left
                        )
                        .Where(new { TemplateOperationId = input.Id }, Tables.TemplateOperationUnsuitabilityItems);

            var unsuitabilityItemsLine = queryFactory.GetList<SelectTemplateOperationUnsuitabilityItemsDto>(queryUnsuitabilityItems).ToList();

            entity.SelectTemplateOperationUnsuitabilityItems = unsuitabilityItemsLine;

            #endregion

            #region Update Control
            var listQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Code = input.Code }, Tables.TemplateOperations);

            var list = queryFactory.GetList<ListTemplateOperationsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.TemplateOperations).Update(new UpdateTemplateOperationsDto
            {
                Name = input.Name,
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                IsCanBeDetected = input.IsCanBeDetected,
                IsCauseExtraCost = input.IsCauseExtraCost,
                IsHighRepairCost = input.IsHighRepairCost,
                IsLongWorktimeforOperator = input.IsLongWorktimeforOperator,
                IsPhysicallyHard = input.IsPhysicallyHard,
                IsRequiresKnowledge = input.IsRequiresKnowledge,
                IsRequiresSkill = input.IsRequiresSkill,
                IsRiskyforOperator = input.IsRiskyforOperator,
                IsSensitive = input.IsSensitive,
                WorkScore = input.WorkScore,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },  "");

            #region TemplateOperationLines
            foreach (var item in input.SelectTemplateOperationLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Insert(new CreateTemplateOperationLinesDto
                    {
                        AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                        Alternative = item.Alternative,
                        OperationTime = item.OperationTime,
                        Priority = item.Priority,
                        ProcessQuantity = item.ProcessQuantity,
                        StationID = item.StationID.GetValueOrDefault(),
                        TemplateOperationID = input.Id,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.TemplateOperationLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectTemplateOperationLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.TemplateOperationLines).Update(new UpdateTemplateOperationLinesDto
                        {
                            AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                            Alternative = item.Alternative,
                            OperationTime = item.OperationTime,
                            Priority = item.Priority,
                            ProcessQuantity = item.ProcessQuantity,
                            StationID = item.StationID.GetValueOrDefault(),
                            TemplateOperationID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = false,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            #endregion

            #region UnsuitabilityItems
            foreach (var item in input.SelectTemplateOperationUnsuitabilityItems)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.TemplateOperationUnsuitabilityItems).Insert(new CreateTemplateOperationUnsuitabilityItemsDto
                    {
                        TemplateOperationId = input.Id,
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
                        ToBeUsed = item.ToBeUsed,
                        UnsuitabilityItemsId = item.UnsuitabilityItemsId
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.TemplateOperationUnsuitabilityItems).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectTemplateOperationUnsuitabilityItemsDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.TemplateOperationUnsuitabilityItems).Update(new UpdateTemplateOperationUnsuitabilityItemsDto
                        {
                            TemplateOperationId = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ToBeUsed = item.ToBeUsed,
                            UnsuitabilityItemsId = item.UnsuitabilityItemsId
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            #endregion

            var templateOperation = queryFactory.Update<SelectTemplateOperationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.TemplateOperations, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["TempOperationsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperation);

        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.TemplateOperations).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<TemplateOperations>(entityQuery);

            var query = queryFactory.Query().From(Tables.TemplateOperations).Update(new UpdateTemplateOperationsDto
            {
                WorkCenterID = entity.WorkCenterID,
                IsCanBeDetected = entity.IsCanBeDetected,
                IsCauseExtraCost = entity.IsCauseExtraCost,
                IsHighRepairCost = entity.IsHighRepairCost,
                IsLongWorktimeforOperator = entity.IsLongWorktimeforOperator,
                IsPhysicallyHard = entity.IsPhysicallyHard,
                IsRequiresKnowledge = entity.IsRequiresKnowledge,
                IsRequiresSkill = entity.IsRequiresSkill,
                IsRiskyforOperator = entity.IsRiskyforOperator,
                IsSensitive = entity.IsSensitive,
                WorkScore = entity.WorkScore,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var templateOperationsDto = queryFactory.Update<SelectTemplateOperationsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectTemplateOperationsDto>(templateOperationsDto);

        }

        public async Task<IDataResult<IList<SelectTemplateOperationUnsuitabilityItemsDto>>> GetUnsuitabilityItemsAsync(Guid workCenterId, Guid templateOperationId)
        {
            List<SelectTemplateOperationUnsuitabilityItemsDto> list = new List<SelectTemplateOperationUnsuitabilityItemsDto>();

            if (templateOperationId != Guid.Empty)
            {
                var queryUnsuitabilityItems = queryFactory
                       .Query()
                       .From(Tables.TemplateOperationUnsuitabilityItems)
                       .Select<TemplateOperationUnsuitabilityItems>(tol => new { tol.LineNr, tol.Id, tol.DataOpenStatus, tol.DataOpenStatusUserId, tol.TemplateOperationId, tol.ToBeUsed })
                       .Join<UnsuitabilityItems>
                       (
                           s => new { UnsuitabilityItemsId = s.Id, UnsuitabilityItemsName = s.Name },
                           nameof(TemplateOperationUnsuitabilityItems.UnsuitabilityItemsId),
                           nameof(UnsuitabilityItems.Id),
                           JoinType.Left
                       )
                       .Where(new { TemplateOperationId = templateOperationId }, Tables.TemplateOperationUnsuitabilityItems);

                var unsuitabilityItemsLine = queryFactory.GetList<SelectTemplateOperationUnsuitabilityItemsDto>(queryUnsuitabilityItems).ToList();


                var unsuitabilityItemsQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { StationGroupId = workCenterId }, "");

                var unsuitabilityItemsList = queryFactory.GetList<SelectUnsuitabilityItemsDto>(unsuitabilityItemsQuery).ToList();

                var lineNr = unsuitabilityItemsLine.Max(s => s.LineNr);

                foreach (var item in unsuitabilityItemsList)
                {
                    if (!unsuitabilityItemsLine.Any(t => t.UnsuitabilityItemsId == item.Id))
                    {
                        unsuitabilityItemsLine.Add(new SelectTemplateOperationUnsuitabilityItemsDto
                        {
                            CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = Guid.Empty,
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            LineNr = lineNr,
                            ToBeUsed = false,
                            UnsuitabilityItemsId = item.Id,
                            UnsuitabilityItemsName = item.Name
                        });

                        lineNr++;
                    }
                }

                list = unsuitabilityItemsLine;
            }
            else
            {
                var unsuitabilityItemsQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select<UnsuitabilityItems>(s => new {s.Id,s.Code,s.Name}).Where(new { StationGroupId = workCenterId },"");

                var unsuitabilityItemsList = queryFactory.GetList<SelectUnsuitabilityItemsDto>(unsuitabilityItemsQuery).ToList();

                int lineNr = 1;

                foreach (var item in unsuitabilityItemsList)
                {
                    list.Add(new SelectTemplateOperationUnsuitabilityItemsDto
                    {
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = Guid.Empty,
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = lineNr,
                        ToBeUsed = false,
                        UnsuitabilityItemsId = item.Id,
                        UnsuitabilityItemsName = item.Name
                    });

                    lineNr++;
                }
            }

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectTemplateOperationUnsuitabilityItemsDto>>(list);

        }
    }
}
