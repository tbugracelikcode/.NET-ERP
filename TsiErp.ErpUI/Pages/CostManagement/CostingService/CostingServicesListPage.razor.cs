using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Entities.ProductProperty.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Components.Commons.Spinner;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.CostManagement.CostingService
{
    public partial class CostingServicesListPage
    {
        #region Değişkenler

        public bool CostingServiceModalVisible = false;

        DateTime? FilterStartDate = null;

        DateTime? FilterEndDate = null;

        ProductTypeEnum? ProductType = null;

        Guid ProductGroupID = Guid.Empty;

        string ProductGroupName = string.Empty;

        Guid ProductID = Guid.Empty;

        string ProductCode = string.Empty;

        string ProductName = string.Empty;

        int CalculatingMethod = 0;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        #endregion

        [Inject]
        SpinnerService SpinnerService { get; set; }
        [Inject]
        ModalManager ModalManager { get; set; }

        protected override async void OnInitialized()
        {

            foreach (var item in _costCalculationMethodComboBox)
            {

                item.Text = L[item.Text];

            }

            foreach (var item in types)
            {
                item.ProductTypeName = L[item.ProductTypeName];
            }

            var now = GetSQLDateAppService.GetDateFromSQL().Date;

            FilterEndDate = now;

            FilterStartDate = now;

            CostingServiceModalVisible = true;

            await InvokeAsync(StateHasChanged);
        }

        public async void OnCalculateButtonClicked()
        {
            SpinnerService.Show();
            await Task.Delay(100);

            var ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.AsQueryable();

            if (ProductsList.Count() > 0)
            {
                if (ProductType != null)
                {
                    ProductsList = ProductsList.Where(t => t.ProductType == ProductType);
                }
                if (ProductGroupID != Guid.Empty)
                {
                    ProductsList = ProductsList.Where(t => t.ProductGrpID == ProductGroupID);
                }
                if (ProductID != Guid.Empty)
                {
                    ProductsList = ProductsList.Where(t => t.Id == ProductID);
                }

                var resultList = ProductsList.ToList();

                try
                {
                    foreach (var product in resultList)
                    {
                        if (CalculatingMethod == 1) //FIFO
                        {
                            var stockFicheLineList = (await StockFichesAppService.GetOutputList(ProductID, FilterStartDate, FilterEndDate)).ToList();

                            if (stockFicheLineList != null && stockFicheLineList.Count > 0)
                            {
                                decimal productCost = (await StockFichesAppService.CalculateProductFIFOCostAsync(product.Id, stockFicheLineList));

                                var updatedLine = stockFicheLineList.LastOrDefault();

                                var stockFicheDataSource = (await StockFichesAppService.GetAsync(updatedLine.StockFicheID)).Data;

                                if (stockFicheDataSource != null && stockFicheDataSource.Id != Guid.Empty)
                                {
                                    int lineIndex = stockFicheDataSource.SelectStockFicheLines.IndexOf(updatedLine);

                                    stockFicheDataSource.SelectStockFicheLines[lineIndex].UnitOutputCost = productCost;

                                    var updatedEntity = ObjectMapper.Map<SelectStockFichesDto, UpdateStockFichesDto>(stockFicheDataSource);

                                    await StockFichesAppService.UpdateAsync(updatedEntity);

                                    await ModalManager.MessagePopupAsync(L["UIMessageCalculateTitle"], L["UIMessageCalculateMessage"]);
                                }
                            }
                            else
                            {
                                SpinnerService.Hide();
                                await ModalManager.MessagePopupAsync(L["UIMessageStockFicheLineTitle"], L["UIMessageStockFicheLineMessage"]);
                            }


                        }
                        else if (CalculatingMethod == 2) //LIFO
                        {
                            var stockFicheLineList = (await StockFichesAppService.GetOutputList(ProductID, FilterStartDate, FilterEndDate)).ToList();

                            if (stockFicheLineList != null && stockFicheLineList.Count > 0)
                            {
                                decimal productCost = (await StockFichesAppService.CalculateProductLIFOCostAsync(product.Id, stockFicheLineList));

                                var updatedLine = stockFicheLineList.LastOrDefault();

                                var stockFicheDataSource = (await StockFichesAppService.GetAsync(updatedLine.StockFicheID)).Data;

                                if (stockFicheDataSource != null && stockFicheDataSource.Id != Guid.Empty)
                                {
                                    int lineIndex = stockFicheDataSource.SelectStockFicheLines.IndexOf(updatedLine);

                                    stockFicheDataSource.SelectStockFicheLines[lineIndex].UnitOutputCost = productCost;

                                    var updatedEntity = ObjectMapper.Map<SelectStockFichesDto, UpdateStockFichesDto>(stockFicheDataSource);

                                    await StockFichesAppService.UpdateAsync(updatedEntity);

                                    await ModalManager.MessagePopupAsync(L["UIMessageCalculateTitle"], L["UIMessageCalculateMessage"]);
                                }
                            }
                            else
                            {
                                SpinnerService.Hide();
                                await ModalManager.MessagePopupAsync(L["UIMessageStockFicheLineTitle"], L["UIMessageStockFicheLineMessage"]);
                            }
                        }
                    }

                   
                }

                catch
                {
                    SpinnerService.Hide();
                    throw new DuplicateCodeException(L["CalculateException"]);
                }
            }


            SpinnerService.Hide();

            await InvokeAsync(StateHasChanged);
        }

        public void HideCalculateModal()
        {
            CostingServiceModalVisible = false;

            var now = GetSQLDateAppService.GetDateFromSQL().Date;

            FilterEndDate = now;

            FilterStartDate = now;

            ProductType = null;

            ProductGroupID = Guid.Empty;

            ProductGroupName = string.Empty;

            ProductID = Guid.Empty;

            ProductCode = string.Empty;

            ProductName = string.Empty;

            CalculatingMethod = 0;
        }

        #region Ürün Grubu ButtonEdit

        SfTextBox ProductGroupsButtonEdit;
        bool SelectProductGroupsPopupVisible = false;
        List<ListProductGroupsDto> ProductGroupsList = new List<ListProductGroupsDto>();

        public async Task ProductGroupsOnCreateIcon()
        {
            var ProductGroupsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductGroupsButtonClickEvent);
            await ProductGroupsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductGroupsButtonClick } });
        }

        private async Task GetProductGroupsList()
        {
            ProductGroupsList = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.ToList();
        }

        public async void ProductGroupsButtonClickEvent()
        {
            SelectProductGroupsPopupVisible = true;
            await GetProductGroupsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductGroupsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                ProductGroupID = Guid.Empty;
                ProductGroupName = string.Empty;
            }
        }

        public async void ProductGroupsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductGroupsDto> args)
        {
            var selectedProductGroup = args.RowData;

            if (selectedProductGroup != null)
            {
                ProductGroupID = selectedProductGroup.Id;
                ProductGroupName = selectedProductGroup.Name;
                SelectProductGroupsPopupVisible = false;

                await InvokeAsync(StateHasChanged);
            }
        }


        #endregion

        #region Stok Türü Combobox


        public IEnumerable<SelectProductsDto> types = GetEnumDisplayTypeNames<ProductTypeEnum>();

        public static List<SelectProductsDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<ProductTypeEnum>()
                       .Select(x => new SelectProductsDto
                       {
                           ProductType = x,
                           ProductTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
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
        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

            if (ProductType != null)
            {
                ProductsList = ProductsList.Where(t => t.ProductType == ProductType).ToList();
            }
            if (ProductGroupID != Guid.Empty)
            {
                ProductsList = ProductsList.Where(t => t.ProductGrpID == ProductGroupID).ToList();
            }
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
                ProductID = Guid.Empty;
                ProductCode = string.Empty;
                ProductName = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                ProductID = selectedProduct.Id;
                ProductCode = selectedProduct.Code;
                ProductName = selectedProduct.Name;

                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Hesaplama Yöntemi Combobox

        public class CostCalculationMethodComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<CostCalculationMethodComboBox> _costCalculationMethodComboBox = new List<CostCalculationMethodComboBox>
        {
            new CostCalculationMethodComboBox(){ID = "1", Text="FIFOCombo"},
            new CostCalculationMethodComboBox(){ID = "2", Text="LIFOCombo"},
            new CostCalculationMethodComboBox(){ID = "3", Text="AverageCombo"}
        };

        private void CostCalculationMethodComboBoxValueChangeHandler(ChangeEventArgs<string, CostCalculationMethodComboBox> args)
        {
            if (args.ItemData != null)
            {


                switch (args.ItemData.ID)
                {
                    case "1":
                        CalculatingMethod = 1;
                        break;

                    case "2":
                        CalculatingMethod = 2;
                        break;
                    case "3":
                        CalculatingMethod = 3;
                        break;


                    default: break;
                }
            }
        }

        #endregion

    }
}

