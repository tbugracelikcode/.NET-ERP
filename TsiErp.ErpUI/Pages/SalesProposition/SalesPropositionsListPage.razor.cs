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
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesProposition
{
    public partial class SalesPropositionsListPage
    {
        SfComboBox<string, ListCurrentAccountCardsDto> CurrentAccountCardsComboBox;
        List<ListCurrentAccountCardsDto> CurrentAccountCardsList = new List<ListCurrentAccountCardsDto>();

        SfComboBox<string, ListBranchesDto> BranchesComboBox;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        SfComboBox<string, ListWarehousesDto> WarehousesComboBox;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

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



    }
}
