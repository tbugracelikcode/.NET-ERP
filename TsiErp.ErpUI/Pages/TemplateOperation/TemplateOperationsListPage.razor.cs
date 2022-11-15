using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.TemplateOperation
{
    public partial class TemplateOperationsListPage
    {
        #region ComboBox Listeleri

        SfComboBox<string, ListStationsDto> LineStationsComboBox;
        List<ListStationsDto> LineStationsList = new List<ListStationsDto>();

        #endregion

        private SfGrid<ListTemplateOperationsDto> _grid;
        private SfGrid<SelectTemplateOperationLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectTemplateOperationLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectTemplateOperationLinesDto> GridLineList = new List<SelectTemplateOperationLinesDto>();

        private bool LineCrudPopup = false;


        protected override async void OnInitialized()
        {
            BaseCrudService = TemplateOperationsAppService;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

            await GetLineStationsList();
        }

        #region Şablon Operasyon Satır Modalı İşlemleri
        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectTemplateOperationsDto()
            {
                IsActive = true
            };
            DataSource.SelectTemplateOperationLines = new List<SelectTemplateOperationLinesDto>();
            GridLineList = DataSource.SelectTemplateOperationLines;

            ShowEditPage();


            await Task.CompletedTask;
        }

        protected async Task LineBeforeInsertAsync()
        {
            LineDataSource = new SelectTemplateOperationLinesDto()
            {
                Alternative = false,
                Priority = GridLineList.Count + 1
            };
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListTemplateOperationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await TemplateOperationsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectTemplateOperationLines.OrderBy(t => t.Priority).ToList();

                    foreach (var item in GridLineList)
                    {
                        item.StationCode = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data.Code;
                        item.StationName = (await StationsAppService.GetAsync(item.StationID.GetValueOrDefault())).Data.Code;
                    }

                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz şablon operasyon kalıcı olarak silinecektir.");
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

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectTemplateOperationLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectTemplateOperationLinesDto();
                    LineCrudPopup = true;
                    LineDataSource.LineNr = GridLineList.Count + 1;
                    LineDataSource.Priority = GridLineList.Count + 1;
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
                            DataSource.SelectTemplateOperationLines.Remove(args.RowInfo.RowData);

                            for (int index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault()); index < GridLineList.Count(); index++ )
                            {
                                GridLineList[index].Priority -= 1;
                                GridLineList[index].LineNr -= 1;
                            }


                            await _LineGrid.Refresh();
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            if (line != null)
                            {
                                int index = Convert.ToInt32(_LineGrid.SelectedRowIndexes.FirstOrDefault());

                                for (index+=1; index < GridLineList.Count(); index++)
                                {
                                    GridLineList[index].Priority -= 1;
                                    GridLineList[index].LineNr -= 1;
                                }

                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectTemplateOperationLines.Remove(line);

                                DataSource.SelectTemplateOperationLines = GridLineList;

                                
                                await GetListDataSourceAsync();

                                await _LineGrid.Refresh();
                                await InvokeAsync(StateHasChanged);

                            }
                            else
                            {
                                DataSource.SelectTemplateOperationLines.Remove(line);
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
                if (DataSource.SelectTemplateOperationLines.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectTemplateOperationLines.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
                    {
                        DataSource.SelectTemplateOperationLines[selectedLineIndex] = LineDataSource;
                    }
                }
                else
                {
                    DataSource.SelectTemplateOperationLines.Add(LineDataSource);
                }
            }
            else
            {
                int selectedLineIndex = DataSource.SelectTemplateOperationLines.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectTemplateOperationLines[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectTemplateOperationLines;
            GridLineList = GridLineList.OrderBy(t => t.Priority).ToList();
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }

        public override void GetTotal()
        {

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

                DataSource.SelectTemplateOperationLines = GridLineList;

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

                DataSource.SelectTemplateOperationLines = GridLineList;

                await _LineGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
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
