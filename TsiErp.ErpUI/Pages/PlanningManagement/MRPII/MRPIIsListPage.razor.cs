using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.MRPII
{
    public partial class MRPIIsListPage : IDisposable
    {
        private SfGrid<SelectMRPIILinesDto> _LineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectMRPIILinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectMRPIILinesDto> GridLineList = new List<SelectMRPIILinesDto>();

        private bool LineCrudPopup = false;

        #region Planning Parameters

        int MRPIISourceModule;

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = MRPIIsService;
            _L = L;


            MRPIISourceModule = (await PlanningManagementParametersAppService.GetPlanningManagementParametersAsync()).Data.MRPIISourceModule;

            if (MRPIISourceModule == 1) // OrderAcceptanceRecords
            {
                await GetOrderAcceptances();
            }
            else if (MRPIISourceModule == 2) // SalesOrders
            {
                await GetSalesOrders();
            }

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "MRPIIChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region MRPII Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectMRPIIsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("MRPIIChildMenu"),
                CalculationDate = GetSQLDateAppService.GetDateFromSQL().Date
            };

            DataSource.SelectMRPIILines = new List<SelectMRPIILinesDto>();
            GridLineList = DataSource.SelectMRPIILines;
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

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MRPIILineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextChange"], Id = "changed" }); break;
                            case "MRPIILineContextCalculate":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextCalculate"], Id = "calculate" }); break;
                            case "MRPIILineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIILineContextRefresh"], Id = "refresh" }); break;
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
                            case "MRPIIsContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextAdd"], Id = "new" }); break;
                            case "MRPIIsContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextChange"], Id = "changed" }); break;
                            case "MRPIIsContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextDelete"], Id = "delete" }); break;
                            case "MRPIIsContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPIIsContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListMRPIIsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                    DataSource = (await MRPIIsService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectMRPIILines;

                    ShowEditPage();
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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectMRPIILinesDto> args)
        {
            switch (args.Item.Id)
            {
                #region Unused Part


                //case "delete":

                //    var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                //    if (res == true)
                //    {
                //        var line = args.RowInfo.RowData;

                //        if (line.Id == Guid.Empty)
                //        {
                //            DataSource.SelectMRPIILines.Remove(args.RowInfo.RowData);
                //        }
                //        else
                //        {
                //            if (line != null)
                //            {
                //                await DeleteAsync(args.RowInfo.RowData.Id);
                //                DataSource.SelectMRPIILines.Remove(line);
                //                await GetListDataSourceAsync();
                //            }
                //            else
                //            {
                //                DataSource.SelectMRPIILines.Remove(line);
                //            }
                //        }

                //        await _LineGrid.Refresh();
                //        GetTotal();
                //        await InvokeAsync(StateHasChanged);
                //    }

                //    break;

                #endregion

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "calculate":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;

                    if(MRPIISourceModule == 1) //Order Acceptance
                    {
                        var orderAcceptanceLineAmount = (await OrderAcceptanceRecordsAppService.GetAsync(LineDataSource.OrderAcceptanceID.GetValueOrDefault())).Data.SelectOrderAcceptanceRecordLines.Where(t => t.ProductID == LineDataSource.ProductID).Select(t => t.OrderAmount).FirstOrDefault();

                        var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(LineDataSource.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                        if (bomLineList != null && bomLineList.Count > 0)
                        {
                            decimal TotalOperationTime = 0;

                            foreach (var bomLine in bomLineList)
                            {
                                #region Tahmini Bitiş Tarihi

                                Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                                if (productsOperationID != Guid.Empty)
                                {
                                    var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                    if (productsOperationLineList != null && productsOperationLineList.Count > 0)
                                    {
                                        TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * orderAcceptanceLineAmount;
                                    }
                                }

                                LineDataSource.EstimatedProductionEndDate = LineDataSource.ReferanceDate.AddSeconds((double)TotalOperationTime);


                                #endregion

                            }
                        }

                    }

                    else if (MRPIISourceModule == 2) //Sales Orders
                    {
                        var salesOrderLineAmount = (await SalesOrdersAppService.GetAsync(LineDataSource.SalesOrderID.GetValueOrDefault())).Data.SelectSalesOrderLines.Where(t => t.ProductID == LineDataSource.ProductID).Select(t => t.Quantity).FirstOrDefault();

                        var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(LineDataSource.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                        if (bomLineList != null && bomLineList.Count > 0)
                        {
                            decimal TotalOperationTime = 0;

                            foreach (var bomLine in bomLineList)
                            {
                                #region Tahmini Bitiş Tarihi

                                Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                                if (productsOperationID != Guid.Empty)
                                {
                                    var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                    if (productsOperationLineList != null && productsOperationLineList.Count > 0)
                                    {
                                        TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * salesOrderLineAmount;
                                    }
                                }

                                LineDataSource.EstimatedProductionEndDate = LineDataSource.ReferanceDate.AddSeconds((double)TotalOperationTime);


                                #endregion

                            }
                        }

                    }

                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public void HideLinesPopup()
        {
            LineCrudPopup = false;
        }

        protected async Task OnLineSubmit()
        {

            if (LineDataSource.Id == Guid.Empty)
            {
                if (DataSource.SelectMRPIILines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectMRPIILines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectMRPIILines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectMRPIILines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectMRPIILines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectMRPIILines[selectedLineIndex] = LineDataSource;
                    DataSource.SelectMRPIILines[selectedLineIndex].EstimatedProductionEndDate = DateTime.MinValue;
                    DataSource.SelectMRPIILines[selectedLineIndex].EstimatedProductionStartDate = LineDataSource.ReferanceDate;
                    DataSource.SelectMRPIILines[selectedLineIndex].EstimatedPurchaseSupplyDate = LineDataSource.ReferanceDate;
                }
            }

            GridLineList = DataSource.SelectMRPIILines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        public async void BringLinesButtonClicked()
        {
            DateTime now = GetSQLDateAppService.GetDateFromSQL();

            if (MRPIISourceModule == 1) // OrderAcceptanceRecords
            {
                if (BindingOrderAcceptances == null)
                {
                    BindingOrderAcceptances = new List<Guid>();
                }
                else
                {
                    foreach (var id in BindingOrderAcceptances)
                    {
                        var orderAcceptance = (await OrderAcceptanceRecordsAppService.GetAsync(id)).Data;
                        var orderAcceptanceLineList = orderAcceptance.SelectOrderAcceptanceRecordLines.ToList();

                        if (orderAcceptanceLineList != null && orderAcceptanceLineList.Count > 0)
                        {

                            foreach (var orderAcceptanceLine in orderAcceptanceLineList)
                            {
                                var product = (await ProductsAppService.GetAsync(orderAcceptanceLine.ProductID.GetValueOrDefault())).Data;

                                if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Üretim)
                                {
                                    SelectMRPIILinesDto mrpIILineModel1 = new SelectMRPIILinesDto
                                    {
                                        ProductID = product.Id,
                                        ProductCode = product.Code,
                                        ProductName = product.Name,
                                        LinkedProductCode = string.Empty,
                                        LinkedProductID = Guid.Empty,
                                        Description_ = string.Empty,
                                        ReferanceDate = DataSource.CalculationDate,
                                        LinkedProductName = string.Empty,
                                        LineNr = GridLineList.Count + 1,
                                        EstimatedProductionStartDate = now,
                                        EstimatedProductionEndDate = now,
                                        EstimatedPurchaseSupplyDate = now,
                                        SalesOrderID = Guid.Empty,
                                        SalesOrderNo = string.Empty,
                                        OrderAcceptanceID = orderAcceptance.Id,
                                        OrderAcceptanceNo = orderAcceptance.Code
                                    };

                                    GridLineList.Add(mrpIILineModel1);

                                    int indexFinishedProduct = GridLineList.IndexOf(mrpIILineModel1);

                                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(product.Id)).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                                    if (bomLineList != null && bomLineList.Count > 0)
                                    {
                                        decimal TotalOperationTime = 0;

                                        DateTime? biggestDate = null;

                                        foreach (var bomLine in bomLineList)
                                        {
                                            #region Tahmini Bitiş Tarihi

                                            Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                                            if (productsOperationID != Guid.Empty)
                                            {
                                                var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                                if (productsOperationLineList != null && productsOperationLineList.Count > 0)
                                                {
                                                    TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * orderAcceptanceLine.OrderAmount;
                                                }
                                            }

                                            GridLineList[indexFinishedProduct].EstimatedProductionEndDate = GridLineList[indexFinishedProduct].ReferanceDate.AddSeconds((double)TotalOperationTime);


                                            #endregion

                                            if (orderAcceptanceLine.PurchaseSupplyDate > biggestDate)
                                            {

                                                GridLineList[indexFinishedProduct].EstimatedProductionStartDate = biggestDate.GetValueOrDefault();

                                                GridLineList[indexFinishedProduct].EstimatedPurchaseSupplyDate = biggestDate.GetValueOrDefault();
                                            }


                                        }
                                    }
                                }
                            }
                        }
                    }

                    await _LineGrid.Refresh();
                }
            }

            else if (MRPIISourceModule == 2) // SalesOrders
            {
                if (BindingSalesOrders == null)
                {
                    BindingSalesOrders = new List<Guid>();
                }
                else
                {
                    foreach (var id in BindingSalesOrders)
                    {
                        var salesOrder = (await SalesOrdersAppService.GetAsync(id)).Data;
                        var salesOrderLineList = salesOrder.SelectSalesOrderLines.Where(t => t.SalesOrderLineStateEnum == Entities.Enums.SalesOrderLineStateEnum.Beklemede).ToList();

                        if (salesOrderLineList != null && salesOrderLineList.Count > 0)
                        {
                            foreach (var salesOrderLine in salesOrderLineList)
                            {
                                var product = (await ProductsAppService.GetAsync(salesOrderLine.ProductID.GetValueOrDefault())).Data;

                                if (product.SupplyForm == Entities.Enums.ProductSupplyFormEnum.Üretim)
                                {
                                    SelectMRPIILinesDto mrpIILineModel1 = new SelectMRPIILinesDto
                                    {
                                        ProductID = product.Id,
                                        ProductCode = product.Code,
                                        ProductName = product.Name,
                                        LinkedProductCode = string.Empty,
                                        Description_ = string.Empty,
                                        ReferanceDate = DataSource.CalculationDate,
                                        LinkedProductID = Guid.Empty,
                                        LinkedProductName = string.Empty,
                                        LineNr = GridLineList.Count + 1,
                                        EstimatedProductionStartDate = now,
                                        EstimatedProductionEndDate = now,
                                        EstimatedPurchaseSupplyDate = now,
                                        SalesOrderID = salesOrder.Id,
                                        SalesOrderNo = salesOrder.FicheNo,
                                        OrderAcceptanceID = Guid.Empty,
                                        OrderAcceptanceNo = string.Empty

                                    };

                                    GridLineList.Add(mrpIILineModel1);

                                    int indexFinishedProduct = GridLineList.IndexOf(mrpIILineModel1);

                                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(product.Id)).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                                    if (bomLineList != null && bomLineList.Count > 0)
                                    {
                                        DateTime? biggestDate = null;

                                        decimal TotalOperationTime = 0;

                                        foreach (var bomLine in bomLineList)
                                        {
                                            #region Tahmini Bitiş Tarihi

                                            Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t=>t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t=>t.Id).FirstOrDefault();

                                            if( productsOperationID != Guid.Empty)
                                            {
                                                var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                                if(productsOperationLineList != null && productsOperationLineList.Count > 0)
                                                {
                                                    TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * salesOrderLine.Quantity;
                                                }
                                            }

                                            GridLineList[indexFinishedProduct].EstimatedProductionEndDate = GridLineList[indexFinishedProduct].ReferanceDate.AddSeconds((double)TotalOperationTime);

                                            #endregion

                                            if (salesOrderLine.PurchaseSupplyDate > biggestDate)
                                            {

                                                GridLineList[indexFinishedProduct].EstimatedProductionStartDate = biggestDate.GetValueOrDefault();

                                                GridLineList[indexFinishedProduct].EstimatedPurchaseSupplyDate = biggestDate.GetValueOrDefault();
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    await _LineGrid.Refresh();
                }

            }

        }

        public async void CalculateAllEndDatesButtonClicked()
        {
            foreach(var line in GridLineList)
            {
                int lineIndex = GridLineList.IndexOf(line);

                if(MRPIISourceModule == 1) //OrderAcceptanceRecords
                {
                    var orderAcceptanceLineAmount = (await OrderAcceptanceRecordsAppService.GetAsync(line.OrderAcceptanceID.GetValueOrDefault())).Data.SelectOrderAcceptanceRecordLines.Where(t => t.ProductID == line.ProductID).Select(t => t.OrderAmount).FirstOrDefault();

                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(line.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                    if (bomLineList != null && bomLineList.Count > 0)
                    {
                        decimal TotalOperationTime = 0;

                        foreach (var bomLine in bomLineList)
                        {
                            #region Tahmini Bitiş Tarihi

                            Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                            if (productsOperationID != Guid.Empty)
                            {
                                var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                if (productsOperationLineList != null && productsOperationLineList.Count > 0)
                                {
                                    TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * orderAcceptanceLineAmount;
                                }
                            }

                            GridLineList[lineIndex].EstimatedProductionEndDate = GridLineList[lineIndex].ReferanceDate.AddSeconds((double)TotalOperationTime);


                            #endregion

                        }
                    }
                }

                else if (MRPIISourceModule == 2) //Sales Orders
                {
                    var salesOrderLineAmount = (await SalesOrdersAppService.GetAsync(line.SalesOrderID.GetValueOrDefault())).Data.SelectSalesOrderLines.Where(t => t.ProductID == line.ProductID).Select(t => t.Quantity).FirstOrDefault();

                    var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(line.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.Where(t => (int)t.SupplyForm == 2).ToList();

                    if (bomLineList != null && bomLineList.Count > 0)
                    {
                        decimal TotalOperationTime = 0;

                        foreach (var bomLine in bomLineList)
                        {
                            #region Tahmini Bitiş Tarihi

                            Guid productsOperationID = (await ProductsOperationsAppService.GetListAsync(new ListProductsOperationsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID.GetValueOrDefault()).Select(t => t.Id).FirstOrDefault();

                            if (productsOperationID != Guid.Empty)
                            {
                                var productsOperationLineList = (await ProductsOperationsAppService.GetAsync(productsOperationID)).Data.SelectProductsOperationLines;

                                if (productsOperationLineList != null && productsOperationLineList.Count > 0)
                                {
                                    TotalOperationTime = productsOperationLineList.Sum(t => t.OperationTime) * bomLine.Quantity * salesOrderLineAmount;
                                }
                            }

                            GridLineList[lineIndex].EstimatedProductionEndDate = GridLineList[lineIndex].ReferanceDate.AddSeconds((double)TotalOperationTime);


                            #endregion

                        }
                    }
                }
            }

            await _LineGrid.Refresh();
        }
       

        #region OrderAcceptances MultiSelect

        List<ListOrderAcceptanceRecordsDto> OrderAcceptances = new List<ListOrderAcceptanceRecordsDto>();
        List<Guid> BindingOrderAcceptances = new List<Guid>();

        private async Task GetOrderAcceptances()
        {
            OrderAcceptances = (await OrderAcceptanceRecordsAppService.GetListAsync(new ListOrderAcceptanceRecordsParameterDto())).Data.Where(t => t.OrderAcceptanceRecordState == Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisFiyatOnayiVerildi).ToList();
        }

        #endregion

        #region SalesOrders MultiSelect

        List<ListSalesOrderDto> SalesOrders = new List<ListSalesOrderDto>();
        List<Guid> BindingSalesOrders = new List<Guid>();

        private async Task GetSalesOrders()
        {
            SalesOrders = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.SalesOrderState == Entities.Enums.SalesOrderStateEnum.Beklemede).ToList();
        }

        #endregion


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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPIIChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
