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
using TsiErp.Business.Entities.MaintenanceInstruction.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenanceInstructions.Page;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    [ServiceRegistration(typeof(IMaintenanceInstructionsAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceInstructionsAppService : ApplicationService<MaintenanceInstructionsResource>, IMaintenanceInstructionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public MaintenanceInstructionsAppService(IStringLocalizer<MaintenanceInstructionsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> CreateAsync(CreateMaintenanceInstructionsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<MaintenanceInstructions>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Insert(new CreateMaintenanceInstructionsDto
            {
                InstructionName = input.InstructionName,
                Note_ = input.Note_,
                PeriodID = input.PeriodID,
                PeriodTime = input.PeriodTime,
                PlannedMaintenanceTime = input.PlannedMaintenanceTime,
                StationID = input.StationID.GetValueOrDefault(),
                Code = input.Code,
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
            });

            foreach (var item in input.SelectMaintenanceInstructionLines)
            {
                var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Insert(new CreateMaintenanceInstructionLinesDto
                {
                    Amount = item.Amount,
                    InstructionDescription = item.InstructionDescription,
                    InstructionID = addedEntityId,
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
                    ProductID = item.ProductID,
                    UnitSetID = item.UnitSetID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var maintenanceInstruction = queryFactory.Insert<SelectMaintenanceInstructionsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MainInstructionsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainInstructionsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("*").Where(new { Id = id }, "");

            var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

            if (maintenanceInstructions.Id != Guid.Empty && maintenanceInstructions != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Delete(LoginedUserService.UserId).Where(new { InstructionID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var maintenanceInstruction = queryFactory.Update<SelectMaintenanceInstructionsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainInstructionsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var maintenanceInstructionLines = queryFactory.Update<SelectMaintenanceInstructionLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceInstructionLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectMaintenanceInstructionLinesDto>(maintenanceInstructionLines);
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructions)
                   .Select<MaintenanceInstructions>(null)
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(MaintenanceInstructions.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                        nameof(MaintenanceInstructions.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.MaintenanceInstructions);

            var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructionLines)
                   .Select<MaintenanceInstructionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code },
                        nameof(MaintenanceInstructionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceInstructionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { InstructionID = id }, Tables.MaintenanceInstructionLines);

            var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

            maintenanceInstructions.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

            LogsAppService.InsertLogToDatabase(maintenanceInstructions, maintenanceInstructions, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructions);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceInstructionsDto>>> GetListAsync(ListMaintenanceInstructionsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructions)
                   .Select<MaintenanceInstructions>(s => new { s.Code, s.InstructionName, s.Id })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(MaintenanceInstructions.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                        nameof(MaintenanceInstructions.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.MaintenanceInstructions);

            var maintenanceInstructions = queryFactory.GetList<ListMaintenanceInstructionsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMaintenanceInstructionsDto>>(maintenanceInstructions);
        }

        [ValidationAspect(typeof(UpdateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateAsync(UpdateMaintenanceInstructionsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructions).Select("*")
                    .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(MaintenanceInstructions.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                        nameof(MaintenanceInstructions.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.MaintenanceInstructions);

            var entity = queryFactory.Get<SelectMaintenanceInstructionsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructionLines)
                   .Select<MaintenanceInstructionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code },
                        nameof(MaintenanceInstructionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceInstructionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { InstructionID = input.Id }, Tables.MaintenanceInstructionLines);

            var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

            entity.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.MaintenanceInstructions)
                           .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                           .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code },
                                nameof(MaintenanceInstructions.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Join<MaintenancePeriods>
                            (
                                mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                                nameof(MaintenanceInstructions.PeriodID),
                                nameof(MaintenancePeriods.Id),
                                JoinType.Left
                            )
                            .Where(new { Code = input.Code }, Tables.MaintenanceInstructions);

            var list = queryFactory.GetList<ListMaintenanceInstructionsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Update(new UpdateMaintenanceInstructionsDto
            {
                InstructionName = input.InstructionName,
                Note_ = input.Note_,
                PeriodID = input.PeriodID,
                PeriodTime = input.PeriodTime,
                PlannedMaintenanceTime = input.PlannedMaintenanceTime,
                StationID = input.StationID.GetValueOrDefault(),
                Code = input.Code,
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
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectMaintenanceInstructionLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Insert(new CreateMaintenanceInstructionLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        DeletionTime = null,
                        InstructionID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID,
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectMaintenanceInstructionLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Update(new UpdateMaintenanceInstructionLinesDto
                        {
                            InstructionID = input.Id,
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
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
                            ProductID = item.ProductID,
                            UnitSetID = item.UnitSetID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var maintenanceInstruction = queryFactory.Update<SelectMaintenanceInstructionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainInstructionsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructions)
                   .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(MaintenanceInstructions.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                        nameof(MaintenanceInstructions.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { StationID = stationID, PeriodID = periodID }, Tables.MaintenanceInstructions);

            var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceInstructionLines)
                   .Select<MaintenanceInstructionLines>(mil => new { mil.UnitSetID, mil.ProductID, mil.LineNr, mil.InstructionID, mil.InstructionDescription, mil.Id, mil.DataOpenStatusUserId, mil.DataOpenStatus, mil.Amount })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code },
                        nameof(MaintenanceInstructionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceInstructionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { InstructionID = maintenanceInstructions.Id }, Tables.MaintenanceInstructionLines);

            var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

            maintenanceInstructions.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

            LogsAppService.InsertLogToDatabase(maintenanceInstructions, maintenanceInstructions, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Get, maintenanceInstructions.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructions);
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<MaintenanceInstructions>(entityQuery);

            var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Update(new UpdateMaintenanceInstructionsDto
            {
                InstructionName = entity.InstructionName,
                Note_ = entity.Note_,
                PeriodID = entity.PeriodID,
                PeriodTime = entity.PeriodTime,
                PlannedMaintenanceTime = entity.PlannedMaintenanceTime,
                StationID = entity.StationID,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var maintenanceInstructionsDto = queryFactory.Update<SelectMaintenanceInstructionsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructionsDto);
        }
    }
}
