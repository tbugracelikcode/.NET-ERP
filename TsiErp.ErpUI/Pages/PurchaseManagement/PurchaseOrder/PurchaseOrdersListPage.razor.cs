
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PurchaseManagement.PurchaseOrder
{
    public partial class PurchaseOrdersListPage : IDisposable
    {
        private SfGrid<SelectPurchaseOrderLinesDto> _LineGrid;
        private SfGrid<CreateStockReceiptFishes> _CreateStockFishesGrid;
        private SfGrid<CreateStockReceiptFishes> _CancelOrderGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();
        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseOrderLinesDto LineDataSource;
        CreateStockReceiptFishes CreateStockReceiptFishesDataSource;
        CreateStockReceiptFishes CancelOrderDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> CreateStockFishesGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseOrderLinesDto> GridLineList = new List<SelectPurchaseOrderLinesDto>();

        List<CreateStockReceiptFishes> CreateStockFishesList = new List<CreateStockReceiptFishes>();

        List<CreateStockReceiptFishes> CancelOrderList = new List<CreateStockReceiptFishes>();

        private bool LineCrudPopup = false;
        private bool CreateStockFishesCrudPopup = false;
        private bool CancelOrderCrudPopup = false;

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        #region Birim Setleri ButtonEdit
        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                LineDataSource.UnitSetID = selectedUnitSet.Id;
                LineDataSource.UnitSetCode = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Para Birimleri ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrencyPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            await GetCurrenciesList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrencyID = selectedUnitSet.Id;
                DataSource.CurrencyCode = selectedUnitSet.Name;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit

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
            await GetBranchesList();
            await InvokeAsync(StateHasChanged);
        }

        public void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
                DataSource.BranchName = string.Empty;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                DataSource.BranchName = selectedUnitSet.Name;
                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Ödeme Planı ButtonEdit

        SfTextBox PaymentPlansButtonEdit;
        bool SelectPaymentPlansPopupVisible = false;
        List<ListPaymentPlansDto> PaymentPlansList = new List<ListPaymentPlansDto>();

        public async Task PaymentPlansOnCreateIcon()
        {
            var PaymentPlansButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, PaymentPlansButtonClickEvent);
            await PaymentPlansButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", PaymentPlansButtonClick } });
        }

        public async void PaymentPlansButtonClickEvent()
        {
            SelectPaymentPlansPopupVisible = true;
            await GetPaymentPlansList();
            await InvokeAsync(StateHasChanged);
        }

        public void PaymentPlansOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.PaymentPlanID = Guid.Empty;
                DataSource.PaymentPlanName = string.Empty;
            }
        }

        public async void PaymentPlansDoubleClickHandler(RecordDoubleClickEventArgs<ListPaymentPlansDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.PaymentPlanID = selectedUnitSet.Id;
                DataSource.PaymentPlanName = selectedUnitSet.Name;
                SelectPaymentPlansPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Depo ButtonEdit

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
            await GetWarehousesList();
            await InvokeAsync(StateHasChanged);
        }

        public void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WarehouseID = Guid.Empty;
                DataSource.WarehouseCode = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.WarehouseID = selectedUnitSet.Id;
                DataSource.WarehouseCode = selectedUnitSet.Code;
                SelectWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit;
        SfTextBox CurrentAccountCardsNameButtonEdit;
        bool SelectCurrentAccountCardsPopupVisible = false;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        public async Task CurrentAccountCardsCodeOnCreateIcon()
        {
            var CurrentAccountCardsCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCodeButtonClickEvent);
            await CurrentAccountCardsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCodeButtonClick } });
        }

        public async void CurrentAccountCardsCodeButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task CurrentAccountCardsNameOnCreateIcon()
        {
            var CurrentAccountCardsNameButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsNameButtonClickEvent);
            await CurrentAccountCardsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsNameButtonClick } });
        }

        public async void CurrentAccountCardsNameButtonClickEvent()
        {
            SelectCurrentAccountCardsPopupVisible = true;
            await GetCurrentAccountCardsList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrentAccountCardsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id;
                DataSource.CurrentAccountCardCode = selectedUnitSet.Code;
                DataSource.CurrentAccountCardName = selectedUnitSet.Name;
                SelectCurrentAccountCardsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                LineDataSource.ProductID = selectedProduct.Id;
                LineDataSource.ProductCode = selectedProduct.Code;
                LineDataSource.ProductName = selectedProduct.Name;
                LineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                LineDataSource.UnitSetID = selectedProduct.UnitSetID;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            await GetProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductionOrderID = Guid.Empty;
                DataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                DataSource.ProductionOrderID = selectedProductionOrder.Id;
                DataSource.ProductionOrderFicheNo = selectedProductionOrder.FicheNo;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Üretim Emri ButtonEdit - Satır

        SfTextBox LineProductionOrdersButtonEdit;
        List<ListProductionOrdersDto> LineProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task LineProductionOrdersOnCreateIcon()
        {
            var LineProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineProductionOrdersButtonClickEvent);
            await LineProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineProductionOrdersButtonClick } });
        }

        public async void LineProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            await GetLineProductionOrdersList();
            await InvokeAsync(StateHasChanged);
        }

        public void LineProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductionOrderID = Guid.Empty;
                LineDataSource.ProductionOrderFicheNo = string.Empty;
            }
        }

        public async void LineProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedLineProductionOrder = args.RowData;

            if (selectedLineProductionOrder != null)
            {
                LineDataSource.ProductionOrderID = selectedLineProductionOrder.Id;
                LineDataSource.ProductionOrderFicheNo = selectedLineProductionOrder.FicheNo;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Sipariş İptal Modalı İşlemleri

        public async void OnCancelOrderContextMenuClick(ContextMenuClickEventArgs<CreateStockReceiptFishes> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    CancelOrderDataSource = args.RowInfo.RowData;

                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        int selectedIndex = CancelOrderList.IndexOf(CancelOrderDataSource);

                            CancelOrderList[selectedIndex].SelectedLine = true;
                       
                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "multiselect":

                    CancelOrderDataSource = args.RowInfo.RowData;
                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        if (_CancelOrderGrid.SelectedRecords.Count > 0)
                        {
                            foreach (var selectedRow in _CancelOrderGrid.SelectedRecords)
                            {
                                if (selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                                {

                                    int selectedRowIndex = CancelOrderList.IndexOf(selectedRow);
                                    CancelOrderList[selectedRowIndex].SelectedLine = true;
                                }
                            }
                        }


                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "removeall":

                    CancelOrderDataSource = args.RowInfo.RowData;
                    if (CancelOrderDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                    {
                        foreach (var line in CancelOrderList)
                        {
                            if (line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                            {
                                int lineIndex = CancelOrderList.IndexOf(line);
                                CancelOrderList[lineIndex].SelectedLine = false;
                            }
                        }



                        await _CancelOrderGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


            }
        }

        public async void CancelOrderButtonClicked()
        {
            var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationCancelOrderMessage"]);

            if (res == true)
            {
                if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Iptal)
                {
                    if (DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Tamamlandi || DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.KismiTamamlandi)
                    {
                        var stockFicheID = (await StockFichesAppService.GetListAsync(new ListStockFichesParameterDto())).Data.Where(t => t.PurchaseOrderID == DataSource.Id).Select(t => t.Id).FirstOrDefault();

                        await StockFichesAppService.DeleteAsync(stockFicheID);


                    }

                    foreach (var line in DataSource.SelectPurchaseOrderLinesDto)
                    {
                        int lineIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(line);
                        DataSource.SelectPurchaseOrderLinesDto[lineIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Iptal;
                    }

                    DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Iptal;
                    var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
                    await UpdateAsync(updateInput);
                }
            }

            HideCancelOrderPopup();
        }

        public void HideCancelOrderPopup()
        {

            CancelOrderList.Clear();
            CancelOrderCrudPopup = false;

        }

        #endregion

        #region Stok Giriş Fişi Oluşturma Modalı İşlemleri

        public class CreateStockReceiptFishes
        {
            public Entities.Enums.PurchaseOrderLineStateEnum PurchaseStateLine { get; set; }
            public bool SelectedLine { get; set; }
            public Guid? ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public Guid? UnitSetID { get; set; }
            public string UnitSetCode { get; set; }
            public Guid? LineID { get; set; }
        }

        protected void CreateStockReceiptFishesContextMenuItems()
        {
            if (CreateStockFishesGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "StockReceiptFichesContextSelect":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextSelect"], Id = "select" }); break;
                            case "StockReceiptFichesContextMultiSelect":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextMultiSelect"], Id = "multiselect" }); break;
                            case "StockReceiptFichesContextRemoveAll":
                                CreateStockFishesGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockReceiptFichesContextRemoveAll"], Id = "removeall" }); break;
                            default: break;
                        }
                    }
                }
            }
        }


        public async void OnCreateStockReceiptFishesContextMenuClick(ContextMenuClickEventArgs<CreateStockReceiptFishes> args)
        {
            switch (args.Item.Id)
            {
                case "select":
                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;

                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        int selectedIndex = CreateStockFishesList.IndexOf(CreateStockReceiptFishesDataSource);

                         CreateStockFishesList[selectedIndex].SelectedLine = true;
                       
                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "multiselect":

                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;
                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        if (_CreateStockFishesGrid.SelectedRecords.Count > 0)
                        {
                            foreach (var selectedRow in _CreateStockFishesGrid.SelectedRecords)
                            {
                                if (selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || selectedRow.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                                {

                                    int selectedRowIndex = CreateStockFishesList.IndexOf(selectedRow);
                                    CreateStockFishesList[selectedRowIndex].SelectedLine = true;
                                }
                            }
                        }


                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "removeall":

                    CreateStockReceiptFishesDataSource = args.RowInfo.RowData;
                    if (CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || CreateStockReceiptFishesDataSource.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                    {
                        foreach (var line in CreateStockFishesList)
                        {
                            if (line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || line.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                            {
                                int lineIndex = CreateStockFishesList.IndexOf(line);
                                CreateStockFishesList[lineIndex].SelectedLine = false;
                            }
                        }



                        await _CreateStockFishesGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;


            }
        }

        public async void CreateStockFishesButtonClicked()
        {
            List<SelectStockFicheLinesDto> stockFicheLineList = new List<SelectStockFicheLinesDto>();

            foreach (var item in CreateStockFishesList)
            {
                if (item.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || item.PurchaseStateLine != Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                {
                    if (item.SelectedLine)
                    {
                        SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                        {
                            FicheType = Entities.Enums.StockFicheTypeEnum.StokGirisFisi,
                            LineAmount = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).Select(t => t.LineTotalAmount).FirstOrDefault(),
                            LineNr = stockFicheLineList.Count + 1,
                            LineDescription = string.Empty,
                            ProductID = item.ProductID,
                            ProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            PurchaseOrderID = DataSource.Id,
                            PurchaseOrderFicheNo = DataSource.FicheNo,
                            PurchaseOrderLineID = item.LineID,
                            Quantity = item.Quantity,
                            StockFicheID = Guid.Empty,
                            UnitPrice = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).Select(t => t.UnitPrice).FirstOrDefault(),
                            UnitSetCode = item.UnitSetCode,
                            UnitSetID = item.UnitSetID,

                        };
                        stockFicheLineList.Add(stockFicheLineModel);
                        var line = DataSource.SelectPurchaseOrderLinesDto.Where(t => t.Id == item.LineID).FirstOrDefault();
                        int datasourcelineIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(line);
                        DataSource.SelectPurchaseOrderLinesDto[datasourcelineIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi;
                    }
                }
            }

            CreateStockFichesDto stockFichesModel = new CreateStockFichesDto
            {
                BranchID = DataSource.BranchID.GetValueOrDefault(),
                CurrencyID = DataSource.CurrencyID.GetValueOrDefault(),
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                Description_ = string.Empty,
                ExchangeRate = DataSource.ExchangeRate,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                FicheType = 50,
                InputOutputCode = 0,
                NetAmount = DataSource.NetAmount,
                ProductionOrderID = DataSource.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderID = DataSource.Id,
                SpecialCode = DataSource.SpecialCode,
                WarehouseID = DataSource.WarehouseID.GetValueOrDefault(),
                Time_ = null,
            };

            stockFichesModel.SelectStockFicheLines = stockFicheLineList;

            await StockFichesAppService.CreateAsync(stockFichesModel);

            if (CreateStockFishesList.Where(t => t.SelectedLine == false).Count() == 0)
            {
                DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Tamamlandi;
            }
            else
            {
                DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.KismiTamamlandi;
            }

            var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
            await UpdateAsync(updateInput);

            await ModalManager.MessagePopupAsync(L["UIInformationStockFichesCreatedTitle"], L["UIInformationStockFichesCreatedMessage"]);

            HideCreateStockFichesPopup();

            await InvokeAsync(StateHasChanged);
        }

        public void HideCreateStockFichesPopup()
        {

            CreateStockFishesList.Clear();
            CreateStockFishesCrudPopup = false;

        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = PurchaseOrdersAppService;
            _L = L;


            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateStockReceiptFishesContextMenuItems();
            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;
        }

        #region Teklif Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseOrdersDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu"),
                PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Beklemede
            };

            DataSource.SelectPurchaseOrderLinesDto = new List<SelectPurchaseOrderLinesDto>();
            GridLineList = DataSource.SelectPurchaseOrderLinesDto;

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
                            case "PurchaseOrderLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextAdd"], Id = "new" }); break;
                            case "PurchaseOrderLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextChange"], Id = "changed" }); break;
                            case "PurchaseOrderLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextDelete"], Id = "delete" }); break;
                            case "PurchaseOrderLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PurchaseOrderContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextAdd"], Id = "new" }); break;
                            case "PurchaseOrderContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextChange"], Id = "changed" }); break;
                            case "PurchaseOrderContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextDelete"], Id = "delete" }); break;
                            case "PurchaseOrderContextApproveOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextApproveOrder"], Id = "approveorder" }); break;
                            case "PurchaseOrderContextCreateStockFiches":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextCreateStockFiches"], Id = "createstockfiches" }); break;
                            case "PurchaseOrderContextCancelOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextCancelOrder"], Id = "cancelorder" }); break;
                            case "PurchaseOrderContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseOrderContextRefresh"], Id = "refresh" }); break;
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
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "approveorder":
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState == Entities.Enums.PurchaseOrderStateEnum.Beklemede)
                    {
                        foreach (var line in GridLineList.ToList())
                        {
                            if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Beklemede)
                            {
                                int lineIndex = GridLineList.IndexOf(line);
                                line.PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Onaylandı;
                                GridLineList[lineIndex] = line;
                            }
                        }

                        DataSource.SelectPurchaseOrderLinesDto = GridLineList;
                        DataSource.PurchaseOrderState = Entities.Enums.PurchaseOrderStateEnum.Onaylandı;
                        var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);
                        await UpdateAsync(updateInput);
                        await ModalManager.MessagePopupAsync(L["UIInformationTitle"], L["UIInformationApproveOrder"]);

                    }

                    await InvokeAsync(StateHasChanged);
                    break;

                case "createstockfiches":
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Onaylandı)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningStockFichesTitle"], L["UIWarningStockFichesMessage"]);
                    }
                    else
                    {
                        bool selectedLine = false;

                        foreach (var line in GridLineList)
                        {
                            if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Tamamlandi || line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.KismiTamamlandi)
                            {
                                selectedLine = true;
                            }
                            CreateStockReceiptFishes createStockReceiptFichesModel = new CreateStockReceiptFishes
                            {
                                PurchaseStateLine = line.PurchaseOrderLineStateEnum,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                Quantity = line.Quantity,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                                SelectedLine = selectedLine,
                                LineID = line.Id,
                            };

                            CreateStockFishesList.Add(createStockReceiptFichesModel);
                        }

                        CreateStockFishesCrudPopup = true;
                    }


                    await InvokeAsync(StateHasChanged);
                    break;

                case "cancelorder":
                    DataSource = (await PurchaseOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseOrderLinesDto;

                    if (DataSource.PurchaseOrderState != Entities.Enums.PurchaseOrderStateEnum.Iptal)
                    {
                        bool selectedLine = true;

                        foreach (var line in GridLineList)
                        {
                            if (line.PurchaseOrderLineStateEnum == Entities.Enums.PurchaseOrderLineStateEnum.Iptal)
                            {
                                selectedLine = false;
                            }
                            CreateStockReceiptFishes createStockReceiptFichesModel = new CreateStockReceiptFishes
                            {
                                PurchaseStateLine = line.PurchaseOrderLineStateEnum,
                                ProductCode = line.ProductCode,
                                ProductID = line.ProductID,
                                ProductName = line.ProductName,
                                Quantity = line.Quantity,
                                UnitSetCode = line.UnitSetCode,
                                UnitSetID = line.UnitSetID,
                                SelectedLine = selectedLine,
                                LineID = line.Id,
                            };

                            CancelOrderList.Add(createStockReceiptFichesModel);
                        }

                        CancelOrderCrudPopup = true;
                    }


                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseOrderLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectPurchaseOrderLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.PaymentPlanID = DataSource.PaymentPlanID;
                    LineDataSource.PaymentPlanName = DataSource.PaymentPlanName;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectPurchaseOrderLinesDto.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPurchaseOrderLinesDto.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPurchaseOrderLinesDto.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
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
            if (LineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase1"]);
            }
            else if (LineDataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase2"]);
            }
            else if (LineDataSource.Quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase3"]);
            }
            else
            {
                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectPurchaseOrderLinesDto.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectPurchaseOrderLinesDto.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectPurchaseOrderLinesDto[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectPurchaseOrderLinesDto.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectPurchaseOrderLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseOrderLinesDto[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectPurchaseOrderLinesDto;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        public override async void LineCalculate()
        {
            LineDataSource.LineAmount = (LineDataSource.Quantity * LineDataSource.UnitPrice) - LineDataSource.DiscountAmount;
            LineDataSource.VATamount = (LineDataSource.LineAmount * LineDataSource.VATrate) / 100;

            if (LineDataSource.DiscountAmount > 0)
            {
                if (LineDataSource.Quantity > 0 && LineDataSource.UnitPrice > 0)
                {
                    LineDataSource.DiscountRate = (LineDataSource.DiscountAmount / (LineDataSource.Quantity * LineDataSource.UnitPrice)) * 100;
                }

                if (LineDataSource.Quantity == 0 || LineDataSource.UnitPrice == 0)
                {
                    LineDataSource.DiscountRate = 0;
                    LineDataSource.DiscountAmount = 0;
                }
            }
            else
            {
                LineDataSource.DiscountRate = 0;
            }

            LineDataSource.LineTotalAmount = LineDataSource.LineAmount + LineDataSource.VATamount;

            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {
            DataSource.GrossAmount = GridLineList.Sum(x => x.LineAmount) + GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalDiscountAmount = GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalVatExcludedAmount = DataSource.GrossAmount - DataSource.TotalDiscountAmount;
            DataSource.TotalVatAmount = GridLineList.Sum(x => x.VATamount);
            DataSource.NetAmount = GridLineList.Sum(x => x.LineTotalAmount);
        }

        protected override async Task OnSubmit()
        {
            SelectPurchaseOrdersDto result;

            if (DataSource.Id == Guid.Empty)
            {
                foreach(var item in DataSource.SelectPurchaseOrderLinesDto)
                {
                    int itemIndex = DataSource.SelectPurchaseOrderLinesDto.IndexOf(item);
                    DataSource.SelectPurchaseOrderLinesDto[itemIndex].PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Beklemede;
                }

                var createInput = ObjectMapper.Map<SelectPurchaseOrdersDto, CreatePurchaseOrdersDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            if (result == null)
            {

                return;
            }

            await GetListDataSourceAsync();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideEditPage();

            if (DataSource.Id == Guid.Empty)
            {
                DataSource.Id = result.Id;
            }

            if (savedEntityIndex > -1)
                SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
            else
                SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
        }


        #endregion

        #region GetList Metotları

        private async Task GetLineProductionOrdersList()
        {
            LineProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }
        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        private async Task GetBranchesList()
        {
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }


        private async Task GetWarehousesList()
        {
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetPaymentPlansList()
        {
            PaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu");
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
