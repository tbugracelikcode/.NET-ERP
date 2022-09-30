using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesProposition
{
    public partial class SalesPropositionsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListPaymentPlansDto> LinePaymentPlansComboBox;
        List<ListPaymentPlansDto> LinePaymentPlansList = new List<ListPaymentPlansDto>();

        SfComboBox<string, ListWarehousesDto> LineWarehousesComboBox;
        List<ListWarehousesDto> LineWarehousesList = new List<ListWarehousesDto>();

        SfComboBox<string, ListUnitSetsDto> UnitSetsComboBox;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        SfComboBox<string, ListBranchesDto> BranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        SfComboBox<string, ListWarehousesDto> WarehousesComboBox;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion



        [Inject]
        ModalManager ModalManager { get; set; }

        SelectSalesPropositionLinesDto LineDataSource = new SelectSalesPropositionLinesDto();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<ListSalesPropositionLinesDto> GridLineList = new List<ListSalesPropositionLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = SalesPropositionsAppService;
        }

        #region Teklif Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectSalesPropositionsDto()
            {
                Date_ = DateTime.Today,
                ValidityDate_ = DateTime.Today.AddDays(15)
            };

            DataSource.SelectSalesPropositionLines = new List<SelectSalesPropositionLinesDto>();

            ShowEditPage();

            CreateLineContextMenuItems();

            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }

        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<ListSalesPropositionLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    //BeforeInsertAsync();
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    //SelectFirstDataRow = false;
                    //DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    //EditPageVisible = true;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz ?");

                    if (res == true)
                    {
                        //SelectFirstDataRow = false;
                        //await DeleteAsync(args.RowInfo.RowData.Id);
                        //await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    //await GetListDataSourceAsync();
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
            //SelectSalesPropositionLinesDto result;

            //if(LineDataSource.Id == Guid.Empty)
            //{
            //    var createInput = ObjectMapper.Map<SelectSalesPropositionLinesDto, CreateSalesPropositionLinesDto>(LineDataSource);

            //    result = (await CreateAsync(createInput)).Data;

            //    if (result != null)
            //        LineDataSource.Id = result.Id;
            //}
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

        public async Task CurrentAccountCardOpened(PopupEventArgs args)
        {
            if (CurrentAccountCardsList.Count == 0)
            {
                await GetCurrentAccountCardsList();
            }
        }

        private void CurrentAccountCardValueChanged(ChangeEventArgs<string, ListCurrentAccountCardsDto> args)
        {
            DataSource.CurrentAccountCardID = args.ItemData.Id;
            DataSource.CurrentAccountCardName = args.ItemData.Name;
            DataSource.CurrentAccountCardCode = args.ItemData.Code;
        }
        #endregion

        #region Şubeler
        public async Task BranchFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await BranchesComboBox.FilterAsync(BranchesList, query);
        }

        private async Task GetBranchesList()
        {
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        public async Task BranchOpened(PopupEventArgs args)
        {
            if (BranchesList.Count == 0)
            {
                await GetBranchesList();
            }
        }

        private void BranchValueChanged(ChangeEventArgs<string, ListBranchesDto> args)
        {
            DataSource.BranchID = args.ItemData.Id;
            DataSource.BranchCode = args.ItemData.Code;
        }
        #endregion

        #region Depolar
        public async Task WarehouseFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await WarehousesComboBox.FilterAsync(WarehousesList, query);
        }

        private async Task GetWarehousesList()
        {
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        public async Task WarehouseOpened(PopupEventArgs args)
        {
            if (WarehousesList.Count == 0)
            {
                await GetWarehousesList();
            }
        }

        private void WarehouseValueChanged(ChangeEventArgs<string, ListWarehousesDto> args)
        {
            DataSource.WarehouseID = args.ItemData.Id;
            DataSource.WarehouseCode = args.ItemData.Code;
        }
        #endregion

        #region Para Birimleri
        public async Task CurrencyFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await CurrenciesComboBox.FilterAsync(CurrenciesList, query);
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        public async Task CurrencyOpened(PopupEventArgs args)
        {
            if (CurrenciesList.Count == 0)
            {
                await GetCurrenciesList();
            }
        }

        private void CurrencyValueChanged(ChangeEventArgs<string, ListCurrenciesDto> args)
        {
            DataSource.CurrencyID = args.ItemData.Id;
            DataSource.CurrencyCode = args.ItemData.Code;
        }
        #endregion

        #region Stok Kartları -Teklif Satırları
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

        public async Task ProductOpened(PopupEventArgs args)
        {
            if (ProductsList.Count == 0)
            {
                await GetProductsList();
            }
        }

        private void ProductValueChanged(ChangeEventArgs<string, ListProductsDto> args)
        {
            LineDataSource.ProductID = args.ItemData.Id;
            LineDataSource.ProductCode = args.ItemData.Code;
            LineDataSource.ProductName = args.ItemData.Name;
        }
        #endregion

        #region Birim Setleri -Teklif Satırları
        public async Task UnitSetFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await UnitSetsComboBox.FilterAsync(UnitSetsList, query);
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        public async Task UnitSetOpened(PopupEventArgs args)
        {
            if (UnitSetsList.Count == 0)
            {
                await GetUnitSetsList();
            }
        }

        private void UnitSetValueChanged(ChangeEventArgs<string, ListUnitSetsDto> args)
        {
            LineDataSource.UnitSetID = args.ItemData.Id;
            LineDataSource.UnitSetCode = args.ItemData.Code;
        }
        #endregion

        #region Depolar - Teklif Satırları
        public async Task LineWareHouseFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineWarehousesComboBox.FilterAsync(LineWarehousesList, query);
        }

        private async Task GetLineWareHousesList()
        {
            LineWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        public async Task LineWareHouseOpened(PopupEventArgs args)
        {
            if (LineWarehousesList.Count == 0)
            {
                await GetLineWareHousesList();
            }
        }

        private void LineWareHouseValueChanged(ChangeEventArgs<string, ListWarehousesDto> args)
        {
            LineDataSource.WarehouseID = args.ItemData.Id;
            LineDataSource.WarehouseCode = args.ItemData.Code;
        }
        #endregion

        #region Ödeme Planları - Teklif Satırları
        public async Task LinePaymentPlanFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LinePaymentPlansComboBox.FilterAsync(LinePaymentPlansList, query);
        }

        private async Task GetLinePaymentPlansList()
        {
            LinePaymentPlansList = (await PaymentPlansAppService.GetListAsync(new ListPaymentPlansParameterDto())).Data.ToList();
        }

        public async Task LinePaymentPlanOpened(PopupEventArgs args)
        {
            if (LinePaymentPlansList.Count == 0)
            {
                await GetLinePaymentPlansList();
            }
        }

        private void LinePaymentPlanValueChanged(ChangeEventArgs<string, ListPaymentPlansDto> args)
        {
            LineDataSource.PaymentPlanID = args.ItemData.Id;
            LineDataSource.PaymentPlanCode = args.ItemData.Code;
        }
        #endregion

    }
}
