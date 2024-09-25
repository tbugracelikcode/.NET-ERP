using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.XlsIO;
using System.Data;
using System.Dynamic;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Models;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.Forecast
{
    public partial class ForecastsListPage : IDisposable
    {
        private SfGrid<SelectForecastLinesDto> _LineGrid;

        SelectForecastLinesDto LineDataSource;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        [Inject]
        SpinnerService SpinnerService { get; set; }
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectForecastLinesDto> GridLineList = new List<SelectForecastLinesDto>();

        private bool LineCrudPopup = false;

        public bool PurchaseReservedQuantityModalVisible = false;

        #region Şube ButtonEdit

        SfTextBox ForecastBranchesButtonEdit;
        bool ForecastSelectBranchesPopupVisible = false;
        List<ListBranchesDto> ForecastBranchesList = new List<ListBranchesDto>();

        public async Task ForecastBranchesOnCreateIcon()
        {
            var ForecastBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastBranchesButtonClickEvent);
            await ForecastBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ForecastBranchesButtonClick } });
        }

        public async void ForecastBranchesButtonClickEvent()
        {
            ForecastSelectBranchesPopupVisible = true;
            await GetBranchesList();
            await InvokeAsync(StateHasChanged);
        }

        public void ForecastBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
                DataSource.BranchName = string.Empty;
            }
        }

        public async void ForecastBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                DataSource.BranchName = selectedUnitSet.Name;
                ForecastSelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox ForecastCurrentAccountCardsCodeButtonEdit;
        SfTextBox ForecastCurrentAccountCardsNameButtonEdit;
        bool ForecastSelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> ForecastCurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var ForecastCurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastCurrentAccountCardsCodeButtonClickEvent);
            await ForecastCurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ForecastCurrentAccountCardsCodeButtonClick } });
        }

        public async void ForecastCurrentAccountCardsCodeButtonClickEvent()
        {
            ForecastSelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task ForecastCurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastCurrentAccountCardsNameButtonClickEvent);
            await ForecastCurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsNameButtonClick } });
        }

        public async void ForecastCurrentAccountCardsNameButtonClickEvent()
        {
            ForecastSelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ForecastCurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void ForecastCurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id;
                DataSource.CurrentAccountCardCode = selectedUnitSet.Code;
                DataSource.CurrentAccountCardName = selectedUnitSet.Name;
                ForecastSelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ForecastProductsCodeButtonEdit;
        SfTextBox ForecastProductsNameButtonEdit;
        bool ForecastSelectProductsPopupVisible = false;
        List<ListProductsDto> ForecastProductsList = new List<ListProductsDto>();
        public async Task ForecastProductsCodeOnCreateIcon()
        {
            var ForecastProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastProductsCodeButtonClickEvent);
            await ForecastProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ForecastProductsButtonClick } });
        }

        public async void ForecastProductsCodeButtonClickEvent()
        {
            ForecastSelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ForecastProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastProductsNameButtonClickEvent);
            await ForecastProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ForecastProductsNameButtonClickEvent()
        {
            ForecastSelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ForecastProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
                LineDataSource.CustomerProductCode = string.Empty;
            }
        }

        public async void ForecastProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                LineDataSource.ProductName = selectedProduct.Name;

                string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(selectedProduct.Id, DataSource.CurrentAccountCardID.GetValueOrDefault());

                if (!string.IsNullOrEmpty(supplierReferanceNumber))
                {
                    LineDataSource.CustomerProductCode = supplierReferanceNumber;
                }
                else
                {
                    LineDataSource.CustomerProductCode = "-";
                }

                ForecastSelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Dönem ButtonEdit

        SfTextBox ForecastPeriodsButtonEdit;
        bool ForecastSelectPeriodsPopupVisible = false;
        List<ListPeriodsDto> ForecastPeriodsList = new List<ListPeriodsDto>();

        public async Task ForecastPeriodsOnCreateIcon()
        {
            var ForecastPeriodsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastPeriodsButtonClickEvent);
            await ForecastPeriodsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ForecastPeriodsButtonClick } });
        }

        public async void ForecastPeriodsButtonClickEvent()
        {
            ForecastSelectPeriodsPopupVisible = true;
            await GetPeriodsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ForecastPeriodsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PeriodID = Guid.Empty;
                DataSource.PeriodCode = string.Empty;
                DataSource.PeriodName = string.Empty;
            }
        }

        public async void ForecastPeriodsDoubleClickHandler(RecordDoubleClickEventArgs<ListPeriodsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.PeriodID = selectedUnitSet.Id;
                DataSource.PeriodCode = selectedUnitSet.Code;
                DataSource.PeriodName = selectedUnitSet.Name;
                ForecastSelectPeriodsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = ForecastsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ForecastsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateMRPLineContextMenuItems();

        }

        #region Forecast Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            var salesManagementParameter = (await SalesManagementParametersAppService.GetSalesManagementParametersAsync()).Data;
            DataSource = new SelectForecastsDto()
            {
                CreationDate_ = GetSQLDateAppService.GetDateFromSQL(),
                ValidityStartDate = GetSQLDateAppService.GetDateFromSQL().Date,
                ValidityEndDate = GetSQLDateAppService.GetDateFromSQL().Date,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ForecastsChildMenu"),
                BranchID = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchID : Guid.Empty,
                BranchCode = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchCode : string.Empty,
                BranchName = salesManagementParameter != null && salesManagementParameter.Id != Guid.Empty ? salesManagementParameter.DefaultBranchName : string.Empty
            };

            DataSource.SelectForecastLines = new List<SelectForecastLinesDto>();
            GridLineList = DataSource.SelectForecastLines;

            EditPageVisible = true;


            await Task.CompletedTask;
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
                            case "ForecastLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastLineContextAdd"], Id = "new" }); break;
                            case "ForecastLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastLineContextChange"], Id = "changed" }); break;
                            case "ForecastLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastLineContextDelete"], Id = "delete" }); break;
                            case "ForecastLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
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
                            case "ForecastContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastContextAdd"], Id = "new" }); break;
                            case "ForecastContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastContextChange"], Id = "changed" }); break;
                            case "ForecastContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastContextDelete"], Id = "delete" }); break;
                            case "ForecastContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastContextRefresh"], Id = "refresh" }); break;
                            case "ForecastContextMRP":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ForecastContextMRP"], Id = "mrp" }); break;
                            default: break;
                        }
                    }
                }
            }
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListForecastsDto> args)
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
                    DataSource = (await ForecastsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectForecastLines;


                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":
                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
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

                case "mrp":
                    if (args.RowInfo.RowData != null)
                    {

                        SpinnerService.Show();
                    await Task.Delay(100);

                    DataSource = (await ForecastsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    MRPFilterModalVisible = true;
                    MRPFilterStartDate = DataSource.ValidityStartDate.Value;
                    MRPFilterEndDate = DataSource.ValidityEndDate.Value;

                    SpinnerService.Hide();

                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectForecastLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.CurrentAccountCardID == Guid.Empty || DataSource.CurrentAccountCardID == null)
                    {
                        await ModalManager.WarningPopupAsync(L["UICurrentAccountTitle"], L["UICurrentAccountMessage"]);
                    }
                    else
                    {
                        LineDataSource = new SelectForecastLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.StartDate = null;
                        LineDataSource.EndDate = null;
                        LineDataSource.LineNr = GridLineList.Count + 1;
                    }
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupLineMessage"]);

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectForecastLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectForecastLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectForecastLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }
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
            if (LineDataSource.Amount == 0 || LineDataSource.ProductID == Guid.Empty || LineDataSource.StartDate ==null|| LineDataSource.EndDate == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);
            }
            else
            {
                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectForecastLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectForecastLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectForecastLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectForecastLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectForecastLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectForecastLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectForecastLines;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        #endregion

        #region GetList Metotları

        private async Task GetPeriodsList()
        {
            ForecastPeriodsList = (await PeriodsAppService.GetListAsync(new ListPeriodsParameterDto())).Data.ToList();
        }

        private async Task GetCurrentAccountCardsList()
        {
            ForecastCurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        private async Task GetBranchesList()
        {
            ForecastBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ForecastProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox ForecastCodeButtonEdit;

        public async Task ForecastCodeOnCreateIcon()
        {
            var ForecastCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ForecastCodeButtonClickEvent);
            await ForecastCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ForecastCodesButtonClick } });
        }

        public async void ForecastCodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ForecastsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Malzeme İhtiyaç Listesi Filtre Modal

        private bool MRPFilterModalVisible = false;

        public DateTime MRPFilterStartDate { get; set; }

        public DateTime MRPFilterEndDate { get; set; }

        protected async Task OnMRPFilterSubmit()
        {
            MRPLinesList.Clear();

            var startDate = Convert.ToDateTime(MRPFilterStartDate.ToShortDateString());
            var endDate = Convert.ToDateTime(MRPFilterEndDate.ToShortDateString());

            var LineList = DataSource.SelectForecastLines.Where(t => t.StartDate >= startDate && MRPFilterEndDate <= t.EndDate).ToList();

            SelectMRPsDto mRPsDto = new SelectMRPsDto
            {

                Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu"),
                Date_ = DateTime.Today,
                IsMaintenanceMRP = false,
                ReferanceDate = GetSQLDateAppService.GetDateFromSQL(),
                MaintenanceMRPID = Guid.Empty,
                OrderAcceptanceID = DataSource.Id,
                MaintenanceMRPCode = string.Empty,
            };

            MRPDataSource = mRPsDto;

            var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;
            var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

            foreach (var line in LineList)
            {
                var bomLineList = (await BillsofMaterialsAppService.GetbyProductIDAsync(line.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                decimal amountofProduct = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == line.ProductID).Select(t => t.Amount).Sum();

                CreateMRPLine(branch, warehouse, line, bomLineList, amountofProduct);

                foreach (var bomLine in bomLineList)
                {
                    if ((int)bomLine.SupplyForm == 2)
                    {
                        var bomLineList2 = (await BillsofMaterialsAppService.GetbyProductIDAsync(bomLine.ProductID.GetValueOrDefault())).Data.SelectBillsofMaterialLines.ToList();

                        decimal amountofProduct2 = (await GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Data.Where(t => t.ProductID == bomLine.ProductID).Select(t => t.Amount).Sum();

                        CreateMRPLine(branch, warehouse, line, bomLineList2, amountofProduct2);
                    }
                }
            }

            MRPDataSource.SelectMRPLines = MRPLinesList;



            MRPCrudModalVisible = true;

            await InvokeAsync(StateHasChanged);
        }

        public void HideMRPFilterPopup()
        {
            MRPFilterModalVisible = false;
        }


        private void CreateMRPLine(SelectBranchesDto branch, SelectWarehousesDto warehouse, SelectForecastLinesDto line, List<SelectBillsofMaterialLinesDto> bomLineList, decimal amountofProduct)
        {
            foreach (var bomline in bomLineList)
            {
                if ((int)bomline.SupplyForm == 1)
                {
                    #region Mamül Reçete satırdaki Temin Şekli Satınalma
                    int calculatedAmount = Convert.ToInt32(bomline.Quantity);


                    SelectMRPLinesDto mrpLineModel = new SelectMRPLinesDto
                    {
                        Amount = calculatedAmount,
                        isCalculated = true,
                        isPurchase = false,
                        ProductID = bomline.ProductID.GetValueOrDefault(),
                        ProductCode = bomline.ProductCode,
                        ProductName = bomline.ProductName,
                        SalesOrderID = Guid.Empty,
                        UnitSetID = bomline.UnitSetID,
                        LineNr = MRPLinesList.Count + 1,
                        UnitSetCode = bomline.UnitSetCode,
                        SupplyDate = GetSQLDateAppService.GetDateFromSQL(),
                        UnitPrice = 0,
                        State_ = string.Empty,
                        ReservedAmount = 0,
                        CurrentAccountCardID = Guid.Empty,
                        CurrentAccountCardCode = string.Empty,
                        CurrentAccountCardName = string.Empty,
                        AmountOfStock = amountofProduct,
                        RequirementAmount = calculatedAmount,
                        SalesOrderLineID = Guid.Empty,
                        SalesOrderFicheNo = string.Empty,
                        BranchID = branch.Id,
                        WarehouseID = warehouse.Id,
                        BranchCode = branch.Code,
                        WarehouseCode = warehouse.Code,
                        OrderAcceptanceID = DataSource.Id,
                        isStockUsage = false,
                        OrderAcceptanceLineID = line.Id
                    };

                    MRPLinesList.Add(mrpLineModel);
                    #endregion

                }
                //else if(bomline.ProductSupplyType == 2)  yarı mamülün yarı mamülü (yavuz'un fantezisi)
                //{
                //    var bomLineList2 = (BillsofMaterialsAppService.GetbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Result.Data.SelectBillsofMaterialLines.ToList();

                //    decimal amountofProduct2 = ( GrandTotalStockMovementsAppService.GetListAsync(new ListGrandTotalStockMovementsParameterDto())).Result.Data.Where(t => t.ProductID == bomline.ProductID).Select(t => t.Amount).Sum();

                //    CreateMRPLine(branch, warehouse, line, bomLineList2, amountofProduct2);
                //}
            }
        }


        #endregion

        #region MRP

        private SfGrid<SelectMRPLinesDto> _MRPLineGrid;

        public List<ContextMenuItemModel> MRPLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectMRPLinesDto> MRPLinesList = new List<SelectMRPLinesDto>();

        SelectMRPsDto MRPDataSource;
        SelectMRPLinesDto MRPLineDataSource;

        public bool MRPCrudModalVisible = false;
        public bool MRPLineCrudPopup = false;

        #region Planning Parameters

        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;
        Guid? BranchIDButtonEdit;
        Guid? WarehouseIDButtonEdit;
        string WarehouseCodeButtonEdit;
        string BranchCodeButtonEdit;

        #endregion


        List<SupplierSelectionGrid> SupplierSelectionList = new List<SupplierSelectionGrid>();

        public bool SupplierSelectionPopup = false;

        protected void CreateMRPLineContextMenuItems()
        {
            if (MRPLineGridContextMenu.Count() == 0)
            {
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextDoNotCalculate"], Id = "dontcalculate" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextStockUsage"], Id = "stockusage" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextReservePurchase"], Id = "reservepurchase" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextChange"], Id = "changed" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextRefresh"], Id = "refresh" });
                MRPLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MRPLineContextSupplier"], Id = "supplier" });

            }
        }


        public async void OnMRPListContextMenuClick(ContextMenuClickEventArgs<SelectMRPLinesDto> args)
        {
            switch (args.Item.Id)
            {


                case "dontcalculate":
                    if (args.RowInfo.RowData != null)
                    {

                        var line = args.RowInfo.RowData;
                    var selectedIndex = MRPLinesList.FindIndex(t => t.SalesOrderID == line.SalesOrderID && t.ProductID == line.ProductID);
                    if (selectedIndex >= 0)
                    {
                        MRPLinesList[selectedIndex].isCalculated = false;
                    }

                    await _MRPLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;


                case "stockusage":
                    if (args.RowInfo.RowData != null)
                    {

                        var selectedline = args.RowInfo.RowData;
                    var Index = MRPLinesList.FindIndex(t => t.SalesOrderID == selectedline.SalesOrderID && t.ProductID == selectedline.ProductID);
                    if (Index >= 0)
                    {
                        MRPLinesList[Index].isStockUsage = !selectedline.isStockUsage;

                        if (MRPLinesList[Index].isStockUsage)
                        {
                            var stockAmount = MRPLinesList[Index].AmountOfStock;
                            var bomAmount = MRPLinesList[Index].Amount;
                            var purchaseAmount = MRPLinesList[Index].RequirementAmount;
                            var reservedAmount = MRPLinesList[Index].ReservedAmount;

                            if (stockAmount > bomAmount) // stok miktarı > reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                MRPLinesList[Index].RequirementAmount = 0;
                                MRPLinesList[Index].ReservedAmount = bomAmount;
                                MRPLinesList[Index].AmountOfStock = stockAmount - bomAmount;
                            }
                            else if (stockAmount < bomAmount) // stok miktarı < reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                MRPLinesList[Index].AmountOfStock = 0;
                                MRPLinesList[Index].RequirementAmount = bomAmount - stockAmount;
                                MRPLinesList[Index].ReservedAmount = stockAmount;
                            }
                            else if (stockAmount == bomAmount) // stok miktarı = reçeteden gelen toplam ihtiyaç miktarıysa
                            {
                                MRPLinesList[Index].AmountOfStock = 0;
                                MRPLinesList[Index].RequirementAmount = 0;
                                MRPLinesList[Index].ReservedAmount = stockAmount;
                            }
                        }

                        else // Stoktan kullanılmayacaksa
                        {
                            MRPLinesList[Index].RequirementAmount = MRPLinesList[Index].Amount;
                            MRPLinesList[Index].ReservedAmount = 0;
                        }
                    }

                    await _MRPLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;
                case "reservepurchase":
                    if (args.RowInfo.RowData != null)
                    {

                        MRPLineDataSource = args.RowInfo.RowData;

                        PurchaseReservedQuantityModalVisible = true;

                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        MRPLineDataSource = args.RowInfo.RowData;
                    MRPLineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await _MRPLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "supplier":

                    if (args.RowInfo.RowData != null)
                    {

                        MRPLineDataSource = args.RowInfo.RowData;

                    SupplierSelectionList.Clear();

                    var purchasePriceLinesList = (await PurchasePricesAppService.GetSelectLineListAsync(MRPLineDataSource.ProductID.GetValueOrDefault())).Data.ToList();

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

        #region Kod Button Edit
        SfTextBox MRPCodeButtonEdit;

        public async Task MRPCodeOnCreateIcon()
        {
            var MRPCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MRPCodeButtonClickEvent);
            await MRPCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", MRPCodesButtonClick } });
        }

        public async void MRPCodeButtonClickEvent()
        {
            MRPDataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("MRPChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region MRP Crud ButtonEdits

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

                foreach (var item in MRPLinesList)
                {
                    item.WarehouseID = warehouse.Id;
                    item.WarehouseCode = warehouse.Code;
                }
                await _MRPLineGrid.Refresh();
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

                foreach (var item in MRPLinesList)
                {
                    item.WarehouseID = WarehouseIDButtonEdit;
                    item.WarehouseCode = WarehouseCodeButtonEdit;
                }

                await _MRPLineGrid.Refresh();
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

                foreach (var item in MRPLinesList)
                {
                    item.BranchID = branch.Id;
                    item.BranchCode = branch.Code;
                }
                await _MRPLineGrid.Refresh();
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                BranchIDButtonEdit = selectedBranch.Id;
                BranchCodeButtonEdit = selectedBranch.Code;

                foreach (var item in MRPLinesList)
                {
                    item.BranchID = BranchIDButtonEdit;
                    item.BranchCode = BranchCodeButtonEdit;
                }

                await _MRPLineGrid.Refresh();
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
                MRPLineDataSource.WarehouseID = Guid.Empty;
                MRPLineDataSource.WarehouseCode = string.Empty;
            }

            await Task.CompletedTask;
        }

        public async void LineWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                MRPLineDataSource.WarehouseID = selectedWarehouse.Id;
                MRPLineDataSource.WarehouseCode = selectedWarehouse.Code;
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
                MRPLineDataSource.BranchID = Guid.Empty;
                MRPLineDataSource.BranchCode = string.Empty;
            }


            await Task.CompletedTask;
        }

        public async void LineBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                MRPLineDataSource.BranchID = selectedBranch.Id;
                MRPLineDataSource.BranchCode = selectedBranch.Code;
                SelectLineBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        public async void SupplierDoubleClickHandler(RecordDoubleClickEventArgs<Models.SupplierSelectionGrid> args)
        {
            var selectedSupplier = args.RowData;

            if (selectedSupplier != null)
            {
                MRPLineDataSource.CurrencyID = selectedSupplier.CurrenyID;
                MRPLineDataSource.CurrencyCode = selectedSupplier.CurrenyCode;
                MRPLineDataSource.CurrentAccountCardID = selectedSupplier.CurrentAccountID;
                MRPLineDataSource.CurrentAccountCardName = selectedSupplier.CurrentAccountName;
                MRPLineDataSource.UnitPrice = selectedSupplier.UnitPrice;
                MRPLineDataSource.SupplyDate = MRPLineDataSource.SupplyDate.AddDays(selectedSupplier.SupplyDate);
                HideSupplierSelectionPopup();
                await _MRPLineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public void HideSupplierSelectionPopup()
        {
            SupplierSelectionList.Clear();
            SupplierSelectionPopup = false;
        }

        #endregion

        public void HideMRPLinesPopup()
        {
            MRPLineCrudPopup = false;
        }

        public void HideMRPPopup()
        {
            MRPCrudModalVisible = false;
        }

        public async void OnMRPSubmit()
        {
            var createdMRPEntity = ObjectMapper.Map<SelectMRPsDto, CreateMRPsDto>(MRPDataSource);

            await MRPsService.CreateAsync(createdMRPEntity);

            MRPCrudModalVisible = false;

            await InvokeAsync(StateHasChanged);

        }

        public async Task OnLineSubmitPurchaseReserved()
        {

            if (MRPLineDataSource.Id == Guid.Empty)
            {
                if (MRPDataSource.SelectMRPLines.Contains(MRPLineDataSource))
                {
                    int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.LineNr == MRPLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                    }
                }
                else
                {
                    MRPDataSource.SelectMRPLines.Add(MRPLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.Id == MRPLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                }
            }

            MRPLinesList = MRPDataSource.SelectMRPLines;
            await _MRPLineGrid.Refresh();

            PurchaseReservedQuantityModalVisible = false;

            await InvokeAsync(StateHasChanged);


        }

        public void HidePurchaseReservedQuantity()
        {
            PurchaseReservedQuantityModalVisible = false;
        }

        public void ReferanceDateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            if (MRPLinesList != null && MRPLinesList.Count > 0)
            {
                foreach (var line in MRPLinesList)
                {
                    int index = MRPLinesList.IndexOf(line);
                    MRPLinesList[index].SupplyDate = args.Value;
                }

                _MRPLineGrid.Refresh();
            }
        }

        protected async Task OnMRPLineSubmit()
        {

            if (MRPLineDataSource.Id == Guid.Empty)
            {
                if (MRPDataSource.SelectMRPLines.Contains(MRPLineDataSource))
                {
                    int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.LineNr == MRPLineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                    }
                }
                else
                {
                    MRPDataSource.SelectMRPLines.Add(MRPLineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = MRPDataSource.SelectMRPLines.FindIndex(t => t.Id == MRPLineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    MRPDataSource.SelectMRPLines[selectedLineIndex] = MRPLineDataSource;
                }
            }

            MRPLinesList = MRPDataSource.SelectMRPLines;
            await _MRPLineGrid.Refresh();

            HideMRPLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        #endregion


        public DataTable table = new DataTable();

        private void OnChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                #region Expando Object Örneği

                var path = file.FileInfo.Name;
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                var check = ExcelService.ImportGetPath(path);
                FileStream openFileStream = new FileStream(check, FileMode.OpenOrCreate);
                file.Stream.WriteTo(openFileStream);


                FileStream fileStream = new FileStream(check, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = application.Workbooks.Open(fileStream);
                IWorksheet worksheet = workbook.Worksheets[0];
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                GenerateListFromTable(table);

                #endregion

            }
        }

        string[] Columns;
        public List<ExpandoObject> ProductList;
        public async void GenerateListFromTable(DataTable input)
        {
            var list = new List<ExpandoObject>();
            Columns = input.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            foreach (DataRow row in input.Rows)
            {
                System.Dynamic.ExpandoObject e = new System.Dynamic.ExpandoObject();
                foreach (DataColumn col in input.Columns)
                    e.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                list.Add(e);
            }
            ProductList = list;

            int lineNr = 1;

            foreach (var item in ProductList)
            {
                dynamic row = item;

                #region Row DBNull Kontrolleri
                string productCode = !string.IsNullOrEmpty(Convert.ToString(row.ProductCode)) ? (string)row.ProductCode : string.Empty;

                string _amount = !string.IsNullOrEmpty(Convert.ToString(row.Amount)) ? (string)row.Amount : "0";

                string _startDate = !string.IsNullOrEmpty(Convert.ToString(row.StartDate)) ? (string)row.StartDate : DateTime.Today.ToShortDateString();

                string _endDate = !string.IsNullOrEmpty(Convert.ToString(row.EndDate)) ? (string)row.EndDate : DateTime.Today.ToShortDateString();

                DateTime EndDate = Convert.ToDateTime(_endDate);
                DateTime StartDate = Convert.ToDateTime(_startDate);
                int Amount = Convert.ToInt32(_amount);

                #endregion

                var product = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => t.Code == productCode).FirstOrDefault();

                string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(product.Id, DataSource.CurrentAccountCardID.GetValueOrDefault());

                SelectForecastLinesDto line = new SelectForecastLinesDto()
                {
                    ProductCode = productCode,
                    Amount = Amount,
                    CustomerProductCode = supplierReferanceNumber,
                    EndDate = EndDate,
                    ForecastID = DataSource.Id,
                    LineNr = lineNr,
                    ProductID = product.Id,
                    ProductName = product.Name,
                    StartDate = StartDate
                };

                DataSource.SelectForecastLines.Add(line);
                lineNr++;
            }

            await _LineGrid.Refresh();
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }


}
