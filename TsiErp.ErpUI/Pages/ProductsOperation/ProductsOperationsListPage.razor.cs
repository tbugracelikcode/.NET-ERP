using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductsOperation
{
    public partial class ProductsOperationsListPage
    {
        private SfGrid<SelectProductsOperationLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectProductsOperationLinesDto LineDataSource;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectProductsOperationLinesDto> GridLineList = new List<SelectProductsOperationLinesDto>();


        private bool LineCrudPopup = false;

        protected override async void OnInitialized()
        {
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            BaseCrudService = ProductsOperationsAppService;
            _L = L;

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

        #region İş İstasyonu ButtonEdit

        SfTextBox StationsCodeButtonEdit;
        SfTextBox StationsNameButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsCodeOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsCodeButtonClickEvent);
            await StationsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsCodeButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public async Task StationsNameOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsNameButtonClickEvent);
            await StationsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsNameButtonClickEvent()
        {
            SelectStationsPopupVisible = true;
            await GetStationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                LineDataSource.StationID = Guid.Empty;
                LineDataSource.StationCode = string.Empty;
                LineDataSource.StationName = string.Empty;
            }
        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                LineDataSource.StationID = selectedUnitSet.Id;
                LineDataSource.StationCode = selectedUnitSet.Code;
                LineDataSource.StationName = selectedUnitSet.Code;
                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şablon Operasyon ButtonEdit

        SfTextBox TemplateOperationButtonEdit;
        bool SelectTemplateOperationsPopupVisible = false;
        List<ListTemplateOperationsDto> TemplateOperationsList = new List<ListTemplateOperationsDto>();

        public async Task TemplateOperationOnCreateIcon()
        {
            var TemplateOperationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TemplateOperationButtonClickEvent);
            await TemplateOperationButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TemplateOperationsButtonClick } });
        }

        public async void TemplateOperationButtonClickEvent()
        {
            SelectTemplateOperationsPopupVisible = true;
            await GetTemplateOperationsList();
            await InvokeAsync(StateHasChanged);
        }

        public void TemplateOperationsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.TemplateOperationID = Guid.Empty;
                DataSource.TemplateOperationCode = string.Empty;
                DataSource.TemplateOperationName = string.Empty;
            }
        }

        public async void TemplateOperationsDoubleClickHandler(RecordDoubleClickEventArgs<ListTemplateOperationsDto> args)
        {
            var selectedTemplateOperation = args.RowData;

            if (selectedTemplateOperation != null)
            {
                DataSource.TemplateOperationID = selectedTemplateOperation.Id;
                DataSource.TemplateOperationCode = selectedTemplateOperation.Code;
                DataSource.TemplateOperationName = selectedTemplateOperation.Code;
                foreach (var line in selectedTemplateOperation.SelectTemplateOperationLines)
                {
                    SelectProductsOperationLinesDto lineModel = new SelectProductsOperationLinesDto
                    {
                        AdjustmentAndControlTime = line.AdjustmentAndControlTime,
                        Alternative = line.Alternative,
                        LineNr = line.LineNr,
                        StationID = line.StationID,
                        StationCode = line.StationCode,
                        StationName = line.StationName,
                        ProductsOperationName = DataSource.Name,
                        ProductsOperationID = DataSource.Id,
                        ProductsOperationCode = DataSource.Code,
                        ProcessQuantity = line.ProcessQuantity,
                        Priority = line.Priority,
                        OperationTime = line.OperationTime,
                    };

                    GridLineList.Add(lineModel);
                    await _LineGrid.Refresh();
                }
                SelectTemplateOperationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Ürüne Özel Operasyon Satır Modalı İşlemleri

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectProductsOperationsDto()
            {
                IsActive = true
            };

            DataSource.SelectProductsOperationLines = new List<SelectProductsOperationLinesDto>();
            GridLineList = DataSource.SelectProductsOperationLines;

            EditPageVisible = true;


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
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextRefresh"], Id = "refresh" });
            }
        }

        public async override void ShowEditPage()
        {

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductsOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    DataSource = (await ProductsOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectProductsOperationLines.OrderBy(t => t.Priority).ToList();

                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageBase"]);
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

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

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

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetTemplateOperationsList()
        {
            TemplateOperationsList = (await TemplateOperationsAppService.GetListAsync(new ListTemplateOperationsParameterDto())).Data.ToList();
        }

        #endregion
    }
}

