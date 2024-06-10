using Syncfusion.Blazor.Grids;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using Syncfusion.Blazor.DropDowns;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;
using DevExpress.Blazor.Grid.Internal;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.ErpUI.Helpers;
using TsiErp.Business.Extensions.ObjectMapping;

namespace TsiErp.ErpUI.Pages.ShippingManagement.PackageFiche
{
    public partial class PackageFichesListPage : IDisposable
    {
        public class ProductionOrderReferanceNumber
        {
            public string ProductionOrderNo { get; set; }
            public Guid ProductionOrderID { get; set; }
            public string ProductionOrderReferenceNo { get; set; }



        }
        private SfGrid<SelectPackageFicheLinesDto> _LineGrid;


        SelectPackageFicheLinesDto LineDataSource = new();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPackageFicheLinesDto> GridLineList = new List<SelectPackageFicheLinesDto>();

        List<ProductionOrderReferanceNumber> ProductionOrderReferenceNoList = new List<ProductionOrderReferanceNumber>();

        private bool LineCrudPopup = false;


        protected override async void OnInitialized()
        {
            BaseCrudService = PackageFichesAppService;
            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PackageFichesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Paket Fişleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPackageFichesDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                PackageContent = 0,
                NumberofPackage = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("PackageFichesChildMenu")
            };

            DataSource.SelectPackageFicheLines = new List<SelectPackageFicheLinesDto>();
            GridLineList = DataSource.SelectPackageFicheLines;


