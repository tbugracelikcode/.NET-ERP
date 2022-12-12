using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.ErpUI.Pages.PurchaseUnsuitabilityReport
{
    public partial class PurchaseUnsuitabilityReportsListPage
    {

        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListPurchaseOrdersDto> PurchaseOrdersComboBox;
        List<ListPurchaseOrdersDto> PurchaseOrdersList = new List<ListPurchaseOrdersDto>();

        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = PurchaseUnsuitabilityReportsService;

            await GetProductsList();
            await GetPurchaseOrdersList();
            await GetCurrentAccountCardsList();
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchaseUnsuitabilityReportsDto()
            {
                Date_ = DateTime.Today
            };

            ShowEditPage();

            await Task.CompletedTask;
        }

        #region Ürünler
        public async Task ProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ProductsComboBox.FilterAsync(ProductsList, query);
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task ProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ProductID = args.ItemData.Id;
                DataSource.ProductName = args.ItemData.Name;
                DataSource.ProductCode = args.ItemData.Code;
            }
            else
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.ProductCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Satın Alma Siparişleri
        public async Task PurchaseOrderFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await PurchaseOrdersComboBox.FilterAsync(PurchaseOrdersList, query);
        }

        private async Task GetPurchaseOrdersList()
        {
            PurchaseOrdersList = (await PurchaseOrdersAppService.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.ToList();
        }

        public async Task PurchaseOrderValueChangeHandler(ChangeEventArgs<string, ListPurchaseOrdersDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.OrderID = args.ItemData.Id;
                DataSource.OrderFicheNo = args.ItemData.FicheNo;
            }
            else
            {
                DataSource.OrderID = Guid.Empty;
                DataSource.OrderFicheNo = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Cari Hesap Kartları
        public async Task CurrentAccountCardFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await CurrentAccountCardsComboBox.FilterAsync(CurrentAccountCardsList, query);
        }

        private async Task GetCurrentAccountCardsList()
        {
            CurrentAccountCardsList = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        public async Task CurrentAccountCardValueChangeHandler(ChangeEventArgs<string, ListCurrentAccountCardsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrentAccountCardID = args.ItemData.Id;
                DataSource.CurrentAccountCardName = args.ItemData.Name;
                DataSource.CurrentAccountCardCode = args.ItemData.Code;
            }
            else
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardName = string.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
