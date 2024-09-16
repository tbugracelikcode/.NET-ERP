using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.MRP
{
    public partial class MRPsListPage : IDisposable
    {
        private SfGrid<SelectMRPLinesDto> _LineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }

        SelectMRPLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMRPLinesDto> GridLineList = new List<SelectMRPLinesDto>();

        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();

        List<SupplierSelectionGrid> SupplierSelectionList = new List<SupplierSelectionGrid>();

        private bool LineCrudPopup = false;

        public bool SupplierSelectionPopup = false;

        public class SupplierSelectionGrid
        {
            public string ProductCode { get; set; }

            public decimal UnitPrice { get; set; }

            public Guid? CurrentAccountID { get; set; }

            public string CurrentAccountName { get; set; }

            public Guid? CurrenyID { get; set; }

            public string CurrenyCode { get; set; }

            public int SupplyDate { get; set; }
        }

        public bool PurchaseReservedQuantityModalVisible = false;

        #region Planning Parameters

        int MRPPurchaseTransaction;
        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;
        Guid? BranchIDButtonEdit;
        Guid? WarehouseIDButtonEdit;
        string WarehouseCodeButtonEdit;
        string BranchCodeButtonEdit;

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = MRPsService;
            _L = L;

            var purchaseParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            MRPPurchaseTransaction = (await PlanningManagementParametersAppService.GetPlanningManagementParametersAsync()).Data.MRPPurchaseTransaction;
            BranchIDParameter = purchaseParameter.BranchID;
            WarehouseIDParameter = purchaseParameter.WarehouseID;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "MRPChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region MRP Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            BranchCodeButtonEdit = string.Empty;
            BranchIDButtonEdit = Guid.Empty;
            WarehouseCodeButtonEdit = string.Empty;
            WarehouseIDButtonEdit = Guid.Empty;

            DataSource = new SelectMRPsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu"),
                MaintenanceMRPID = Guid.Empty,
                ReferanceDate = GetSQLDateAppService.GetDateFromSQL().Date,
                IsMaintenanceMRP = false,

            };
            await GetSalesOrdersList();
            DataSource.SelectMRPLines = new List<SelectMRPLinesDto>();
            GridLineList = DataSource.SelectMRPLines;
            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                //LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDelete"], Id = "delete" });

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MRPLineContextDoNotCalculate":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDoNotCalculate"], Id = "dontcalculate" }); break;
                            case "MRPLineContextDeleteOrderLines":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDeleteOrderLines"], Id = "deleteorderlines" }); break;
                            case "MRPLineContextStockUsage":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextStockUsage"], Id = "stockusage" }); break;
                            case "MRPLineContextReservePurchase":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextReservePurchase"], Id = "reservepurchase" }); break;
                            case "MRPLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextChange"], Id = "changed" }); break;
                            case "MRPLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextRefresh"], Id = "refresh" }); break;
                            case "MRPLineContextSupplier":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextSupplier"], Id = "supplier" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MRPContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextAdd"], Id = "new" }); break;
                            case "MRPContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextChange"], Id = "changed" }); break;
                            case "MRPContextConvertPurchase":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextConvertPurchase"], Id = "convertpurchase" }); break;
                            case "MRPContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextDelete"], Id = "delete" }); break;
                            case "MRPContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMRPsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    BindingSalesOrders.Clear();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                    BindingSalesOrders.Clear();
                    DataSource = (await MRPsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMRPLines;

                    SalesOrdersList.Clear();

                    var salesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();

                    foreach (var salesOrder in salesOrdersList)
                    {
                        if (!GridLineList.Any(t => t.SalesOrderID.GetValueOrDefault() == salesOrder.Id))
                        {
                            SalesOrdersList.Add(salesOrder);
                        }
                    }


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "convertpurchase":

                    if (args.RowInfo.RowData != null)
                    {

                        var purcres = await ModalManager.ConfirmationAsync(L["UIConvertPurchaseTitle"], L["UIConvertPurchaseMessage"]);
                    if (purcres == true)
                    {

                        DataSource = (await MRPsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                        var mrpLineList = DataSource.SelectMRPLines;

                        if (mrpLineList.Any(t => t.WarehouseID == null || t.WarehouseID == Guid.Empty || t.BranchID == null || t.BranchID == Guid.Empty))
                        {
                            SpinnerService.Hide();
                            await ModalManager.WarningPopupAsync(L["UIWarningConvertPurchaseTitle"], L["UIWarningConvertPurchaseMessage"]);
                        }
                        else
                        {


                            List<SelectStockFicheLinesDto> stockFicheLineList = new List<SelectStockFicheLinesDto>();

                            if (MRPPurchaseTransaction == 1) //PurchaseOrder
                            {
                                var groupedList2 = DataSource.SelectMRPLines.GroupBy(t => new { t.BranchID, t.WarehouseID, t.CurrencyID, t.CurrentAccountCardID }, (key, group) => new
                                {
                                    BranchID = key.BranchID,
                                    WarehouseID = key.WarehouseID,
                                    CurrencyID = key.CurrencyID,
                                    CurrentAccountCardID = key.CurrentAccountCardID,
                                    Data = group.ToList()
                                });

                                List<SelectPurchaseOrderLinesDto> purchaseOrderLineList = new List<SelectPurchaseOrderLinesDto>();

                                Guid? BranchIDData = Guid.Empty;
                                Guid? WarehouseIDData = Guid.Empty;

                                foreach (var item in groupedList2.ToList())
                                {
                                    foreach (var data in item.Data)
                                    {
                                        var product = (await ProductsAppService.GetAsync(data.ProductID.GetValueOrDefault())).Data;

                                        DateTime? supplyDate = null;

                                        if (data.RequirementAmount == 0)
                                        {
                                            supplyDate = GetSQLDateAppService.GetDateFromSQL();
                                        }

                                        SelectPurchaseOrderLinesDto purchaseOrderLineModel = new SelectPurchaseOrderLinesDto
                                        {
                                            DiscountAmount = 0,
                                            DiscountRate = 0,
                                            PartyNo = string.Empty,
                                            ExchangeRate = 0,
                                            LikedPurchaseRequestLineID = Guid.Empty,
                                            LineAmount = 0,
                                            LineNr = purchaseOrderLineList.Count + 1,
                                            LineDescription = string.Empty,
                                            LineTotalAmount = 0,
                                            LinkedPurchaseRequestID = Guid.Empty,
                                            OrderAcceptanceID = data.OrderAcceptanceID.GetValueOrDefault(),
                                            OrderAcceptanceLineID = data.OrderAcceptanceLineID.GetValueOrDefault(),
                                            WorkOrderCreationDate = null,
                                            VATrate = 0,
                                            VATamount = 0,
                                            UnitSetID = product.UnitSetID,
                                            UnitSetCode = product.UnitSet,
                                            UnitPrice = 0,
                                            Quantity = data.RequirementAmount,
                                            PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Beklemede,
                                            PurchaseOrderID = Guid.Empty,
                                            ProductName = product.Name,
                                            ProductionOrderID = Guid.Empty,
                                            ProductionOrderFicheNo = string.Empty,
                                            ProductID = product.Id,
                                            ProductCode = product.Code,
                                            PaymentPlanID = Guid.Empty,
                                            PaymentPlanName = string.Empty,
                                            SupplyDate = supplyDate
                                        };

                                        BranchIDData = data.BranchID;
                                        WarehouseIDData = data.WarehouseID;
                                        purchaseOrderLineList.Add(purchaseOrderLineModel);

                                        if (data.ReservedAmount + data.PurchaseReservedAmount > 0)
                                        {
                                            SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                                            {
                                                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                                FicheType = Entities.Enums.StockFicheTypeEnum.StokRezerveFisi,
                                                LineNr = stockFicheLineList.Count + 1,
                                                UnitSetID = data.UnitSetID,
                                                PartyNo = string.Empty,
                                                ProductionOrderID = Guid.Empty,
                                                InputOutputCode = 0,
                                                UnitSetCode = data.UnitSetCode,
                                                UnitPrice = data.UnitPrice,
                                                UnitOutputCost = 0,
                                                Quantity = data.ReservedAmount + data.PurchaseReservedAmount,
                                                MRPLineID = data.Id,
                                                MRPID = DataSource.Id,
                                                ProductID = data.ProductID,
                                                ProductCode = data.ProductCode,
                                                LineAmount = data.UnitPrice * data.RequirementAmount,
                                                PurchaseOrderLineID = Guid.Empty,
                                                PurchaseOrderID = Guid.Empty,
                                                PurchaseOrderFicheNo = string.Empty,
                                                LineDescription = string.Empty,
                                                ProductionDateReferance = string.Empty,
                                                ProductName = data.ProductName,
                                            };
                                            stockFicheLineList.Add(stockFicheLineModel);
                                        }




                                    }

                                    CreatePurchaseOrdersDto purchaseOrderModel = new CreatePurchaseOrdersDto
                                    {
                                        WorkOrderCreationDate = null,
                                        BranchID = BranchIDData,
                                        WarehouseID = WarehouseIDData,
                                        MaintenanceMRPID = DataSource.MaintenanceMRPID,
                                        TotalVatExcludedAmount = 0,
                                        CurrencyID = Guid.Empty,
                                        MRPID = DataSource.Id,
                                        OrderAcceptanceID = DataSource.OrderAcceptanceID.GetValueOrDefault(),
                                        CurrentAccountCardID = Guid.Empty,
                                        ShippingAdressID = Guid.Empty,
                                        TotalDiscountAmount = 0,
                                        TotalVatAmount = 0,
                                        Time_ = string.Empty,
                                        SpecialCode = string.Empty,
                                        PurchaseOrderState = 1,
                                        ProductionOrderID = Guid.Empty,
                                        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                        Description_ = string.Empty,
                                        GrossAmount = 0,
                                        PaymentPlanID = Guid.Empty,
                                        NetAmount = 0,
                                        LinkedPurchaseRequestID = Guid.Empty,
                                        ExchangeRate = 0,
                                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu")
                                    };

                                    purchaseOrderModel.SelectPurchaseOrderLinesDto = purchaseOrderLineList;

                                    var purchaseOrder = await PurchaseOrdersAppService.ConvertToPurchaseOrderMRPAsync(purchaseOrderModel);

                                    CreateStockFichesDto stockFicheModel = new CreateStockFichesDto
                                    {
                                        BranchID = BranchIDData,
                                        WarehouseID = WarehouseIDData,
                                        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                        Description_ = string.Empty,
                                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                                        ExchangeRate = 0,
                                        Time_ = null,
                                        FicheType = 55,
                                        SpecialCode = string.Empty,
                                        PurchaseOrderID = purchaseOrder.Data.Id,
                                        PurchaseRequestID = Guid.Empty,
                                        ProductionOrderID = Guid.Empty,
                                        ProductionDateReferance = string.Empty,
                                        InputOutputCode = 1,
                                        NetAmount = stockFicheLineList.Select(t => t.LineAmount).Sum(),
                                        CurrencyID = Guid.Empty,
                                    };

                                    stockFicheModel.SelectStockFicheLines = stockFicheLineList;

                                    await StockFichesAppService.CreateAsync(stockFicheModel);
                                }


                            }

                            else if (MRPPurchaseTransaction == 2) //PurchaseRequest
                            {
                                var groupedList2 = DataSource.SelectMRPLines.GroupBy(t => new { t.BranchID, t.WarehouseID }, (key, group) => new
                                {
                                    BranchID = key.BranchID,
                                    WarehouseID = key.WarehouseID,
                                    Data = group.ToList()
                                });

                                List<SelectPurchaseRequestLinesDto> purchaseRequestLineList = new List<SelectPurchaseRequestLinesDto>();

                                Guid? BranchIDData = Guid.Empty;
                                Guid? WarehouseIDData = Guid.Empty;

                                foreach (var item in groupedList2.ToList())
                                {
                                    foreach (var data in item.Data)
                                    {
                                        var product = (await ProductsAppService.GetAsync(data.ProductID.GetValueOrDefault())).Data;

                                        SelectPurchaseRequestLinesDto purchaseRequestLineModel = new SelectPurchaseRequestLinesDto
                                        {
                                            DiscountAmount = 0,
                                            DiscountRate = 0,
                                            ExchangeRate = 0,
                                            LineAmount = 0,
                                            LineNr = purchaseRequestLineList.Count + 1,
                                            LineDescription = string.Empty,
                                            LineTotalAmount = 0,
                                            VATrate = 0,
                                            VATamount = 0,
                                            UnitSetID = product.UnitSetID,
                                            UnitSetCode = product.UnitSet,
                                            UnitPrice = 0,
                                            Quantity = data.RequirementAmount,
                                            ProductName = product.Name,
                                            ProductionOrderID = Guid.Empty,
                                            ProductionOrderFicheNo = string.Empty,
                                            ProductID = product.Id,
                                            ProductCode = product.Code,
                                            PaymentPlanID = Guid.Empty,
                                            PaymentPlanName = string.Empty,
                                            PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Beklemede,
                                            PurchaseRequestID = Guid.Empty,
                                            OrderConversionDate = null,

                                        };

                                        BranchIDData = data.BranchID;
                                        WarehouseIDData = data.WarehouseID;
                                        purchaseRequestLineList.Add(purchaseRequestLineModel);

                                        if (data.ReservedAmount + data.PurchaseReservedAmount > 0)
                                        {
                                            SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                                            {
                                                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                                FicheType = Entities.Enums.StockFicheTypeEnum.StokRezerveFisi,
                                                LineNr = stockFicheLineList.Count + 1,
                                                UnitSetID = data.UnitSetID,
                                                UnitSetCode = data.UnitSetCode,
                                                UnitPrice = data.UnitPrice,
                                                UnitOutputCost = 0,
                                                Quantity = data.ReservedAmount + data.PurchaseReservedAmount,
                                                MRPLineID = data.Id,
                                                InputOutputCode = 0,
                                                MRPID = DataSource.Id,
                                                ProductID = data.ProductID,
                                                ProductCode = data.ProductCode,
                                                LineAmount = data.UnitPrice * data.RequirementAmount,
                                                PurchaseOrderLineID = Guid.Empty,
                                                PurchaseOrderID = Guid.Empty,
                                                PurchaseOrderFicheNo = string.Empty,
                                                LineDescription = string.Empty,
                                                ProductionDateReferance = string.Empty,
                                                ProductName = data.ProductName,
                                            };
                                            stockFicheLineList.Add(stockFicheLineModel);
                                        }
                                    }

                                    CreatePurchaseRequestsDto purchaseRequestModel = new CreatePurchaseRequestsDto
                                    {
                                        BranchID = BranchIDData,
                                        WarehouseID = WarehouseIDData,
                                        TotalVatExcludedAmount = 0,
                                        CurrencyID = Guid.Empty,
                                        MRPID = DataSource.Id,
                                        CurrentAccountCardID = Guid.Empty,
                                        ValidityDate_ = GetSQLDateAppService.GetDateFromSQL(),
                                        RevisionTime = string.Empty,
                                        RevisionDate = null,
                                        PurchaseRequestState = 1,
                                        PropositionRevisionNo = string.Empty,
                                        TotalDiscountAmount = 0,
                                        TotalVatAmount = 0,
                                        Time_ = string.Empty,
                                        SpecialCode = string.Empty,
                                        ProductionOrderID = Guid.Empty,
                                        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                        Description_ = string.Empty,
                                        GrossAmount = 0,
                                        PaymentPlanID = Guid.Empty,
                                        NetAmount = 0,
                                        LinkedPurchaseRequestID = Guid.Empty,
                                        ExchangeRate = 0,
                                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseRequestsChildMenu")
                                    };

                                    purchaseRequestModel.SelectPurchaseRequestLines = purchaseRequestLineList;

                                    var purchaseRequest = await PurchaseRequestsAppService.ConvertToPurchaseRequestMRPAsync(purchaseRequestModel);

                                    CreateStockFichesDto stockFicheModel = new CreateStockFichesDto
                                    {
                                        BranchID = BranchIDData,
                                        WarehouseID = WarehouseIDData,
                                        Date_ = GetSQLDateAppService.GetDateFromSQL(),
                                        Description_ = string.Empty,
                                        FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                                        ExchangeRate = 0,
                                        Time_ = null,
                                        FicheType = 55,
                                        SpecialCode = string.Empty,
                                        PurchaseRequestID = purchaseRequest.Data.Id,
                                        PurchaseOrderID = Guid.Empty,
                                        ProductionOrderID = Guid.Empty,
                                        ProductionDateReferance = string.Empty,
                                        InputOutputCode = 1,
                                        NetAmount = stockFicheLineList.Select(t => t.LineAmount).Sum(),
                                        CurrencyID = Guid.Empty,
                                    };

                                    stockFicheModel.SelectStockFicheLines = stockFicheLineList;

                                    await StockFichesAppService.CreateAsync(stockFicheModel);
                                }
                            }
                        }

                    }
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _grid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMRPLinesDto> args)
        {
            switch (args.Item.Id)
            {
                #region Delete Case
                //case "delete":

                //    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                //    if (res == true)
                //    {
                //        var line = args.RowInfo.RowData;

                //        if (line.Id == Guid.Empty)
                //        {
                //            DataSource.SelectMRPLines.Remove(args.RowInfo.RowData);
                //        }
                //        else
                //        {
                //            if (line != null)
                //            {
                //                await DeleteAsync(args.RowInfo.RowData.Id);
                //                DataSource.SelectMRPLines.Remove(line);
                //                await GetListDataSourceAsync();
                //            }
                //            else
                //            {
                //                DataSource.SelectMRPLines.Remove(line);
                //            }
                //        }

                //        await _LineGrid.Refresh();
                //        GetTotal();
                //        await InvokeAsync(StateHasChanged);
                //    }

                //    break;
                #endregion

                case "dontcalculate":
                    if (args.RowInfo.RowData != null)
                    {

                        var line = args.RowInfo.RowData;
                    var selectedIndex = GridLineList.FindIndex(t => t.SalesOrderID == line.SalesOrderID && t.ProductID == line.ProductID);
                    if (selectedIndex >= 0)
                    {
                        GridLineList[selectedIndex].isCalculated = false;
                    }

                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "deleteorderlines":
                    if (args.RowInfo.RowData != null)
                    {

                        var selectedSalesOrderId = args.RowInfo.RowData.SalesOrderID;

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationSalesOrderLineBase"]);

                    if (res == true)
                    {
                        var notdeletedList = GridLineList.Where(t => t.SalesOrderID != selectedSalesOrderId).ToList();
                        GridLineList = notdeletedList;
                        await _LineGrid.Refresh();

                    }
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "stockusage":
                    if (args.RowInfo.RowData != null)
                    {

                        var selectedline = args.RowInfo.RowData;
                    var Index = GridLineList.IndexOf(selectedline);
                    if (Index >= 0)
                    {
                        GridLineList[Index].isStockUsage = !selectedline.isStockUsage;

                        if (GridLineList[Index].isStockUsage) // Stoktan kullanılacaksa
                        {
                            var stockAmount = GridLineList[Index].AmountOfStock;
                            var bomAmount = GridLineList[Index].Amount;
                            var purchaseAmount = GridLineList[Index].RequirementAmount;
                            var reservedAmount = GridLineList[Index].ReservedAmount;

                            if (stockAmount > bomAmount) // stok miktarı > reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                GridLineList[Index].RequirementAmount = 0;
                                GridLineList[Index].ReservedAmount = bomAmount;
                                GridLineList[Index].AmountOfStock = stockAmount - bomAmount;
                            }
                            else if (stockAmount < bomAmount) // stok miktarı < reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                GridLineList[Index].AmountOfStock = 0;
                                GridLineList[Index].RequirementAmount = bomAmount - stockAmount;
                                GridLineList[Index].ReservedAmount = stockAmount;
                            }
                            else if (stockAmount == bomAmount) // stok miktarı = reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                GridLineList[Index].AmountOfStock = 0;
                                GridLineList[Index].RequirementAmount = 0;
                                GridLineList[Index].ReservedAmount = stockAmount;
                            }
                        }

                        else // Stoktan kullanılmayacaksa
                        {
                            GridLineList[Index].RequirementAmount = GridLineList[Index].Amount;
                            GridLineList[Index].ReservedAmount = 0;
                        }
                    }

                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "reservepurchase":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;

                    PurchaseReservedQuantityModalVisible = true;

                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "supplier":

                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;

                    SupplierSelectionList.Clear();

                    var purchasePriceLinesList = (await PurchasePricesAppService.GetSelectLineListAsync(LineDataSource.ProductID.GetValueOrDefault())).Data.ToList();

                    if (purchasePriceLinesList != null && purchasePriceLinesList.Count > 0)
                    {
                        var tempPurchasePriceLines = purchasePriceLinesList.Select(t => t.PurchasePriceID).Distinct();

                        foreach (var purchasePriceID in tempPurchasePriceLines)
                        {
                            var purchasePrice = (await PurchasePricesAppService.GetAsync(purchasePriceID)).Data;

                            if (purchasePrice != null && purchasePriceID != Guid.Empty && purchasePrice.IsApproved == false)
                            {
                                purchasePriceLinesList = purchasePriceLinesList.Where(t => t.PurchasePriceID != purchasePriceID).ToList();
                                //Onaylı olmayan fiyat kayıtlarına ait satırları yok etme
                            }
                        }

                        var groupedPurchasePriceLineList = purchasePriceLinesList.GroupBy(t => new { t.CurrencyID, t.CurrentAccountCardID }, (key, group) => new { CurrencyID = key.CurrencyID, CurrentAccountCardID = key.CurrentAccountCardID, Data = group.ToList() });

                        foreach (var item in groupedPurchasePriceLineList)
                        {
                            foreach (var data in item.Data)
                            {
                                SupplierSelectionGrid supplierSelectionModel = new SupplierSelectionGrid
                                {
                                    CurrentAccountName = data.CurrentAccountCardName,
                                    CurrentAccountID = data.CurrentAccountCardID,
                                    CurrenyCode = data.CurrencyCode,
                                    CurrenyID = data.CurrencyID,
                                    ProductCode = data.ProductCode,
                                    UnitPrice = data.Price,
                                    SupplyDate = data.SupplyDateDay
                                };

                                SupplierSelectionList.Add(supplierSelectionModel);
                            }
                        }

                        SupplierSelectionPopup = true;

                    }

                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        public void HideSupplierSelectionPopup()
        {
            SupplierSelectionList.Clear();
            SupplierSelectionPopup = false;
        }

        public async void SupplierDoubleClickHandler(RecordDoubleClickEventArgs<SupplierSelectionGrid> args)
        {
            var selectedSupplier = args.RowData;

            if (selectedSupplier != null)
            {
                LineDataSource.CurrencyID = selectedSupplier.CurrenyID;
                LineDataSource.CurrencyCode = selectedSupplier.CurrenyCode;
                LineDataSource.CurrentAccountCardID = selectedSupplier.CurrentAccountID;
                LineDataSource.CurrentAccountCardName = selectedSupplier.CurrentAccountName;
                LineDataSource.UnitPrice = selectedSupplier.UnitPrice;
                LineDataSource.SupplyDate = LineDataSource.SupplyDate.AddDays(selectedSupplier.SupplyDate);
                HideSupplierSelectionPopup();
                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public void ReferanceDateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            if (GridLineList != null && GridLineList.Count > 0)
            {
                foreach (var line in GridLineList)
                {
                    int index = GridLineList.IndexOf(line);
                    GridLineList[index].SupplyDate = args.Value;
                }

                _LineGrid.Refresh();
            }
        }

        public async void AddSelectedSalesOrderButtonClicked()
        {
            var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;
            var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

            foreach (var orderId in BindingSalesOrders)
            {
                if (!GridLineList.Any(t => t.SalesOrderID.GetValueOrDefault() == orderId))
                {
                    var salesOrder = (await SalesOrdersAppService.GetAsync(orderId)).Data;
                    var salesOrderLineList = salesOrder.SelectSalesOrderLines.ToList();

                    foreach (var orderline in salesOrderLineList)
                    {

                        var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(orderline.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                        foreach (var bomline in bomLineList)
                        {

                            var product = (await ProductsAppService.GetAsync(bomline.ProductID.GetValueOrDefault())).Data;

                            if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Satınalma)
                            {
                                int calculatedAmount = Convert.ToInt32(orderline.Quantity * bomline.Quantity);

                                decimal amountofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomline.ProductID).Select(t => t.Amount).Sum();

                                SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                                {
                                    Amount = calculatedAmount,
                                    isCalculated = true,
                                    isPurchase = false,
                                    ProductID = bomline.ProductID.GetValueOrDefault(),
                                    MRPID = DataSource.Id,
                                    ProductCode = bomline.ProductCode,
                                    ProductName = bomline.ProductName,
                                    SalesOrderID = orderId,
                                    UnitSetID = bomline.UnitSetID,
                                    LineNr = GridLineList.Count + 1,
                                    UnitSetCode = bomline.UnitSetCode,
                                    AmountOfStock = amountofProduct,
                                    RequirementAmount = Math.Abs(Convert.ToInt32(amountofProduct) - calculatedAmount),
                                    SalesOrderLineID = orderline.Id,
                                    SalesOrderFicheNo = salesOrder.FicheNo,
                                    BranchID = branch.Id,
                                    WarehouseID = warehouse.Id,
                                    BranchCode = branch.Code,
                                    WarehouseCode = warehouse.Code,
                                };

                                GridLineList.Add(mrpLineModel);
                            }
                            else if(product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Üretim)
                            {
                                var bomLineProductionList = (await BillsofMaterialsAppService.GetbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                                foreach(var bomLineProduction in bomLineProductionList)
                                {
                                    var productProduction = (await ProductsAppService.GetAsync(bomLineProduction.ProductID.GetValueOrDefault())).Data;

                                    if(productProduction.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Satınalma)
                                    {
                                        int calculatedAmountProduction = Convert.ToInt32(bomLineProduction.Quantity * bomline.Quantity);

                                        decimal amountofProductProduction = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomLineProduction.ProductID).Select(t => t.Amount).Sum();


                                        SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                                        {
                                            Amount = calculatedAmountProduction,
                                            isCalculated = true,
                                            isPurchase = false,
                                            ProductID = bomLineProduction.ProductID.GetValueOrDefault(),
                                            MRPID = DataSource.Id,
                                            ProductCode = bomLineProduction.ProductCode,
                                            ProductName = bomLineProduction.ProductName,
                                            SalesOrderID = orderId,
                                            UnitSetID = bomLineProduction.UnitSetID,
                                            LineNr = GridLineList.Count + 1,
                                            UnitSetCode = bomLineProduction.UnitSetCode,
                                            AmountOfStock = amountofProductProduction,
                                            RequirementAmount = Math.Abs(Convert.ToInt32(amountofProductProduction) - calculatedAmountProduction),
                                            SalesOrderLineID = orderline.Id,
                                            SalesOrderFicheNo = salesOrder.FicheNo,
                                            BranchID = branch.Id,
                                            WarehouseID = warehouse.Id,
                                            BranchCode = branch.Code,
                                            WarehouseCode = warehouse.Code,
                                        };

                                        GridLineList.Add(mrpLineModel);
                                    }
                                }
                            }

                        }
                    }

                    await _LineGrid.Refresh();
                }
            }
        }



        #region SalesOrders

        List<Guid> BindingSalesOrders = new List<Guid>();

        private async Task GetSalesOrdersList()
        {
            SalesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();
        }

        #endregion


        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectMRPLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectMRPLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectMRPLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectMRPLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectMRPLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectMRPLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectMRPLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public void HidePurchaseReservedQuantity()
        {
            PurchaseReservedQuantityModalVisible = false;
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Depo ButtonEdit (Tüm LineList)

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehousesOnCreateIcon()
        {
            var WarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehousesButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                WarehouseIDButtonEdit = Guid.Empty;
                WarehouseCodeButtonEdit = string.Empty;



                var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

                foreach (var item in GridLineList)
                {
                    item.WarehouseID = warehouse.Id;
                    item.WarehouseCode = warehouse.Code;
                }
                await _LineGrid.Refresh();
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                WarehouseIDButtonEdit = selectedWarehouse.Id;
                WarehouseCodeButtonEdit = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;

                foreach (var item in GridLineList)
                {
                    item.WarehouseID = WarehouseIDButtonEdit;
                    item.WarehouseCode = WarehouseCodeButtonEdit;
                }

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit (Tüm LineList)

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchesOnCreateIcon()
        {
            var BranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchesButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                BranchIDButtonEdit = Guid.Empty;
                BranchCodeButtonEdit = string.Empty;

                var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;

                foreach (var item in GridLineList)
                {
                    item.BranchID = branch.Id;
                    item.BranchCode = branch.Code;
                }
                await _LineGrid.Refresh();
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                BranchIDButtonEdit = selectedBranch.Id;
                BranchCodeButtonEdit = selectedBranch.Code;

                foreach (var item in GridLineList)
                {
                    item.BranchID = BranchIDButtonEdit;
                    item.BranchCode = BranchCodeButtonEdit;
                }

                await _LineGrid.Refresh();
                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Depo ButtonEdit

        SfTextBox LineWarehousesButtonEdit;
        bool SelectLineWarehousesPopupVisible = false;
        List<ListWarehousesDto> LineWarehousesList = new List<ListWarehousesDto>();

        public async Task LineWarehousesOnCreateIcon()
        {
            var LineWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineWarehousesButtonClickEvent);
            await LineWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineWarehousesButtonClick } });
        }

        public async void LineWarehousesButtonClickEvent()
        {
            SelectLineWarehousesPopupVisible = true;
            LineWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void LineWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.WarehouseID = Guid.Empty;
                LineDataSource.WarehouseCode = string.Empty;
            }

            await Task.CompletedTask;
        }

        public async void LineWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                LineDataSource.WarehouseID = selectedWarehouse.Id;
                LineDataSource.WarehouseCode = selectedWarehouse.Code;
                SelectLineWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit 

        SfTextBox LineBranchesButtonEdit;
        bool SelectLineBranchesPopupVisible = false;
        List<ListBranchesDto> LineBranchesList = new List<ListBranchesDto>();

        public async Task LineBranchesOnCreateIcon()
        {
            var LineBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineBranchesButtonClickEvent);
            await LineBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineBranchesButtonClick } });
        }

        public async void LineBranchesButtonClickEvent()
        {
            SelectLineBranchesPopupVisible = true;
            LineBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void LineBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.BranchID = Guid.Empty;
                LineDataSource.BranchCode = string.Empty;
            }


            await Task.CompletedTask;
        }

        public async void LineBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                LineDataSource.BranchID = selectedBranch.Id;
                LineDataSource.BranchCode = selectedBranch.Code;
                SelectLineBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
