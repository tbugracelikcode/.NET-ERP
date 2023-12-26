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

namespace TsiErp.ErpUI.Pages.ShippingManagement.PackageFiche
{
    public partial class PackageFichesListPage
    {
        private SfGrid<SelectPackageFicheLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectPackageFicheLinesDto> GridLineList = new List<SelectPackageFicheLinesDto>();

        public bool refno = false;


        protected override async void OnInitialized()
        {
            BaseCrudService = PackageFichesAppService;
            _L = L;
            CreateMainContextMenuItems();

        }

        #region Paket Fişleri Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPackageFichesDto()
            {
                Date_ = DateTime.Today,
                Code = FicheNumbersAppService.GetFicheNumberAsync("PackageFichesChildMenu"),
                PackageContent = 0,
                NumberofPackage = 0
            };

            DataSource.SelectPackageFicheLines = new List<SelectPackageFicheLinesDto>();
            GridLineList = DataSource.SelectPackageFicheLines;

            foreach (var item in _packageTypeComboBox)
            {
                item.Text = L[item.Text];
            }

            refno = false;

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


        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["PackageFichesContextRefresh"], Id = "refresh" });
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
            if (DataSource.PackageContent == 0 || DataSource.NumberofPackage == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPackageTitle"], L["UIWarningPackageMessage"]);
            }
            else
            {
                SelectPackageFicheLinesDto packageFicheLineModel = new SelectPackageFicheLinesDto
                {
                    PackingDate = DataSource.Date_,
                    LineNr = 1,
                    ProductID = DataSource.ProductID,
                    ProductName = DataSource.ProductName,
                    ProductCode = DataSource.ProductCode,
                    Quantity = DataSource.PackageContent * DataSource.NumberofPackage,
                    ProductionOrderID = Guid.Empty,
                    ProductionOrderFicheNo = string.Empty,
                    Status_ = string.Empty,
                };

                GridLineList.Add(packageFicheLineModel);
                refno = true;
                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
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
        List<ListProductionOrdersDto> ProductionOrdersList = new List<ListProductionOrdersDto>();
        Guid ProductionOrdersID = Guid.Empty;
        string ProductionOrdersNo = string.Empty;

        public async Task ProductionOrdersOnCreateIcon()
        {
            var ProductionOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductionOrdersButtonClickEvent);
            await ProductionOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductionOrdersButtonClick } });
        }

        public async void ProductionOrdersButtonClickEvent()
        {
            SelectProductionOrdersPopupVisible = true;
            ProductionOrdersList = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t=>t.FinishedProductID == DataSource.ProductID).ToList();

            await InvokeAsync(StateHasChanged);
        }


        public async void ProductionOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ProductionOrdersID = Guid.Empty;
                ProductionOrdersNo = string.Empty;

                GridLineList[0].ProductionOrderID = ProductionOrdersID;
                GridLineList[0].ProductionOrderFicheNo = ProductionOrdersNo;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);

            }
        }

        public async void ProductionOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListProductionOrdersDto> args)
        {
            var selectedProductionOrder = args.RowData;

            if (selectedProductionOrder != null)
            {
                ProductionOrdersID = selectedProductionOrder.Id;
                ProductionOrdersNo = selectedProductionOrder.FicheNo;
                GridLineList[0].ProductionOrderID = ProductionOrdersID;
                GridLineList[0].ProductionOrderFicheNo = ProductionOrdersNo;
                await _LineGrid.Refresh();
                SelectProductionOrdersPopupVisible = false;
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
            if (DataSource.SalesOrderID == Guid.Empty || DataSource.SalesOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSalesOrderTitle"], L["UIWarningSalesOrderMessage"]);
            }
            else
            {
                SelectProductsPopupVisible = true;
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
    }
}
