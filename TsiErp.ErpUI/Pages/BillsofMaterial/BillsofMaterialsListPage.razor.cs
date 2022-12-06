using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.BillsofMaterial
{
    public partial class BillsofMaterialsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListProductsDto> FinishedProductsComboBox;
        List<ListProductsDto> FinishedProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListUnitSetsDto> LineUnitSetsComboBox;
        List<ListUnitSetsDto> LineUnitSetsList = new List<ListUnitSetsDto>();

        SfComboBox<string, ListProductsDto> LineProductsComboBox;
        List<ListProductsDto> LineProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListProductsDto> LineFinishedProductsComboBox;
        List<ListProductsDto> LineFinishedProductsList = new List<ListProductsDto>();

        #endregion

        private SfGrid<ListBillsofMaterialsDto> _grid;
        private SfGrid<SelectBillsofMaterialLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectBillsofMaterialLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectBillsofMaterialLinesDto> GridLineList = new List<SelectBillsofMaterialLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            BaseCrudService = BillsofMaterialsAppService;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetFinishedProductsList();
            await GetLineProductsList();
            await GetLineUnitSetsList();
        }

        #region Reçete Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectBillsofMaterialsDto()
            {
                IsActive = true
            };

            DataSource.SelectBillsofMaterialLines = new List<SelectBillsofMaterialLinesDto>();
            GridLineList = DataSource.SelectBillsofMaterialLines;

            ShowEditPage();


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

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListBillsofMaterialsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await BillsofMaterialsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectBillsofMaterialLines;

                    foreach (var item in GridLineList)
                    {
                        item.FinishedProductCode = DataSource.FinishedProductCode;
                        item.ProductCode = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.ProductName = (await ProductsAppService.GetAsync(item.ProductID.GetValueOrDefault())).Data.Code;
                        item.UnitSetCode = (await UnitSetsAppService.GetAsync(item.UnitSetID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz reçete, kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectBillsofMaterialLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectBillsofMaterialLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
                    LineDataSource.FinishedProductID = DataSource.FinishedProductID;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    LineDataSource = args.RowInfo.RowData;
                    LineCrudPopup = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

                    if (res == true)
                    {
                        //var salesPropositionLines = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectBillsofMaterialLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectBillsofMaterialLines.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectBillsofMaterialLines.Remove(line);
                            }
                        }

                        await _LineGrid.Refresh();
                        GetTotal();
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
                if (DataSource.SelectBillsofMaterialLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectBillsofMaterialLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectBillsofMaterialLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectBillsofMaterialLines[selectedLineIndex] = LineDataSource;
                }
            }

            LineDataSource.FinishedProductID = DataSource.FinishedProductID;
            LineDataSource.FinishedProductCode = DataSource.FinishedProductCode;
            GridLineList = DataSource.SelectBillsofMaterialLines;
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Mamüller
        public async Task FinishedProductFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await FinishedProductsComboBox.FilterAsync(FinishedProductsList, query);
        }

        private async Task GetFinishedProductsList()
        {
            FinishedProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        public async Task FinishedProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.FinishedProductID = args.ItemData.Id;
                DataSource.FinishedProductCode = args.ItemData.Code;
                DataSource.FinishedProducName = args.ItemData.Name;

            }
            else
            {
                DataSource.FinishedProductID = Guid.Empty;
                DataSource.FinishedProductCode = string.Empty;
                DataSource.FinishedProducName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion


        #region Birim Setleri - Reçete Satırları
        public async Task LineUnitSetFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await LineUnitSetsComboBox.FilterAsync(LineUnitSetsList, query);
        }

        private async Task GetLineUnitSetsList()
        {
            LineUnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        public async Task LineUnitSetValueChangeHandler(ChangeEventArgs<string, ListUnitSetsDto> args)
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

        #region Stok Kartları - Reçete Satırları
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
        public async Task LineProductValueChangeHandler(ChangeEventArgs<string, ListProductsDto> args)
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
    }
}
