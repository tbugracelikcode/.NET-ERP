using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Shift
{
    public partial class ShiftsListPage
    {

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
            _L = L;
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        protected override async Task OnSubmit()
        {
            decimal toplamVardiyaSure = DataSource.TotalWorkTime;
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

        public IEnumerable<SelectShiftLinesDto> shifttypes = GetEnumDisplayShiftTypesNames<ShiftLinesTypeEnum>();

        public static List<SelectShiftLinesDto> GetEnumDisplayShiftTypesNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<T>()
                       .Select(x => new SelectShiftLinesDto
                       {
                           Type = x as ShiftLinesTypeEnum?,
                           TypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }


        #endregion

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectShiftsDto()
            {
                IsActive = true
            };

            DataSource.SelectShiftLinesDto = new List<SelectShiftLinesDto>();
            GridLineList = DataSource.SelectShiftLinesDto;

            EditPageVisible = true;


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

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = (bool?)DataSource.GetType().GetProperty("DataOpenStatus").GetValue(DataSource);

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync("Bilgi", "Seçtiğiniz kayıt ..... tarafından kullanılmaktadır.");
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
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
                    IsChanged = true;
                    DataSource = (await ShiftsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    GridLineList = DataSource.SelectShiftLinesDto;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    await DeleteAsync(args.RowInfo.RowData.Id);
                    await GetListDataSourceAsync();
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
            TimeSpan? start;
            TimeSpan? end;
            TimeSpan diff;
            string control;
            decimal result;

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

                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            DataSource.SelectShiftLinesDto.Remove(args.RowInfo.RowData);

                            #region Vardiya Süre Hesaplamaları

                            start = args.RowInfo.RowData.StartHour;
                            end = args.RowInfo.RowData.EndHour;
                            diff = end.GetValueOrDefault() - start.GetValueOrDefault();
                            control = diff.TotalSeconds.ToString("#.00");
                            result = Convert.ToDecimal(control);
                            DataSource.TotalWorkTime -= result;
                            switch (args.RowInfo.RowData.Type)
                            {
                                case ShiftLinesTypeEnum.Mola: DataSource.TotalBreakTime -= result; break;
                                case ShiftLinesTypeEnum.Calisma: DataSource.NetWorkTime -= result; break;
                                case ShiftLinesTypeEnum.Temizlik: break;
                                case ShiftLinesTypeEnum.FazlaMesai: DataSource.Overtime -= result; break;
                            }

                            #endregion

                        }

                        else
                        {

                            #region Vardiya Süre Hesaplamaları

                            start = line.StartHour;
                            end = line.EndHour;
                            diff = end.GetValueOrDefault() - start.GetValueOrDefault();
                            control = diff.TotalSeconds.ToString("#.00");
                            result = Convert.ToDecimal(control);
                            DataSource.TotalWorkTime -= result;

                            switch (line.Type)
                            {
                                case ShiftLinesTypeEnum.Mola: DataSource.TotalBreakTime -= result; break;
                                case ShiftLinesTypeEnum.Calisma: DataSource.NetWorkTime -= result; break;
                                case ShiftLinesTypeEnum.Temizlik: break;
                                case ShiftLinesTypeEnum.FazlaMesai: DataSource.Overtime -= result; break;
                            }

                            #endregion

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

            #region Değişkenler

            int commonEndHour;
            TimeSpan? baslangic = LineDataSource.StartHour;
            TimeSpan? bitis = LineDataSource.EndHour;
            TimeSpan fark = bitis.GetValueOrDefault() - baslangic.GetValueOrDefault();
            string arakontrol = fark.TotalSeconds.ToString("#.00");
            decimal result = Convert.ToDecimal(arakontrol);

            #endregion

            if (LineDataSource.Id == Guid.Empty)
            {
                if(DataSource.SelectShiftLinesDto.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectShiftLinesDto.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if(selectedLineIndex > -1)
                    {
                        DataSource.SelectShiftLinesDto[selectedLineIndex] = LineDataSource;
                    }
                }

                else
                {
                    DataSource.SelectShiftLinesDto.Add(LineDataSource);
                }

            }

            else
            {
                int selectedLineIndex = DataSource.SelectShiftLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

                if (selectedLineIndex > -1)
                {
                    DataSource.SelectShiftLinesDto[selectedLineIndex] = LineDataSource;
                }
            }

            commonEndHour = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Count();
            var Deneme = GridLineList.Where(t => t.StartHour == LineDataSource.StartHour).ToList();

            if (commonEndHour == 0)
            {
                #region Vardiya 24 Saat Kontrolü

                if (DataSource.TotalWorkTime > 86400)
                {
                    await ModalManager.WarningPopupAsync("Uyarı", "Vardiya süresi 24 saatten (86400 saniye) fazla olamaz.");
                }

                else
                {
                    if(result> 0)
                    {
                        GridLineList = DataSource.SelectShiftLinesDto;

                        await _LineGrid.Refresh();

                        HideLinesPopup();

                        GetTotal();

                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await ModalManager.WarningPopupAsync("Uyarı", "Bitiş saati, baslangıç saatinden erken seçilemez.");
                    }
                   


                }

                #endregion
            }

            else if(commonEndHour != 0)
            {
                ShiftLinesTypeEnum? shifttype = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.Type).FirstOrDefault();
                string typeException = "";
                switch (shifttype)
                {
                    case ShiftLinesTypeEnum.Calisma: typeException = "Çalışma"; break;
                    case ShiftLinesTypeEnum.FazlaMesai: typeException = "Fazla Mesai"; break;
                    case ShiftLinesTypeEnum.Mola: typeException = "Mola"; break;
                    case ShiftLinesTypeEnum.Temizlik: typeException = "Temizlik"; break;
                    default: break;
                }
                string hourException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.EndHour).FirstOrDefault().ToString();
                await ModalManager.WarningPopupAsync("Uyarı", "Bitiş saati " + hourException + " olan " + typeException + " ile aynı başlangıç saatine ait başka bir kayıt yapılamaz.");
            }
            
        }

        public override void GetTotal()
        {
            DataSource.TotalBreakTime = 0;
            DataSource.TotalWorkTime = 0;
            DataSource.Overtime = 0;
            DataSource.NetWorkTime = 0;

            foreach (var item in GridLineList)
            {
                TimeSpan? baslangic = item.StartHour;
                TimeSpan? bitis = item.EndHour;
                TimeSpan fark = bitis.GetValueOrDefault() - baslangic.GetValueOrDefault();
                string araKontrol = fark.TotalSeconds.ToString("#.00");


                 #region Vardiya Süreleri Hesaplamaları

                    switch (item.Type)
                    {
                        #region Toplam Mola Süresi
                        case ShiftLinesTypeEnum.Mola:

                            DataSource.TotalBreakTime += Convert.ToDecimal(araKontrol);
                            break;

                        #endregion

                        #region Toplam Temizlik Süresi
                        case ShiftLinesTypeEnum.Temizlik:

                            //DataSource'a CleaningTime eklenmesi halinde buraya kod yazılacak.
                            break;

                        #endregion

                        #region Toplam Net Çalışma Süresi
                        case ShiftLinesTypeEnum.Calisma:

                            DataSource.NetWorkTime += Convert.ToDecimal(araKontrol);
                            break;

                        #endregion

                        #region Toplam Fazla Mesai Süresi
                        case ShiftLinesTypeEnum.FazlaMesai:

                            DataSource.Overtime += Convert.ToDecimal(araKontrol);
                            break;

                            #endregion

                    }

                    #endregion
               
            }

            DataSource.TotalWorkTime = DataSource.NetWorkTime + DataSource.Overtime + DataSource.TotalBreakTime;
        }
        

        #endregion

    }
}