            ProductionOrdersID = Guid.Empty;
            ProductionOrdersNo = string.Empty;
            ProductionOrdersReferenceNo = string.Empty;

            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        public async override void ShowEditPage()
        {
            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

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
                            case "PackageFichesContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextAdd"], Id = "new" }); break;
                            case "PackageFichesContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextChange"], Id = "changed" }); break;
                            case "PackageFichesContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextDelete"], Id = "delete" }); break;
                            case "PackageFichesContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
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
                            case "PackageFicheLinesContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFicheLinesContextChange"], Id = "changed" }); break;
                            case "PackageFicheLinesContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFicheLinesContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListPackageFichesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await PackageFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectPackageFicheLines;

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
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

        public async void CreateLines()
        {
            if (DataSource.SalesOrderID == null || DataSource.SalesOrderID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessageSalesOrder"]);
            }
            else if (DataSource.ProductID == null || DataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessageProduct"]);
            }
            else if (DataSource.PackageContent == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessagePackageContent"]);
            }
            else if (DataSource.NumberofPackage == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessageNumberofPackage"]);
            }
            else if (string.IsNullOrEmpty(DataSource.PackageType))
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessagePackageType"]);
            }
            else if (string.IsNullOrEmpty(ProductionOrdersReferenceNo))
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessageProductionOrders"]);
            }
            else
            {
                GridLineList.Clear();

                decimal numberOfLines = 0;
                int mod = 0;
                int numberOfPackage = 0;

                if (DataSource.PackageType == L["BigPackage"].Value)
                {
                    numberOfPackage = 18;
                    decimal a = DataSource.NumberofPackage / numberOfPackage;

                    if (a == 1)
                    {
                        numberOfLines = a;
                    }
                    else
                    {
                        numberOfLines = Math.Ceiling(a);
                    }
                    mod = DataSource.NumberofPackage % numberOfPackage;
                }
                else if (DataSource.PackageType == L["SmallPackage"].Value)
                {
                    numberOfPackage = 30;
                    decimal a = DataSource.NumberofPackage / numberOfPackage;

                    if (a == 1)
                    {
                        numberOfLines = a;
                    }
                    else
                    {
                        numberOfLines = Math.Ceiling(a);
                    }
                    mod = DataSource.NumberofPackage % numberOfPackage;
                }

                if (mod != 0)
                {

                    for (int i = 0; i <= numberOfLines; i++)
                    {
                        if (i != numberOfLines)
                        {
                            SelectPackageFicheLinesDto packageFicheLineModel = new SelectPackageFicheLinesDto
                            {
                                PackingDate = DataSource.Date_,
                                LineNr = GridLineList.Count + 1,
                                ProductID = DataSource.ProductID,
                                ProductName = DataSource.ProductName,
                                ProductCode = DataSource.ProductCode,
                                Quantity = DataSource.PackageContent * numberOfPackage,
                                ProductionOrderID = ProductionOrdersID,
                                ProductionOrderFicheNo = ProductionOrdersNo,
                                Status_ = string.Empty,
                                NumberofPackage = numberOfPackage,
                                PackageContent = DataSource.PackageContent,
                            };

                            GridLineList.Add(packageFicheLineModel);
                        }
                        else
                        {
                            SelectPackageFicheLinesDto packageFicheLineModel = new SelectPackageFicheLinesDto
                            {
                                PackingDate = DataSource.Date_,
                                LineNr = GridLineList.Count + 1,
                                ProductID = DataSource.ProductID,
                                ProductName = DataSource.ProductName,
                                ProductCode = DataSource.ProductCode,
                                Quantity = DataSource.PackageContent * mod,
                                ProductionOrderID = ProductionOrdersID,
                                ProductionOrderFicheNo = ProductionOrdersNo,
                                Status_ = string.Empty,
                                NumberofPackage = mod,
                                PackageContent = DataSource.PackageContent,
                            };

                            GridLineList.Add(packageFicheLineModel);
                        }

                    }
                }
                else
                {

                    for (int i = 1; i <= numberOfLines; i++)
                    {
                        SelectPackageFicheLinesDto packageFicheLineModel = new SelectPackageFicheLinesDto
                        {
                            PackingDate = DataSource.Date_,
                            LineNr = GridLineList.Count + 1,
                            ProductID = DataSource.ProductID,
                            ProductName = DataSource.ProductName,
                            ProductCode = DataSource.ProductCode,
                            Quantity = DataSource.PackageContent * numberOfPackage,
                            ProductionOrderID = ProductionOrdersID,
                            ProductionOrderFicheNo = ProductionOrdersNo,
                            Status_ = string.Empty,
                            NumberofPackage = numberOfPackage,
                            PackageContent = DataSource.PackageContent,
                        };

                        GridLineList.Add(packageFicheLineModel);


                    }
                }
                await _LineGrid.Refresh();
            }


            await InvokeAsync(StateHasChanged);
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectPackageFicheLinesDto> args)
        {
            switch (args.Item.Id)
            {


                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
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
                if (DataSource.SelectPackageFicheLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectPackageFicheLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectPackageFicheLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectPackageFicheLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectPackageFicheLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectPackageFicheLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectPackageFicheLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);


        }

        protected override async Task OnSubmit()
        {
            var productionOrderID = GridLineList.Select(t => t.ProductionOrderID).FirstOrDefault();

            if (productionOrderID != null && productionOrderID != Guid.Empty)
            {
                //var stockFicheLineList = (await StockFichesAppService.GetbyProductionOrderAsync(productionOrderID.GetValueOrDefault())).Data.SelectStockFicheLines.Where(t=>t.ProductID == DataSource.ProductID).ToList();

                var stockFicheLineList = (await StockFichesAppService.GetbyProductionOrderAsync(ProductionOrdersID));

                if (stockFicheLineList != null && stockFicheLineList.Count > 0)
                {
                    if (stockFicheLineList.Select(t => t.Quantity).Sum() < GridLineList.Select(T => T.Quantity).Sum())
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningQuantityTitle"], L["UIWarningQuantityMessage"]);
                    }
                    else
                    {
                        //decimal bottomLimit = DataSource.ProductUnitWeight - ((DataSource.ProductUnitWeight * 2) / 100);
                        //decimal upperLimit = DataSource.ProductUnitWeight + ((DataSource.ProductUnitWeight * 2) / 100);

                        //if (DataSource.UnitWeight >= bottomLimit && DataSource.UnitWeight <= upperLimit)
                        //{
                        SelectPackageFichesDto result;

                        if (DataSource.Id == Guid.Empty)
                        {
                            var createInput = ObjectMapper.Map<SelectPackageFichesDto, CreatePackageFichesDto>(DataSource);

                            result = (await CreateAsync(createInput)).Data;

                            if (result != null)
                                DataSource.Id = result.Id;
                        }
                        else
                        {
                            var updateInput = ObjectMapper.Map<SelectPackageFichesDto, UpdatePackageFichesDto>(DataSource);

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
                        //}
                        //else if (DataSource.UnitWeight < bottomLimit || DataSource.UnitWeight > upperLimit)
                        //{
                        //    await ModalManager.WarningPopupAsync(L["UIWarningUnitWeightTitle"], L["UIWarningUnitWeightMessage"]);
                        //}
                    }
                }
            }

        }


        #endregion

        #region Satış Siparişi ButtonEdit

        SfTextBox SalesOrdersButtonEdit;
        bool SelectSalesOrdersPopupVisible = false;
        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();
        List<SelectSalesOrderLinesDto> SalesOrdersLineList = new List<SelectSalesOrderLinesDto>();

        public async Task SalesOrdersOnCreateIcon()
        {
            var SalesOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SalesOrdersButtonClickEvent);
            await SalesOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SalesOrdersButtonClick } });
        }

        public async void SalesOrdersButtonClickEvent()
        {
            SelectSalesOrdersPopupVisible = true;
            SalesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();

            await InvokeAsync(StateHasChanged);
        }


        public void SalesOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SalesOrderID = Guid.Empty;
                DataSource.SalesOrderFicheNo = string.Empty;
                DataSource.CustomerCode = string.Empty;
                DataSource.SalesOrderCustomerOrderNo = string.Empty;
                DataSource.CurrentAccountID = Guid.Empty;
                SalesOrdersLineList.Clear();
            }
        }

        public async void SalesOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListSalesOrderDto> args)
        {
            var selectedSalesOrder = args.RowData;

            if (selectedSalesOrder != null)
            {
                DataSource.SalesOrderID = selectedSalesOrder.Id;
                DataSource.SalesOrderFicheNo = selectedSalesOrder.FicheNo;
                DataSource.CustomerCode = selectedSalesOrder.CustomerCode;
                DataSource.SalesOrderCustomerOrderNo = selectedSalesOrder.CustomerOrderNr;
                DataSource.CurrentAccountID = selectedSalesOrder.CurrentAccountCardID;
                SalesOrdersLineList = (await SalesOrdersAppService.GetAsync(selectedSalesOrder.Id)).Data.SelectSalesOrderLines.ToList();
                SelectSalesOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Üretim Emri ButtonEdit

        SfTextBox ProductionOrdersButtonEdit;
        bool SelectProductionOrdersPopupVisible = false;
        Guid ProductionOrdersID = Guid.Empty;
        string ProductionOrdersNo = string.Empty;
        string ProductionOrdersReferenceNo = string.Empty;

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            if (DataSource.ProductID != Guid.Empty && DataSource.ProductID != null && DataSource.SalesOrderID != Guid.Empty && DataSource.SalesOrderID != null)
            {
                ProductionOrderReferenceNoList.Clear();

                var stockFicheLines = (await StockFichesAppService.GetbyProductionOrderAsync(ProductionOrdersID));

                foreach (var line in stockFicheLines)
                {
                    if (!ProductionOrderReferenceNoList.Any(t => t.ProductionOrderReferenceNo == line.ProductionDateReferance))
                    {
                        ProductionOrderReferanceNumber referenceModel = new ProductionOrderReferanceNumber
                        {
                            ProductionOrderID = ProductionOrdersID,
                            ProductionOrderNo = ProductionOrdersNo,
                            ProductionOrderReferenceNo = line.ProductionDateReferance
                        };

                        ProductionOrderReferenceNoList.Add(referenceModel);
                    }
                }

                SelectProductionOrdersPopupVisible = true;

            }

            await InvokeAsync(StateHasChanged);
        }


        public async void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ProductionOrdersReferenceNo = string.Empty;

                foreach (var line in GridLineList)
                {
                    int lineIndex = GridLineList.IndexOf(line);
                    GridLineList[lineIndex].ProductionOrderID = Guid.Empty;
                    GridLineList[lineIndex].ProductionOrderFicheNo = string.Empty;
                }

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);

            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ProductionOrderReferanceNumber> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                ProductionOrdersReferenceNo = selectedProductionOrder.ProductionOrderReferenceNo;

                foreach (var line in GridLineList)
                {
                    int lineIndex = GridLineList.IndexOf(line);
                    GridLineList[lineIndex].ProductionOrderID = ProductionOrdersID;
                    GridLineList[lineIndex].ProductionOrderFicheNo = selectedProductionOrder.ProductionOrderReferenceNo;
                }
                await _LineGrid.Refresh();
                SelectProductionOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit = new();
        SfTextBox ProductsNameButtonEdit = new();
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            if (DataSource.SalesOrderID == Guid.Empty || DataSource.SalesOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSalesOrderTitle"], L["UIWarningSalesOrderMessage"]);
            }
            else
            {
                SelectProductsPopupVisible = true;

                ProductsList.Clear();
                var productList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                foreach (var products in productList)
                {
                    if (SalesOrdersLineList.Where(t => t.ProductID == products.Id).Count() > 0)
                    {
                        ProductsList.Add(products);
                    }
                }
            }
            await InvokeAsync(StateHasChanged);
        }

        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            if (DataSource.SalesOrderID == Guid.Empty || DataSource.SalesOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSalesOrderTitle"], L["UIWarningSalesOrderMessage"]);
            }
            else
            {
                SelectProductsPopupVisible = true;
                ProductsList.Clear();
                var productList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                foreach (var products in productList)
                {
                    if (SalesOrdersLineList.Where(t => t.ProductID == products.Id).Count() > 0)
                    {
                        ProductsList.Add(products);
                    }
                }
            }
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.ProductUnitWeight = 0;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                DataSource.ProductUnitWeight = selectedProduct.UnitWeight;
                var productionOrder = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.FinishedProductID == selectedProduct.Id && t.OrderID == DataSource.SalesOrderID).FirstOrDefault();

                if (productionOrder != null && productionOrder.Id != Guid.Empty)
                {
                    ProductionOrdersID = productionOrder.Id;
                    ProductionOrdersNo = productionOrder.FicheNo;
                }

                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Koli Türü ComboBox
        public class PackageTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<PackageTypeComboBox> _packageTypeComboBox = new List<PackageTypeComboBox>
        {
            new PackageTypeComboBox(){ID = "Big", Text="BigPackage"},
            new PackageTypeComboBox(){ID = "Small", Text="SmallPackage"}
        };

        private void PackageTypeComboBoxValueChangeHandler(ChangeEventArgs<string, PackageTypeComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "Big":
                        DataSource.PackageType = L["BigPackage"].Value;
                        break;

                    case "Small":
                        DataSource.PackageType = L["SmallPackage"].Value;
                        break;


                    default: break;
                }
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PackageFichesChildMenu");
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
