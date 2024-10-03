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
using TsiErp.Business.Entities.PlannedMaintenance.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PlannedMaintenances.Page;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    [ServiceRegistration(typeof(IPlannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class PlannedMaintenancesAppService : ApplicationService<PlannedMaintenancesResource>, IPlannedMaintenancesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PlannedMaintenancesAppService(IStringLocalizer<PlannedMaintenancesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreatePlannedMaintenanceValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> CreateAsync(CreatePlannedMaintenancesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Select("RegistrationNo").Where(new { RegistrationNo = input.RegistrationNo }, "");
            var list = queryFactory.ControlList<PlannedMaintenances>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Insert(new CreatePlannedMaintenancesDto
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
                PlannedDate = input.PlannedDate,
                PlannedTime = input.PlannedTime,
                RemainingTime = input.RemainingTime,
                StartDate = input.StartDate,
                Status = input.Status,
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

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Insert(new CreatePlannedMaintenanceLinesDto
                {
                    Amount = item.Amount,
                    InstructionDescription = item.InstructionDescription,
                    MaintenanceNote = item.MaintenanceNote,
                    PlannedMaintenanceID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var maintenance = queryFactory.Insert<SelectPlannedMaintenancesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PlannedMainChildMenu", input.RegistrationNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PlannedMainChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Select("*").Where(new { Id = id }, "");

            var maintenances = queryFactory.Get<SelectPlannedMaintenancesDto>(query);

            if (maintenances.Id != Guid.Empty && maintenances != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { PlannedMaintenanceID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var maintenance = queryFactory.Update<SelectPlannedMaintenancesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PlannedMainChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains("*Not*"))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                     
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
                return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var maintenanceLines = queryFactory.Update<SelectPlannedMaintenanceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PlannedMaintenanceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPlannedMaintenanceLinesDto>(maintenanceLines);
            }
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenances)
                   .Select<PlannedMaintenances>(null)
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PlannedMaintenances);

            var maintenances = queryFactory.Get<SelectPlannedMaintenancesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenanceLines)
                   .Select<PlannedMaintenanceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PlannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PlannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { PlannedMaintenanceID = id }, Tables.PlannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectPlannedMaintenanceLinesDto>(queryLines).ToList();

            maintenances.SelectPlannedMaintenanceLines = maintenanceLine;

            LogsAppService.InsertLogToDatabase(maintenances, maintenances, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenances);
        }

        public async Task<IDataResult<IList<ListPlannedMaintenancesDto>>> GetListAsync(ListPlannedMaintenancesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PlannedMaintenances)
                   .Select<PlannedMaintenances>(null)
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PlannedMaintenances);

            var maintenances = queryFactory.GetList<ListPlannedMaintenancesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPlannedMaintenancesDto>>(maintenances);
        }

        [ValidationAspect(typeof(UpdatePlannedMaintenanceValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateAsync(UpdatePlannedMaintenancesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.PlannedMaintenances)
                   .Select("*")
                   .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(PlannedMaintenances.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<MaintenancePeriods>
                    (
                        mp => new { PeriodID = mp.Id, PeriodName = mp.Name },
                        nameof(PlannedMaintenances.PeriodID),
                        nameof(MaintenancePeriods.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PlannedMaintenances);

            var entity = queryFactory.Get<SelectPlannedMaintenancesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PlannedMaintenanceLines)
                   .Select<PlannedMaintenanceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PlannedMaintenanceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PlannedMaintenanceLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { PlannedMaintenanceID = input.Id }, Tables.PlannedMaintenanceLines);

            var maintenanceLine = queryFactory.GetList<SelectPlannedMaintenanceLinesDto>(queryLines).ToList();

            entity.SelectPlannedMaintenanceLines = maintenanceLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PlannedMaintenances)
                           .Select<PlannedMaintenances>(pm => new { pm.Status, pm.StationID, pm.StartDate, pm.RemainingTime, pm.RegistrationNo, pm.PlannedTime, pm.PlannedDate, pm.PeriodTime, pm.PeriodID, pm.OccuredTime, pm.NumberofCaregivers, pm.Note_, pm.Id, pm.DataOpenStatusUserId, pm.DataOpenStatus, pm.CompletionDate, pm.Caregiver })
                           .Join<Stations>
                            (
                                s => new { StationCode = s.Code },
                                nameof(PlannedMaintenances.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Join<MaintenancePeriods>
                            (
                                mp => new { PeriodName = mp.Name },
                                nameof(PlannedMaintenances.PeriodID),
                                nameof(MaintenancePeriods.Id),
                                JoinType.Left
                            )
                            .Where(new { RegistrationNo = input.RegistrationNo }, Tables.PlannedMaintenances);

            var list = queryFactory.GetList<ListPlannedMaintenancesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.RegistrationNo != input.RegistrationNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Update(new UpdatePlannedMaintenancesDto
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
                PlannedDate = input.PlannedDate,
                PlannedTime = input.PlannedTime,
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
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },"");

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Insert(new CreatePlannedMaintenanceLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        MaintenanceNote = item.MaintenanceNote,
                        PlannedMaintenanceID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPlannedMaintenanceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PlannedMaintenanceLines).Update(new UpdatePlannedMaintenanceLinesDto
                        {
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
                            MaintenanceNote = item.MaintenanceNote,
                            PlannedMaintenanceID = input.Id,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var maintenance = queryFactory.Update<SelectPlannedMaintenancesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PlannedMaintenances, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PlannedMainChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenance);
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PlannedMaintenances).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PlannedMaintenances>(entityQuery);

            var query = queryFactory.Query().From(Tables.PlannedMaintenances).Update(new UpdatePlannedMaintenancesDto
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
                PlannedDate = entity.PlannedDate,
                PlannedTime = entity.PlannedTime,
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

            var maintenancesDto = queryFactory.Update<SelectPlannedMaintenancesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(maintenancesDto);
        }
    }
}
