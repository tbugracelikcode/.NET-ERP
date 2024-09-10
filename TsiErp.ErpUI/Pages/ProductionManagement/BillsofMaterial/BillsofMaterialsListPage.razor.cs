using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.BillsofMaterial
{
    public partial class BillsofMaterialsListPage : IDisposable
    {

        private SfGrid<SelectBillsofMaterialLinesDto> _LineGrid;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectBillsofMaterialLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectBillsofMaterialLinesDto> GridLineList = new List<SelectBillsofMaterialLinesDto>();

        private bool LineCrudPopup = false;

        public bool isCustomerCodeAvailable = false;

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

        protected override async void OnInitialized()
        {
            BaseCrudService = BillsofMaterialsAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "BOMChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Reçete Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectBillsofMaterialsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("BOMChildMenu")
            };

            DataSource.SelectBillsofMaterialLines = new List<SelectBillsofMaterialLinesDto>();
            GridLineList = DataSource.SelectBillsofMaterialLines;

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
                            case "BoMLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMLineContextAdd"], Id = "new" }); break;
                            case "BoMLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMLineContextChange"], Id = "changed" }); break;
                            case "BoMLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMLineContextDelete"], Id = "delete" }); break;
                            case "BoMLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMLineContextRefresh"], Id = "refresh" }); break;
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
                            case "BoMContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMContextAdd"], Id = "new" }); break;
                            case "BoMContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMContextChange"], Id = "changed" }); break;
                            case "BoMContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMContextDelete"], Id = "delete" }); break;
                            case "BoMContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["BoMContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListBillsofMaterialsDto> args)
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
                    DataSource = (await BillsofMaterialsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectBillsofMaterialLines;

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
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectBillsofMaterialLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":

                    if (DataSource.FinishedProductCode != null)
                    {
                        LineDataSource = new SelectBillsofMaterialLinesDto();
                        LineCrudPopup = true;
                        LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
                        LineDataSource.FinishedProductID = DataSource.FinishedProductID;
                        LineDataSource.LineNr = GridLineList.Count + 1;
                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["DeleteConfirmationTitleBase"], L["UILineNewContextWarning"]);
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

                        var res = await ModalManager.ConfirmationAsync(L["UILineDeleteContextAttentionTitle"], L["UILineDeleteConfirmation"]);

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectBillsofMaterialLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectBillsofMaterialLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectBillsofMaterialLines.Remove(line);
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
            if (LineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["DeleteConfirmationTitleBase"], L["UILineSubmitWithoutUnitset"]);
            }
            else if (LineDataSource.Quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["DeleteConfirmationTitleBase"], L["UILineSubmitQuantityZero"]);
            }
            else
            {
                if (LineDataSource.Id == Guid.Empty)
                {
                    if (DataSource.SelectBillsofMaterialLines.Contains(LineDataSource))
                    {
                        int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                        }
                    }
                    else
                    {
                        DataSource.SelectBillsofMaterialLines.Add(LineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.Id == LineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                    }
                }

                LineDataSource.FinishedProductID = DataSource.FinishedProductID;
                LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
                GridLineList = DataSource.SelectBillsofMaterialLines;
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

        }

        protected override async Task OnSubmit()
        {
            if (isCustomerCodeAvailable)
            {
                if (DataSource.CurrentAccountCardID == Guid.Empty && DataSource.CurrentAccountCardID == null)
                {
                    await ModalManager.WarningPopupAsync(L["UIWarningCustomerTitle"], L["UIWarningCustomerMessage"]);
                }
                else
                {
                    #region OnSubmit Kodları

                    SelectBillsofMaterialsDto result;

                    if (DataSource.Id == Guid.Empty)
                    {
                        var createInput = ObjectMapper.Map<SelectBillsofMaterialsDto, CreateBillsofMaterialsDto>(DataSource);

                        result = (await CreateAsync(createInput)).Data;

                        if (result != null)
                            DataSource.Id = result.Id;
                    }
                    else
                    {
                        var updateInput = ObjectMapper.Map<SelectBillsofMaterialsDto, UpdateBillsofMaterialsDto>(DataSource);

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

                    #endregion
                }
            }
            else
            {
                #region OnSubmit Kodları

                SelectBillsofMaterialsDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectBillsofMaterialsDto, CreateBillsofMaterialsDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectBillsofMaterialsDto, UpdateBillsofMaterialsDto>(DataSource);

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

                #endregion
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

        public async void ProductsNameButtonClickEvent()
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

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
                LineDataSource.SupplyForm = 0;
                LineDataSource.UnitSetCode = string.Empty;
                LineDataSource.MaterialType = 0;
                LineDataSource.UnitSetID = Guid.Empty;
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
                LineDataSource.MaterialType = selectedProduct.ProductType;
                LineDataSource.SupplyForm = selectedProduct.SupplyForm;

                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task CalculateQuantity(Guid productId)
        {
            double pi = 3.14;

            decimal density = (await ProductionManagementParametersService.GetProductionManagementParametersAsync()).Data.Density_;

            decimal size = LineDataSource.Size;

            if (density == 0)
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["UIEmptyDensityError"]);
                await InvokeAsync(StateHasChanged);
            }
            else if (size == 0)
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["UIEmptySizeError"]);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                var product = (await ProductsAppService.GetAsync(productId)).Data;
                if (product.Id != Guid.Empty)
                {
                    if (product.ProductType == ProductTypeEnum.HM)
                    {
                        if (product.RawMaterialType == RowMaterialTypeEnum.MilHammadde)
                        {
                            size = size + product.SawWastage;
                            decimal capDeger = product.RadiusValue;
                            decimal r = capDeger / 2;
                            decimal kg = ((decimal)pi * (r * r) * size * density) / 1000000;
                            LineDataSource.Quantity = kg;
                        }

                        if (product.RawMaterialType == RowMaterialTypeEnum.SacHammadde)
                        {
                            decimal width = product.Width_;
                            decimal tickness = product.Tickness_;
                            decimal kg = ((width * tickness * size) * size) / 1000000;
                            LineDataSource.Quantity = kg;
                        }

                        if (product.RawMaterialType == RowMaterialTypeEnum.BoruHammadde)
                        {
                            size = size + product.SawWastage;
                            decimal kg = size / 1000;
                            LineDataSource.Quantity = kg;
                        }
                    }

                    await Task.CompletedTask;
                }
            }
        }

        #endregion

        #region Cari Hesap ButtonEdit

        SfTextBox CurrentAccountCardsCodeButtonEdit = new();
        SfTextBox CurrentAccountCardsCustomerCodeButtonEdit = new();
        SfTextBox CurrentAccountCardsNameButtonEdit = new();
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

        public async Task CurrentAccountCardsCustomerCodeOnCreateIcon()
        {
            var CurrentAccountCardsCustomerCodeButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrentAccountCardsCustomerCodeButtonClickEvent);
            await CurrentAccountCardsCustomerCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrentAccountCardsCustomerCodeButtonClick } });
        }

        public async void CurrentAccountCardsCustomerCodeButtonClickEvent()
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
                DataSource.CustomerCode = string.Empty;

                if (DataSource.FinishedProductID != Guid.Empty && DataSource.FinishedProductID != null)
                {
                    DataSource.Name = DataSource.FinishedProductCode + " / " + DataSource.FinishedProducName;
                }
                else
                {
                    DataSource.Name = string.Empty;
                }
            }
        }

        public async void CurrentAccountCardsDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrentAccountCardsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                DataSource.CurrentAccountCardID = selectedUnitSet.Id; ;
                DataSource.CustomerCode = selectedUnitSet.CustomerCode;
                SelectCurrentAccountCardsPopupVisible = false;

                if (DataSource.FinishedProductID != Guid.Empty && DataSource.FinishedProductID != null && !string.IsNullOrEmpty(DataSource.CustomerCode))
                {

                    if(DataSource.Name.EndsWith("/"))
                    {
                        DataSource.Name = DataSource.Name + "  " + DataSource.CustomerCode;
                    }
                    else
                    {
                        DataSource.Name = DataSource.Name + " / " + DataSource.CustomerCode;
                    }

                    
                }

                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Mamül Button Edit

        SfTextBox FinishedProductsCodeButtonEdit;
        SfTextBox FinishedProductsNameButtonEdit;
        bool SelectFinishedProductsPopupVisible = false;
        List<ListProductsDto> FinishedProductsList = new List<ListProductsDto>();
        public async Task FinishedProductsCodeOnCreateIcon()
        {
            var FinishedProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, FinishedProductsCodeButtonClickEvent);
            await FinishedProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", FinishedProductsButtonClick } });
        }

        public async void FinishedProductsCodeButtonClickEvent()
        {
            SelectFinishedProductsPopupVisible = true;
            await GetFinishedProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task FinishedProductsNameOnCreateIcon()
        {
            var FinishedProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, FinishedProductsNameButtonClickEvent);
            await FinishedProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", FinishedProductsButtonClick } });
        }

        public async void FinishedProductsNameButtonClickEvent()
        {
            SelectFinishedProductsPopupVisible = true;
            await GetFinishedProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void FinishedProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.FinishedProductID = Guid.Empty;
                DataSource.FinishedProductCode = string.Empty;
                DataSource.FinishedProducName = string.Empty;
                DataSource.CustomerCode = string.Empty;
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.Name = string.Empty;
                isCustomerCodeAvailable = false;
            }
        }

        public async void FinishedProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedFinishedProduct = args.RowData;

            if (selectedFinishedProduct != null)
            {
                DataSource.FinishedProductID = selectedFinishedProduct.Id;
                DataSource.FinishedProductCode = selectedFinishedProduct.Code;
                DataSource.FinishedProducName = selectedFinishedProduct.Name;
                DataSource.ProductType = selectedFinishedProduct.ProductType;

                if (DataSource.ProductType == ProductTypeEnum.MM)
                {
                    isCustomerCodeAvailable = true;
                }
                else if (DataSource.ProductType == ProductTypeEnum.YM)
                {
                    isCustomerCodeAvailable = false;
                }

                DataSource.Name = DataSource.FinishedProductCode + " / " + DataSource.FinishedProducName;

                SelectFinishedProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetFinishedProductsList()
        {
            FinishedProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => !string.IsNullOrEmpty(t.CustomerCode)).ToList();
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("BOMChildMenu");
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
