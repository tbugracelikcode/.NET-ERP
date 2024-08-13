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
using TsiErp.Business.Entities.UnplannedMaintenance.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnplannedMaintenances.Page;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    [ServiceRegistration(typeof(IUnplannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class UnplannedMaintenancesAppService : ApplicationService<UnplannedMaintenancesResource>, IUnplannedMaintenancesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public UnplannedMaintenancesAppService(IStringLocalizer<UnplannedMaintenancesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> CreateAsync(CreateUnplannedMaintenancesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("RegistrationNo").Where(new { RegistrationNo = input.RegistrationNo },  "");
            var list = queryFactory.ControlList<UnplannedMaintenances>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Insert(new CreateUnplannedMaintenancesDto
            {
                RegistrationNo = input.RegistrationNo,
                StationID = input.StationID.GetValueOrDefault(),
                CompletionDate = input.CompletionDate,
                Caregiver = input.Caregiver,
                Note_ = input.Note_,
                NumberofCaregivers = input.NumberofCaregivers,
                OccuredTime = input.OccuredTime,
                PeriodID = input.PeriodID.GetValueOrDefault(),
                PeriodTime = input.PeriodTime,
                UnplannedDate = input.UnplannedDate,
                UnplannedTime = input.UnplannedTime,
                RemainingTime = input.RemainingTime,
                StartDate = input.StartDate,
                Status = input.Status,
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

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Insert(new CreateUnplannedMaintenanceLinesDto
                {
                    Amount = item.Amount,
                    InstructionDescription = item.InstructionDescription,
                    MaintenanceNote = item.MaintenanceNote,
                    UnplannedMaintenanceID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var maintenance = queryFactory.Insert<SelectUnplannedMaintenancesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnplannedMainChildMenu", input.RegistrationNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Insert, maintenance.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnplannedMainChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("*").Where(new { Id = id }, "");

            var maintenances = queryFactory.Get<SelectUnplannedMaintenancesDto>(query);

            if (maintenances.Id != Guid.Empty && maintenances != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { UnplannedMaintenanceID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var maintenance = queryFactory.Update<SelectUnplannedMaintenancesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnplannedMainChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = string.Empty,
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
                                RecordNumber = string.Empty,
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
                return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var maintenanceLines = queryFactory.Update<SelectUnplannedMaintenanceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnplannedMaintenanceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectUnplannedMaintenanceLinesDto>(maintenanceLines);
            }
        }

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(null)
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.UnplannedMaintenances);

            var maintenances = queryFactory.Get<SelectUnplannedMaintenancesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.UnplannedMaintenanceLines)
                   .Select<UnplannedMaintenanceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(UnplannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(UnplannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { UnplannedMaintenanceID = id }, Tables.UnplannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectUnplannedMaintenanceLinesDto>(queryLines).ToList();

            maintenances.SelectUnplannedMaintenanceLines = maintenanceLine;

            LogsAppService.InsertLogToDatabase(maintenances, maintenances, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenances);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnplannedMaintenancesDto>>> GetListAsync(ListUnplannedMaintenancesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                 .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(s => new { s.RegistrationNo, s.Caregiver, s.StartDate, s.UnplannedDate, s.Id })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.UnplannedMaintenances);

            var maintenances = queryFactory.GetList<ListUnplannedMaintenancesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnplannedMaintenancesDto>>(maintenances);
        }

        [ValidationAspect(typeof(UpdateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateAsync(UpdateUnplannedMaintenancesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.UnplannedMaintenances)
                   .Select("*")
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id },Tables.UnplannedMaintenances);

            var entity = queryFactory.Get<SelectUnplannedMaintenancesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.UnplannedMaintenanceLines)
                   .Select<UnplannedMaintenanceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(UnplannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(UnplannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { UnplannedMaintenanceID = input.Id }, Tables.UnplannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectUnplannedMaintenanceLinesDto>(queryLines).ToList();

            entity.SelectUnplannedMaintenanceLines = maintenanceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.UnplannedMaintenances)
                   .Select<UnplannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.UnplannedDate, pm.UnplannedTime, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                   .Join<Stations>
                    (
                        s => new { StationCode = s.Code },
                        nameof(UnplannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodName = mp.Name },
                        nameof(UnplannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                            .Where(new { RegistrationNo = input.RegistrationNo }, Tables.UnplannedMaintenances);

            var list = queryFactory.GetList<ListUnplannedMaintenancesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.RegistrationNo != input.RegistrationNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Update(new UpdateUnplannedMaintenancesDto
            {
                RegistrationNo = input.RegistrationNo,
                StationID = input.StationID.GetValueOrDefault(),
                CompletionDate = input.CompletionDate,
                Caregiver = input.Caregiver,
                Note_ = input.Note_,
                NumberofCaregivers = input.NumberofCaregivers,
                OccuredTime = input.OccuredTime,
                PeriodID = input.PeriodID.GetValueOrDefault(),
                PeriodTime = input.PeriodTime,
                UnplannedDate = input.UnplannedDate,
                UnplannedTime = input.UnplannedTime,
                RemainingTime = input.RemainingTime,
                StartDate = input.StartDate,
                Status = input.Status,
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

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Insert(new CreateUnplannedMaintenanceLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        MaintenanceNote = item.MaintenanceNote,
                        UnplannedMaintenanceID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectUnplannedMaintenanceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.UnplannedMaintenanceLines).Update(new UpdateUnplannedMaintenanceLinesDto
                        {
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
                            MaintenanceNote = item.MaintenanceNote,
                            UnplannedMaintenanceID = input.Id,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var maintenance = queryFactory.Update<SelectUnplannedMaintenancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.UnplannedMaintenances, LogType.Update, maintenance.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnplannedMainChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenance);
        }

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnplannedMaintenances).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<UnplannedMaintenances>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnplannedMaintenances).Update(new UpdateUnplannedMaintenancesDto
            {
                RegistrationNo = entity.RegistrationNo,
                StationID = entity.StationID,
                CompletionDate = entity.CompletionDate,
                Caregiver = entity.Caregiver,
                Note_ = entity.Note_,
                NumberofCaregivers = entity.NumberofCaregivers,
                OccuredTime = entity.OccuredTime,
                PeriodID = entity.PeriodID,
                PeriodTime = entity.PeriodTime,
                UnplannedTime = entity.UnplannedTime,
                UnplannedDate = entity.UnplannedDate,
                RemainingTime = entity.RemainingTime,
                StartDate = entity.StartDate,
                Status = (int)entity.Status,
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

            var maintenancesDto = queryFactory.Update<SelectUnplannedMaintenancesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(maintenancesDto);
        }
    }
}
