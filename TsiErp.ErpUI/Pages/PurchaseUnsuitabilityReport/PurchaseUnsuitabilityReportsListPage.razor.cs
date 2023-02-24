using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.ErpUI.Pages.PurchaseUnsuitabilityReport
{
    public partial class PurchaseUnsuitabilityReportsListPage
    {


        public class UnsComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<UnsComboBox> _unsComboBox = new List<UnsComboBox>
        {
            new UnsComboBox(){ID = "Rejection", Text="Red"},
            new UnsComboBox(){ID = "Correction", Text="Düzeltme"},
            new UnsComboBox(){ID = "ToBeUsedAs", Text="Olduğu Gibi Kullanılacak"},
            new UnsComboBox(){ID = "ContactSupplier", Text="Tedarikçi ile İrtibat"}
        };

        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseUnsuitabilityReportsService;

        }

        private void UnsComboBoxValueChangeHandler(ChangeEventArgs<string, UnsComboBox> args)
        {
            switch (args.ItemData.ID)
            {
                case "Rejection":
                    DataSource.IsReject = true;
                    DataSource.IsCorrection = false;
                    DataSource.IsToBeUsedAs = false;
                    DataSource.IsContactSupplier = false;
                    break;

                case "Correction":
                    DataSource.IsReject = false;
                    DataSource.IsCorrection = true;
                    DataSource.IsToBeUsedAs = false;
                    DataSource.IsContactSupplier = false;
                    break;

                case "ToBeUsedAs":
                    DataSource.IsReject = false;
                    DataSource.IsCorrection = false;
                    DataSource.IsToBeUsedAs = true;
                    DataSource.IsContactSupplier = false;
                    break;

                case "ContactSupplier":
                    DataSource.IsReject = false;
                    DataSource.IsCorrection = false;
                    DataSource.IsToBeUsedAs = false;
                    DataSource.IsContactSupplier = true;
                    break;

                default: break;
            }
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseUnsuitabilityReportsDto()
            {
                Date_ = DateTime.Today
            };

            EditPageVisible = true;

            await Task.CompletedTask;
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
                SelectPurchaseOrdersPopupVisible = false;
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

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetPurchaseOrdersList()
        {
            PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.ToList();
        }
    }
}
