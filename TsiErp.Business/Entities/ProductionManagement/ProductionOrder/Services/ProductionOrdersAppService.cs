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
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.ProductionOrder.Validations;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.StockAddress.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.ReportDtos;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionOrders.Page;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    [ServiceRegistration(typeof(IProductionOrdersAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrdersAppService : ApplicationService<ProductionOrdersResource>, IProductionOrdersAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        private IRoutesAppService RoutesAppService { get; set; }
        private IProductsOperationsAppService ProductsOperationsAppService { get; set; }
        private IStationsAppService StationsAppService { get; set; }
        private IBillsofMaterialsAppService BillsofMaterialsAppService { get; set; }
        private IProductsAppService ProductsAppService { get; set; }
        private IStockAddressesAppService StockAddressesAppService { get; set; }
        private IProductionManagementParametersAppService ProductionManagementParametersService { get; set; }

        public ProductionOrdersAppService(IStringLocalizer<ProductionOrdersResource> l, IFicheNumbersAppService ficheNumbersAppService, IRoutesAppService routesAppService, IProductsOperationsAppService productsOperationsAppService, IStationsAppService stationsAppService, IBillsofMaterialsAppService billsofMaterialsAppService, IProductsAppService productsAppService, IGetSQLDateAppService getSQLDateAppService, IStockAddressesAppService stockAddressesAppService, IProductionManagementParametersAppService productionManagementParametersService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            RoutesAppService = routesAppService;
            ProductsOperationsAppService = productsOperationsAppService;
            StationsAppService = stationsAppService;
            BillsofMaterialsAppService = billsofMaterialsAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            ProductsAppService = productsAppService;
            StockAddressesAppService = stockAddressesAppService;
            ProductionManagementParametersService = productionManagementParametersService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> ConverttoProductionOrder(CreateProductionOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo },  "");

            var list = queryFactory.ControlList<ProductionOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion


            Guid addedEntityId = GuidGenerator.CreateGuid();

            #region Finished Production Order
            var productionOrderQuery = queryFactory.Query().From(Tables.ProductionOrders).Insert(new CreateProductionOrdersDto
            {
                BOMID = input.BOMID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                TechnicalDrawingUpdateDate_ = input.TechnicalDrawingUpdateDate_,
                TechnicalDrawingUpdateDescription_ = input.TechnicalDrawingUpdateDescription_,
                LinkedProductID = input.LinkedProductID.GetValueOrDefault(),
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                OrderID = input.OrderID.GetValueOrDefault(),
                OrderLineID = input.OrderLineID.GetValueOrDefault(),
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductionOrderState = input.ProductionOrderState,
                ProductTreeID = input.ProductTreeID.GetValueOrDefault(),
                ProductTreeLineID = input.ProductTreeLineID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                Cancel_ = input.Cancel_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                BranchID = input.BranchID.GetValueOrDefault(),
                WarehouseID = input.WarehouseID.GetValueOrDefault()
            });

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProductionOrdersChildMenu", input.FicheNo);
            #endregion

            #region Finished Product Production Work Order

            var productProductionRouteLines = (await RoutesAppService.GetAsync(input.RouteID.GetValueOrDefault())).Data;

            foreach (var item in productProductionRouteLines.SelectRouteLines.OrderBy(t => t.LineNr).ToList())
            {
                var productOperation = (await ProductsOperationsAppService.GetAsync(item.ProductsOperationID)).Data;

                Guid stationId = productOperation.SelectProductsOperationLines.Where(t => t.Priority == 1).Select(t => t.StationID).FirstOrDefault().GetValueOrDefault();

                Guid stationGroupId = (await StationsAppService.GetAsync(stationId)).Data.GroupID;

                CreateWorkOrdersDto workOrder = new CreateWorkOrdersDto
                {
                    CurrentAccountCardID = input.CurrentAccountID.GetValueOrDefault(),
                    IsCancel = false,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false,
                    AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                    LineNr = item.LineNr,
                    LinkedWorkOrderID = Guid.Empty,
                    OccuredFinishDate = null,
                    PropositionID = input.PropositionID.GetValueOrDefault(),
                    WorkOrderState = (int)WorkOrderStateEnum.Baslamadi,
                    StationID = stationId,
                    ProductionOrderID = addedEntityId,
                    RouteID = input.RouteID.GetValueOrDefault(),
                    PlannedQuantity = input.PlannedQuantity,
                    OccuredStartDate = null,
                    ProducedQuantity = 0,
                    OperationTime = item.OperationTime,
                    ProductID = input.FinishedProductID.GetValueOrDefault(),
                    ProductsOperationID = item.ProductsOperationID,
                    StationGroupID = stationGroupId,
                    WorkOrderNo = FicheNumbersAppService.GetFicheNumberAsync("WorkOrdersChildMenu"),
                    Id = GuidGenerator.CreateGuid(),
                    OrderID = input.OrderID.GetValueOrDefault()
                };

                var workOrderQuery = queryFactory.Query().From(Tables.WorkOrders).Insert(workOrder);

                productionOrderQuery.Sql = productionOrderQuery.Sql + QueryConstants.QueryConstant + workOrderQuery.Sql;

                await FicheNumbersAppService.UpdateFicheNumberAsync("WorkOrdersChildMenu", workOrder.WorkOrderNo);
            }


            #endregion

            #region Bom Line Production Order

            var finishedProduct = (await ProductsAppService.GetAsync(input.FinishedProductID.GetValueOrDefault())).Data;

            var finishedProductBom = (await BillsofMaterialsAppService.GetbyCurrentAccountIDAsync(input.CurrentAccountID.GetValueOrDefault(), input.FinishedProductID.GetValueOrDefault())).Data;

            foreach (var item in finishedProductBom.SelectBillsofMaterialLines)
            {
                var supplyForm = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.SupplyForm;

                if (supplyForm == ProductSupplyFormEnum.Üretim)
                {
                    #region Line Production Order
                    var lineProductProductionRoute = (await RoutesAppService.GetListAsync(new ListRoutesParameterDto())).Data.Where(t => t.ProductID == item.ProductID && t.TechnicalApproval == true && t.Approval == true).FirstOrDefault();

                    var lineBom = (await BillsofMaterialsAppService.GetbyCurrentAccountIDAsync(input.CurrentAccountID.GetValueOrDefault(), item.ProductID.GetValueOrDefault())).Data;

                    CreateProductionOrdersDto procutionOrderBomLine = new CreateProductionOrdersDto
                    {
                        OrderID = input.OrderID.GetValueOrDefault(),
                        FinishedProductID = item.ProductID.GetValueOrDefault(),
                        LinkedProductID = input.FinishedProductID.GetValueOrDefault(),
                        PlannedQuantity = item.Quantity * input.PlannedQuantity,
                        ProducedQuantity = 0,
                        CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                        Cancel_ = false,
                        CustomerOrderNo = input.CustomerOrderNo,
                        Date_ = DateTime.Today,
                        Description_ = "",
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        IsDeleted = false,
                        ProductionOrderState = (int)ProductionOrderStateEnum.Baslamadi,
                        ProductTreeID = Guid.Empty,
                        ProductTreeLineID = Guid.Empty,
                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("ProductionOrdersChildMenu"),
                        OrderLineID = input.OrderLineID.GetValueOrDefault(),
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        LinkedProductionOrderID = addedEntityId,
                        PropositionID = input.PropositionID.GetValueOrDefault(),
                        PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                        BOMID = lineBom.Id,
                        RouteID = lineProductProductionRoute.Id,
                        Id = GuidGenerator.CreateGuid(),
                        BranchID = input.BranchID.GetValueOrDefault(),
                        WarehouseID = input.WarehouseID.GetValueOrDefault()
                    };

                    var procutionOrderBomLineQuery = queryFactory.Query().From(Tables.ProductionOrders).Insert(procutionOrderBomLine);

                    await FicheNumbersAppService.UpdateFicheNumberAsync("ProductionOrdersChildMenu", procutionOrderBomLine.FicheNo);

                    productionOrderQuery.Sql = productionOrderQuery.Sql + QueryConstants.QueryConstant + procutionOrderBomLineQuery.Sql;
                    #endregion



                    #region Line Production Work Order

                    var lineProductProductionRouteLines = (await RoutesAppService.GetAsync(lineProductProductionRoute.Id)).Data;

                    foreach (var route in lineProductProductionRouteLines.SelectRouteLines.OrderBy(t => t.LineNr).ToList())
                    {
                        var productOperation = (await ProductsOperationsAppService.GetAsync(route.ProductsOperationID)).Data;

                        Guid stationId = productOperation.SelectProductsOperationLines.Where(t => t.Priority == 1).Select(t => t.StationID).FirstOrDefault().GetValueOrDefault();

                        Guid stationGroupId = (await StationsAppService.GetAsync(stationId)).Data.GroupID;

                        CreateWorkOrdersDto workOrder = new CreateWorkOrdersDto
                        {
                            CurrentAccountCardID = input.CurrentAccountID.GetValueOrDefault(),
                            IsCancel = false,
                            CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            IsDeleted = false,
                            AdjustmentAndControlTime = route.AdjustmentAndControlTime,
                            LineNr = route.LineNr,
                            LinkedWorkOrderID = Guid.Empty,
                            OccuredFinishDate = null,
                            PropositionID = input.PropositionID.GetValueOrDefault(),
                            WorkOrderState = (int)WorkOrderStateEnum.Baslamadi,
                            StationID = stationId,
                            ProductionOrderID = procutionOrderBomLine.Id,
                            RouteID = procutionOrderBomLine.RouteID.GetValueOrDefault(),
                            PlannedQuantity = procutionOrderBomLine.PlannedQuantity,
                            OccuredStartDate = null,
                            ProducedQuantity = 0,
                            OperationTime = route.OperationTime,
                            ProductID = procutionOrderBomLine.FinishedProductID.GetValueOrDefault(),
                            ProductsOperationID = route.ProductsOperationID,
                            StationGroupID = stationGroupId,
                            WorkOrderNo = FicheNumbersAppService.GetFicheNumberAsync("WorkOrdersChildMenu"),
                            Id = GuidGenerator.CreateGuid(),
                            OrderID = input.OrderID.GetValueOrDefault()
                        };

                        var workOrderQuery = queryFactory.Query().From(Tables.WorkOrders).Insert(workOrder);

                        productionOrderQuery.Sql = productionOrderQuery.Sql + QueryConstants.QueryConstant + workOrderQuery.Sql;

                        await FicheNumbersAppService.UpdateFicheNumberAsync("WorkOrdersChildMenu", workOrder.WorkOrderNo);
                    }

                    #endregion
                }
            }

            #endregion

            var productionOrders = queryFactory.Insert<SelectProductionOrdersDto>(productionOrderQuery, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["SalesOrdersChildMenu"],  L["SalesOrderContextProdOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["SalesOrderContextProdOrder"],
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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
                            ContextMenuName_ = L["SalesOrderContextProdOrder"],
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion


            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }


        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> CreateAsync(CreateProductionOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");

            var list = queryFactory.ControlList<ProductionOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ProductionOrders).Insert(new CreateProductionOrdersDto
            {
                BOMID = input.BOMID,
                FicheNo = input.FicheNo,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                TechnicalDrawingUpdateDescription_ = input.TechnicalDrawingUpdateDescription_,
                TechnicalDrawingUpdateDate_ = input.TechnicalDrawingUpdateDate_,
                CustomerOrderNo = input.CustomerOrderNo,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                LinkedProductID = input.LinkedProductID.GetValueOrDefault(),
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                OrderID = input.OrderID.GetValueOrDefault(),
                OrderLineID = input.OrderLineID.GetValueOrDefault(),
                PlannedQuantity = input.PlannedQuantity,
                BranchID = input.BranchID.GetValueOrDefault(),
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                ProducedQuantity = input.ProducedQuantity,
                ProductionOrderState = input.ProductionOrderState,
                ProductTreeID = input.ProductTreeID.GetValueOrDefault(),
                ProductTreeLineID = input.ProductTreeLineID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                Cancel_ = input.Cancel_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var productionOrders = queryFactory.Insert<SelectProductionOrdersDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProductionOrdersChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionOrdersChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion


            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }



        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("LinkedProductionOrderID", new List<string>
            {
                Tables.ProductionOrders
            });

            DeleteControl.ControlList.Add("ProductionOrderID", new List<string>
            {
                Tables.ContractTrackingFiches,
                Tables.ContractUnsuitabilityReports,
                Tables.OperationUnsuitabilityReports,
                Tables.PackageFicheLines,
                Tables.PurchaseOrderLines,
                Tables.ShipmentPlanningLines,
                Tables.PurchaseOrders,
                Tables.PurchaseRequestLines,
                Tables.PurchaseRequests,
                Tables.StockFiches,
                Tables.WorkOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.ProductionOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionOrdersChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = entity.FicheNo,
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
                                RecordNumber = entity.FicheNo,
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
                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);
            }
        }


        public async Task<IDataResult<SelectProductionOrdersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductionOrders).Select<ProductionOrders>(null)
                        .Join<SalesOrders>
                        (
                            so => new { OrderFicheNo = so.FicheNo, OrderID = so.Id, CustomerOrderNo = so.CustomerOrderNr },
                            nameof(ProductionOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                        .Join<SalesOrderLines>
                        (
                            sol => new { OrderLineID = sol.Id },
                            nameof(ProductionOrders.OrderLineID),
                            nameof(SalesOrderLines.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code, FinishedProductName = p.Name },
                            nameof(ProductionOrders.FinishedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name, LinkedProductID = p.Id },
                            nameof(ProductionOrders.LinkedProductID),
                            nameof(Products.Id),
                            "LinkedProduct",
                            JoinType.Left
                        )
                         .Join<UnitSets>
                        (
                            u => new { UnitSetCode = u.Code, UnitSetID = u.Id },
                            nameof(ProductionOrders.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<BillsofMaterials>
                        (
                            bom => new { BOMID = bom.Id, BOMCode = bom.Code, BOMName = bom.Name },
                            nameof(ProductionOrders.BOMID),
                            nameof(BillsofMaterials.Id),
                            JoinType.Left
                        )
                         .Join<Routes>
                        (
                            r => new { RouteID = r.Id, RouteCode = r.Code, RouteName = r.Name },
                            nameof(ProductionOrders.RouteID),
                            nameof(Routes.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionID = sp.Id, PropositionFicheNo = sp.FicheNo },
                            nameof(ProductionOrders.PropositionID),
                            nameof(SalesPropositions.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositionLines>
                        (
                            spl => new { PropositionLineID = spl.Id },
                            nameof(ProductionOrders.PropositionLineID),
                            nameof(SalesPropositionLines.Id),
                            JoinType.Left
                        )
                         .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(ProductionOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(ProductionOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<TechnicalDrawings>
                    (
                        w => new { TechnicalDrawingID = w.Id, TechnicalDrawingNo = w.RevisionNo },
                        nameof(ProductionOrders.TechnicalDrawingID),
                        nameof(TechnicalDrawings.Id),
                        JoinType.Left
                    )
                        .Join<CurrentAccountCards>
                        (
                            ca => new
                            {
                                CurrentAccountID = ca.Id,
                                CurrentAccountCode = ca.Code,
                                CurrentAccountName = ca.Name,
                                CustomerCode = ca.CustomerCode
                            },
                            nameof(ProductionOrders.CurrentAccountID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.ProductionOrders);

            var productionOrder = queryFactory.Get<SelectProductionOrdersDto>(query);

            LogsAppService.InsertLogToDatabase(productionOrder, productionOrder, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrder);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetListAsync(ListProductionOrdersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductionOrders).Select<ProductionOrders>(s => new { s.FicheNo, s.ProductionOrderState, s.PlannedQuantity, s.ProducedQuantity })
                        .Join<SalesOrders>
                        (
                            so => new { OrderID = so.Id, OrderFicheNo = so.FicheNo, CustomerOrderNo = so.CustomerOrderNr },
                            nameof(ProductionOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                           .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(ProductionOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(ProductionOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Products>
                    (
                        p => new { FinishedProductCode = p.Code, FinishedProductName = p.Name },
                        nameof(ProductionOrders.FinishedProductID),
                        nameof(Products.Id),
                        "FinishedProduct",
                        JoinType.Left
                    )
                     .Join<Products>
                    (
                        p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name },
                        nameof(ProductionOrders.LinkedProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<UnitSets>
                    (
                        u => new { UnitSetCode = u.Code },
                        nameof(ProductionOrders.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<BillsofMaterials>
                    (
                        bom => new { BOMCode = bom.Code, BOMName = bom.Name },
                        nameof(ProductionOrders.BOMID),
                        nameof(BillsofMaterials.Id),
                        JoinType.Left
                    )
                     .Join<Routes>
                    (
                        r => new { RouteCode = r.Code, RouteName = r.Name },
                        nameof(ProductionOrders.RouteID),
                        nameof(Routes.Id),
                        JoinType.Left
                    )
                     .Join<SalesPropositions>
                    (
                        sp => new { PropositionFicheNo = sp.FicheNo },
                        nameof(ProductionOrders.PropositionID),
                        nameof(SalesPropositions.Id),
                        JoinType.Left
                    )
                     .Join<TechnicalDrawings>
                    (
                        w => new { TechnicalDrawingID = w.Id, TechnicalDrawingNo = w.RevisionNo },
                        nameof(ProductionOrders.TechnicalDrawingID),
                        nameof(TechnicalDrawings.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new
                        {
                            CurrentAccountCode = ca.Code,
                            CurrentAccountName = ca.Name,
                            CustomerCode = ca.CustomerCode
                        },
                        nameof(ProductionOrders.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                   .Where(null, Tables.ProductionOrders);

            var productionOrders = queryFactory.GetList<ListProductionOrdersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionOrdersDto>>(productionOrders);

        }

        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetNotCanceledListAsync(ListProductionOrdersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductionOrders).Select<ProductionOrders>(s => new { s.FicheNo, s.ProductionOrderState, s.PlannedQuantity, s.ProducedQuantity })
                        .Join<SalesOrders>
                        (
                            so => new { OrderID = so.Id, OrderFicheNo = so.FicheNo, CustomerOrderNo = so.CustomerOrderNr },
                            nameof(ProductionOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                           .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(ProductionOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(ProductionOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )

                         .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code, FinishedProductName = p.Name },
                            nameof(ProductionOrders.FinishedProductID),
                            nameof(Products.Id),
                            "FinishedProduct",
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name },
                            nameof(ProductionOrders.LinkedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<UnitSets>
                        (
                            u => new { UnitSetCode = u.Code },
                            nameof(ProductionOrders.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<BillsofMaterials>
                        (
                            bom => new { BOMCode = bom.Code, BOMName = bom.Name },
                            nameof(ProductionOrders.BOMID),
                            nameof(BillsofMaterials.Id),
                            JoinType.Left
                        )
                         .Join<Routes>
                        (
                            r => new { RouteCode = r.Code, RouteName = r.Name },
                            nameof(ProductionOrders.RouteID),
                            nameof(Routes.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionFicheNo = sp.FicheNo },
                            nameof(ProductionOrders.PropositionID),
                            nameof(SalesPropositions.Id),
                            JoinType.Left
                        )
                        .Join<TechnicalDrawings>
                    (
                        w => new { TechnicalDrawingID = w.Id, TechnicalDrawingNo = w.RevisionNo },
                        nameof(ProductionOrders.TechnicalDrawingID),
                        nameof(TechnicalDrawings.Id),
                        JoinType.Left
                    )
                        .Join<CurrentAccountCards>
                        (
                            ca => new
                            {
                                CurrentAccountCode = ca.Code,
                                CurrentAccountName = ca.Name,
                                CustomerCode = ca.CustomerCode
                            },
                            nameof(ProductionOrders.CurrentAccountID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                   .Where(new { Cancel_ = false },Tables.ProductionOrders);

            var productionOrders = queryFactory.GetList<ListProductionOrdersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionOrdersDto>>(productionOrders);

        }

        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetCanceledListAsync(ListProductionOrdersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductionOrders).Select<ProductionOrders>(s => new { s.FicheNo, s.ProductionOrderState, s.PlannedQuantity, s.ProducedQuantity })
                        .Join<SalesOrders>
                        (
                            so => new { OrderID = so.Id, OrderFicheNo = so.FicheNo, CustomerOrderNo = so.CustomerOrderNr },
                            nameof(ProductionOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                           .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(ProductionOrders.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                     .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(ProductionOrders.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )

                         .Join<Products>
                        (
                            p => new { FinishedProductCode = p.Code, FinishedProductName = p.Name },
                            nameof(ProductionOrders.FinishedProductID),
                            nameof(Products.Id),
                            "FinishedProduct",
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name },
                            nameof(ProductionOrders.LinkedProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<UnitSets>
                        (
                            u => new { UnitSetCode = u.Code },
                            nameof(ProductionOrders.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<BillsofMaterials>
                        (
                            bom => new { BOMCode = bom.Code, BOMName = bom.Name },
                            nameof(ProductionOrders.BOMID),
                            nameof(BillsofMaterials.Id),
                            JoinType.Left
                        )
                         .Join<Routes>
                        (
                            r => new { RouteCode = r.Code, RouteName = r.Name },
                            nameof(ProductionOrders.RouteID),
                            nameof(Routes.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionFicheNo = sp.FicheNo },
                            nameof(ProductionOrders.PropositionID),
                            nameof(SalesPropositions.Id),
                            JoinType.Left
                        )
                        .Join<TechnicalDrawings>
                    (
                        w => new { TechnicalDrawingID = w.Id, TechnicalDrawingNo = w.RevisionNo },
                        nameof(ProductionOrders.TechnicalDrawingID),
                        nameof(TechnicalDrawings.Id),
                        JoinType.Left
                    )
                        .Join<CurrentAccountCards>
                        (
                            ca => new
                            {
                                CurrentAccountCode = ca.Code,
                                CurrentAccountName = ca.Name,
                                CustomerCode = ca.CustomerCode
                            },
                            nameof(ProductionOrders.CurrentAccountID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                   .Where(new { Cancel_ = true }, Tables.ProductionOrders);

            var productionOrders = queryFactory.GetList<ListProductionOrdersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionOrdersDto>>(productionOrders);

        }

        [ValidationAspect(typeof(UpdateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateAsync(UpdateProductionOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductionOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.GetList<ProductionOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
            {
                BOMID = input.BOMID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                LinkedProductID = input.LinkedProductID.GetValueOrDefault(),
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                TechnicalDrawingUpdateDate_ = input.TechnicalDrawingUpdateDate_,
                TechnicalDrawingUpdateDescription_ = input.TechnicalDrawingUpdateDescription_,
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                OrderID = input.OrderID.GetValueOrDefault(),
                OrderLineID = input.OrderLineID.GetValueOrDefault(),
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductionOrderState = input.ProductionOrderState,
                BranchID = input.BranchID.GetValueOrDefault(),
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                ProductTreeID = input.ProductTreeID.GetValueOrDefault(),
                ProductTreeLineID = input.ProductTreeLineID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                Cancel_ = input.Cancel_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },"");

            var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, productionOrders, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionOrdersChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }

        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateOccuredAmountEntryAsync(UpdateProductionOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductionOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.GetList<ProductionOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
            {
                BOMID = input.BOMID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                LinkedProductID = input.LinkedProductID.GetValueOrDefault(),
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                TechnicalDrawingUpdateDate_ = input.TechnicalDrawingUpdateDate_,
                TechnicalDrawingUpdateDescription_ = input.TechnicalDrawingUpdateDescription_,
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                OrderID = input.OrderID.GetValueOrDefault(),
                OrderLineID = input.OrderLineID.GetValueOrDefault(),
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductionOrderState = input.ProductionOrderState,
                BranchID = input.BranchID.GetValueOrDefault(),
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                ProductTreeID = input.ProductTreeID.GetValueOrDefault(),
                ProductTreeLineID = input.ProductTreeLineID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                Cancel_ = input.Cancel_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, productionOrders, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["ProductionOrdersChildMenu"],  L["ProductionOrderContextOccuredAmountEntry"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["ProductionOrderContextOccuredAmountEntry"],
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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
                            ContextMenuName_ = L["ProductionOrderContextOccuredAmountEntry"],
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }

        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateChangeTechDrawingAsync(UpdateProductionOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductionOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.GetList<ProductionOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
            {
                BOMID = input.BOMID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                FinishedProductID = input.FinishedProductID.GetValueOrDefault(),
                LinkedProductID = input.LinkedProductID.GetValueOrDefault(),
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                TechnicalDrawingUpdateDate_ = input.TechnicalDrawingUpdateDate_,
                TechnicalDrawingUpdateDescription_ = input.TechnicalDrawingUpdateDescription_,
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                OrderID = input.OrderID.GetValueOrDefault(),
                OrderLineID = input.OrderLineID.GetValueOrDefault(),
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductionOrderState = input.ProductionOrderState,
                BranchID = input.BranchID.GetValueOrDefault(),
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                ProductTreeID = input.ProductTreeID.GetValueOrDefault(),
                ProductTreeLineID = input.ProductTreeLineID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                PropositionLineID = input.PropositionLineID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                UnitSetID = input.UnitSetID.GetValueOrDefault(),
                Cancel_ = input.Cancel_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, productionOrders, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["ProductionOrdersChildMenu"], L["ProductionOrderContextChangeTechnicalDrawing"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["ProductionOrderContextChangeTechnicalDrawing"],
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.FicheNo,
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
                            ContextMenuName_ = L["ProductionOrderContextChangeTechnicalDrawing"],
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }

        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("Id").Where(new { Id = id }, "");
            var entity = queryFactory.Get<ProductionOrders>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
            {
                BOMID = entity.BOMID,
                FicheNo = entity.FicheNo,
                CurrentAccountID = entity.CurrentAccountID,
                TechnicalDrawingUpdateDescription_ = entity.TechnicalDrawingUpdateDescription_,
                TechnicalDrawingUpdateDate_ = entity.TechnicalDrawingUpdateDate_,
                TechnicalDrawingID = entity.TechnicalDrawingID,
                CustomerOrderNo = entity.CustomerOrderNo,
                FinishedProductID = entity.FinishedProductID,
                LinkedProductID = entity.LinkedProductID,
                LinkedProductionOrderID = entity.LinkedProductionOrderID,
                OrderID = entity.OrderID,
                OrderLineID = entity.OrderLineID,
                PlannedQuantity = entity.PlannedQuantity,
                ProducedQuantity = entity.ProducedQuantity,
                BranchID = entity.BranchID,
                WarehouseID = entity.WarehouseID,
                ProductionOrderState = (int)entity.ProductionOrderState,
                ProductTreeID = entity.ProductTreeID,
                ProductTreeLineID = entity.ProductTreeLineID,
                PropositionID = entity.PropositionID,
                PropositionLineID = entity.PropositionLineID,
                RouteID = entity.RouteID,
                UnitSetID = entity.UnitSetID,
                Cancel_ = entity.Cancel_,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

        }

        public async Task<IDataResult<IList<RawMaterialRequestFormReportDto>>> CreateRawMaterialRequestFormReportAsync(Guid productionOrderId)
        {
            List<RawMaterialRequestFormReportDto> reportSource = new List<RawMaterialRequestFormReportDto>();

            #region GetProduction Order

            var productionOrder = (await GetAsync(productionOrderId)).Data;

            if (productionOrder.Id != Guid.Empty)
            {
                var bom = (await BillsofMaterialsAppService.GetAsync(productionOrder.BOMID.GetValueOrDefault())).Data;

                foreach (var bomLine in bom.SelectBillsofMaterialLines)
                {
                    RawMaterialRequestFormReportDto r = new RawMaterialRequestFormReportDto();

                    switch (bomLine.MaterialType)
                    {
                        case ProductTypeEnum.TM:
                            r.StokTuru = "Ticari Mal";
                            break;
                        case ProductTypeEnum.HM:
                            r.StokTuru = "Hammadde";
                            break;
                        case ProductTypeEnum.YM:
                            r.StokTuru = "Yarı Mamül";
                            break;
                        case ProductTypeEnum.MM:
                            r.StokTuru = "Mamül";
                            break;
                        case ProductTypeEnum.BP:
                            break;
                        case ProductTypeEnum.TK:
                            break;
                        case ProductTypeEnum.KLP:
                            r.StokTuru = "Kalıp";
                            break;
                        case ProductTypeEnum.APRT:
                            r.StokTuru = "Aparat";
                            break;
                        default:
                            break;
                    }

                    r.StokKodu = bomLine.ProductCode;
                    r.StokAciklamasi = bomLine.ProductName;
                    r.VaryantKodu = productionOrder.CustomerCode;
                    r.Birim = bomLine.UnitSetCode;

                    var adresList = (await StockAddressesAppService.GetStockAddressByStockIdAsync(bomLine.ProductID.GetValueOrDefault())).Data.ToList();

                    string adres = "";

                    for (int a = 1; a <= adresList.Count; a++)
                    {
                        string ad = "";
                        ad = adresList[a - 1].StockSectionName + "-" + adresList[a - 1].StockShelfName + "-" + adresList[a - 1].StockColumnName + "-" + adresList[a - 1].StockNumberName;

                        if (a == 1)
                        {
                            adres = ad;
                        }
                        else
                        {
                            adres = adres + " / " + ad;
                        }
                    }

                    r.StokAdres = adres;

                    if (bomLine.Size==null || bomLine.Size==0)
                    {
                        r.Boy = 0;
                        r.Adet = productionOrder.PlannedQuantity * bomLine.Quantity;
                        r.Aciklama = bomLine._Description;
                    }
                    else
                    {
                        var lineProduct = (await ProductsAppService.GetAsync(bomLine.ProductID.GetValueOrDefault())).Data;

                        r.Boy = bomLine.Size;

                        if(lineProduct.Id != Guid.Empty)
                        {
                            decimal testereBoyFire = lineProduct.SawWastage;
                            r.Boy += testereBoyFire;
                            decimal stokBoyu = lineProduct.ProductSize;
                            decimal tamSayi = Math.Floor(stokBoyu / r.Boy);

                            r.Boy = productionOrder.PlannedQuantity / tamSayi;

                            if (r.StokKodu.StartsWith("HB"))
                            {
                                r.Adet = (r.Boy * stokBoyu) / 1000;
                                r.Aciklama = "1 Boy = " + string.Format("{0:0}", stokBoyu) + " mm";
                            }
                            else if (r.StokKodu.StartsWith("HS"))
                            {
                                r.Adet = productionOrder.PlannedQuantity * bomLine.Quantity;
                            }
                            else
                            {

                                decimal ozkutle = (await ProductionManagementParametersService.GetProductionManagementParametersAsync()).Data.Density_;


                                decimal cap = lineProduct.RadiusValue;
                                double pi = 3.14;
                                decimal rr = Convert.ToDecimal(cap) / 2;
                                decimal kg = ((decimal)pi * (rr * rr) * stokBoyu * ozkutle) / 1000000;


                                r.Adet = r.Boy * kg;
                                r.Aciklama = "1 Boy = " + string.Format("{0:0}", stokBoyu) + " mm";
                            }

                            r.UretimEmriNo = productionOrder.FicheNo;
                            r.AnaUrunAdi = productionOrder.FinishedProductName;
                            r.TeminSekli = bomLine.SupplyForm == ProductSupplyFormEnum.Üretim ? "Üretim" : "Satınalma";
                            reportSource.Add(r);
                        }
                    }
                }
            }

            #endregion


            await Task.CompletedTask;
            return new SuccessDataResult<IList<RawMaterialRequestFormReportDto>>(reportSource);
        }
    }
}
