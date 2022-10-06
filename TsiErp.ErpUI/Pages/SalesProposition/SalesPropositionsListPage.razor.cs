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
using Syncfusion.Blazor.HeatMap.Internal;
using Newtonsoft.Json;

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
        SfComboBox<string, ListBranchesDto> LineBranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        SfComboBox<string, ListWarehousesDto> WarehousesComboBox;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<ListSalesPropositionsDto> _grid;
        private SfGrid<SelectSalesPropositionLinesDto> _LineGrid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectSalesPropositionLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        //List<ListSalesPropositionLinesDto> GridLineList = new List<ListSalesPropositionLinesDto>();
        List<SelectSalesPropositionLinesDto> GridLineList = new List<SelectSalesPropositionLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = SalesPropositionsAppService;

            await GetCurrentAccountCardsList();
            await GetBranchesList();
            await GetWarehousesList();
            await GetCurrenciesList();
            await GetProductsList();
            await GetUnitSetsList();
            await GetLinePaymentPlansList();
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectSalesPropositionLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectSalesPropositionLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.PaymentPlanID = DataSource.PaymentPlanID;
                    LineDataSource.PaymentPlanCode = DataSource.PaymentPlanCode;
                    LineDataSource.BranchID = DataSource.BranchID;
                    LineDataSource.BranchCode = DataSource.BranchCode;
                    LineDataSource.WarehouseID = DataSource.WarehouseID;
                    LineDataSource.WarehouseCode = DataSource.WarehouseCode;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    //DataSource.SelectSalesPropositionLines.Add(LineDataSource);
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    //DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _LineGrid.Refresh();
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
            if (LineDataSource.Id == Guid.Empty)
            {
                LineDataSource.Id = ApplicationService.GuidGenerator.CreateGuid();
                DataSource.SelectSalesPropositionLines.Add(LineDataSource);
            }
            else
            {
                int selectedLineIndex = DataSource.SelectSalesPropositionLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectSalesPropositionLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectSalesPropositionLines;

            await _LineGrid.Refresh();

            HideLinesPopup();
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
                DataSource.CurrentAccountCardCode = args.ItemData.Code;
            }
            else
            {
                DataSource.CurrentAccountCardID = Guid.Empty;
                DataSource.CurrentAccountCardCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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

        public async Task BranchValueChangeHandler(ChangeEventArgs<string, ListBranchesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.BranchID = args.ItemData.Id;
                DataSource.BranchCode = args.ItemData.Code;
            }
            else
            {
                DataSource.BranchID = Guid.Empty;
                DataSource.BranchCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        public async Task LineBranchValueChangeHandler(ChangeEventArgs<string, ListBranchesDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.BranchID = args.ItemData.Id;
                LineDataSource.BranchCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.BranchID = Guid.Empty;
                LineDataSource.BranchCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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
            LineWarehousesList = WarehousesList;
        }
        public async Task WarehouseValueChangeHandler(ChangeEventArgs<string, ListWarehousesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.WarehouseID = args.ItemData.Id;
                DataSource.WarehouseCode = args.ItemData.Code;
            }
            else
            {
                DataSource.WarehouseID = Guid.Empty;
                DataSource.WarehouseCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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
        public async Task CurrencyValueChangeHandler(ChangeEventArgs<string, ListCurrenciesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrencyID = args.ItemData.Id;
                DataSource.CurrencyCode = args.ItemData.Code;
            }
            else
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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
        public async Task ProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.ProductID = args.ItemData.Id;
                LineDataSource.ProductCode = args.ItemData.Code;
                LineDataSource.ProductName = args.ItemData.Name;
            }
            else
            {
                LineDataSource.ProductID = Guid.Empty;
                LineDataSource.ProductCode = string.Empty;
                LineDataSource.ProductName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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

        public async Task UnitSetValueChangeHandler(ChangeEventArgs<string, ListUnitSetsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.UnitSetID = args.ItemData.Id;
                LineDataSource.UnitSetCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.UnitSetID = Guid.Empty;
                LineDataSource.UnitSetCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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

        public async Task LineWareHouseValueChangeHandler(ChangeEventArgs<string, ListWarehousesDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.WarehouseID = args.ItemData.Id;
                LineDataSource.WarehouseCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.WarehouseID = Guid.Empty;
                LineDataSource.WarehouseCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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

        public async Task LinePaymentPlanValueChangeHandler(ChangeEventArgs<string, ListPaymentPlansDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.PaymentPlanID = args.ItemData.Id;
                LineDataSource.PaymentPlanCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.PaymentPlanID = Guid.Empty;
                LineDataSource.PaymentPlanCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
