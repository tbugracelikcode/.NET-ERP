using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services;
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionTrackings.Page;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService<ProductionTrackingsResource>, IProductionTrackingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        private IOperationStockMovementsAppService OperationStockMovementsAppService { get; set; }
        private IWorkOrdersAppService WorkOrdersAppService { get; set; }

        public ProductionTrackingsAppService(IStringLocalizer<ProductionTrackingsResource> l, IFicheNumbersAppService ficheNumbersAppService, IOperationStockMovementsAppService operationStockMovementsAppService, IWorkOrdersAppService workOrdersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            OperationStockMovementsAppService = operationStockMovementsAppService;
            WorkOrdersAppService = workOrdersAppService;
        }

        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<ProductionTrackings>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                #region Operation Time

                double operationTime = 0;

                if (input.OperationStartTime > input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime) - Math.Abs(input.OperationEndTime.Value.TotalSeconds - input.OperationStartTime.Value.TotalSeconds);
                }
                else if (input.OperationStartTime < input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime) + Math.Abs(input.OperationEndTime.Value.TotalSeconds - input.OperationStartTime.Value.TotalSeconds);
                }
                else if (input.OperationStartTime == input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Insert(new CreateProductionTrackingsDto
                {
                    AdjustmentTime = input.AdjustmentTime,
                    EmployeeID = input.EmployeeID,
                    HaltTime = input.HaltTime,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Description_ = input.Description_,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = Convert.ToDecimal(operationTime),
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = input.WorkOrderID,
                    Code = input.Code,
                    CreationTime = DateTime.Now,
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

                #region Halt Lines
                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Insert(new CreateProductionTrackingHaltLinesDto
                    {
                        HaltID = item.HaltID,
                        HaltTime = item.HaltTime,
                        ProductionTrackingID = addedEntityId,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                #endregion

                #region Operation Stock Movement And Work Order 

                var workOrder = (await WorkOrdersAppService.GetAsync(input.WorkOrderID)).Data;

                if (workOrder != null)
                {
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

                            var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, false, false, "").UseIsDelete(false);

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

                        var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, false, false, "").UseIsDelete(false);

                        query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                    }
                    #endregion

                    #region Work Order

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
                        LastModificationTime = DateTime.Now,
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
                        ProducedQuantity = workOrder.ProducedQuantity + input.ProducedQuantity
                    };

                    var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, false, false, "").UseIsDelete(false);

                    query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                    #endregion
                }

                #endregion

                var productionTracking = queryFactory.Insert<SelectProductionTrackingsDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("ProdTrackingsChildMenu", input.Code);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Id = id }, false, false, "");

                var productionTrackings = queryFactory.Get<SelectProductionTrackingsDto>(query);

                if (productionTrackings.Id != Guid.Empty && productionTrackings != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ProductionTrackings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Delete(LoginedUserService.UserId).Where(new { ProductionTrackingID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;



                    #region Operation Stock Movement And Work Order

                    var workOrder = (await WorkOrdersAppService.GetAsync(productionTrackings.WorkOrderID)).Data;

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

                            var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, false, false, "").UseIsDelete(false);

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

                                var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, false, false, "").UseIsDelete(false);

                                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                            }
                        }

                        #endregion

                        #region Work Order

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
                            LastModificationTime = DateTime.Now,
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
                            ProducedQuantity = workOrder.ProducedQuantity - productionTrackings.ProducedQuantity
                        };

                        var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, false, false, "").UseIsDelete(false);

                        deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                        #endregion
                    }

                    #endregion


                    var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var productionTrackingLines = queryFactory.Update<SelectProductionTrackingHaltLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionTrackingHaltLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductionTrackingHaltLinesDto>(productionTrackingLines);
                }
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                            e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ProductionTrackings);

                var productionTrackings = queryFactory.Get<SelectProductionTrackingsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackingHaltLines)
                       .Select("*")
                       .Join<HaltReasons>
                        (
                            hr => new { HaltID = hr.Id, HaltName = hr.Name, HaltCode = hr.Code },
                            nameof(ProductionTrackingHaltLines.HaltID),
                            nameof(HaltReasons.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductionTrackingID = id }, false, false, Tables.ProductionTrackingHaltLines);

                var productionTrackingLine = queryFactory.GetList<SelectProductionTrackingHaltLinesDto>(queryLines).ToList();

                productionTrackings.SelectProductionTrackingHaltLines = productionTrackingLine;

                LogsAppService.InsertLogToDatabase(productionTrackings, productionTrackings, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Get, id);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackings);
            }

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                        .Where(null, false, false, Tables.ProductionTrackings);

                var productionTrackings = queryFactory.GetList<ListProductionTrackingsDto>(query).ToList();

                return new SuccessDataResult<IList<ListProductionTrackingsDto>>(productionTrackings);
            }
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
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
                            e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.ProductionTrackings);

                var entity = queryFactory.Get<SelectProductionTrackingsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackingHaltLines)
                       .Select<ProductionTrackingHaltLines>(null)
                       .Join<HaltReasons>
                        (
                            hr => new { HaltID = hr.Id, HaltName = hr.Name, HaltCode = hr.Code },
                            nameof(ProductionTrackingHaltLines.HaltID),
                            nameof(HaltReasons.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductionTrackingID = input.Id }, false, false, Tables.ProductionTrackingHaltLines);

                var productionTrackingLine = queryFactory.GetList<SelectProductionTrackingHaltLinesDto>(queryLines).ToList();

                entity.SelectProductionTrackingHaltLines = productionTrackingLine ?? new List<SelectProductionTrackingHaltLinesDto>();

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
                                .Where(new { Code = input.Code }, false, false, Tables.ProductionTrackings);

                var list = queryFactory.GetList<ListProductionTrackingsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                #region Operation Time

                double operationTime = 0;

                if (input.OperationStartTime > input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime) - Math.Abs(input.OperationEndTime.Value.TotalSeconds - input.OperationStartTime.Value.TotalSeconds);
                }
                else if (input.OperationStartTime < input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime) + Math.Abs(input.OperationEndTime.Value.TotalSeconds - input.OperationStartTime.Value.TotalSeconds);
                }
                else if (input.OperationStartTime == input.OperationEndTime)
                {
                    operationTime = (input.OperationEndDate - input.OperationStartDate).TotalDays * Convert.ToDouble(input.OperationTime - input.HaltTime);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
                {
                    AdjustmentTime = input.AdjustmentTime,
                    EmployeeID = input.EmployeeID,
                    HaltTime = input.HaltTime,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Description_ = input.Description_,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = Convert.ToDecimal(operationTime),
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = input.WorkOrderID,
                    Code = input.Code,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = input.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = input.Id }, false, false, "");

                if (input.SelectProductionTrackingHaltLinesDto != null)
                {
                    foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                    {
                        if (item.Id == Guid.Empty)
                        {
                            var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Insert(new CreateProductionTrackingHaltLinesDto
                            {
                                HaltID = item.HaltID,
                                HaltTime = item.HaltTime,
                                LastModifierId = Guid.Empty,
                                ProductionTrackingID = input.Id,
                                CreationTime = DateTime.Now,
                                CreatorId = LoginedUserService.UserId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = Guid.Empty,
                                DeletionTime = null,
                                Id = GuidGenerator.CreateGuid(),
                                IsDeleted = false,
                                LastModificationTime = null
                            });

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                        }
                        else
                        {
                            var lineGetQuery = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                            var line = queryFactory.Get<SelectProductionTrackingHaltLinesDto>(lineGetQuery);

                            if (line != null)
                            {
                                var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Update(new UpdateProductionTrackingHaltLinesDto
                                {
                                    HaltTime = item.HaltTime,
                                    HaltID = item.HaltID,
                                    ProductionTrackingID = input.Id,
                                    CreationTime = line.CreationTime,
                                    CreatorId = line.CreatorId,
                                    DataOpenStatus = false,
                                    DataOpenStatusUserId = Guid.Empty,
                                    DeleterId = line.DeleterId.GetValueOrDefault(),
                                    DeletionTime = line.DeletionTime.GetValueOrDefault(),
                                    Id = item.Id,
                                    IsDeleted = item.IsDeleted,
                                    LastModificationTime = DateTime.Now,
                                    LastModifierId = LoginedUserService.UserId,
                                }).Where(new { Id = line.Id }, false, false, "");

                                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                            }
                        }
                    }
                }


                #region Operation Stock Movement And Work Order

                var workOrder = (await WorkOrdersAppService.GetAsync(input.WorkOrderID)).Data;

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

                                var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, false, false, "").UseIsDelete(false);

                                query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence; ;
                            }
                        }

                        if(input.ProducedQuantity > entity.ProducedQuantity)
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

                                var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = previousOperationStockMovement.Id }, false, false, "").UseIsDelete(false);

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

                        var operationStockMovementQuery = queryFactory.Query().From(Tables.OperationStockMovements).Update(updateOperationStockMovement).Where(new { Id = operationStockMovement.Id }, false, false, "").UseIsDelete(false);

                        query.Sql = query.Sql + QueryConstants.QueryConstant + operationStockMovementQuery.Sql + " where " + operationStockMovementQuery.WhereSentence;
                    }
                    #endregion

                    #region Work Order


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
                        LastModificationTime = DateTime.Now,
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
                        ProducedQuantity = workOrder.ProducedQuantity + (input.ProducedQuantity - entity.ProducedQuantity)
                    };

                    var workOrderUpdateQuery = queryFactory.Query().From(Tables.WorkOrders).Update(updatedWorkOrder).Where(new { Id = workOrder.Id }, false, false, "").UseIsDelete(false);

                    query.Sql = query.Sql + QueryConstants.QueryConstant + workOrderUpdateQuery.Sql + " where " + workOrderUpdateQuery.WhereSentence;

                    #endregion
                }



                #endregion

                var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<ProductionTrackings>(entityQuery);

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
                {
                    AdjustmentTime = entity.AdjustmentTime,
                    EmployeeID = entity.EmployeeID,
                    HaltTime = entity.HaltTime,
                    IsFinished = entity.IsFinished,
                    OperationEndDate = entity.OperationEndDate,
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
                }).Where(new { Id = id }, false, false, "");

                var productionTrackingsDto = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);
                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackingsDto);

            }
        }

    }
}
