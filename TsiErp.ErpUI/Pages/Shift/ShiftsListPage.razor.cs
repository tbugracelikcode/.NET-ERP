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
            int hourListCount;

            if (LineDataSource.Id == Guid.Empty)
            {
                //LineDataSource.Id = ApplicationService.GuidGenerator.CreateGuid();
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

            hourListCount = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Count();

            if (hourListCount == 0)
            {
                

                #region Vardiya 24 Saat Kontrolü

                if (DataSource.TotalWorkTime > 86400)
                {
                    DataSource.TotalWorkTime = 0;
                    DataSource.TotalBreakTime = 0;
                    DataSource.NetWorkTime = 0;
                    DataSource.Overtime = 0;
                    await ModalManager.WarningPopupAsync("Uyarı", "Vardiya süresi 24 saatten (86400 saniye) fazla olamaz.");
                }

                else
                {
                    GridLineList = DataSource.SelectShiftLinesDto;
                    GetTotal();
                    await _LineGrid.Refresh();

                    HideLinesPopup();

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
                                DataSource.TotalWorkTime += Convert.ToDecimal(araKontrol);
                                break;

                            #endregion

                            #region Toplam Temizlik Süresi
                            case ShiftLinesTypeEnum.Temizlik:


                                DataSource.TotalWorkTime += Convert.ToDecimal(araKontrol);
                                break;

                            #endregion

                            #region Toplam Net Çalışma Süresi
                            case ShiftLinesTypeEnum.Calisma:

                                DataSource.NetWorkTime += Convert.ToDecimal(araKontrol);
                                DataSource.TotalWorkTime += Convert.ToDecimal(araKontrol);
                                break;

                            #endregion

                            #region Toplam Fazla Mesai Süresi
                            case ShiftLinesTypeEnum.FazlaMesai:

                                DataSource.Overtime += Convert.ToDecimal(araKontrol);
                                DataSource.TotalWorkTime += Convert.ToDecimal(araKontrol);
                                break;

                                #endregion

                        }

                        #endregion

                    }

                    await InvokeAsync(StateHasChanged);


                }

                #endregion
            }

            else
            {
                string typeException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.Type).FirstOrDefault().ToString();
                string hourException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.EndHour).FirstOrDefault().ToString();
                await ModalManager.WarningPopupAsync("Uyarı", "Bitiş saati " + hourException + " olan " + typeException + " ile aynı başlangıç saatine ait başka bir kayıt yapılamaz.");
            }




            #region Eski Vardiya Süresi Hesaplama Kodları

            //hourListCount = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Count();

            //if (hourListCount == 0)
            //{
            //    #region Vardiya Süreleri Hesaplamaları

            //    switch (LineDataSource.Type)
            //    {
            //        #region Toplam Mola Süresi
            //        case ShiftLinesTypeEnum.Mola:


            //            TimeSpan? baslangicMola = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Mola && t.Id == LineDataSource.Id).Select(t => t.StartHour).LastOrDefault();
            //            TimeSpan? bitisMola = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Mola && t.Id == LineDataSource.Id).Select(t => t.EndHour).LastOrDefault();
            //            TimeSpan farkMola = bitisMola.GetValueOrDefault() - baslangicMola.GetValueOrDefault();
            //            string arakontrolMola = farkMola.TotalSeconds.ToString("#.00");
            //            DataSource.TotalBreakTime += Convert.ToDecimal(arakontrolMola);
            //            DataSource.TotalWorkTime += Convert.ToDecimal(arakontrolMola);
            //            break;

            //        #endregion

            //        #region Toplam Temizlik Süresi
            //        case ShiftLinesTypeEnum.Temizlik:

            //            TimeSpan? baslangicTemizlik = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Temizlik && t.Id == LineDataSource.Id).Select(t => t.StartHour).LastOrDefault();
            //            TimeSpan? bitisTemizlik = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Temizlik && t.Id == LineDataSource.Id).Select(t => t.StartHour).LastOrDefault();
            //            TimeSpan farkTemizlik = bitisTemizlik.GetValueOrDefault() - baslangicTemizlik.GetValueOrDefault();
            //            string arakontrolTemizlik = farkTemizlik.TotalSeconds.ToString("#.00");
            //            DataSource.TotalWorkTime += Convert.ToDecimal(arakontrolTemizlik);
            //            break;

            //        #endregion

            //        #region Toplam Net Çalışma Süresi
            //        case ShiftLinesTypeEnum.Calisma:

            //            TimeSpan? baslangicNetCalisma = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Calisma && t.Id == LineDataSource.Id).Select(t => t.StartHour).LastOrDefault();
            //            TimeSpan? bitisNetCalisma = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.Calisma && t.Id == LineDataSource.Id).Select(t => t.EndHour).LastOrDefault();
            //            TimeSpan farkNetCalisma = bitisNetCalisma.GetValueOrDefault() - baslangicNetCalisma.GetValueOrDefault();
            //            string arakontrolNetCalisma = farkNetCalisma.TotalSeconds.ToString("#.00");
            //            DataSource.NetWorkTime += Convert.ToDecimal(arakontrolNetCalisma);
            //            DataSource.TotalWorkTime += Convert.ToDecimal(arakontrolNetCalisma);
            //            break;

            //        #endregion

            //        #region Toplam Fazla Mesai Süresi
            //        case ShiftLinesTypeEnum.FazlaMesai:

            //            TimeSpan? baslangicFazlaMesai = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.FazlaMesai && t.Id == LineDataSource.Id).Select(t => t.StartHour).LastOrDefault();
            //            TimeSpan? bitisFazlaMesai = GridLineList.Where(t => t.Type == ShiftLinesTypeEnum.FazlaMesai && t.Id == LineDataSource.Id).Select(t => t.EndHour).LastOrDefault();
            //            TimeSpan farkFazlaMesai = bitisFazlaMesai.GetValueOrDefault() - baslangicFazlaMesai.GetValueOrDefault();
            //            string arakontrolFazlaMesai = farkFazlaMesai.TotalSeconds.ToString("#.00");
            //            DataSource.Overtime += Convert.ToDecimal(arakontrolFazlaMesai);
            //            DataSource.TotalWorkTime += Convert.ToDecimal(arakontrolFazlaMesai);
            //            break;

            //            #endregion
            //    }

            //    #endregion

            //    #region Vardiya 24 Saat Kontrolü

            //    if (DataSource.TotalWorkTime > 86400)
            //    {
            //        DataSource.TotalWorkTime = 0;
            //        DataSource.TotalBreakTime = 0;
            //        DataSource.NetWorkTime = 0;
            //        DataSource.Overtime = 0;
            //        await ModalManager.WarningPopupAsync("Uyarı", "Vardiya süresi 24 saatten (86400 saniye) fazla olamaz.");
            //    }
            //    else
            //    {
            //        GridLineList = DataSource.SelectShiftLinesDto;
            //        GetTotal();
            //        await _LineGrid.Refresh();

            //        HideLinesPopup();
            //        await InvokeAsync(StateHasChanged);
            //    }

            //    #endregion
            //}

            //else
            //{
            //    string typeException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.Type).FirstOrDefault().ToString();
            //    string hourException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.EndHour).FirstOrDefault().ToString();
            //    await ModalManager.WarningPopupAsync("Uyarı", "Bitiş saati " + hourException + " olan " + typeException + " ile aynı başlangıç saatine ait başka bir kayıt yapılamaz.");
            //}

            #endregion

        }

        #endregion
    }
}
