using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.Business.Extensions.ObjectMapping;

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

        #region Değişkenler

        decimal totalWorkTime;
        decimal totalBreakTime;
        decimal totalCleaningTime;
        decimal totalNetWorkTime;
        decimal totalOvertime;

        #endregion


        protected override async void OnInitialized()
        {
            BaseCrudService = ShiftsAppService;

            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        protected override async Task OnSubmit()
        {
            decimal toplamVardiyaSure = ListDataSource.Sum(t => t.TotalWorkTime);
            if (toplamVardiyaSure > 86400)
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Vardiyaların toplam çalışma süreleri, 24 saati geçemez.");
            }
            else
            {
                SelectShiftsDto result;

                if (DataSource.Id == Guid.Empty)
                {
                    var createInput = ObjectMapper.Map<SelectShiftsDto, CreateShiftsDto>(DataSource);

                    result = (await CreateAsync(createInput)).Data;

                    if (result != null)
                        DataSource.Id = result.Id;
                }
                else
                {
                    var updateInput = ObjectMapper.Map<SelectShiftsDto, UpdateShiftsDto>(DataSource);

                    result = (await UpdateAsync(updateInput)).Data;
                }

                if (result == null)
                {

                    return;
                }

                await GetListDataSourceAsync();

                var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

                HideEditPage();

                if (DataSource.Id == Guid.Empty)
                {
                    DataSource.Id = result.Id;
                }

                if (savedEntityIndex > -1)
                    SelectedItem = ListDataSource.SetSelectedItem(savedEntityIndex);
                else
                    SelectedItem = ListDataSource.GetEntityById(DataSource.Id);
            }


        }

        #region Vardiya Satır Modalı İşlemleri

        #region Vardiya Satır Enum Combobox

        List<ComboBoxEnumItem<ShiftLinesTypeEnum>> ShiftTypesList = new List<ComboBoxEnumItem<ShiftLinesTypeEnum>>();

        public string[] Types { get; set; }

        public string[] EnumValues = Enum.GetNames(typeof(ShiftLinesTypeEnum));


        #endregion

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectShiftsDto()
            {
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
                            var shifts = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                            var line = shifts.SelectShiftLinesDto.Find(t => t.Id == args.RowInfo.RowData.Id);

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

           

            int hourListCount = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Count();

            if(hourListCount == 0)
            {
                #region Vardiya Süreleri Hesaplamaları

                switch (LineDataSource.Type)
                {
                    #region Toplam Mola Süresi
                    case ShiftLinesTypeEnum.Mola:


                        TimeSpan? baslangicMola = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Mola).Select(t => t.StartHour).FirstOrDefault();
                        TimeSpan? bitisMola = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Mola).Select(t => t.EndHour).FirstOrDefault();
                        TimeSpan farkMola = bitisMola.GetValueOrDefault() - baslangicMola.GetValueOrDefault();
                        string arakontrolMola = farkMola.TotalSeconds.ToString("#.00");
                        totalBreakTime += Convert.ToDecimal(arakontrolMola);
                        DataSource.TotalBreakTime = totalBreakTime;
                        totalWorkTime += Convert.ToDecimal(arakontrolMola);
                        DataSource.TotalWorkTime = totalWorkTime;
                        break;

                    #endregion

                    #region Toplam Temizlik Süresi
                    case ShiftLinesTypeEnum.Temizlik:

                        TimeSpan? baslangicTemizlik = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Temizlik).Select(t => t.StartHour).FirstOrDefault();
                        TimeSpan? bitisTemizlik = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Temizlik).Select(t => t.StartHour).FirstOrDefault();
                        TimeSpan farkTemizlik = bitisTemizlik.GetValueOrDefault() - baslangicTemizlik.GetValueOrDefault();
                        string arakontrolTemizlik = farkTemizlik.TotalSeconds.ToString("#.00");
                        totalCleaningTime = Convert.ToDecimal(arakontrolTemizlik);
                        totalWorkTime += Convert.ToDecimal(arakontrolTemizlik);
                        DataSource.TotalWorkTime = totalWorkTime;
                        break;

                    #endregion

                    #region Toplam Net Çalışma Süresi
                    case ShiftLinesTypeEnum.Calisma:

                        TimeSpan? baslangicNetCalisma = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Calisma).Select(t => t.StartHour).FirstOrDefault();
                        TimeSpan? bitisNetCalisma = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Calisma).Select(t => t.EndHour).FirstOrDefault();
                        TimeSpan farkNetCalisma = bitisNetCalisma.GetValueOrDefault() - baslangicNetCalisma.GetValueOrDefault();
                        string arakontrolNetCalisma = farkNetCalisma.TotalSeconds.ToString("#.00");
                        totalNetWorkTime += Convert.ToDecimal(arakontrolNetCalisma);
                        DataSource.NetWorkTime = totalNetWorkTime;
                        totalWorkTime += Convert.ToDecimal(arakontrolNetCalisma);
                        DataSource.TotalWorkTime = totalWorkTime;
                        break;

                    #endregion

                    #region Toplam Fazla Mesai Süresi
                    case ShiftLinesTypeEnum.FazlaMesai:

                        TimeSpan? baslangicFazlaMesai = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.FazlaMesai).Select(t => t.StartHour).FirstOrDefault();
                        TimeSpan? bitisFazlaMesai = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.FazlaMesai).Select(t => t.EndHour).FirstOrDefault();
                        TimeSpan farkFazlaMesai = bitisFazlaMesai.GetValueOrDefault() - baslangicFazlaMesai.GetValueOrDefault();
                        string arakontrolFazlaMesai = farkFazlaMesai.TotalSeconds.ToString("#.00");
                        totalOvertime += Convert.ToDecimal(arakontrolFazlaMesai);
                        DataSource.Overtime = totalOvertime;
                        totalWorkTime += Convert.ToDecimal(arakontrolFazlaMesai);
                        DataSource.TotalWorkTime = totalWorkTime;
                        break;

                        #endregion
                }

                #endregion

                GridLineList = DataSource.SelectShiftLinesDto;
                GetTotal();
                await _LineGrid.Refresh();

                HideLinesPopup();
                await InvokeAsync(StateHasChanged);
            }

            else
            {
                string typeException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.Type).FirstOrDefault().ToString();
                string hourException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.EndHour).FirstOrDefault().ToString();
                await ModalManager.WarningPopupAsync("Uyarı" , "Bitiş saati " + hourException + " olan " + typeException + " ile aynı başlangıç saatine ait başka bir kayıt yapılamaz.");
            }

        }

        #endregion
    }
}
