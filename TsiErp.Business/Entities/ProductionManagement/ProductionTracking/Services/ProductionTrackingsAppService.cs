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
using TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionTrackings.Page;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService<ProductionTrackingsResource>, IProductionTrackingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        private IOperationStockMovementsAppService OperationStockMovementsAppService { get; set; }

        private IWorkOrdersAppService WorkOrdersAppService { get; set; }

        private IProductionOrdersAppService ProductionOrdersAppService { get; set; }


        public ProductionTrackingsAppService(IStringLocalizer<ProductionTrackingsResource> l, IFicheNumbersAppService ficheNumbersAppService, IOperationStockMovementsAppService operationStockMovementsAppService, IWorkOrdersAppService workOrdersAppService, IProductionOrdersAppService productionOrdersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            OperationStockMovementsAppService = operationStockMovementsAppService;
            WorkOrdersAppService = workOrdersAppService;
            ProductionOrdersAppService = productionOrdersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            var productionTrackings = (await GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.WorkOrderID == input.WorkOrderID && t.Id != input.Id).ToList();

            var workOrder = (await WorkOrdersAppService.GetAsync(input.WorkOrderID.GetValueOrDefault())).Data;

            var workOrderList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID).ToList();

            var listQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<ProductionTrackings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();



            var query = queryFactory.Query().From(Tables.ProductionTrackings).Insert(new CreateProductionTrackingsDto
            {
                AdjustmentTime = input.AdjustmentTime,
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
                HaltTime = input.HaltTime,
                IsFinished = input.IsFinished,
                OperationEndDate = input.OperationEndDate,
                OperationEndTime = input.OperationEndTime,
                HaltReasonID = input.HaltReasonID.GetValueOrDefault(),
                ProductionTrackingTypes = input.ProductionTrackingTypes,
                OperationStartDate = input.OperationStartDate,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                OperationStartTime = input.OperationStartTime,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ShiftID = input.ShiftID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderID = input.WorkOrderID.GetValueOrDefault(),
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
                ProductID = input.ProductID,
                ProductionOrderID = input.ProductionOrderID,
                ProductsOperationID = input.ProductsOperationID,
                FaultyQuantity = input.FaultyQuantity

            });

            #region Operation Stock Movement And Work Order 

            if (workOrder != null)
            {

                var productionOrder = (await ProductionOrdersAppService.GetAsync(workOrder.ProductionOrderID.GetValueOrDefault())).Data;

                #region Previous Operation Stock Movement Update

                if (workOrder.LineNr > 1)
                {
                    var previousWorkOrderId = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID && t.LineNr == workOrder.LineNr - 1).Select(t => t.Id).FirstOrDefault();

                    var previousWorkOrder = (await WorkOrdersAppService.GetAsync(previousWorkOrderId)).Data;

                    var previousOperationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(previousWorkOrder.ProductionOrderID.GetValueOrDefault(), previousWorkOrder.ProductsOperationID.GetValueOrDefault())).Data;


                    if (previousOperationStockMovement.Id != Guid.Empty)
                    {
                        var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                        {
                            Id = previousOperationStockMovement.Id,
                            OperationID = previousWorkOrder.ProductsOperationID.GetValueOrDefault(),
                            ProductionorderID = previousWorkOrder.ProductionOrderID.GetValueOrDefault(),
                            TotalAmount = previousOperationStockMovement.TotalAmount - input.ProducedQuantity
                        };

                        var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, "").UseIsDelete(false);

                        query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                    }
                }

                #endregion

                #region Operation Stock Movement
                var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(workOrder.ProductionOrderID.GetValueOrDefault(), workOrder.ProductsOperationID.GetValueOrDefault())).Data;

                if (operationStockMovement.Id == Guid.Empty)
                {
                    var createOperationStockMovement = new CreateOperationStockMovementsDto
                    {
                        Id = GuidGenerator.CreateGuid(),
                        OperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        ProductionorderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        TotalAmount = input.ProducedQuantity
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
                        TotalAmount = operationStockMovement.TotalAmount + input.ProducedQuantity
                    };

                    var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, "").UseIsDelete(false);

                    query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                }
                #endregion

                #region Work Order

                if (productionTrackings.Count > 0)
                {
                    if (workOrder.PlannedQuantity == (productionTrackings.Sum(t => t.ProducedQuantity) + input.ProducedQuantity))
                    {
                        workOrder.WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        workOrder.OccuredFinishDate = new DateTime(input.OperationEndDate.GetValueOrDefault().Year, input.OperationEndDate.GetValueOrDefault().Month, input.OperationEndDate.GetValueOrDefault().Day, input.OperationEndTime.Value.Hours, input.OperationEndTime.Value.Minutes, input.OperationEndTime.Value.Seconds);

                        workOrderList.Where(t => t.Id == workOrder.Id).FirstOrDefault().WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        int productionOrderStateCount = workOrderList.Where(t => t.WorkOrderState != WorkOrderStateEnum.Tamamlandi).Count();

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = productionOrderStateCount == 0 ? (int)ProductionOrderStateEnum.Tamamlandi : (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                    else if (workOrder.PlannedQuantity > (productionTrackings.Sum(t => t.ProducedQuantity) + input.ProducedQuantity))
                    {

                        workOrder.WorkOrderState = WorkOrderStateEnum.DevamEdiyor;

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                }
                else
                {
                    if (workOrder.PlannedQuantity == input.ProducedQuantity)
                    {
                        workOrder.WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        workOrder.OccuredStartDate = new DateTime(input.OperationStartDate.Year, input.OperationStartDate.Month, input.OperationStartDate.Day, input.OperationStartTime.Value.Hours, input.OperationStartTime.Value.Minutes, input.OperationStartTime.Value.Seconds);

                        workOrder.OccuredFinishDate = new DateTime(input.OperationEndDate.GetValueOrDefault().Year, input.OperationEndDate.GetValueOrDefault().Month, input.OperationEndDate.GetValueOrDefault().Day, input.OperationEndTime.Value.Hours, input.OperationEndTime.Value.Minutes, input.OperationEndTime.Value.Seconds);

                        workOrderList.Where(t => t.Id == workOrder.Id).FirstOrDefault().WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        int productionOrderStateCount = workOrderList.Where(t => t.WorkOrderState != WorkOrderStateEnum.Tamamlandi).Count();

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = productionOrderStateCount == 0 ? (int)ProductionOrderStateEnum.Tamamlandi : (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                    else if (workOrder.PlannedQuantity > input.ProducedQuantity)
                    {

                        workOrder.WorkOrderState = WorkOrderStateEnum.DevamEdiyor;

                        workOrder.OccuredStartDate = new DateTime(input.OperationStartDate.Year, input.OperationStartDate.Month, input.OperationStartDate.Day, input.OperationStartTime.Value.Hours, input.OperationStartTime.Value.Minutes, input.OperationStartTime.Value.Seconds);

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                BranchID = productionOrder.BranchID,
                                WarehouseID = productionOrder.WarehouseID,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                }

                var updatedWorkOrder = new UpdateWorkOrdersDto
                {
                    Id = workOrder.Id,
                    PlannedQuantity = workOrder.PlannedQuantity,
                    AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                    CreationTime = workOrder.CreationTime,
                    CreatorId = workOrder.CreatorId,
                    CurrentAccountCardID = workOrder.CurrentAccountCardID,
                    DataOpenStatus = workOrder.DataOpenStatus.GetValueOrDefault(),
                    DataOpenStatusUserId = workOrder.DataOpenStatusUserId.GetValueOrDefault(),
                    DeleterId = workOrder.DeleterId.GetValueOrDefault(),
                    DeletionTime = workOrder.DeletionTime,
                    IsCancel = workOrder.IsCancel,
                    IsDeleted = workOrder.IsDeleted,
                    LastModificationTime = now,
                    LastModifierId = LoginedUserService.UserId,
                    LineNr = workOrder.LineNr,
                    LinkedWorkOrderID = workOrder.LinkedWorkOrderID.GetValueOrDefault(),
                    OccuredFinishDate = workOrder.OccuredFinishDate,
                    OccuredStartDate = workOrder.OccuredStartDate,
                    OperationTime = workOrder.OperationTime,
                    ProductID = workOrder.ProductID.GetValueOrDefault(),
                    ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                    ProductsOperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                    PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                    RouteID = workOrder.RouteID.GetValueOrDefault(),
                    StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                    StationID = workOrder.StationID.GetValueOrDefault(),
                    WorkOrderNo = workOrder.WorkOrderNo,
                    WorkOrderState = (int)workOrder.WorkOrderState,
                    ProducedQuantity = workOrder.ProducedQuantity + input.ProducedQuantity,
                    OrderID = workOrder.OrderID,
                    SplitQuantity = workOrder.SplitQuantity,
                    IsContractUnsuitabilityWorkOrder = workOrder.IsContractUnsuitabilityWorkOrder,
                    IsOperationUnsuitabilityWorkOrder = workOrder.IsOperationUnsuitabilityWorkOrder
                };

                var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                #endregion


            }

            #endregion

            var productionTracking = queryFactory.Insert<SelectProductionTrackingsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProdTrackingsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdTrackingsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var productionTrackings = (await GetAsync(id)).Data;


            var deleteQuery = queryFactory.Query().From(Tables.ProductionTrackings).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");


            var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(deleteQuery, "Id", true);

            var workOrder = (await WorkOrdersAppService.GetAsync(productionTrackings.WorkOrderID)).Data;

            var prudctionTrackingList = (await GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.WorkOrderID == workOrder.Id && t.Id != id).ToList();

            var productionOrder = (await ProductionOrdersAppService.GetAsync(workOrder.ProductionOrderID.GetValueOrDefault())).Data;

            #region Operation Stock Movement And Work Order

            if (workOrder != null)
            {

                #region Operation Stock Movement

                var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(workOrder.ProductionOrderID.GetValueOrDefault(), workOrder.ProductsOperationID.GetValueOrDefault())).Data;

                if (operationStockMovement.Id != Guid.Empty)
                {
                    var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                    {
                        Id = operationStockMovement.Id,
                        OperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        ProductionorderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        TotalAmount = operationStockMovement.TotalAmount - productionTrackings.ProducedQuantity
                    };

                    var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, "").UseIsDelete(false);

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence;
                }

                #endregion

                #region Previous Operation Stock Movement Update

                if (workOrder.LineNr > 1)
                {
                    var previousWorkOrderId = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID && t.LineNr == workOrder.LineNr - 1).Select(t => t.Id).FirstOrDefault();

                    var previousWorkOrder = (await WorkOrdersAppService.GetAsync(previousWorkOrderId)).Data;

                    var previousOperationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(previousWorkOrder.ProductionOrderID.GetValueOrDefault(), previousWorkOrder.ProductsOperationID.GetValueOrDefault())).Data;


                    if (previousOperationStockMovement.Id != Guid.Empty)
                    {
                        var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                        {
                            Id = previousOperationStockMovement.Id,
                            OperationID = previousWorkOrder.ProductsOperationID.GetValueOrDefault(),
                            ProductionorderID = previousWorkOrder.ProductionOrderID.GetValueOrDefault(),
                            TotalAmount = previousOperationStockMovement.TotalAmount + productionTrackings.ProducedQuantity
                        };

                        var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, "").UseIsDelete(false);

                        deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence;
                    }
                }

                #endregion

                #region Work Order And Production Order
                if (prudctionTrackingList.Count == 0)
                {
                    var updatedWorkOrder = new UpdateWorkOrdersDto
                    {
                        Id = workOrder.Id,
                        PlannedQuantity = workOrder.PlannedQuantity,
                        AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                        CreationTime = workOrder.CreationTime,
                        CreatorId = workOrder.CreatorId,
                        CurrentAccountCardID = workOrder.CurrentAccountCardID,
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
                        OccuredStartDate = null,
                        OperationTime = workOrder.OperationTime,
                        ProductID = workOrder.ProductID.GetValueOrDefault(),
                        ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        ProductsOperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                        RouteID = workOrder.RouteID.GetValueOrDefault(),
                        StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                        StationID = workOrder.StationID.GetValueOrDefault(),
                        WorkOrderNo = workOrder.WorkOrderNo,
                        WorkOrderState = (int)WorkOrderStateEnum.Baslamadi,
                        ProducedQuantity = workOrder.ProducedQuantity - productionTrackings.ProducedQuantity
                    };

                    var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                    var updateProductionOrder = new UpdateProductionOrdersDto
                    {
                        BOMID = productionOrder.BOMID.GetValueOrDefault(),
                        Cancel_ = productionOrder.Cancel_,
                        CreationTime = productionOrder.CreationTime,
                        CreatorId = productionOrder.CreatorId,
                        CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                        CustomerOrderNo = productionOrder.CustomerOrderNo,
                        DataOpenStatus = productionOrder.DataOpenStatus,
                        DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                        Date_ = productionOrder.Date_,
                        DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                        DeletionTime = productionOrder.DeletionTime,
                        Description_ = productionOrder.Description_,
                        FicheNo = productionOrder.FicheNo,
                        FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                        Id = productionOrder.Id,
                        IsDeleted = productionOrder.IsDeleted,
                        LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        LastModifierId = LoginedUserService.UserId,
                        LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                        LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                        OrderID = productionOrder.OrderID.GetValueOrDefault(),
                        OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                        PlannedQuantity = productionOrder.PlannedQuantity,
                        ProducedQuantity = productionOrder.ProducedQuantity,
                        ProductionOrderState = (int)ProductionOrderStateEnum.Baslamadi,
                        ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                        ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                        PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                        PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                        RouteID = productionOrder.RouteID.GetValueOrDefault(),
                        UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                    };

                    var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                }
                else
                {
                    var updatedWorkOrder = new UpdateWorkOrdersDto
                    {
                        Id = workOrder.Id,
                        PlannedQuantity = workOrder.PlannedQuantity,
                        AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                        CreationTime = workOrder.CreationTime,
                        CreatorId = workOrder.CreatorId,
                        CurrentAccountCardID = workOrder.CurrentAccountCardID,
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
                        OccuredStartDate = workOrder.OccuredStartDate,
                        OperationTime = workOrder.OperationTime,
                        ProductID = workOrder.ProductID.GetValueOrDefault(),
                        ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        ProductsOperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                        RouteID = workOrder.RouteID.GetValueOrDefault(),
                        StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                        StationID = workOrder.StationID.GetValueOrDefault(),
                        WorkOrderNo = workOrder.WorkOrderNo,
                        WorkOrderState = (int)WorkOrderStateEnum.DevamEdiyor,
                        ProducedQuantity = workOrder.ProducedQuantity - productionTrackings.ProducedQuantity
                    };

                    var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                    var updateProductionOrder = new UpdateProductionOrdersDto
                    {
                        BOMID = productionOrder.BOMID.GetValueOrDefault(),
                        Cancel_ = productionOrder.Cancel_,
                        CreationTime = productionOrder.CreationTime,
                        CreatorId = productionOrder.CreatorId,
                        CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                        CustomerOrderNo = productionOrder.CustomerOrderNo,
                        DataOpenStatus = productionOrder.DataOpenStatus,
                        DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                        Date_ = productionOrder.Date_,
                        DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                        DeletionTime = productionOrder.DeletionTime,
                        Description_ = productionOrder.Description_,
                        FicheNo = productionOrder.FicheNo,
                        FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                        Id = productionOrder.Id,
                        IsDeleted = productionOrder.IsDeleted,
                        LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        LastModifierId = LoginedUserService.UserId,
                        LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                        LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                        OrderID = productionOrder.OrderID.GetValueOrDefault(),
                        OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                        PlannedQuantity = productionOrder.PlannedQuantity,
                        ProducedQuantity = productionOrder.ProducedQuantity,
                        ProductionOrderState = (int)ProductionOrderStateEnum.DevamEdiyor,
                        ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                        ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                        PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                        PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                        RouteID = productionOrder.RouteID.GetValueOrDefault(),
                        UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                    };

                    var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                }


                #endregion
            }

            #endregion

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Delete, id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdTrackingsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = productionTrackings.Code,
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
                            RecordNumber = productionTrackings.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);

        }





        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductionTrackings)
                   .Select<ProductionTrackings>(null)
                   .Join<WorkOrders>
                    (
                        wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                        nameof(ProductionTrackings.WorkOrderID),
                        nameof(WorkOrders.Id),
                        JoinType.Left
                    )
                     .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                     .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(ProductionTrackings.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<Shifts>
                    (
                        sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                        nameof(ProductionTrackings.ShiftID),
                        nameof(Shifts.Id),
                        JoinType.Left
                    )
                    .Join<Employees>
                    (
                        e => new { EmployeeID = e.Id, EmployeeName = e.Name, EmployeeSurname = e.Surname },
                        nameof(ProductionTrackings.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        e => new { ProductID = e.Id, ProductCode = e.Code },
                        nameof(ProductionTrackings.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        e => new { ProductionOrderID = e.Id, ProductionOrderCode = e.FicheNo },
                        nameof(ProductionTrackings.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        e => new { ProductsOperationID = e.Id, ProductOperationName = e.Name },
                        nameof(ProductionTrackings.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.ProductionTrackings);

            var productionTrackings = queryFactory.Get<SelectProductionTrackingsDto>(query);


            LogsAppService.InsertLogToDatabase(productionTrackings, productionTrackings, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackings);

        }

        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductionTrackings)
                   .Select<ProductionTrackings>(s => new { s.OperationStartDate, s.OperationEndDate, s.PlannedQuantity, s.OperationTime, s.HaltTime, s.AdjustmentTime, s.IsFinished, s.Id, s.ProducedQuantity, s.OperationStartTime, s.OperationEndTime, s.FaultyQuantity, s.Code, s.ProductionTrackingTypes })
                   .Join<WorkOrders>
                    (
                        wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                        nameof(ProductionTrackings.WorkOrderID),
                        nameof(WorkOrders.Id),
                        JoinType.Left
                    )
                      .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                     .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(ProductionTrackings.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<Shifts>
                    (
                        sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                        nameof(ProductionTrackings.ShiftID),
                        nameof(Shifts.Id),
                        JoinType.Left
                    )
                    .Join<Employees>
                    (
                        e => new { EmployeeID = e.Id, EmployeeName = e.Name, EmployeeSurname = e.Surname },
                        nameof(ProductionTrackings.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                      .Join<CurrentAccountCards>
                    (
                        e => new { CustomerCode = e.CustomerCode },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        e => new { ProductID = e.Id, ProductCode = e.Code },
                        nameof(ProductionTrackings.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        e => new { ProductionOrderID = e.Id, ProductionOrderCode = e.FicheNo },
                        nameof(ProductionTrackings.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        e => new { ProductsOperationID = e.Id, ProductOperationName = e.Name },
                        nameof(ProductionTrackings.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.ProductionTrackings);

            var productionTrackings = queryFactory.GetList<ListProductionTrackingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionTrackingsDto>>(productionTrackings);
        }

        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListbyWorkOrderIDAsync(Guid workOrderID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductionTrackings)
                   .Select<ProductionTrackings>(null)
                   .Join<WorkOrders>
                    (
                        wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                        nameof(ProductionTrackings.WorkOrderID),
                        nameof(WorkOrders.Id),
                        JoinType.Left
                    )
                      .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                     .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(ProductionTrackings.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<Shifts>
                    (
                        sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                        nameof(ProductionTrackings.ShiftID),
                        nameof(Shifts.Id),
                        JoinType.Left
                    )
                    .Join<Employees>
                    (
                        e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                        nameof(ProductionTrackings.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                      .Join<CurrentAccountCards>
                    (
                        e => new { CustomerCode = e.CustomerCode },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        e => new { ProductID = e.Id, ProductCode = e.Code },
                        nameof(ProductionTrackings.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        e => new { ProductionOrderID = e.Id, ProductionOrderCode = e.FicheNo },
                        nameof(ProductionTrackings.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        e => new { ProductsOperationID = e.Id, ProductOperationName = e.Name },
                        nameof(ProductionTrackings.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { WorkOrderID = workOrderID }, Tables.ProductionTrackings);

            var productionTrackings = queryFactory.GetList<ListProductionTrackingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionTrackingsDto>>(productionTrackings);
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {

            var productionTrackings = (await GetListAsync(new ListProductionTrackingsParameterDto())).Data.Where(t => t.WorkOrderID == input.WorkOrderID && t.Id != input.Id).ToList();

            var workOrder = (await WorkOrdersAppService.GetAsync(input.WorkOrderID.GetValueOrDefault())).Data;

            var workOrderList = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID).ToList();

            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.ProductionTrackings)
                   .Select<ProductionTrackings>(null)
                   .Join<WorkOrders>
                    (
                        wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                        nameof(ProductionTrackings.WorkOrderID),
                        nameof(WorkOrders.Id),
                        JoinType.Left
                    )
                      .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                     .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(ProductionTrackings.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<Shifts>
                    (
                        sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                        nameof(ProductionTrackings.ShiftID),
                        nameof(Shifts.Id),
                        JoinType.Left
                    )
                    .Join<Employees>
                    (
                        e => new { EmployeeID = e.Id, EmployeeName = e.Name, EmployeeSurname = e.Surname },
                        nameof(ProductionTrackings.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                      .Join<CurrentAccountCards>
                    (
                        e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        e => new { ProductID = e.Id, ProductCode = e.Code },
                        nameof(ProductionTrackings.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        e => new { ProductionOrderID = e.Id, ProductionOrderCode = e.FicheNo },
                        nameof(ProductionTrackings.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        e => new { ProductsOperationID = e.Id, ProductOperationName = e.Name },
                        nameof(ProductionTrackings.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.ProductionTrackings);

            var entity = queryFactory.Get<SelectProductionTrackingsDto>(entityQuery);

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.ProductionTrackings)
                           .Select<ProductionTrackings>(null)
                           .Join<WorkOrders>
                            (
                                wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                                nameof(ProductionTrackings.WorkOrderID),
                                nameof(WorkOrders.Id),
                                JoinType.Left
                            )
                              .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                             .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code },
                                nameof(ProductionTrackings.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Join<Shifts>
                            (
                                sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                                nameof(ProductionTrackings.ShiftID),
                                nameof(Shifts.Id),
                                JoinType.Left
                            )
                            .Join<Employees>
                            (
                                e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                                nameof(ProductionTrackings.EmployeeID),
                                nameof(Employees.Id),
                                JoinType.Left
                            )
                              .Join<CurrentAccountCards>
                    (
                        e => new { CustomerCode = e.CustomerCode },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.ProductionTrackings);

            var list = queryFactory.GetList<ListProductionTrackingsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion



            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
            {
                AdjustmentTime = input.AdjustmentTime,
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
                HaltTime = input.HaltTime,
                IsFinished = input.IsFinished,
                OperationEndDate = input.OperationEndDate,
                OperationEndTime = input.OperationEndTime,
                ProductionTrackingTypes = input.ProductionTrackingTypes,
                HaltReasonID = input.HaltReasonID.GetValueOrDefault(),
                OperationStartDate = input.OperationStartDate,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                OperationStartTime = input.OperationStartTime,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ShiftID = input.ShiftID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderID = input.WorkOrderID.GetValueOrDefault(),
                Code = input.Code,
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
                ProductID = input.ProductID,
                ProductionOrderID = input.ProductionOrderID,
                ProductsOperationID = input.ProductsOperationID,
                FaultyQuantity = input.FaultyQuantity,
            }).Where(new { Id = input.Id }, "");

            #region Operation Stock Movement And Work Order


            var productionOrder = (await ProductionOrdersAppService.GetAsync(workOrder.ProductionOrderID.GetValueOrDefault())).Data;

            if (workOrder != null)
            {
                #region Previous Operation Stock Movement Update

                if (workOrder.LineNr > 1)
                {

                    if (input.ProducedQuantity < entity.ProducedQuantity)
                    {
                        var previousWorkOrderId = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID && t.LineNr == workOrder.LineNr - 1).Select(t => t.Id).FirstOrDefault();

                        var previousWorkOrder = (await WorkOrdersAppService.GetAsync(previousWorkOrderId)).Data;

                        var previousOperationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(previousWorkOrder.ProductionOrderID.GetValueOrDefault(), previousWorkOrder.ProductsOperationID.GetValueOrDefault())).Data;


                        if (previousOperationStockMovement.Id != Guid.Empty)
                        {
                            var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                            {
                                Id = previousOperationStockMovement.Id,
                                OperationID = previousWorkOrder.ProductsOperationID.GetValueOrDefault(),
                                ProductionorderID = previousWorkOrder.ProductionOrderID.GetValueOrDefault(),
                                TotalAmount = previousOperationStockMovement.TotalAmount + (entity.ProducedQuantity - input.ProducedQuantity)
                            };

                            var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                        }
                    }

                    if (input.ProducedQuantity > entity.ProducedQuantity)
                    {
                        var previousWorkOrderId = (await WorkOrdersAppService.GetListAsync(new ListWorkOrdersParameterDto())).Data.Where(t => t.ProductionOrderID == workOrder.ProductionOrderID && t.LineNr == workOrder.LineNr - 1).Select(t => t.Id).FirstOrDefault();

                        var previousWorkOrder = (await WorkOrdersAppService.GetAsync(previousWorkOrderId)).Data;

                        var previousOperationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(previousWorkOrder.ProductionOrderID.GetValueOrDefault(), previousWorkOrder.ProductsOperationID.GetValueOrDefault())).Data;


                        if (previousOperationStockMovement.Id != Guid.Empty)
                        {
                            var updateOperationStockMovement = new UpdateOperationStockMovementsDto
                            {
                                Id = previousOperationStockMovement.Id,
                                OperationID = previousWorkOrder.ProductsOperationID.GetValueOrDefault(),
                                ProductionorderID = previousWorkOrder.ProductionOrderID.GetValueOrDefault(),
                                TotalAmount = previousOperationStockMovement.TotalAmount - (input.ProducedQuantity - entity.ProducedQuantity)
                            };

                            var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                        }
                    }

                }

                #endregion

                #region Operation Stock Movement
                var operationStockMovement = (await OperationStockMovementsAppService.GetByProductionOrderIdAsync(workOrder.ProductionOrderID.GetValueOrDefault(), workOrder.ProductsOperationID.GetValueOrDefault())).Data;

                if (operationStockMovement.Id == Guid.Empty)
                {
                    var createOperationStockMovement = new CreateOperationStockMovementsDto
                    {
                        Id = GuidGenerator.CreateGuid(),
                        OperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                        ProductionorderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                        TotalAmount = input.ProducedQuantity
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
                        TotalAmount = operationStockMovement.TotalAmount + (input.ProducedQuantity - entity.ProducedQuantity)
                    };

                    var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, "").UseIsDelete(false);

                    query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence;
                }
                #endregion

                #region Work Order

                if (productionTrackings.Count > 0)
                {
                    if (workOrder.PlannedQuantity == (productionTrackings.Sum(t => t.ProducedQuantity) + input.ProducedQuantity))
                    {
                        workOrder.WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        workOrder.OccuredFinishDate = new DateTime(input.OperationEndDate.GetValueOrDefault().Year, input.OperationEndDate.GetValueOrDefault().Month, input.OperationEndDate.GetValueOrDefault().Day, input.OperationEndTime.Value.Hours, input.OperationEndTime.Value.Minutes, input.OperationEndTime.Value.Seconds);

                        workOrderList.Where(t => t.Id == workOrder.Id).FirstOrDefault().WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        int productionOrderStateCount = workOrderList.Where(t => t.WorkOrderState != WorkOrderStateEnum.Tamamlandi).Count();



                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = productionOrderStateCount == 0 ? (int)ProductionOrderStateEnum.Tamamlandi : (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                    else if (workOrder.PlannedQuantity > (productionTrackings.Sum(t => t.ProducedQuantity) + input.ProducedQuantity))
                    {

                        workOrder.WorkOrderState = WorkOrderStateEnum.DevamEdiyor;

                        workOrder.OccuredFinishDate = null;

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                }
                else
                {
                    if (workOrder.PlannedQuantity == input.ProducedQuantity)
                    {
                        workOrder.WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        workOrder.OccuredStartDate = new DateTime(input.OperationStartDate.Year, input.OperationStartDate.Month, input.OperationStartDate.Day, input.OperationStartTime.Value.Hours, input.OperationStartTime.Value.Minutes, input.OperationStartTime.Value.Seconds);

                        workOrder.OccuredFinishDate = new DateTime(input.OperationEndDate.GetValueOrDefault().Year, input.OperationEndDate.GetValueOrDefault().Month, input.OperationEndDate.GetValueOrDefault().Day, input.OperationEndTime.Value.Hours, input.OperationEndTime.Value.Minutes, input.OperationEndTime.Value.Seconds);

                        workOrderList.Where(t => t.Id == workOrder.Id).FirstOrDefault().WorkOrderState = WorkOrderStateEnum.Tamamlandi;

                        int productionOrderStateCount = workOrderList.Where(t => t.WorkOrderState != WorkOrderStateEnum.Tamamlandi).Count();

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = productionOrderStateCount == 0 ? (int)ProductionOrderStateEnum.Tamamlandi : (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                    else if (workOrder.PlannedQuantity > input.ProducedQuantity)
                    {

                        workOrder.WorkOrderState = WorkOrderStateEnum.DevamEdiyor;

                        workOrder.OccuredStartDate = new DateTime(input.OperationStartDate.Year, input.OperationStartDate.Month, input.OperationStartDate.Day, input.OperationStartTime.Value.Hours, input.OperationStartTime.Value.Minutes, input.OperationStartTime.Value.Seconds);

                        workOrder.OccuredFinishDate = null;

                        if (productionOrder.Id != Guid.Empty)
                        {
                            var updateProductionOrder = new UpdateProductionOrdersDto
                            {
                                BOMID = productionOrder.BOMID.GetValueOrDefault(),
                                Cancel_ = productionOrder.Cancel_,
                                CreationTime = productionOrder.CreationTime,
                                CreatorId = productionOrder.CreatorId,
                                CurrentAccountID = productionOrder.CurrentAccountID.GetValueOrDefault(),
                                CustomerOrderNo = productionOrder.CustomerOrderNo,
                                DataOpenStatus = productionOrder.DataOpenStatus,
                                DataOpenStatusUserId = productionOrder.DataOpenStatusUserId.GetValueOrDefault(),
                                Date_ = productionOrder.Date_,
                                DeleterId = productionOrder.DeleterId.GetValueOrDefault(),
                                DeletionTime = productionOrder.DeletionTime,
                                Description_ = productionOrder.Description_,
                                FicheNo = productionOrder.FicheNo,
                                FinishedProductID = productionOrder.FinishedProductID.GetValueOrDefault(),
                                Id = productionOrder.Id,
                                IsDeleted = productionOrder.IsDeleted,
                                LastModificationTime = now,
                                LastModifierId = LoginedUserService.UserId,
                                LinkedProductID = productionOrder.LinkedProductID.GetValueOrDefault(),
                                LinkedProductionOrderID = productionOrder.LinkedProductionOrderID.GetValueOrDefault(),
                                OrderID = productionOrder.OrderID.GetValueOrDefault(),
                                OrderLineID = productionOrder.OrderLineID.GetValueOrDefault(),
                                PlannedQuantity = productionOrder.PlannedQuantity,
                                ProducedQuantity = productionOrder.ProducedQuantity,
                                ProductionOrderState = (int)ProductionOrderStateEnum.DevamEdiyor,
                                ProductTreeID = productionOrder.ProductTreeID.GetValueOrDefault(),
                                ProductTreeLineID = productionOrder.ProductTreeLineID.GetValueOrDefault(),
                                PropositionID = productionOrder.PropositionID.GetValueOrDefault(),
                                PropositionLineID = productionOrder.PropositionLineID.GetValueOrDefault(),
                                RouteID = productionOrder.RouteID.GetValueOrDefault(),
                                UnitSetID = productionOrder.UnitSetID.GetValueOrDefault()
                            };

                            var productionOrderUpdateQuery = queryFactory.Query().From(Tables.ProductionOrders).Update(updateProductionOrder).Where(new { Id = productionOrder.Id }, "").UseIsDelete(false);

                            query.Sql = query.Sql + QueryConstants.QueryConstant + productionOrderUpdateQuery.Sql + " where " + productionOrderUpdateQuery.WhereSentence;
                        }
                    }
                }

                var updatedWorkOrder = new UpdateWorkOrdersDto
                {
                    Id = workOrder.Id,
                    PlannedQuantity = workOrder.PlannedQuantity,
                    AdjustmentAndControlTime = workOrder.AdjustmentAndControlTime,
                    CreationTime = workOrder.CreationTime,
                    CreatorId = workOrder.CreatorId,
                    CurrentAccountCardID = workOrder.CurrentAccountCardID,
                    DataOpenStatus = workOrder.DataOpenStatus.GetValueOrDefault(),
                    DataOpenStatusUserId = workOrder.DataOpenStatusUserId.GetValueOrDefault(),
                    DeleterId = workOrder.DeleterId.GetValueOrDefault(),
                    DeletionTime = workOrder.DeletionTime,
                    IsCancel = workOrder.IsCancel,
                    IsDeleted = workOrder.IsDeleted,
                    LastModificationTime = now,
                    LastModifierId = LoginedUserService.UserId,
                    LineNr = workOrder.LineNr,
                    LinkedWorkOrderID = workOrder.LinkedWorkOrderID.GetValueOrDefault(),
                    OccuredFinishDate = workOrder.OccuredFinishDate,
                    OccuredStartDate = workOrder.OccuredStartDate,
                    OperationTime = workOrder.OperationTime,
                    ProductID = workOrder.ProductID.GetValueOrDefault(),
                    ProductionOrderID = workOrder.ProductionOrderID.GetValueOrDefault(),
                    ProductsOperationID = workOrder.ProductsOperationID.GetValueOrDefault(),
                    PropositionID = workOrder.PropositionID.GetValueOrDefault(),
                    RouteID = workOrder.RouteID.GetValueOrDefault(),
                    StationGroupID = workOrder.StationGroupID.GetValueOrDefault(),
                    StationID = workOrder.StationID.GetValueOrDefault(),
                    WorkOrderNo = workOrder.WorkOrderNo,
                    WorkOrderState = (int)workOrder.WorkOrderState,
                    ProducedQuantity = workOrder.ProducedQuantity + (input.ProducedQuantity - entity.ProducedQuantity),
                    OrderID = workOrder.OrderID,
                    IsOperationUnsuitabilityWorkOrder = workOrder.IsOperationUnsuitabilityWorkOrder,
                    IsContractUnsuitabilityWorkOrder = workOrder.IsContractUnsuitabilityWorkOrder,
                    SplitQuantity = workOrder.SplitQuantity
                };

                var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, "").UseIsDelete(false);

                query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                #endregion
            }

            #endregion

            var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdTrackingsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);

        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ProductionTrackings>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
            {
                AdjustmentTime = entity.AdjustmentTime,
                HaltReasonID = entity.HaltReasonID,
                ProductionTrackingTypes = (int)entity.ProductionTrackingTypes,
                EmployeeID = entity.EmployeeID,
                HaltTime = entity.HaltTime,
                IsFinished = entity.IsFinished,
                OperationEndDate = entity.OperationEndDate.GetValueOrDefault(),
                OperationEndTime = entity.OperationEndTime,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                Description_ = entity.Description_,
                OperationStartDate = entity.OperationStartDate,
                OperationStartTime = entity.OperationStartTime,
                OperationTime = entity.OperationTime,
                PlannedQuantity = entity.PlannedQuantity,
                ProducedQuantity = entity.ProducedQuantity,
                ShiftID = entity.ShiftID,
                StationID = entity.StationID,
                WorkOrderID = entity.WorkOrderID,
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
                ProductID = entity.ProductID,
                ProductionOrderID = entity.ProductionOrderID,
                ProductsOperationID = entity.ProductsOperationID,
                FaultyQuantity = entity.FaultyQuantity

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var productionTrackingsDto = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackingsDto);

        }

        public async Task<IDataResult<IList<SelectProductionTrackingsDto>>> GetListbyOprStartDateRangeIsEqualAsync(DateTime startDate, DateTime endDate)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ProductionTrackings)
                   .Select<ProductionTrackings>(s => new { s.OperationStartDate, s.OperationEndDate, s.PlannedQuantity, s.OperationTime, s.HaltTime, s.AdjustmentTime, s.IsFinished, s.Id, s.ProductionTrackingTypes })
                   .Join<WorkOrders>
                    (
                        wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.WorkOrderNo },
                        nameof(ProductionTrackings.WorkOrderID),
                        nameof(WorkOrders.Id),
                        JoinType.Left
                    )
                      .Join<HaltReasons>
                    (
                        wo => new { HaltReasonID = wo.Id, HaltReasonCode = wo.Code },
                        nameof(ProductionTrackings.HaltReasonID),
                        nameof(HaltReasons.Id),
                        JoinType.Left
                    )
                     .Join<Stations>
                    (
                        s => new { StationID = s.Id, StationCode = s.Code },
                        nameof(ProductionTrackings.StationID),
                        nameof(Stations.Id),
                        JoinType.Left
                    )
                    .Join<Shifts>
                    (
                        sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                        nameof(ProductionTrackings.ShiftID),
                        nameof(Shifts.Id),
                        JoinType.Left
                    )
                    .Join<Employees>
                    (
                        e => new { EmployeeID = e.Id, EmployeeName = e.Name, EmployeeSurname = e.Surname },
                        nameof(ProductionTrackings.EmployeeID),
                        nameof(Employees.Id),
                        JoinType.Left
                    )
                      .Join<CurrentAccountCards>
                    (
                        e => new { CustomerCode = e.CustomerCode, CurrentAccountCardID = e.Id },
                        nameof(ProductionTrackings.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
            .Join<Products>
            (
                        e => new { ProductID = e.Id, ProductCode = e.Code, ProductType = e.ProductType, ProductGroupID = e.ProductGrpID },
                        nameof(ProductionTrackings.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        e => new { ProductionOrderID = e.Id, ProductionOrderCode = e.FicheNo },
                        nameof(ProductionTrackings.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        e => new { ProductsOperationID = e.Id, ProductOperationName = e.Name },
                        nameof(ProductionTrackings.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .WhereDateRangeIsEqual("OperationStartDate", startDate, endDate, Tables.ProductionTrackings);

            var productionTrackings = queryFactory.GetList<SelectProductionTrackingsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectProductionTrackingsDto>>(productionTrackings);
        }
    }
}
