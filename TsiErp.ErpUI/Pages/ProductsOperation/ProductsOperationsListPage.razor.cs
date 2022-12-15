using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;

namespace TsiErp.ErpUI.Pages.ProductsOperation
{
    public partial class ProductsOperationsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListStationsDto> LineStationsComboBox;
        List<ListStationsDto> LineStationsList = new List<ListStationsDto>();

        SfComboBox<string, ListProductsDto> ProductsComboBox;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();

        SfComboBox<string, ListTemplateOperationsDto> TemplateOperationsComboBox;
        List<ListTemplateOperationsDto> TemplateOperationsList = new List<ListTemplateOperationsDto>();

        #endregion

        private SfGrid<ListProductsOperationsDto> _grid;
        private SfGrid<SelectProductsOperationLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectProductsOperationLinesDto LineDataSource;

        SelectTemplateOperationsDto TemplateOperationDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductsOperationLinesDto> GridLineList = new List<SelectProductsOperationLinesDto>();

        List<SelectTemplateOperationLinesDto> TemplateOperationLineList = new List<SelectTemplateOperationLinesDto>();

        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            BaseCrudService = ProductsOperationsAppService;

            await GetProductsList();
            await GetLineStationsList();
            await GetTemplateOperationsList();
        }

        #region Ürüne Özel Operasyon Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsOperationsDto()
            {
                IsActive = true
            };

            DataSource.SelectProductsOperationLines = new List<SelectProductsOperationLinesDto>();
            GridLineList = DataSource.SelectProductsOperationLines;

            ShowEditPage();


            await Task.CompletedTask;
        }

        protected async Task LineBeforeInsertAsync()
        {
            LineDataSource = new SelectProductsOperationLinesDto()
            {
                Alternative = false,
                Priority = GridLineList.Count + 1,
                LineNr = GridLineList.Count + 1
            };

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductsOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await ProductsOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();

                    foreach (var item in GridLineList)
                    {
                        item.StationCode = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data.Code;
                        item.StationName = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz ürüne özel operasyon kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectProductsOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectProductsOperationLinesDto();
                    LineCrudPopup = true;
                    await LineBeforeInsertAsync();
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

                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectProductsOperationLines.Remove(args.RowInfo.RowData);



                            await _LineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {

                                int selectedIndex = GridLineList.FindIndex(t => t.Id == line.Id);

                                if (selectedIndex >= 0)
                                {
                                    int selectedPriority = GridLineList[selectedIndex].Priority;


                                    GridLineList.Remove(line);

                                    for (int i = 0; i < GridLineList.Count; i++)
                                    {
                                        GridLineList[i].LineNr = i + 1;
                                        GridLineList[i].Priority = i + 1;
                                    }

                                    DataSource.SelectProductsOperationLines = GridLineList;

                                    await _LineGrid.Refresh();

                                    await DeleteAsync(args.RowInfo.RowData.Id);
                                    await GetListDataSourceAsync();
                                    await InvokeAsync(StateHasChanged);
                                }
                            }
                            else
                            {
                                DataSource.SelectProductsOperationLines.Remove(line);
                            }
                        }
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
                if (DataSource.SelectProductsOperationLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectProductsOperationLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectProductsOperationLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectProductsOperationLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectProductsOperationLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();

            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        public async void ArrowUpBtnClicked()
        {
            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == 0))
            {
                GridLineList[index].Priority -= 1;
                GridLineList[index - 1].Priority += 1;
                GridLineList[index].LineNr -= 1;
                GridLineList[index - 1].LineNr += 1;

                GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = GridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void ArrowDownBtnClicked()
        {

            var index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

            if (!(index == GridLineList.Count()))
            {
                GridLineList[index].Priority += 1;
                GridLineList[index + 1].Priority -= 1;
                GridLineList[index].LineNr += 1;
                GridLineList[index + 1].LineNr -= 1;

                GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();

                DataSource.SelectProductsOperationLines = GridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

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
                DataSource.ProductCode = args.ItemData.Code; ;
            }
            else
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Şablon Operasyonlar 

        public async Task TemplateOperationFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await TemplateOperationsComboBox.FilterAsync(TemplateOperationsList, query);
        }

        private async Task GetTemplateOperationsList()
        {
            TemplateOperationsList = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.ToList();
        }

        public async Task TemplateOperationValueChangeHandler(ChangeEventArgs<string, ListTemplateOperationsDto> args)
        {
            if (args.ItemData != null)
            {
                if (DataSource.ProductID != null)
                {
                    DataSource.TemplateOperationID = args.ItemData.Id;
                    DataSource.TemplateOperationCode = args.ItemData.Code;
                    DataSource.TemplateOperationName = args.ItemData.Name;
                    DataSource.Name = DataSource.ProductCode + "-" + args.ItemData.Name;
                    DataSource.Code = DataSource.ProductCode + "-" + args.ItemData.Code;

                    TemplateOperationDataSource = (await TemplateOperationsAppService.GetAsync(args.ItemData.Id)).Data;
                    TemplateOperationLineList = TemplateOperationDataSource.SelectTemplateOperationLines;
                    var stationList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();

                    foreach (var item in TemplateOperationLineList)
                    {
                        SelectProductsOperationLinesDto _productsOperationLine = new SelectProductsOperationLinesDto
                        {
                            AdjustmentAndControlTime = item.AdjustmentAndControlTime,
                            Alternative = item.Alternative,
                            OperationTime = item.OperationTime,
                            LineNr = item.LineNr,
                            Priority = item.Priority,
                            ProcessQuantity = item.ProcessQuantity,
                            ProductsOperationCode = DataSource.Code,
                            ProductsOperationID = DataSource.Id,
                            ProductsOperationName = DataSource.Name,
                            StationCode = stationList.Where(t=>t.Id == item.StationID).Select(t=>t.Code).FirstOrDefault(),
                            StationID = item.StationID,
                            StationName = stationList.Where(t => t.Id == item.StationID).Select(t => t.Name).FirstOrDefault()

                        };
                        GridLineList.Add(_productsOperationLine);
                      
                       
                    }
                    await _LineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await ModalManager.WarningPopupAsync("Uyarı", "Ürün seçmeden şablon operasyon seçilemez.");
                }

            }
            else
            {
                DataSource.TemplateOperationID = Guid.Empty;
                DataSource.TemplateOperationCode = string.Empty;
                DataSource.TemplateOperationName = string.Empty;
                DataSource.Name = string.Empty;
                DataSource.Code = string.Empty;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region İş İstasyonları - Operasyon Satırları

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
                LineDataSource.StationName = args.ItemData.Name;
            }
            else
            {
                LineDataSource.StationID = Guid.Empty;
                LineDataSource.StationCode = string.Empty;
                LineDataSource.StationName = string.Empty;
            }
            LineCalculate();
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}

