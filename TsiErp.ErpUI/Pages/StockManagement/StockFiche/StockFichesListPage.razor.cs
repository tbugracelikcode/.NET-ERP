﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.StockManagement.StockFiche
{
    public partial class StockFichesListPage : IDisposable
    {

        #region Stock Parameters

        bool futureDateParameter;
        DateTime MaxDate = DateTime.MinValue;

        #endregion

        private SfGrid<SelectStockFicheLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectStockFicheLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectStockFicheLinesDto> GridLineList = new List<SelectStockFicheLinesDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        private bool LineCrudPopup = false;
        private bool Visibility { get; set; }


        #region Combobox İşlemleri

        public IEnumerable<SelectStockFichesDto> types = GetEnumDisplayTypeNames<StockFicheTypeEnum>();

        public static List<SelectStockFichesDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<StockFicheTypeEnum>()
                       .Select(x => new SelectStockFichesDto
                       {
                           FicheType = x,
                           FicheTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
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
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.BranchID = selectedUnitSet.Id;
                DataSource.BranchCode = selectedUnitSet.Code;
                SelectBranchesPopupVisible = false;
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
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.CurrencyID = selectedCurrency.Id;
                DataSource.CurrencyCode = selectedCurrency.Name;
                SelectCurrencyPopupVisible = false;
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
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                DataSource.WarehouseID = selectedWarehouse.Id;
                DataSource.WarehouseCode = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;
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
                DataSource.ProductionOrderCode = string.Empty;
                DataSource.ProductionDateReferenceID = Guid.Empty;
                DataSource.ProductionDateReferenceNo = string.Empty;
            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                DataSource.ProductionOrderID = selectedProductionOrder.Id;
                DataSource.ProductionOrderCode = selectedProductionOrder.FicheNo;
                DataSource.ProductionDateReferenceID = selectedProductionOrder.ProductionDateReferenceID;
                DataSource.ProductionDateReferenceNo = selectedProductionOrder.ProductionDateReferenceNo;
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
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
                LineDataSource.UnitSetID = selectedProduct.UnitSetID;
                LineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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

        public void TransactionExchangeCurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TransactionExchangeCurrencyID = Guid.Empty;
                DataSource.TransactionExchangeCurrencyCode = string.Empty;
            }
        }

        public async void TransactionExchangeCurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                DataSource.TransactionExchangeCurrencyID = selectedCurrency.Id;
                DataSource.TransactionExchangeCurrencyCode = selectedCurrency.Name;
                SelectTransactionExchangeCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
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
            DataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region GetList Metotları

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

        private async Task GetProductionOrdersList()
        {
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }
        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            BaseCrudService = StockFichesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "StockFichesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            var stockParameterDataSource = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;

            futureDateParameter = stockParameterDataSource.FutureDateParameter;

            DateTime MaxDate = !futureDateParameter ? GetSQLDateAppService.GetDateFromSQL() : new DateTime(9999, 12, 31);

        }

        #region Stok Fişleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            var stockManagementParameter = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;


            DataSource = new SelectStockFichesDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                Time_ = GetSQLDateAppService.GetDateFromSQL().TimeOfDay,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                ProductionOrderID = Guid.Empty,
                CurrencyID = Guid.Empty,
                BranchID = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultBranchID : Guid.Empty,
                WarehouseID = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultWarehouseID : Guid.Empty,
                BranchCode = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultBranchCode : string.Empty,
                BranchName = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultBranchName : string.Empty,
                WarehouseCode = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultWarehouseCode : string.Empty,
                WarehouseName = stockManagementParameter != null && stockManagementParameter.Id != Guid.Empty ? stockManagementParameter.DefaultWarehouseName : string.Empty,
            };
            DataSource.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();
            GridLineList = DataSource.SelectStockFicheLines;
            Visibility = false;
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
                            case "StockFicheLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextAdd"], Id = "new" }); break;
                            case "StockFicheLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextChange"], Id = "changed" }); break;
                            case "StockFicheLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextDelete"], Id = "delete" }); break;
                            case "StockFicheLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextRefresh"], Id = "refresh" }); break;
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
                            case "StockFichesGeneralConAdd":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "StockFicheContextAddStockIncome":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockIncome"], Id = "income" }); break;
                                            case "StockFicheContextAddStockOutput":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockOutput"], Id = "output" }); break;
                                            case "StockFicheContextAddConsume":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddConsume"], Id = "consume" }); break;
                                            case "StockFicheContextAddWastege":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWastege"], Id = "wastage" }); break;
                                            case "StockFicheContextAddProductionIncome":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddProductionIncome"], Id = "proincome" }); break;
                                            //case "StockFicheContextAddWarehouse":
                                            //    subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWarehouse"], Id = "warehouse" }); break;
                                            case "StockFicheContextAddReserved":
                                                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddReserved"], Id = "reserved" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFichesGeneralConAdd"], Id = "add", Items = subMenus }); break;

                            case "StockFicheContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextChange"], Id = "changed" }); break;
                            case "StockFicheContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextDelete"], Id = "delete" }); break;
                            case "StockFicheContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextRefresh"], Id = "refresh" }); break;

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListStockFichesDto> args)
        {
            foreach (var item in types)
            {
                item.FicheTypeName = L[item.FicheTypeName];
            }

            switch (args.Item.Id)
            {
                case "wastage":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.FireFisi;
                    Visibility = false;
                    EditPageVisible = true;
                    break;

                case "consume":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.SarfFisi;
                    Visibility = false;
                    EditPageVisible = true;
                    break;

                case "proincome":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.UretimdenGirisFisi;
                    Visibility = true;
                    EditPageVisible = true;
                    break;

                //case "warehouse":

                //    await BeforeInsertAsync();
                //    DataSource.FicheType = StockFicheTypeEnum.DepoSevkFisi;
                //    EditPageVisible = true;
                //    break;
                case "reserved":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.StokRezerveFisi;
                    Visibility = false;
                    EditPageVisible = true;
                    break;

                case "income":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.StokGirisFisi;
                    Visibility = false;
                    EditPageVisible = true;
                    break;

                case "output":

                    await BeforeInsertAsync();
                    DataSource.FicheType = StockFicheTypeEnum.StokCikisFisi;
                    Visibility = false;
                    EditPageVisible = true;
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                    DataSource = (await StockFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectStockFicheLines;


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

                default:
                    break;
            }

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectStockFicheLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (DataSource.FicheType == 0)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningFicheTypeTitleBase"], L["UIWarningFicheTypeMessageBase"]);
                    }
                    else
                    {
                        LineDataSource = new SelectStockFicheLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.FicheType = DataSource.FicheType;
                        LineDataSource.LineNr = GridLineList.Count + 1;
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

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {
var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectStockFicheLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectStockFicheLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectStockFicheLines.Remove(line);
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

            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
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
                LineCalculate();

                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectStockFicheLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectStockFicheLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectStockFicheLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectStockFicheLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectStockFicheLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectStockFicheLines[selectedLineIndex] = LineDataSource;
                    }
                }

                GridLineList = DataSource.SelectStockFicheLines;

                DataSource.NetAmount = GridLineList.Sum(t => t.LineAmount);

                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        public override async void LineCalculate()
        {
            LineDataSource.LineAmount = LineDataSource.Quantity * LineDataSource.UnitPrice;
            LineDataSource.TransactionExchangeLineAmount = LineDataSource.Quantity * LineDataSource.TransactionExchangeUnitPrice;

            await Task.CompletedTask;
        }


        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
