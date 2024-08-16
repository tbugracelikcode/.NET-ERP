using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Shift
{
    public partial class ShiftsListPage : IDisposable
    {

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

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

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ShiftsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();

        }

        protected override async Task OnSubmit()
        {
            decimal toplamVardiyaSure = DataSource.TotalWorkTime;
            if (toplamVardiyaSure > 86400)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupTitle24HourBase"]);
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
                       .Cast<ShiftLinesTypeEnum>()
                       .Select(x => new SelectShiftLinesDto
                       {
                           Type = x,
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
                Code = FicheNumbersAppService.GetFicheNumberAsync("ShiftsChildMenu")
            };

            DataSource.SelectShiftLinesDto = new List<SelectShiftLinesDto>();
            GridLineList = DataSource.SelectShiftLinesDto;

            EditPageVisible = true;


            await Task.CompletedTask;
        }

        protected void CreateLineContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ShiftLineContextAdd":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftLineContextAdd"], Id = "new" }); break;
                            case "ShiftLineContextChange":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftLineContextChange"], Id = "changed" }); break;
                            case "ShiftLineContextDelete":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftLineContextDelete"], Id = "delete" }); break;
                            case "ShiftLineContextRefresh":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftLineContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ShiftContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftContextAdd"], Id = "new" }); break;
                            case "ShiftContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftContextChange"], Id = "changed" }); break;
                            case "ShiftContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftContextDelete"], Id = "delete" }); break;
                            case "ShiftContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ShiftContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {
                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
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
                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);
                    if (res == true)
                    {
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await _grid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
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

            foreach (var item in shifttypes)
            {
                item.TypeName = L[item.TypeName];
            }

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

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

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
                if (DataSource.SelectShiftLinesDto.Contains(LineDataSource))
                {
                    int selectedLineIndex = DataSource.SelectShiftLinesDto.FindIndex(t => t.LineNr == LineDataSource.LineNr);

                    if (selectedLineIndex > -1)
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

            //commonEndHour = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Count();

            //if (commonEndHour == 0)
            //{
            #region Vardiya 24 Saat Kontrolü

            if (DataSource.TotalWorkTime > 86400)
            {
                await ModalManager.WarningPopupAsync("Uyarı", "Vardiya süresi 24 saatten (86400 saniye) fazla olamaz.");
            }

            else
            {
                if (result > 0)
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
            //}

            //else if (commonEndHour != 0)
            //{
            //    ShiftLinesTypeEnum? shifttype = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.Type).FirstOrDefault();
            //    string typeException = "";
            //    switch (shifttype)
            //    {
            //        case ShiftLinesTypeEnum.Calisma: typeException = "Çalışma"; break;
            //        case ShiftLinesTypeEnum.FazlaMesai: typeException = "Fazla Mesai"; break;
            //        case ShiftLinesTypeEnum.Mola: typeException = "Mola"; break;
            //        case ShiftLinesTypeEnum.Temizlik: typeException = "Temizlik"; break;
            //        default: break;
            //    }
            //    //string hourException = GridLineList.Where(t => t.EndHour == LineDataSource.StartHour).Select(t => t.EndHour).FirstOrDefault().ToString();
            //    //await ModalManager.WarningPopupAsync("Uyarı", "Bitiş saati " + hourException + " olan " + typeException + " ile aynı başlangıç saatine ait başka bir kayıt yapılamaz.");
            //}

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



        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ShiftsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
