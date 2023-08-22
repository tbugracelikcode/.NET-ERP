using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.PurchaseUnsuitabilityReport
{
    public partial class PurchaseUnsuitabilityReportsListPage
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Rejection", Text="ComboboxRejection"},
            new UnsComboBox(){ID = "Correction", Text="ComboboxCorrection"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="ComboboxToBeUsedAs"},
            new UnsComboBox(){ID = "ContactSupplier", Text="ComboboxContactSupplier"}
        };

        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseUnsuitabilityReportsService;
            _L = L;

        }

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "Rejection":
                        DataSource.Action_ = L["ComboboxRejection"].Value;
                        break;

                    case "Correction":
                        DataSource.Action_ = L["ComboboxCorrection"].Value;
                        break;

                    case "ToBeUsedAs":
                        DataSource.Action_ = L["ComboboxToBeUsedAs"].Value;
                        break;

                    case "ContactSupplier":
                        DataSource.Action_ = L["ComboboxContactSupplier"].Value;
                        break;

                    default: break;
                }
            }
            else
            {
                DataSource.Action_ = string.Empty;
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseUnsuitabilityReportsDto()
            {
                Date_ = DateTime.Today
            };

            foreach (var item in _unsComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {
            foreach (var item in _unsComboBox)
            {
                item.Text = L[item.Text];
            }

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
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
            if (DataSource.OrderID == Guid.Empty || DataSource.OrderID == null)
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationSelectOrderBase"]);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                SelectProductsPopupVisible = true;
                await GetProductsList();
                await InvokeAsync(StateHasChanged);
            }
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
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
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
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Satın Alma Siparişleri ButtonEdit

        SfTextBox PurchaseOrdersButtonEdit;
        bool SelectPurchaseOrdersPopupVisible = false;
        List<ListPurchaseOrdersDto> PurchaseOrdersList = new List<ListPurchaseOrdersDto>();

        public async Task PurchaseOrdersOnCreateIcon()
        {
            var PurchaseOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, PurchaseOrdersButtonClickEvent);
            await PurchaseOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", PurchaseOrdersButtonClick } });
        }

        public async void PurchaseOrdersButtonClickEvent()
        {
            SelectPurchaseOrdersPopupVisible = true;
            await GetPurchaseOrdersList();
            await InvokeAsync(StateHasChanged);
        }


        public void PurchaseOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.OrderID = Guid.Empty;
                DataSource.OrderFicheNo = string.Empty;
            }
        }

        public async void PurchaseOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListPurchaseOrdersDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.OrderID = selectedOrder.Id;
                DataSource.OrderFicheNo = selectedOrder.FicheNo;

                DataSource.CurrentAccountCardID = selectedOrder.CurrentAccountCardID;
                DataSource.CurrentAccountCardName = selectedOrder.CurrentAccountCardName;
                DataSource.CurrentAccountCardCode = selectedOrder.CurrentAccountCardCode;



                SelectPurchaseOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        SfTextBox UnsuitabilityItemsButtonEdit;
        bool SelectUnsuitabilityItemsPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemsOnCreateIcon()
        {
            var UnsuitabilityItemsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemsButtonClickEvent);
            await UnsuitabilityItemsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemsButtonClick } });
        }

        public async void UnsuitabilityItemsButtonClickEvent()
        {
            SelectUnsuitabilityItemsPopupVisible = true;
            await GetUnsuitabilityItemsList();
            await InvokeAsync(StateHasChanged);
        }


        public void UnsuitabilityItemsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.UnsuitabilityItemsID = Guid.Empty;
                DataSource.UnsuitabilityItemsName = string.Empty;
            }
        }

        public async void UnsuitabilityItemsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedOrder = args.RowData;

            if (selectedOrder != null)
            {
                DataSource.UnsuitabilityItemsID =selectedOrder.Id;
                DataSource.UnsuitabilityItemsName = selectedOrder.Name;
                SelectUnsuitabilityItemsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task GetProductsList()
        {
            if (DataSource.OrderID.HasValue && DataSource.OrderID.Value != Guid.Empty)
            {
                var orderLines = (await PurchaseOrdersAppService.GetAsync(DataSource.OrderID.GetValueOrDefault())).Data.SelectPurchaseOrderLinesDto;

                if (orderLines != null)
                {
                    ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data
                        .Where(t => orderLines.Select(p => p.ProductID.Value).Contains(t.Id))
                        .ToList();
                }
            }
        }

        private async Task GetPurchaseOrdersList()
        {
            PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.ToList();
        }

        private async Task GetUnsuitabilityItemsList()
        {
            var unsuitabilityTypesItem = (await UnsuitabilityTypesItemsAppService.GetWithUnsuitabilityItemDescriptionAsync("Purchase")).Data;

            if (unsuitabilityTypesItem != null)
            {
                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto ())).Data.Where(t=>t.UnsuitabilityTypesItemsName== unsuitabilityTypesItem.Name).ToList();
            }
        }
    }
}
