using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Operation.Dtos;
using TsiErp.Entities.Entities.OperationLine.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Operation
{
    public partial class OperationsListPage
    {
        private SfGrid<ListOperationsDto> _grid;

        #region Combobox Listeleri

        SfComboBox<string, ListOperationsDto> LineOperationsComboBox;
        List<ListOperationsDto> LineOperationsList = new List<ListOperationsDto>();

        SfComboBox<string, ListStationsDto> LineStationsComboBox;
        List<ListStationsDto> LineStationsList = new List<ListStationsDto>();

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectOperationLinesDto LineDataSource = new SelectOperationLinesDto();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<ListOperationLinesDto> GridLineList = new List<ListOperationLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = OperationsAppService;
            await GetLineOperationsList();
            await GetLineStationsList();
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }


        #region Operasyon Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationsDto();
            DataSource.SelectOperationLines = new List<SelectOperationLinesDto>();

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<ListOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    //BeforeInsertAsync();
                    LineCrudPopup = true;
                    DataSource.SelectOperationLines.Add(LineDataSource);
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
           
        }

        #endregion

        #region Operasyonlar - Operasyon Satırları
        public async Task LineOperationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineOperationsComboBox.FilterAsync(LineOperationsList, query);
        }

        private async Task GetLineOperationsList()
        {
            LineOperationsList = (await OperationsAppService.GetListAsync(new ListOperationsParameterDto())).Data.ToList();
        }

        public async Task LineOperationValueChangeHandler(ChangeEventArgs<string, ListOperationsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.OperationID = args.ItemData.Id;
                LineDataSource.OperationName = args.ItemData.Name;
            }
            else
            {
                LineDataSource.OperationID = Guid.Empty;
                LineDataSource.OperationName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İstasyonlar - Operasyon Satırları
        public async Task LineStationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineStationsComboBox.FilterAsync(LineStationsList, query);
        }

        private async Task GetLineStationsList()
        {
            LineStationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        public async Task LineStationValueChangeHandler(ChangeEventArgs<string, ListStationsDto> args)
        {
            if (args.ItemData != null)
            {
                LineDataSource.StationID = args.ItemData.Id;
                LineDataSource.StationCode = args.ItemData.Code;
            }
            else
            {
                LineDataSource.StationID = Guid.Empty;
                LineDataSource.StationCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
