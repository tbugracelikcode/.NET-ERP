using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PurchaseManagement.PurchaseRequest
{
    public partial class PurchaseRequestsListPage : IDisposable
    {
        private SfGrid<SelectPurchaseRequestLinesDto> _LineGrid;
        private SfGrid<SelectPurchaseRequestLinesDto> _ConvertToOrderGrid;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectPurchaseRequestLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ConvertToOrderGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPurchaseRequestLinesDto> GridLineList = new List<SelectPurchaseRequestLinesDto>();

        List<SelectPurchaseRequestLinesDto> GridConvertToOrderList = new List<SelectPurchaseRequestLinesDto>();

        private bool LineCrudPopup = false;

        private bool ConvertToOrderCrudPopup = false;

        DateTime MaxDate;

        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseRequestsAppService;
            _L = L;

            futureDateParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data.FutureDateParameter;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PurchaseRequestsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateConvertToOrderContextMenuItems();

            MaxDate = !futureDateParameter ? GetSQLDateAppService.GetDateFromSQL() : new DateTime(10000, 12, 31);
        }

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
                DataSource.CurrencyCode = selectedUnitSet.Code;
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
                DataSource.WarehouseName = string.Empty;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.WarehouseID = selectedUnitSet.Id;
                DataSource.WarehouseCode = selectedUnitSet.Code;
                DataSource.WarehouseName = selectedUnitSet.Name;
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
                DataSource.TransactionExchangeCurrencyCode = selectedUnitSet.Currency;
                DataSource.TransactionExchangeCurrencyID = selectedUnitSet.CurrencyID;
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
                LineDataSource.UnitSetCode = string.Empty;
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.SupplierReferenceNo = string.Empty;
                LineDataSource.VATrate = 0;
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
                LineDataSource.UnitSetID = selectedProduct.UnitSetID;
                LineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                LineDataSource.VATrate = selectedProduct.SaleVAT;

                if (DataSource.CurrentAccountCardID != Guid.Empty && DataSource.CurrentAccountCardID != null)
                {
                    //var lastApprovedPriceID = (await PurchasePricesAppService.GetListAsync(new ListPurchasePricesParameterDto())).Data.Where(t => t.IsApproved == true && t.CurrentAccountCardID == DataSource.CurrentAccountCardID).LastOrDefault().Id;

                    //if (lastApprovedPriceID != Guid.Empty)
                    //{
                    //    LineDataSource.UnitPrice = (await PurchasePricesAppService.GetAsync(lastApprovedPriceID)).Data.SelectPurchasePriceLines.Where(t => t.ProductID == selectedProduct.Id).Select(t => t.Price).FirstOrDefault();

                    //}

                    string supplierReferanceNumber = ProductReferanceNumbersAppService.GetLastSupplierReferanceNumber(selectedProduct.Id, DataSource.CurrentAccountCardID.GetValueOrDefault());

                    if (!string.IsNullOrEmpty(supplierReferanceNumber))
                    {
                        LineDataSource.SupplierReferenceNo = supplierReferanceNumber;
                    }
                    else
                    {
                        LineDataSource.SupplierReferenceNo = "-";
                    }
                }


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
        bool SelectProductionOrdersLinePopupVisible = false;
        List<ListProductionOrdersDto> LineProductionOrdersList = new List<ListProductionOrdersDto>();

        public async Task LineProductionOrdersOnCreateIcon()
        {
            var LineProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, LineProductionOrdersButtonClickEvent);
            await LineProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", LineProductionOrdersButtonClick } });
        }

        public async void LineProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersLinePopupVisible = true;
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
                SelectProductionOrdersLinePopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Satın Almayı Talebe Dönüştürme Modalı İşlemleri

        protected void CreateConvertToOrderContextMenuItems()
        {
            if (ConvertToOrderGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ConvertContextApprove":
                                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = L["ConvertContextApprove"], Id = "approve" }); break;
                            case "ConvertContextPending":
                                ConvertToOrderGridContextMenu.Add(new ContextMenuItemModel { Text = L["ConvertContextPending"], Id = "onhold" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void OnConvertToOrderContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseRequestLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "approve":


                    var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState != Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı;
                        }

                    }

                    DataSource.SelectPurchaseRequestLines = GridConvertToOrderList;

                    #region Talep Durumunu Onaylandı Kaydetme

                    SelectPurchaseRequestsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

                        result = (await UpdateAsync(updateInput)).Data;
                    }

                    #endregion

                    await _ConvertToOrderGrid.Refresh();

                    break;

                case "onhold":

                    selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

                    foreach (var item in selectedIndexList)
                    {
                        if (GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState != Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma)
                        {
                            GridConvertToOrderList[Convert.ToInt32(item)].PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Beklemede;
                        }
                    }

                    DataSource.SelectPurchaseRequestLines = GridConvertToOrderList;

                    #region Teklif Durumunu Beklemede Olarak Kaydetme

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

                        result = (await UpdateAsync(updateInput)).Data;
                    }

                    #endregion

                    await _ConvertToOrderGrid.Refresh();

                    break;

                default:
                    break;
            }
        }

        protected async Task OnConvertToOrderBtnClicked()
        {

            List<SelectPurchaseRequestLinesDto> _orderList = new List<SelectPurchaseRequestLinesDto>();
            var selectedRowList = _ConvertToOrderGrid.SelectedRecords;
            var selectedIndexList = _ConvertToOrderGrid.SelectedRowIndexes;

            foreach (var item in selectedRowList)
            {
                _orderList.Add(item);
            }

            var approvedSelected = _orderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı).ToList();

            if (approvedSelected.Count > 0)
            {
                #region Talebi Siparişe Çevirme

                CreatePurchaseOrdersDto createPurchaseOrder = new CreatePurchaseOrdersDto
                {
                    BranchID = DataSource.BranchID,
                    CurrencyID = DataSource.CurrencyID,
                    CurrentAccountCardID = DataSource.CurrentAccountCardID,
                    Date_ = DataSource.Date_,
                    Description_ = DataSource.Description_,
                    ExchangeRate = DataSource.ExchangeRate,
                    GrossAmount = DataSource.GrossAmount,
                    LinkedPurchaseRequestID = DataSource.Id,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersChildMenu"),
                    NetAmount = DataSource.NetAmount,
                    PaymentPlanID = DataSource.PaymentPlanID,
                    PurchaseOrderState = (int)Entities.Enums.PurchaseOrderStateEnum.Beklemede,
                    SpecialCode = DataSource.SpecialCode,
                    Time_ = DataSource.Time_,
                    TotalDiscountAmount = DataSource.TotalDiscountAmount,
                    TotalVatAmount = DataSource.TotalVatAmount,
                    TotalVatExcludedAmount = DataSource.TotalVatExcludedAmount,
                    WarehouseID = DataSource.WarehouseID,
                    ProductionOrderID = DataSource.ProductionOrderID,
                    PurchaseOrderWayBillStatusEnum = (int)Entities.Enums.PurchaseOrderWayBillStatusEnum.Beklemede,
                    PriceApprovalState = (int)Entities.Enums.PurchaseOrderPriceApprovalStateEnum.Beklemede
                };

                List<SelectPurchaseOrderLinesDto> orderLineList = new List<SelectPurchaseOrderLinesDto>();

                foreach (var item in approvedSelected)
                {
                    SelectPurchaseOrderLinesDto selectPurchaseOrderLine = new SelectPurchaseOrderLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPurchaseRequestLineID = item.Id,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineNr = item.LineNr,
                        LineTotalAmount = item.LineTotalAmount,
                        PaymentPlanID = item.PaymentPlanID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        PurchaseOrderID = Guid.Empty,
                        PurchaseOrderLineStateEnum = Entities.Enums.PurchaseOrderLineStateEnum.Beklemede,
                        UnitPrice = item.UnitPrice,
                        UnitSetID = item.UnitSetID,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        LinkedPurchaseRequestID = createPurchaseOrder.LinkedPurchaseRequestID,
                        PurchaseOrderLineWayBillStatusEnum = Entities.Enums.PurchaseOrderLineWayBillStatusEnum.Beklemede
                    };
                    orderLineList.Add(selectPurchaseOrderLine);
                    item.PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma;
                }

                createPurchaseOrder.SelectPurchaseOrderLinesDto = orderLineList;

                await PurchaseOrdersAppService.ConvertToPurchaseOrderAsync(createPurchaseOrder);

                #endregion

                await ModalManager.MessagePopupAsync(L["UIInformationConvertTitle"], L["UIInformationConvertMessage"]);
            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningConvertTitle"], L["UIWarningConvertMessage"]);
            }

            #region Ana Satırın Sipariş Durumunu Belirleme

            int approvedMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Onaylandı).Count();
            int onholdMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.Beklemede).Count();
            int orderMain = GridConvertToOrderList.Where(t => t.PurchaseRequestLineState == Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma).Count();

            #region Koşullar

            if (orderMain != 0 && orderMain != GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.KismiSatinAlma;
            }
            else if (orderMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.SatinAlma;
            }
            else if (approvedMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.Onaylandı;
            }
            else if (approvedMain != 0 && approvedMain != GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.KismiOnaylandi;
            }
            else if (onholdMain == GridConvertToOrderList.Count())
            {
                DataSource.PurchaseRequestState = Entities.Enums.PurchaseRequestStateEnum.Beklemede;
            }

            #endregion

            #endregion

            GridConvertToOrderList = DataSource.SelectPurchaseRequestLines;

            #region Teklif Durumunu Sipariş Olarak Kaydetme

            SelectPurchaseRequestsDto result;

            if (DataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>(DataSource);

                result = (await CreateAsync(createInput)).Data;

                if (result != null)
                    DataSource.Id = result.Id;
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>(DataSource);

                result = (await UpdateAsync(updateInput)).Data;
            }

            #endregion

            GetTotal();
            await _ConvertToOrderGrid.Refresh();
            await InvokeAsync(StateHasChanged);
        }

        public void HideConvertToOrderPopup()
        {
            ConvertToOrderCrudPopup = false;
        }

        #endregion

        #region Teklif Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseRequestsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                ValidityDate_ = GetSQLDateAppService.GetDateFromSQL().AddDays(15),
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseRequestsChildMenu")
            };

            DataSource.SelectPurchaseRequestLines = new List<SelectPurchaseRequestLinesDto>();
            GridLineList = DataSource.SelectPurchaseRequestLines;

            var localCurrency = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.Where(t => t.IsLocalCurrency == true).FirstOrDefault();

            if (localCurrency != null && localCurrency.Id != Guid.Empty)
            {
                DataSource.CurrencyID = localCurrency.Id;
                DataSource.CurrencyCode = localCurrency.Code;
            }

            foreach (var item in PricingCurrencyList)
            {
                item.PricingCurrencyName = L[item.PricingCurrencyName];
            }


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
                            case "PurchaseRequestLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestLineContextAdd"], Id = "new" }); break;
                            case "PurchaseRequestLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestLineContextChange"], Id = "changed" }); break;
                            case "PurchaseRequestLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestLineContextDelete"], Id = "delete" }); break;
                            case "PurchaseRequestLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestLineContextRefresh"], Id = "refresh" }); break;
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
                            case "PurchaseRequestContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestContextAdd"], Id = "new" }); break;
                            case "PurchaseRequestContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestContextChange"], Id = "changed" }); break;
                            case "PurchaseRequestContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestContextDelete"], Id = "delete" }); break;
                            case "PurchaseRequestContextConverttoOrder":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestContextConverttoOrder"], Id = "converttoorder" }); break;
                            case "PurchaseRequestContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PurchaseRequestContextRefresh"], Id = "refresh" }); break;
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
                    foreach (var item in PricingCurrencyList)
                    {
                        item.PricingCurrencyName = L[item.PricingCurrencyName];
                    }

                    EditPageVisible = true;

                    #region Fiyatlandırma Dövizi

                    if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency)
                    {
                        UnitPriceEnabled = true;
                        DiscountAmountEnabled = true;
                        LineAmountEnabled = true;
                        LineTotalAmountEnabled = true;

                        TransactionExchangeUnitPriceEnabled = false;
                        TransactionExchangeDiscountAmountEnabled = false;
                        TransactionExchangeLineAmountEnabled = false;
                        TransactionExchangeLineTotalAmountEnabled = false;
                    }
                    else if (DataSource.PricingCurrency == PricingCurrencyEnum.TransactionCurrency)
                    {
                        UnitPriceEnabled = false;
                        DiscountAmountEnabled = false;
                        LineAmountEnabled = false;
                        LineTotalAmountEnabled = false;

                        TransactionExchangeUnitPriceEnabled = true;
                        TransactionExchangeDiscountAmountEnabled = true;
                        TransactionExchangeLineAmountEnabled = true;
                        TransactionExchangeLineTotalAmountEnabled = true;
                    }
                    else
                    {
                        UnitPriceEnabled = false;
                        DiscountAmountEnabled = false;
                        LineAmountEnabled = false;
                        LineTotalAmountEnabled = false;

                        TransactionExchangeUnitPriceEnabled = false;
                        TransactionExchangeDiscountAmountEnabled = false;
                        TransactionExchangeLineAmountEnabled = false;
                        TransactionExchangeLineTotalAmountEnabled = false;
                    }

                    #endregion

                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPurchaseRequestsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PurchaseRequestsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPurchaseRequestLines;


                    ShowEditPage();
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

                case "converttoorder":

                    DataSource = (await PurchaseRequestsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridConvertToOrderList = DataSource.SelectPurchaseRequestLines;

                    foreach (var item in GridConvertToOrderList)
                    {
                        var productDataSource = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data;

                        item.ProductCode = productDataSource.Code;
                        item.ProductName = productDataSource.Name;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    ConvertToOrderCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPurchaseRequestLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectPurchaseRequestLinesDto();
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
                            DataSource.SelectPurchaseRequestLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectPurchaseRequestLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectPurchaseRequestLines.Remove(line);
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
                LineDataSource.PurchaseRequestLineState = Entities.Enums.PurchaseRequestLineStateEnum.Beklemede;

                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectPurchaseRequestLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectPurchaseRequestLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectPurchaseRequestLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectPurchaseRequestLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectPurchaseRequestLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPurchaseRequestLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectPurchaseRequestLines;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        public void LineQuantityReserved()
        {
            LineDataSource.PurchaseReservedQuantity = LineDataSource.Quantity - LineDataSource.WaitingQuantity;
        }

        public void LineQuantityWaiting()
        {
            LineDataSource.WaitingQuantity = LineDataSource.Quantity - LineDataSource.PurchaseReservedQuantity;
        }

        public override async void LineCalculate()
        {
            if (DataSource.PricingCurrency == PricingCurrencyEnum.LocalCurrency)
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
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                    }
                }
                else
                {
                    LineDataSource.DiscountRate = 0;
                }

                LineDataSource.LineTotalAmount = LineDataSource.LineAmount + LineDataSource.VATamount;


                LineDataSource.TransactionExchangeUnitPrice = 0;
                LineDataSource.TransactionExchangeVATamount = 0;
                LineDataSource.TransactionExchangeDiscountAmount = 0;
                LineDataSource.TransactionExchangeLineAmount = 0;
                LineDataSource.TransactionExchangeLineTotalAmount = 0;
            }
            else
            {
                LineDataSource.TransactionExchangeLineAmount = (LineDataSource.Quantity * LineDataSource.TransactionExchangeUnitPrice) - LineDataSource.TransactionExchangeDiscountAmount;
                LineDataSource.TransactionExchangeVATamount = (LineDataSource.TransactionExchangeLineAmount * LineDataSource.VATrate) / 100;

                if (LineDataSource.TransactionExchangeDiscountAmount > 0)
                {
                    if (LineDataSource.Quantity > 0 && LineDataSource.TransactionExchangeUnitPrice > 0)
                    {
                        LineDataSource.DiscountRate = (LineDataSource.TransactionExchangeDiscountAmount / (LineDataSource.Quantity * LineDataSource.TransactionExchangeUnitPrice)) * 100;
                    }

                    if (LineDataSource.Quantity == 0 || LineDataSource.TransactionExchangeUnitPrice == 0)
                    {
                        LineDataSource.DiscountRate = 0;
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                        LineDataSource.TransactionExchangeDiscountAmount = 0;
                    }
                }
                else
                {
                    LineDataSource.DiscountRate = 0;
                }

                LineDataSource.TransactionExchangeLineTotalAmount = LineDataSource.TransactionExchangeLineAmount + LineDataSource.TransactionExchangeVATamount;


                LineDataSource.UnitPrice = LineDataSource.TransactionExchangeUnitPrice * DataSource.ExchangeRate;
                LineDataSource.VATamount = LineDataSource.TransactionExchangeVATamount * DataSource.ExchangeRate;
                LineDataSource.DiscountAmount = LineDataSource.TransactionExchangeDiscountAmount * DataSource.ExchangeRate;
                LineDataSource.LineAmount = LineDataSource.TransactionExchangeLineAmount * DataSource.ExchangeRate;
                LineDataSource.LineTotalAmount = LineDataSource.TransactionExchangeLineTotalAmount * DataSource.ExchangeRate;

            }

            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {
            DataSource.GrossAmount = GridLineList.Sum(x => x.LineAmount) + GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalDiscountAmount = GridLineList.Sum(x => x.DiscountAmount);
            DataSource.TotalVatExcludedAmount = DataSource.GrossAmount - DataSource.TotalDiscountAmount;
            DataSource.TotalVatAmount = GridLineList.Sum(x => x.VATamount);
            DataSource.NetAmount = GridLineList.Sum(x => x.LineTotalAmount);

            DataSource.TransactionExchangeGrossAmount = GridLineList.Sum(x => x.TransactionExchangeLineAmount) + GridLineList.Sum(x => x.TransactionExchangeDiscountAmount);
            DataSource.TransactionExchangeTotalDiscountAmount = GridLineList.Sum(x => x.TransactionExchangeDiscountAmount);
            DataSource.TransactionExchangeTotalVatExcludedAmount = DataSource.TransactionExchangeGrossAmount - DataSource.TransactionExchangeTotalDiscountAmount;
            DataSource.TransactionExchangeTotalVatAmount = GridLineList.Sum(x => x.TransactionExchangeVATamount);
            DataSource.TransactionExchangeNetAmount = GridLineList.Sum(x => x.TransactionExchangeLineTotalAmount);
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

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
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

        #endregion

        #region İşlem Dövizi Para Birimleri ButtonEdit

        SfTextBox TransactionExchangeCurrenciesButtonEdit;
        bool SelectTransactionExchangeCurrencyPopupVisible = false;
        List<ListCurrenciesDto> TransactionExchangeCurrenciesList = new List<ListCurrenciesDto>();

        public async Task TransactionExchangeCurrenciesOnCreateIcon()
        {
            var TransactionExchangeCurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TransactionExchangeCurrenciesButtonClickEvent);
            await TransactionExchangeCurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TransactionExchangeCurrenciesButtonClick } });
        }

        public async void TransactionExchangeCurrenciesButtonClickEvent()
        {
            SelectTransactionExchangeCurrencyPopupVisible = true;
            TransactionExchangeCurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void TransactionExchangeCurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TransactionExchangeCurrencyID = Guid.Empty;
                DataSource.TransactionExchangeCurrencyCode = string.Empty;
                DataSource.ExchangeRate = 0;
            }
            else
            {
                decimal exRate = 1;

                var exchangeRateType = LoginedUserService.PurchaseRequestExchangeRateType;

                var date = new DateTime(DataSource.Date_.Year, DataSource.Date_.Month, DataSource.Date_.Day);

                var exchangeRate = (await ExchangeRatesService.GetListAsync(new ListExchangeRatesParameterDto())).Data.Where(t => t.Date == date && t.CurrencyId == DataSource.TransactionExchangeCurrencyID).FirstOrDefault();

                if (exchangeRate != null)
                {
                    switch (exchangeRateType)
                    {
                        case 0:
                            exRate = exchangeRate.BuyingRate;
                            break;
                        case 1:
                            exRate = exchangeRate.SaleRate;
                            break;
                        case 2:
                            exRate = exchangeRate.EffectiveBuyingRate;
                            break;
                        case 3:
                            exRate = exchangeRate.EffectiveSaleRate;
                            break;
                        default:
                            exRate = 1;
                            break;
                    }
                }

                DataSource.ExchangeRate = exRate;

            }
        }

        public async void TransactionExchangeCurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.TransactionExchangeCurrencyID = selectedCurrency.Id;
                DataSource.TransactionExchangeCurrencyCode = selectedCurrency.Code;
                SelectTransactionExchangeCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Fiyatlandırma Dövizi Enum Combobox

        public IEnumerable<SelectPurchaseRequestsDto> PricingCurrencyList = GetEnumPricingCurrencyNames<PricingCurrencyEnum>();

        public static List<SelectPurchaseRequestsDto> GetEnumPricingCurrencyNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PricingCurrencyEnum>()
                       .Select(x => new SelectPurchaseRequestsDto
                       {
                           PricingCurrency = x,
                           PricingCurrencyName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        private async void PricingCurrencyValueChangeHandler(ChangeEventArgs<PricingCurrencyEnum, SelectPurchaseRequestsDto> args)
        {
            if (args != null)
            {
                if (args.Value == PricingCurrencyEnum.LocalCurrency)
                {
                    UnitPriceEnabled = true;
                    DiscountAmountEnabled = true;
                    LineAmountEnabled = true;
                    LineTotalAmountEnabled = true;

                    TransactionExchangeUnitPriceEnabled = false;
                    TransactionExchangeDiscountAmountEnabled = false;
                    TransactionExchangeLineAmountEnabled = false;
                    TransactionExchangeLineTotalAmountEnabled = false;
                }
                else if (args.Value == PricingCurrencyEnum.TransactionCurrency)
                {
                    UnitPriceEnabled = false;
                    DiscountAmountEnabled = false;
                    LineAmountEnabled = false;
                    LineTotalAmountEnabled = false;

                    TransactionExchangeUnitPriceEnabled = true;
                    TransactionExchangeDiscountAmountEnabled = true;
                    TransactionExchangeLineAmountEnabled = true;
                    TransactionExchangeLineTotalAmountEnabled = true;
                }
                else
                {
                    UnitPriceEnabled = false;
                    DiscountAmountEnabled = false;
                    LineAmountEnabled = false;
                    LineTotalAmountEnabled = false;

                    TransactionExchangeUnitPriceEnabled = false;
                    TransactionExchangeDiscountAmountEnabled = false;
                    TransactionExchangeLineAmountEnabled = false;
                    TransactionExchangeLineTotalAmountEnabled = false;
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        private bool UnitPriceEnabled = false;
        private bool DiscountAmountEnabled = false;
        private bool LineAmountEnabled = false;
        private bool LineTotalAmountEnabled = false;

        private bool TransactionExchangeUnitPriceEnabled = false;
        private bool TransactionExchangeDiscountAmountEnabled = false;
        private bool TransactionExchangeLineAmountEnabled = false;
        private bool TransactionExchangeLineTotalAmountEnabled = false;

        #endregion

        public async void DateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            decimal exRate = 1;

            var exchangeRateType = LoginedUserService.PurchaseRequestExchangeRateType;

            var date = new DateTime(args.Value.Year, args.Value.Month, args.Value.Day);

            var exchangeRate = (await ExchangeRatesService.GetListAsync(new ListExchangeRatesParameterDto())).Data.Where(t => t.Date == date && t.CurrencyId == DataSource.TransactionExchangeCurrencyID).FirstOrDefault();

            if (exchangeRate != null)
            {
                switch (exchangeRateType)
                {
                    case 0:
                        exRate = exchangeRate.BuyingRate;
                        break;
                    case 1:
                        exRate = exchangeRate.SaleRate;
                        break;
                    case 2:
                        exRate = exchangeRate.EffectiveBuyingRate;
                        break;
                    case 3:
                        exRate = exchangeRate.EffectiveSaleRate;
                        break;
                    default:
                        exRate = 1;
                        break;
                }
            }

            DataSource.Date_ = args.Value;
            DataSource.ExchangeRate = exRate;
            await InvokeAsync(StateHasChanged);
        }

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("PurchaseRequestsChildMenu");
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
