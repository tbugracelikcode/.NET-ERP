﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ContractProductionTracking.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractProductionTrackings.Page;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    [ServiceRegistration(typeof(IContractProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ContractProductionTrackingsAppService : ApplicationService<ContractProductionTrackingsResource>, IContractProductionTrackingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ContractProductionTrackingsAppService(IStringLocalizer<ContractProductionTrackingsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateContractProductionTrackingsValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> CreateAsync(CreateContractProductionTrackingsDto input)
        {
                Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Insert(new CreateContractProductionTrackingsDto
                {
                    Code = input.Code,
                    CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                    EmployeeID = input.EmployeeID.GetValueOrDefault(),
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID.GetValueOrDefault(),
                    ShiftID = input.ShiftID.GetValueOrDefault(),
                    StationID = input.StationID.GetValueOrDefault(),
                    WorkOrderID = input.WorkOrderID,
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

                var contractProductionTrackings = queryFactory.Insert<SelectContractProductionTrackingsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ContractProdTrackingsChildMenu", input.Code);


            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractProdTrackingsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractProdTrackingsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> GetAsync(Guid id)
        {
                var query = queryFactory
                        .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(null)
                            .Join<WorkOrders>
                            (
                                w => new { WorkOrderCode = w.WorkOrderNo, WorkOrderID = w.Id },
                                nameof(ContractProductionTrackings.WorkOrderID),
                                nameof(WorkOrders.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                                nameof(ContractProductionTrackings.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<Stations>
                            (
                                s => new { StationCode = s.Code, StationID = s.Id },
                                nameof(ContractProductionTrackings.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                              .Join<Shifts>
                            (
                                sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                                nameof(ContractProductionTrackings.ShiftID),
                                nameof(Shifts.Id),
                                JoinType.Left
                            )
                             .Join<Employees>
                            (
                                e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                                nameof(ContractProductionTrackings.EmployeeID),
                                nameof(Employees.Id),
                                JoinType.Left
                            )
                             .Join<CurrentAccountCards>
                            (
                                c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                                nameof(ContractProductionTrackings.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, Tables.ContractProductionTrackings);

                var contractProductionTracking = queryFactory.Get<SelectContractProductionTrackingsDto>(query);

                LogsAppService.InsertLogToDatabase(contractProductionTracking, contractProductionTracking, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTracking);

        }

        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
                var query = queryFactory
                       .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(s => new { s.Code, s.OperationStartDate, s.OperationEndDate, s.ProducedQuantity, s.OperationTime, s.PlannedQuantity, s.ShiftCode, s.IsFinished, s.Id })
                           .Join<WorkOrders>
                           (
                               w => new { WorkOrderCode = w.WorkOrderNo, WorkOrderID = w.Id },
                               nameof(ContractProductionTrackings.WorkOrderID),
                               nameof(WorkOrders.Id),
                               JoinType.Left
                           )
                            .Join<Products>
                           (
                               p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                               nameof(ContractProductionTrackings.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
                            .Join<Stations>
                           (
                               s => new { StationCode = s.Code, StationID = s.Id },
                               nameof(ContractProductionTrackings.StationID),
                               nameof(Stations.Id),
                               JoinType.Left
                           )
                             .Join<Shifts>
                           (
                               sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                               nameof(ContractProductionTrackings.ShiftID),
                               nameof(Shifts.Id),
                               JoinType.Left
                           )
                            .Join<Employees>
                           (
                               e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                               nameof(ContractProductionTrackings.EmployeeID),
                               nameof(Employees.Id),
                               JoinType.Left
                           )
                            .Join<CurrentAccountCards>
                           (
                               c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                               nameof(ContractProductionTrackings.CurrentAccountCardID),
                               nameof(CurrentAccountCards.Id),
                               JoinType.Left
                           )
                           .Where(null, Tables.ContractProductionTrackings);


                var contractProductionTrackings = queryFactory.GetList<ListContractProductionTrackingsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContractProductionTrackingsDto>>(contractProductionTrackings);
            
        }

        [ValidationAspect(typeof(UpdateContractProductionTrackingsValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateAsync(UpdateContractProductionTrackingsDto input)
        {
                var entityQuery = queryFactory.Query().From(Tables.ContractProductionTrackings).Select("*").Where(new { Id = input.Id }, "");
                var entity = queryFactory.Get<ContractProductionTrackings>(entityQuery);


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Update(new UpdateContractProductionTrackingsDto
                {
                    CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                    EmployeeID = input.EmployeeID.GetValueOrDefault(),
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID.GetValueOrDefault(),
                    ShiftID = input.ShiftID.GetValueOrDefault(),
                    StationID = input.StationID.GetValueOrDefault(),
                    WorkOrderID = input.WorkOrderID,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime =now,
                    LastModifierId = LoginedUserService.UserId,
                     Code = entity.Code,
                }).Where(new { Id = input.Id }, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, contractProductionTrackings, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractProdTrackingsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            
        }

        public async Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId)
        {
                var query = queryFactory
                       .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(s => new { s.OperationStartDate, s.PlannedQuantity, s.ProducedQuantity, s.Id })
                           .Join<WorkOrders>
                           (
                               w => new { WorkOrderCode = w.WorkOrderNo, WorkOrderID = w.Id },
                               nameof(ContractProductionTrackings.WorkOrderID),
                               nameof(WorkOrders.Id),
                               JoinType.Left
                           )
                            .Join<Products>
                           (
                               p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                               nameof(ContractProductionTrackings.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
                            .Join<Stations>
                           (
                               s => new { StationCode = s.Code, StationID = s.Id },
                               nameof(ContractProductionTrackings.StationID),
                               nameof(Stations.Id),
                               JoinType.Left
                           )
                             .Join<Shifts>
                           (
                               sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                               nameof(ContractProductionTrackings.ShiftID),
                               nameof(Shifts.Id),
                               JoinType.Left
                           )
                            .Join<Employees>
                           (
                               e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                               nameof(ContractProductionTrackings.EmployeeID),
                               nameof(Employees.Id),
                               JoinType.Left
                           )
                            .Join<CurrentAccountCards>
                           (
                               c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                               nameof(ContractProductionTrackings.CurrentAccountCardID),
                               nameof(CurrentAccountCards.Id),
                               JoinType.Left
                           )
                           .Where(new { ProductID = productId }, Tables.ContractProductionTrackings);


                var contractProductionTrackings = queryFactory.GetList<SelectContractProductionTrackingsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectContractProductionTrackingsDto>>(contractProductionTrackings);
            
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
                var entityQuery = queryFactory.Query().From(Tables.ContractProductionTrackings).Select("*").Where(new { Id = id }, "");
                var entity = queryFactory.Get<ContractProductionTrackings>(entityQuery);

                var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Update(new UpdateContractProductionTrackingsDto
                {
                    CurrentAccountID = entity.CurrentAccountCardID,
                    EmployeeID = entity.EmployeeID,
                    IsFinished = entity.IsFinished,
                    OperationEndDate = entity.OperationEndDate,
                    OperationEndTime = entity.OperationEndTime,
                    OperationStartDate = entity.OperationStartDate,
                    OperationStartTime = entity.OperationStartTime,
                    OperationTime = entity.OperationTime,
                    PlannedQuantity = entity.PlannedQuantity,
                    ProducedQuantity = entity.ProducedQuantity,
                    ProductID = entity.ProductID,
                    ShiftID = entity.ShiftID,
                    StationID = entity.StationID,
                    WorkOrderID = entity.WorkOrderID,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,
                     Code = entity.Code,

                }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);

        }
    }
}
