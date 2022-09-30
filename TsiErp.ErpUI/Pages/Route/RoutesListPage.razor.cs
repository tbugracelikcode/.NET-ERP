using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Operation.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Route
{
    public partial class RoutesListPage
    {
        private SfGrid<ListRoutesDto> _grid;


        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListProductsDto> LineProductsComboBox;
        List<ListProductsDto> LineProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListRoutesDto> LineRoutesComboBox;
        List<ListRoutesDto> LineRoutesList = new List<ListRoutesDto>();

        SfComboBox<string, ListOperationsDto> LineOperationsComboBox;
        List<ListOperationsDto> LineOperationsList = new List<ListOperationsDto>();

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectRouteLinesDto LineDataSource = new SelectRouteLinesDto();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<ListRouteLinesDto> GridLineList = new List<ListRouteLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = RoutesAppService;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }

        #region Rota Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
           
            DataSource.SelectRouteLines = new List<SelectRouteLinesDto>();

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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<ListRouteLinesDto> args)
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

        #region Stok Kartları 
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
            DataSource.ProductID = args.ItemData.Id;
            DataSource.ProductCode = args.ItemData.Code;
        }
        #endregion

        #region Stok Kartları - Rota Satırları
        public async Task LineProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineProductsComboBox.FilterAsync(LineProductsList, query);
        }

        private async Task GetLineProductsList()
        {
            LineProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task LineProductOpened(PopupEventArgs args)
        {
            if (LineProductsList.Count == 0)
            {
                await GetLineProductsList();
            }
        }

        private void LineProductValueChanged(ChangeEventArgs<string, ListProductsDto> args)
        {
            LineDataSource.ProductID = args.ItemData.Id;
            LineDataSource.ProductCode = args.ItemData.Code;
        }
        #endregion

        #region Rotalar - Rota Satırları
        public async Task LineRouteFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineRoutesComboBox.FilterAsync(LineRoutesList, query);
        }

        private async Task GetLineRoutesList()
        {
            LineRoutesList = (await RoutesAppService.GetListAsync(new ListRoutesParameterDto())).Data.ToList();
        }

        public async Task LineRouteOpened(PopupEventArgs args)
        {
            if (LineRoutesList.Count == 0)
            {
                await GetLineRoutesList();
            }
        }

        private void LineRouteValueChanged(ChangeEventArgs<string, ListRoutesDto> args)
        {
            LineDataSource.RouteID = args.ItemData.Id;
            LineDataSource.RouteCode = args.ItemData.Code;
        }
        #endregion

        #region Operasyonlar - Rota Satırları
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

        public async Task LineOperationOpened(PopupEventArgs args)
        {
            if (LineOperationsList.Count == 0)
            {
                await GetLineOperationsList();
            }
        }

        private void LineOperationValueChanged(ChangeEventArgs<string, ListOperationsDto> args)
        {
            LineDataSource.OperationID = args.ItemData.Id;
            LineDataSource.OperationName = args.ItemData.Name;
        }
        #endregion
    }
}
