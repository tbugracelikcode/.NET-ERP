using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.ProductionManagement.ContractTrackingFiche.Validations;
using TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractTrackingFiches.Page;

namespace TsiErp.Business.Entities.ContractTrackingFiche.Services
{
    [ServiceRegistration(typeof(IContractTrackingFichesAppService), DependencyInjectionType.Scoped)]

    public class ContractTrackingFichesAppService : ApplicationService<ContractTrackingFichesResource>, IContractTrackingFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IProductionTrackingsAppService ProductionTrackingsAppService { get; set; }

        private IWorkOrdersAppService WorkOrdersAppService { get; set; }
        private IOperationStockMovementsAppService OperationStockMovementsAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ContractTrackingFichesAppService(IStringLocalizer<ContractTrackingFichesResource> l, IFicheNumbersAppService ficheNumbersAppService, IProductionTrackingsAppService productionTrackingsAppService, IWorkOrdersAppService workOrdersAppService, IOperationStockMovementsAppService operationStockMovementsAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            ProductionTrackingsAppService = productionTrackingsAppService;
            WorkOrdersAppService = workOrdersAppService;
            OperationStockMovementsAppService = operationStockMovementsAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreateContractTrackingFichesValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractTrackingFichesDto>> CreateAsync(CreateContractTrackingFichesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Select("FicheNr").Where(new { FicheNr = input.FicheNr }, "");
            var list = queryFactory.ControlList<ContractTrackingFiches>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Insert(new CreateContractTrackingFichesDto
            {
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                ProductID = input.ProductID.GetValueOrDefault(),
                Id = addedEntityId,
                Amount_ = input.Amount_,
                Balance_ = input.Amount_ - input.OccuredAmount_,
                FicheNr = input.FicheNr,
                ContractQualityPlanID = input.ContractQualityPlanID.GetValueOrDefault(),
                QualityPlanCurrentAccountCardID = input.QualityPlanCurrentAccountCardID,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                Description_ = input.Description_,
                EstimatedDate_ = input.EstimatedDate_,
                FicheDate_ = input.FicheDate_,
                OccuredAmount_ = input.OccuredAmount_,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty
            });


            foreach (var item in input.SelectContractTrackingFicheLines)
            {
                #region Contract Tracking Fiche Lines
                var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Insert(new CreateContractTrackingFicheLinesDto
                {
                    StationID = item.StationID.GetValueOrDefault(),
                    ContractTrackingFicheID = addedEntityId,
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
                    IsSent = item.IsSent,
                    OperationID = item.OperationID.GetValueOrDefault(),
                    WorkOrderID = item.WorkOrderID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                #endregion

                #region Work Order

                if (input.SelectContractTrackingFicheLines.Count > item.LineNr)
                {


                    var workOrder = (await WorkOrdersAppService.GetAsync(item.WorkOrderID.GetValueOrDefault())).Data;

                    if (workOrder != null)
                    {

                        #region Operation Stock Movement
                        if (input.SelectContractTrackingFicheLines.Count - 1 == item.LineNr)
                        {
                            var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(workOrder.ProductionOrderID.GetValueOrDefault(), workOrder.ProductsOperationID.GetValueOrDefault())).Data;

                            if (operationStockMovement.Id == Guid.Empty)
                            {
                                var createOperationStockMovement = new CreateOperationStockMovementsDto
                                {
                                    Id = GuidGenerator.CreateGuid(),
                                    OperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                                    ProductionorderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                                    TotalAmount = input.Amount_
                                };

                                var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Insert(createOperationStockMovement).UseIsDelete(false);

                                query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql;
                            }
                            else
                            {

                                var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                                {
                                    Id = operationStockMovement.Id,
                                    OperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                                    ProductionorderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                                    TotalAmount = input.Amount_
                                };

                                var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, "").UseIsDelete(false);

                                query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                            }

                        }
                        #endregion


                        var operationDateTime = new DateTime(input.FicheDate_.Year, input.FicheDate_.Month, input.FicheDate_.Day, _GetSQLDateAppService.GetDateFromSQL().Hour, _GetSQLDateAppService.GetDateFromSQL().Minute, _GetSQLDateAppService.GetDateFromSQL().Second);

                        var updatedWorkOrder = new UpdateWorkOrdersDto
                        {
                            Id = workOrder.Id,
                            PlannedQuantity = workOrder.PlannedQuantity,
                            AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                            CreationTime = workOrder.CreationTime,
                            CreatorId = workOrder.CreatorId,
                            CurrentAccountCardID = workOrder.CurrentAccountCardID.GetValueOrDefault(),
                            DataOpenStatus = workOrder.DataOpenStatus.GetValueOrDefault(),
                            DataOpenStatusUserId = workOrder.DataOpenStatusUserId.GetValueOrDefault(),
                            DeleterId = workOrder.DeleterId.GetValueOrDefault(),
                            DeletionTime = workOrder.DeletionTime,
                            IsCancel = workOrder.IsCancel,
                            IsDeleted = workOrder.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = workOrder.LineNr,
                            LinkedWorkOrderID = workOrder.LinkedWorkOrderID.GetValueOrDefault(),
                            OccuredFinishDate = operationDateTime,
                            OccuredStartDate = operationDateTime,
                            OperationTime = 0,
                            ProductID = workOrder.ProductID.GetValueOrDefault(),
                            ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                            ProductsOperationID = item.OperationID.GetValueOrDefault(),
                            PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                            RouteID = workOrder.RouteID.GetValueOrDefault(),
                            StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                            StationID = workOrder.StationID.GetValueOrDefault(),
                            WorkOrderNo = workOrder.WorkOrderNo,
                            WorkOrderState = (int)WorkOrderStateEnum.Tamamlandi,
                            ProducedQuantity = input.Amount_
                        };

                        var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                        query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;
                    }
                }
                else
                {
                    var workOrder = (await WorkOrdersAppService.GetAsync(item.WorkOrderID.GetValueOrDefault())).Data;

                    if (workOrder != null)
                    {
                        var operationDateTime = new DateTime(input.FicheDate_.Year, input.FicheDate_.Month, input.FicheDate_.Day, _GetSQLDateAppService.GetDateFromSQL().Hour, _GetSQLDateAppService.GetDateFromSQL().Minute, _GetSQLDateAppService.GetDateFromSQL().Second);

                        var updatedWorkOrder = new UpdateWorkOrdersDto
                        {
                            Id = workOrder.Id,
                            PlannedQuantity = workOrder.PlannedQuantity,
                            AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                            CreationTime = workOrder.CreationTime,
                            CreatorId = workOrder.CreatorId,
                            CurrentAccountCardID = workOrder.CurrentAccountCardID.GetValueOrDefault(),
                            DataOpenStatus = workOrder.DataOpenStatus.GetValueOrDefault(),
                            DataOpenStatusUserId = workOrder.DataOpenStatusUserId.GetValueOrDefault(),
                            DeleterId = workOrder.DeleterId.GetValueOrDefault(),
                            DeletionTime = workOrder.DeletionTime,
                            IsCancel = workOrder.IsCancel,
                            IsDeleted = workOrder.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = workOrder.LineNr,
                            LinkedWorkOrderID = workOrder.LinkedWorkOrderID.GetValueOrDefault(),
                            OccuredFinishDate = null,
                            OccuredStartDate = operationDateTime,
                            OperationTime = 0,
                            ProductID = workOrder.ProductID.GetValueOrDefault(),
                            ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                            ProductsOperationID = item.OperationID.GetValueOrDefault(),
                            PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                            RouteID = workOrder.RouteID.GetValueOrDefault(),
                            StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                            StationID = workOrder.StationID.GetValueOrDefault(),
                            WorkOrderNo = workOrder.WorkOrderNo,
                            WorkOrderState = (int)WorkOrderStateEnum.FasonaGonderildi,
                            ProducedQuantity = 0
                        };

                        var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                        query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;
                    }
                }





                #endregion
            }

            foreach (var item in input.SelectContractTrackingFicheAmountEntryLines)
            {
                #region Contract Tracking Fiche Amount Entry Lines
                var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Insert(new CreateContractTrackingFicheAmountEntryLinesDto
                {
                    ContractTrackingFicheID = addedEntityId,
                    Amount_ = item.Amount_,
                    Date_ = item.Date_,
                    Description_ = item.Description_,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                #endregion

            }


            var ContractTrackingFiche = queryFactory.Insert<SelectContractTrackingFichesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ContractTrackingFichesChildMenu", input.FicheNr);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractTrackingFichesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNr,
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
                            RecordNumber = input.FicheNr,
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
            return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ContractTrackingFicheID", new List<string>
            {
                Tables.ContractUnsuitabilityReports
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var deleteQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Delete(LoginedUserService.UserId).Where(new { ContractTrackingFicheID = id }, "");

                var line2DeleteQuery = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Delete(LoginedUserService.UserId).Where(new { ContractTrackingFicheID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;
                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + line2DeleteQuery.Sql + " where " + line2DeleteQuery.WhereSentence;

                var ContractTrackingFiche = queryFactory.Update<SelectContractTrackingFichesDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractTrackingFichesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = entity.FicheNr,
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
                                RecordNumber = entity.FicheNr,
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
                return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);

            }
        }

        public async Task<IResult> DeleteLineAsync(Guid id)
        {


            var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
            var ContractTrackingFicheLines = queryFactory.Update<SelectContractTrackingFicheLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractTrackingFicheLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractTrackingFicheLinesDto>(ContractTrackingFicheLines);


        }

        public async Task<IResult> DeleteAmountEntryLine(Guid id)
        {


            var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
            var ContractTrackingFicheAmountEntryLines = queryFactory.Update<SelectContractTrackingFicheAmountEntryLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractTrackingFicheAmountEntryLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractTrackingFicheAmountEntryLinesDto>(ContractTrackingFicheAmountEntryLines);


        }

        public async Task<IDataResult<SelectContractTrackingFichesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ContractTrackingFiches)
                   .Select<ContractTrackingFiches>(null)
                   .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderNr = p.FicheNo },
                        nameof(ContractTrackingFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractTrackingFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { CurrentAccountCardID = to.Id, CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { QualityPlanCurrentAccountCardID = to.Id, QualityPlanCurrentAccountCardCode = to.Code, QualityPlanCurrentAccountCardName = to.Name, QualityPlanCustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.QualityPlanCurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "QualityPlanCurrentAccountCard",
                        JoinType.Left
                    )
                    .Join<ContractQualityPlans>
                    (
                        to => new { ContractQualityPlanID = to.Id, ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                        nameof(ContractTrackingFiches.ContractQualityPlanID),
                        nameof(ContractQualityPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.ContractTrackingFiches);

            var contractTrackingFiches = queryFactory.Get<SelectContractTrackingFichesDto>(query);

            #region Contract Tracking Fiche Lines
            var queryLines = queryFactory
                           .Query()
                           .From(Tables.ContractTrackingFicheLines)
                           .Select<ContractTrackingFicheLines>(null)
                           .Join<ProductsOperations>
                            (
                                s => new { OperationID = s.Id, OperationCode = s.Code, OperationName = s.Name },
                                nameof(ContractTrackingFicheLines.OperationID),
                                nameof(ProductsOperations.Id),
                                JoinType.Left
                            )
                            .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(ContractTrackingFicheLines.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                             .Join<WorkOrders>
                            (
                                s => new { WorkOrderID = s.Id, WorkOrderNr = s.WorkOrderNo },
                                nameof(ContractTrackingFicheLines.WorkOrderID),
                                nameof(WorkOrders.Id),
                                JoinType.Left
                            )
                            .Where(new { ContractTrackingFicheID = id }, Tables.ContractTrackingFicheLines);

            var ContractTrackingFicheLine = queryFactory.GetList<SelectContractTrackingFicheLinesDto>(queryLines).ToList();

            contractTrackingFiches.SelectContractTrackingFicheLines = ContractTrackingFicheLine;
            #endregion

            #region Contract Tracking Fiche Amount Entry Lines

            var queryAmountEntryLines = queryFactory
                         .Query()
                         .From(Tables.ContractTrackingFicheAmountEntryLines)
                         .Select("*")
                          .Where(new { ContractTrackingFicheID = id }, Tables.ContractTrackingFicheAmountEntryLines);

            var ContractTrackingFicheAmountEntryLine = queryFactory.GetList<SelectContractTrackingFicheAmountEntryLinesDto>(queryAmountEntryLines).ToList();

            contractTrackingFiches.SelectContractTrackingFicheAmountEntryLines = ContractTrackingFicheAmountEntryLine;

            #endregion

            LogsAppService.InsertLogToDatabase(contractTrackingFiches, contractTrackingFiches, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractTrackingFichesDto>(contractTrackingFiches);

        }

        public async Task<IDataResult<IList<ListContractTrackingFichesDto>>> GetListAsync(ListContractTrackingFichesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ContractTrackingFiches)
                   .Select<ContractTrackingFiches>(null)
                  .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderNr = p.FicheNo },
                        nameof(ContractTrackingFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractTrackingFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { QualityPlanCurrentAccountCardID = to.Id, QualityPlanCurrentAccountCardCode = to.Code, QualityPlanCurrentAccountCardName = to.Name, QualityPlanCustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.QualityPlanCurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "QualityPlanCurrentAccountCard",
                        JoinType.Left
                    )
                    .Join<ContractQualityPlans>
                    (
                        to => new { ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                        nameof(ContractTrackingFiches.ContractQualityPlanID),
                        nameof(ContractQualityPlans.Id),
                        JoinType.Left)
                    .Where(null, Tables.ContractTrackingFiches);

            var contractTrackingFiches = queryFactory.GetList<ListContractTrackingFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContractTrackingFichesDto>>(contractTrackingFiches);

        }

        public async Task<IDataResult<IList<ListContractTrackingFichesDto>>> GetListbyProductIDAsync(Guid productID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ContractTrackingFiches)
                   .Select<ContractTrackingFiches>(null)
                  .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderNr = p.FicheNo },
                        nameof(ContractTrackingFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractTrackingFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { QualityPlanCurrentAccountCardID = to.Id, QualityPlanCurrentAccountCardCode = to.Code, QualityPlanCurrentAccountCardName = to.Name, QualityPlanCustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.QualityPlanCurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "QualityPlanCurrentAccountCard",
                        JoinType.Left
                    )
                    .Join<ContractQualityPlans>
                    (
                        to => new { ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                        nameof(ContractTrackingFiches.ContractQualityPlanID),
                        nameof(ContractQualityPlans.Id),
            JoinType.Left)
                    .Where(new { ProductID = productID }, Tables.ContractTrackingFiches);

            var contractTrackingFiches = queryFactory.GetList<ListContractTrackingFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContractTrackingFichesDto>>(contractTrackingFiches);

        }

        public async Task<IDataResult<IList<SelectContractTrackingFicheLinesDto>>> GetLineListbyWorkOrderIDAsync(Guid workOrderID)
        {
            var queryLines = queryFactory
                            .Query()
                            .From(Tables.ContractTrackingFicheLines)
                            .Select<ContractTrackingFicheLines>(s => new { s.IsSent, s.Id })
                            .Join<ProductsOperations>
                             (
                                 s => new { OperationID = s.Id, OperationCode = s.Code, OperationName = s.Name },
                                 nameof(ContractTrackingFicheLines.OperationID),
                                 nameof(ProductsOperations.Id),
                                 JoinType.Left
                             )
                             .Join<Stations>
                             (
                                 s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                 nameof(ContractTrackingFicheLines.StationID),
                                 nameof(Stations.Id),
                                 JoinType.Left
                             )
                              .Join<WorkOrders>
                             (
                                 s => new { WorkOrderID = s.Id, WorkOrderNr = s.WorkOrderNo },
                                 nameof(ContractTrackingFicheLines.WorkOrderID),
                                 nameof(WorkOrders.Id),
                                 JoinType.Left
                             )
                             .Where(new { WorkOrderID = workOrderID }, Tables.ContractTrackingFicheLines);

            var ContractTrackingFicheLine = queryFactory.GetList<SelectContractTrackingFicheLinesDto>(queryLines).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectContractTrackingFicheLinesDto>>(ContractTrackingFicheLine);

        }

        [ValidationAspect(typeof(UpdateContractTrackingFichesValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractTrackingFichesDto>> UpdateAsync(UpdateContractTrackingFichesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                    .From(Tables.ContractTrackingFiches)
                   .Select<ContractTrackingFiches>(null)
                       .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderNr = p.FicheNo },
                        nameof(ContractTrackingFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { CurrentAccountCardID = to.Id, CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractTrackingFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { QualityPlanCurrentAccountCardID = to.Id, QualityPlanCurrentAccountCardCode = to.Code, QualityPlanCurrentAccountCardName = to.Name, QualityPlanCustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.QualityPlanCurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "QualityPlanCurrentAccountCard",
                        JoinType.Left
                    )
                    .Join<ContractQualityPlans>
                    (
                        to => new { ContractQualityPlanID = to.Id, ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                        nameof(ContractTrackingFiches.ContractQualityPlanID),
                        nameof(ContractQualityPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.ContractTrackingFiches);

            var entity = queryFactory.Get<SelectContractTrackingFichesDto>(entityQuery);

            #region Contract Tracking Fiche Lines
            var queryLines = queryFactory
                           .Query()
                           .From(Tables.ContractTrackingFicheLines)
                            .Select<ContractTrackingFicheLines>(null)
                           .Join<ProductsOperations>
                            (
                                s => new { OperationID = s.Id, OperationCode = s.Code, OperationName = s.Name },
                                nameof(ContractTrackingFicheLines.OperationID),
                                nameof(ProductsOperations.Id),
                                JoinType.Left
                            )
                            .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(ContractTrackingFicheLines.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                             .Join<WorkOrders>
                            (
                                s => new { WorkOrderID = s.Id, WorkOrderNr = s.WorkOrderNo },
                                nameof(ContractTrackingFicheLines.WorkOrderID),
                                nameof(WorkOrders.Id),
                                JoinType.Left
                            )
                            .Where(new { ContractTrackingFicheID = input.Id }, Tables.ContractTrackingFicheLines);

            var ContractTrackingFicheLine = queryFactory.GetList<SelectContractTrackingFicheLinesDto>(queryLines).ToList();

            entity.SelectContractTrackingFicheLines = ContractTrackingFicheLine;
            #endregion

            #region Contract Tracking Fiche Amount Entry Lines

            var queryAmountEntryLines = queryFactory
                         .Query()
                         .From(Tables.ContractTrackingFicheAmountEntryLines)
                         .Select("*")
                          .Where(new { ContractTrackingFicheID = input.Id }, Tables.ContractTrackingFicheAmountEntryLines);

            var ContractTrackingFicheAmountEntryLine = queryFactory.GetList<SelectContractTrackingFicheAmountEntryLinesDto>(queryAmountEntryLines).ToList();

            entity.SelectContractTrackingFicheAmountEntryLines = ContractTrackingFicheAmountEntryLine;

            #endregion

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.ContractTrackingFiches)
                     .Select("*")
                  .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderNr = p.FicheNo },
                        nameof(ContractTrackingFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractTrackingFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { CurrentAccountCardCode = to.Code, CurrentAccountCardName = to.Name, CustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        to => new { QualityPlanCurrentAccountCardID = to.Id, QualityPlanCurrentAccountCardCode = to.Code, QualityPlanCurrentAccountCardName = to.Name, QualityPlanCustomerCode = to.CustomerCode },
                        nameof(ContractTrackingFiches.QualityPlanCurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "QualityPlanCurrentAccountCard",
                        JoinType.Left
                    )
                    .Join<ContractQualityPlans>
                    (
                        to => new { ContractQualityPlanDocumentNumber = to.DocumentNumber, ContractQualityPlanDescription = to.Description_ },
                        nameof(ContractTrackingFiches.ContractQualityPlanID),
                        nameof(ContractQualityPlans.Id),
                        JoinType.Left)
                            .Where(new { FicheNr = input.FicheNr }, Tables.ContractTrackingFiches);

            var list = queryFactory.GetList<ListContractTrackingFichesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNr != input.FicheNr)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Update(new UpdateContractTrackingFichesDto
            {
                Amount_ = input.Amount_,
                FicheNr = input.FicheNr,
                ContractQualityPlanID = input.ContractQualityPlanID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                QualityPlanCurrentAccountCardID = input.QualityPlanCurrentAccountCardID,
                Description_ = input.Description_,
                EstimatedDate_ = input.EstimatedDate_,
                Balance_ = input.Amount_ - input.OccuredAmount_,
                ProductID = input.ProductID,
                FicheDate_ = input.FicheDate_,
                OccuredAmount_ = input.OccuredAmount_,
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
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
            }).Where(new { Id = input.Id }, "");

            #region Contract Tracking Fiche Lines
            foreach (var item in input.SelectContractTrackingFicheLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Insert(new CreateContractTrackingFicheLinesDto
                    {
                        IsSent = item.IsSent,
                        OperationID = item.OperationID,
                        WorkOrderID = item.WorkOrderID,
                        StationID = item.StationID,
                        ContractTrackingFicheID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectContractTrackingFicheLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ContractTrackingFicheLines).Update(new UpdateContractTrackingFicheLinesDto
                        {
                            IsSent = item.IsSent,
                            OperationID = item.OperationID,
                            WorkOrderID = item.WorkOrderID,
                            StationID = item.StationID,
                            ContractTrackingFicheID = input.Id,
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
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }
            #endregion

            #region Contract Tracking Fiche Amount Entry lines


            foreach (var item in input.SelectContractTrackingFicheAmountEntryLines)
            {

                if (item.Id == Guid.Empty)
                {
                    var queryAmountEntryLine = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Insert(new CreateContractTrackingFicheAmountEntryLinesDto
                    {
                        Description_ = item.Description_,
                        Date_ = item.Date_,
                        Amount_ = item.Amount_,
                        ContractTrackingFicheID = input.Id,
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

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryAmountEntryLine.Sql;
                }
                else
                {
                    var lineAmountEntryGetQuery = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectContractTrackingFicheAmountEntryLinesDto>(lineAmountEntryGetQuery);

                    if (line != null)
                    {
                        var queryAmountEntryLine = queryFactory.Query().From(Tables.ContractTrackingFicheAmountEntryLines).Update(new UpdateContractTrackingFicheAmountEntryLinesDto
                        {
                            Amount_ = item.Amount_,
                            Date_ = item.Date_,
                            Description_ = item.Description_,
                            ContractTrackingFicheID = input.Id,
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
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryAmountEntryLine.Sql + " where " + queryAmountEntryLine.WhereSentence;
                    }
                }

            }

            #endregion


            var ContractTrackingFiche = queryFactory.Update<SelectContractTrackingFichesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ContractTrackingFiches, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractTrackingFichesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNr,
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
                            RecordNumber = input.FicheNr,
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
            return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFiche);

        }

        public async Task<IDataResult<SelectContractTrackingFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ContractTrackingFiches).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ContractTrackingFiches>(entityQuery);

            var query = queryFactory.Query().From(Tables.ContractTrackingFiches).Update(new UpdateContractTrackingFichesDto
            {
                Amount_ = entity.Amount_,
                FicheNr = entity.FicheNr,
                ContractQualityPlanID = entity.ContractQualityPlanID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Description_ = entity.Description_,
                QualityPlanCurrentAccountCardID = entity.QualityPlanCurrentAccountCardID,
                EstimatedDate_ = entity.EstimatedDate_,
                Balance_ = entity.Balance_,
                ProductID = entity.ProductID,
                FicheDate_ = entity.FicheDate_,
                OccuredAmount_ = entity.OccuredAmount_,
                ProductionOrderID = entity.ProductionOrderID,
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

            var ContractTrackingFichesDto = queryFactory.Update<SelectContractTrackingFichesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractTrackingFichesDto>(ContractTrackingFichesDto);

        }
    }
}
