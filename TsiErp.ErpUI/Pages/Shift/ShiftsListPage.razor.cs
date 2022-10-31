using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using Syncfusion.Blazor.HeatMap.Internal;
using Newtonsoft.Json;
using TsiErp.Entities.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using TsiErp.ErpUI.Helpers;

namespace TsiErp.ErpUI.Pages.Shift
{
    public partial class ShiftsListPage
    {

        private SfGrid<ListShiftsDto> _grid;
        private SfGrid<SelectShiftLinesDto> _LineGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        SelectShiftLinesDto LineDataSource;
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        List<SelectShiftLinesDto> GridLineList = new List<SelectShiftLinesDto>();

        private bool LineCrudPopup = false; 


        protected override async void OnInitialized()
        {
            BaseCrudService = ShiftsAppService;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        #region Vardiya Satır Modalı İşlemleri

        #region Vardiya Satır Enum Combobox

        List<ComboBoxEnumItem<ShiftLinesTypeEnum>> ShiftTypesList = new List<ComboBoxEnumItem<ShiftLinesTypeEnum>>();

        public string[] Types { get; set; }

        public string[] EnumValues = Enum.GetNames(typeof(ShiftLinesTypeEnum));


        #endregion

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectShiftsDto() {
                IsActive = true
            };

            DataSource.SelectShiftLinesDto = new List<SelectShiftLinesDto>();
            GridLineList = DataSource.SelectShiftLinesDto;

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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListShiftsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    DataSource = (await ShiftsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectShiftLinesDto;
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    await DeleteAsync(args.RowInfo.RowData.Id);
                    await InvokeAsync(StateHasChanged);
                    break;

                case "refresh":
                    break;

                default:
                    break;
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectShiftLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    LineDataSource = new SelectShiftLinesDto();
                    LineDataSource.Coefficient = 1;
                    LineCrudPopup = true;
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
                        if (DataSource.Id == Guid.Empty)
                        {
                            DataSource.SelectShiftLinesDto.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            var salesPropositions = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                            var line = salesPropositions.SelectShiftLinesDto.Find(t => t.Id == args.RowInfo.RowData.Id);

                            if (line != null)
                            {
                                await DeleteAsync(args.RowInfo.RowData.Id);
                                DataSource.SelectShiftLinesDto.Remove(line);
                                await GetListDataSourceAsync();
                            }
                            else
                            {
                                DataSource.SelectShiftLinesDto.Remove(line);
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
                LineDataSource.Id = ApplicationService.GuidGenerator.CreateGuid();
                DataSource.SelectShiftLinesDto.Add(LineDataSource);
            }
            else
            {
                int selectedLineIndex = DataSource.SelectShiftLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectShiftLinesDto[selectedLineIndex] = LineDataSource;
                }
            }

            GridLineList = DataSource.SelectShiftLinesDto;
            GetTotal();
            await _LineGrid.Refresh();

            HideLinesPopup();
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
