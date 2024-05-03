using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Schedule;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.MaintenanceInstruction.Services;
using TsiErp.Business.Entities.MaintenancePeriod.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.CalendarColorConstant;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Shared;
using TsiErp.ErpUI.Utilities.ModalUtilities;
using static TsiErp.ErpUI.Pages.QualityControl.Report8D.Report8DsListPage;

namespace TsiErp.ErpUI.Pages.PlanningManagement.Calendar
{
    public partial class CalendarsListPage : IDisposable
    {
        SfSchedule<AppointmentData> ScheduleObj;

        SelectCalendarLinesDto LineDataSource;
        SelectPlannedMaintenancesDto MaintenanceDataSource;
        SelectPlannedMaintenanceLinesDto MaintenanceLineDataSource;
        private CellClickEventArgs CellData { get; set; }
        public bool schedularVisible { get; set; } = false;
        public List<int> YearList = new List<int>();
        public List<string> StationGroupNameList = new List<string>();
        public List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();
        public SfGrid<SelectCalendarDaysDto> _daysGrid;
        private SfGrid<SelectCalendarLinesDto> _LineGrid;
        private SfGrid<ListPlannedMaintenancesDto> _MaintenanceGrid;
        private SfGrid<SelectPlannedMaintenanceLinesDto> _MaintenanceLineGrid;
        public List<SelectCalendarDaysDto> GridDaysList = new List<SelectCalendarDaysDto>();
        public List<SelectCalendarDaysDto> SchedularDaysList = new List<SelectCalendarDaysDto>();
        public List<ListStationsDto> StationsList = new List<ListStationsDto>();
        public List<ListStationsDto> SelectedStations = new List<ListStationsDto>();
        public List<ListShiftsDto> ShiftsList = new List<ListShiftsDto>();
        List<ListPlannedMaintenancesDto> PlannedMaintenanceList = new List<ListPlannedMaintenancesDto>();
        List<SelectPlannedMaintenanceLinesDto> PlannedMaintenanceLineList = new List<SelectPlannedMaintenanceLinesDto>();
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        public List<ContextMenuItemModel> DayGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MaintenanceGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MaintenanceLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectCalendarLinesDto> LineGridList = new List<SelectCalendarLinesDto>();
        public List<SelectCalendarLinesDto> StationWorkStatusList = new List<SelectCalendarLinesDto>();
        [Inject]
        ModalManager ModalManager { get; set; }

        #region Değişkenler

        //private bool isCell;
        public bool chcTumu = true;
        public bool chcCalismaVar;
        public bool chcCalismaYok;
        public bool chcResmiTatil;
        public bool chcTatil;
        public bool chcYarimGun;
        public bool chcYuklemeGunu;
        public bool chcBakim;
        private bool StationsModalVisible = false;
        private bool LineModalVisible = false;
        //string cardbgcolor = "white";
        public DateTime officialHoliday = DateTime.Today;
        public DateTime CurrentDate = DateTime.Today;
        //int selection = 0;
        public DateTime? selectedDate = null;
        public string selectedDateStr = string.Empty;
        public bool isAllStationsChecked = false;
        public string workStatus = string.Empty;
        public bool lineWorkStatusVisible = false;
        public int tempworkStatus = 0;
        private bool MaintenanceModalVisible = false;
        private bool MaintenanceCrudModalVisible = false;
        private bool MaintenanceLineCrudModalVisible = false;
        private bool DayTypeModalVisible = false;
        private bool DayTypeCrudModalVisible = false;
        private bool isDateSpan = false;
        public DateTime StartDateDayType;
        public DateTime EndDateDayType;
        public bool overTimeModalVisible = false;

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = CalendarsService;
            _L = L;
            FinalDataSource = DataSourceEvent;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CalendarChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
            GetYearsList();
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateDayContextMenuItems();
            CreateLineMaintenaceInfosContextMenuItems();
            CreateLineMaintenaceInfosLineContextMenuItems();
            CreateDayTypeContextMenuItems();
            base.OnInitialized();

        }

        #region Çalışma Takvimi Ekleme Modalı Metotları

        private void GetYearsList()
        {
            YearList = Enumerable.Range(GetSQLDateAppService.GetDateFromSQL().Year - 2, 3).ToList();
        }

        public async Task YearValueChangeHandler(ChangeEventArgs<int, int> args)
        {
            if (args.ItemData != 0)
            {
                DataSource.Year = args.ItemData;
            }
            else
            {
                DataSource.Year = 0;
            }
            await InvokeAsync(StateHasChanged);
        }

        protected override Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            input.SelectCalendarDaysDto = GridDaysList;
            return base.CreateAsync(input);
        }

        public async Task AddOfficialHoliday()
        {
            if (!GridDaysList.Select(t => t.Date_).Contains(officialHoliday))
            {
                GridDaysList.Add(new SelectCalendarDaysDto()
                {
                    Date_ = officialHoliday,
                    CalendarDayStateEnum = 3
                });
                await _daysGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await ModalManager.WarningPopupAsync(L["UIWarningTitle"], L["UIWarningDateMessage"]);
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "CalendarContextAdd":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextAdd"], Id = "new" }); break;
                            case "CalendarContextShowCalendar":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextShowCalendar"], Id = "schedular" }); break;
                            case "CalendarContextChange":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextChange"], Id = "changed" }); break;
                            case "CalendarContextDelete":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextDelete"], Id = "delete" }); break;
                            case "CalendarContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextRefresh"], Id = "refresh" }); break;
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

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListCalendarsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "schedular":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    SchedularDaysList = (await CalendarsService.GetDaysListAsync(args.RowInfo.RowData.Id)).Data.ToList();
                    DataSourceEvent = ConvertToAppointmentData(SchedularDaysList);
                    FinalDataSource = DataSourceEvent;
                    schedularVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        protected void CreateDayContextMenuItems()
        {
            if (DayGridContextMenu.Count() == 0)
            {

                var contextID = contextsList.Where(t => t.MenuName == "CalendarDayContextDelete").Select(t => t.Id).FirstOrDefault();
                var permission = UserPermissionsList.Where(t => t.MenuId == contextID).Select(t => t.IsUserPermitted).FirstOrDefault();
                if (permission)
                {
                    DayGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarDayContextDelete"], Id = "delete" });
                }
            }
        }

        public async void OnDayContextMenuClick(ContextMenuClickEventArgs<SelectCalendarDaysDto> args)
        {
            switch (args.Item.Id)
            {
                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIWarningTitle"], L["UIWarningLineDelete"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line != null)
                        {
                            GridDaysList.Remove(line);
                        }

                        await _daysGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                default:
                    break;
            }
        }

        protected override Task BeforeInsertAsync()
        {
            GridDaysList.Clear();

            int thisYear = DateTime.Now.Year;

            #region Default Resmi Tatiller

            SelectCalendarDaysDto zaferBayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 8, 30)
            };

            SelectCalendarDaysDto ulusalEgemenlikBayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 4, 23)
            };

            SelectCalendarDaysDto isciBayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 5, 1)
            };

            SelectCalendarDaysDto ATATURKUAnmaBayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 5, 19)
            };

            SelectCalendarDaysDto temmuz15Bayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 7, 15)
            };

            SelectCalendarDaysDto CUMHURIYETBayrami = new SelectCalendarDaysDto
            {
                CalendarDayStateEnum = 3,
                Date_ = new DateTime(thisYear, 10, 29)
            };

            GridDaysList.Add(ulusalEgemenlikBayrami);
            GridDaysList.Add(isciBayrami);
            GridDaysList.Add(ATATURKUAnmaBayrami);
            GridDaysList.Add(temmuz15Bayrami);
            GridDaysList.Add(zaferBayrami);
            GridDaysList.Add(CUMHURIYETBayrami);

            #endregion

            DataSource = new SelectCalendarsDto()
            {
                IsPlanned = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("CalendarChildMenu"),
                Year = GetSQLDateAppService.GetDateFromSQL().Year
            };

            DataSource.SelectCalendarDaysDto = GridDaysList;



            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #endregion

        #region Çalışma Takvimi Görüntüleme Modalı Metotları

        public void HideScheduleEditPage()
        {
            schedularVisible = false;
            InvokeAsync(StateHasChanged);
        }

        private List<AppointmentData> ConvertToAppointmentData(List<SelectCalendarDaysDto> dtoList)
        {
            List<AppointmentData> DataSourceEvent = new List<AppointmentData> { };
            if (dtoList.Count > 0)
            {
                foreach (var dto in dtoList)
                {
                    AppointmentData data = new AppointmentData();
                    data.StartTime = dto.StartTime;
                    data.EndTime = dto.EndTime;
                    data.ResourceId = dto.CalendarDayStateEnum;
                    data.Subject = dto.Subject;
                    DataSourceEvent.Add(data);
                }
            }

            return DataSourceEvent;
        }

        public void customChange(string switchName)
        {
            DataSourceEvent = FinalDataSource;
            List<int> selectedResource = new List<int>();
            List<AppointmentData> filteredData = new List<AppointmentData>();
            if (chcCalismaVar) { selectedResource.Add(1); }
            if (chcCalismaYok) { selectedResource.Add(2); }
            if (chcResmiTatil) { selectedResource.Add(3); }
            if (chcTatil) { selectedResource.Add(4); }
            if (chcYarimGun) { selectedResource.Add(5); }
            if (chcYuklemeGunu) { selectedResource.Add(6); }
            if (chcBakim) { selectedResource.Add(7); }

            foreach (int resource in selectedResource)
            {

                List<AppointmentData> data = FinalDataSource.Where(x => ResourceList[resource].Id == x.ResourceId).ToList();
                filteredData = filteredData.Concat(data).ToList();
            }

            DataSourceEvent = filteredData;
            if (switchName == "chcTumu") { chcCalismaVar = false; chcCalismaYok = false; chcResmiTatil = false; chcTatil = false; chcYarimGun = false; chcYuklemeGunu = false; chcBakim = false; } else { chcTumu = false; }

            if (chcTumu) { DataSourceEvent = FinalDataSource; }
        }

        public async Task OnCellClick(CellClickEventArgs args)
        {
            args.Cancel = true;
            selectedDate = args.StartTime;
            ShowStationsModal();

            await Task.CompletedTask;
        }

        public void DayTypeButtonClicked()
        {
            DayTypeModalVisible = true;
        }

        public void HideDayTypeModal()
        {
            DayTypeModalVisible = false;
            DayTypeList.Clear();
        }

        public void HideDayTypeCrudModal()
        {
            DayTypeCrudModalVisible = false;
        }

        public class DayTypeModel
        {
            public DateTime DayDate { get; set; }
            public int DayStatus { get; set; }
        }

        public List<DayTypeModel> DayTypeList = new List<DayTypeModel>();
        public List<ContextMenuItemModel> DayTypeGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        DayTypeModel DayTypeDataSource;

        private SfGrid<DayTypeModel> _DayTypeGrid;

        protected void CreateDayTypeContextMenuItems()
        {
            DayTypeGridContextMenu.Add(new ContextMenuItemModel { Text = L["DayTypeContextAdd"], Id = "new" });
            DayTypeGridContextMenu.Add(new ContextMenuItemModel { Text = L["DayTypeContextChange"], Id = "change" });
            DayTypeGridContextMenu.Add(new ContextMenuItemModel { Text = L["DayTypeContextDelete"], Id = "delete" });
            DayTypeGridContextMenu.Add(new ContextMenuItemModel { Text = L["DayTypeContextRefresh"], Id = "refresh" });

        }

        public async void OnDayTypeContextMenuClick(ContextMenuClickEventArgs<DayTypeModel> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    DayTypeDataSource = new DayTypeModel() { };

                    StartDateDayType = DataSource.SelectCalendarDaysDto.Where(t => t.Date_.Month == 1 && t.Date_.Day == 1).Select(t => t.Date_).FirstOrDefault();
                    EndDateDayType = DataSource.SelectCalendarDaysDto.Where(t => t.Date_.Month == 1 && t.Date_.Day == 1).Select(t => t.Date_).FirstOrDefault();

                    foreach (var item in _dayTypeComboBox)
                    {
                        item.Text = L[item.Text];
                    }

                    DayTypeCrudModalVisible = true;
                    await InvokeAsync(StateHasChanged);

                    break;

                case "change":

                    DayTypeDataSource = args.RowInfo.RowData;
                    StartDateDayType = DayTypeDataSource.DayDate;
                    EndDateDayType = DayTypeDataSource.DayDate;

                    foreach (var item in _dayTypeComboBox)
                    {
                        item.Text = L[item.Text];
                    }
                    DayTypeCrudModalVisible = true;
                    await InvokeAsync(StateHasChanged);


                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageDayTypeBase"]);
                    if (res == true)
                    {

                        DayTypeList.Remove(args.RowInfo.RowData);

                        await _DayTypeGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":

                    await _DayTypeGrid.Refresh();
                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }
        }

        public async void DayTypeOnSubmit()
        {
            foreach (var dayType in DayTypeList)
            {
                var updatedDay = DataSource.SelectCalendarDaysDto.Where(t => t.Date_ == dayType.DayDate).FirstOrDefault();

                if (updatedDay != null && updatedDay.Id != Guid.Empty)
                {
                    updatedDay.CalendarDayStateEnum = dayType.DayStatus;
                    CalendarsService.UpdateDays(updatedDay);
                }
            }

            HideDayTypeModal();

            SchedularDaysList = (await CalendarsService.GetDaysListAsync(DataSource.Id)).Data.ToList();
            DataSourceEvent = ConvertToAppointmentData(SchedularDaysList);
            FinalDataSource = DataSourceEvent;

            await InvokeAsync(StateHasChanged);

        }

        public async void DayTypeCrudOnSubmit()
        {
            if (!isDateSpan)
            {
                DayTypeDataSource.DayDate = StartDateDayType;

                if (!DayTypeList.Contains(DayTypeDataSource))
                {
                    DayTypeList.Add(DayTypeDataSource);
                }
                else
                {
                    int typeIndex = DayTypeList.IndexOf(DayTypeDataSource);
                    DayTypeList[typeIndex] = DayTypeDataSource;
                }
            }
            else
            {
                int status = DayTypeDataSource.DayStatus;

                for (DateTime i = StartDateDayType; i <= EndDateDayType; i = i.AddDays(1))
                {
                    if (!DayTypeList.Any(t => t.DayDate == i))
                    {
                        DayTypeDataSource = new DayTypeModel
                        {
                            DayDate = i,
                            DayStatus = status
                        };

                        DayTypeList.Add(DayTypeDataSource);
                    }
                    else
                    {
                        var dayDataSource = DayTypeList.Where(t=>t.DayDate == i).FirstOrDefault();
                        int typeIndex = DayTypeList.IndexOf(dayDataSource);

                        DayTypeDataSource = new DayTypeModel
                        {
                            DayDate = i,
                            DayStatus = status
                        };

                        DayTypeList[typeIndex] = DayTypeDataSource;
                    }
                }
            }


            HideDayTypeCrudModal();

            await _DayTypeGrid.Refresh();
            await InvokeAsync(StateHasChanged);
        }

        public class DayTypeComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<DayTypeComboBox> _dayTypeComboBox = new List<DayTypeComboBox>
        {
            new DayTypeComboBox(){ID = "1", Text="WorkExist"},
            new DayTypeComboBox(){ID = "2", Text="WorkNotExist"},
            new DayTypeComboBox(){ID = "3", Text="OfficialHoliday"},
            new DayTypeComboBox(){ID = "4", Text="Holiday"},
            new DayTypeComboBox(){ID = "5", Text="HalfDay"},
            new DayTypeComboBox(){ID = "6", Text="LoadingDay"},
            new DayTypeComboBox(){ID = "7", Text="UIMaintenancePreviousMenu"},
        };

        private void DayTypeComboBoxValueChangeHandler(ChangeEventArgs<string, DayTypeComboBox> args)
        {
            if (args.ItemData != null)
            {
                switch (args.ItemData.ID)
                {
                    case "1":
                        DayTypeDataSource.DayStatus = 1;
                        break;
                    case "2":
                        DayTypeDataSource.DayStatus = 2;
                        break;
                    case "3":
                        DayTypeDataSource.DayStatus = 3;
                        break;
                    case "4":
                        DayTypeDataSource.DayStatus = 4;
                        break;
                    case "5":
                        DayTypeDataSource.DayStatus = 5;
                        break;
                    case "6":
                        DayTypeDataSource.DayStatus = 6;
                        break;
                    case "7":
                        DayTypeDataSource.DayStatus = 7;
                        break;


                    default: break;
                }
            }
        }

        #endregion

        #region İş İstasyonları Modalı İşlemleri

        private async void ShowStationsModal()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
            StationGroupList = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            StationWorkStatusList = DataSource.SelectCalendarLinesDto;

            var tempGroupList = StationsList.Select(t => t.GroupID).Distinct().ToList();
            foreach (var groupId in tempGroupList)
            {
                string groupname = StationGroupList.Where(t => t.Id == groupId).Select(t => t.Name).FirstOrDefault();
                StationGroupNameList.Add(groupname);
            }
            var a = StationGroupNameList;

            foreach(var station in DataSource.SelectCalendarLinesDto)
            {
                var selectedStation = StationsList.Where(t => t.Id == station.StationID).FirstOrDefault();
                SelectedStations.Add(selectedStation);
            }

            StationsModalVisible = true;
        }

        private void HideStationsModal()
        {
            StationsModalVisible = false;
            SelectedStations.Clear();
        }

        private async void OnSelectAllStationsChange(string stationGroup)
        {
            var tempselectedList = SelectedStations.Select(t => t.StationGroup).ToList();

            if (!tempselectedList.Contains(stationGroup))
            {
                var addedList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.Where(t => t.StationGroup == stationGroup).ToList();

                foreach (var addedItems in addedList)
                {
                    SelectedStations.Add(addedItems);
                }

            }
            else
            {
                var deletedStationList = SelectedStations.Where(t => t.StationGroup != stationGroup).ToList();
                SelectedStations = deletedStationList;
            }
        }

        private async void OnSelectStation(ListStationsDto station)
        {
            if (SelectedStations.Contains(station))
            {
                await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["UIInformationSelectedStation"]);

            }
            else
            {
                SelectedStations.Add(station);
            }

            await InvokeAsync(() => StateHasChanged());

        }

        private void Remove(ListStationsDto station)
        {
            SelectedStations.Remove(station);

            InvokeAsync(() => StateHasChanged());
        }

        private void OnAddAllStationsChange(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (isAllStationsChecked)
            {
                foreach (var station in StationsList)
                {
                    if (!SelectedStations.Contains(station))
                    {
                        SelectedStations.Add(station);
                    }
                }
            }
            else
            {
                SelectedStations.Clear();
            }
        }

        #endregion

        #region Takvim Satır Modalı İşlemleri

        private async void ShowLineModal()
        {
            selectedDateStr = selectedDate.ToString();

            ShiftsList = (await ShiftsAppService.GetListAsync(new ListShiftsParameterDto())).Data.ToList();

            if (DataSource.SelectCalendarLinesDto.Where(t => t.Date_ == selectedDate).Count() == 0)
            {

                //var lineList = (await CalendarsService.GetLineListAsync(DataSource.Id)).Data.Where(t=>t.Date_ == selectedDay.).ToList();

                List<SelectCalendarLinesDto> templist = new List<SelectCalendarLinesDto>();

                foreach (var shift in ShiftsList)
                {
                    foreach (var station in SelectedStations)
                    {
                        SelectCalendarLinesDto lineRecord = new SelectCalendarLinesDto
                        {
                            CalendarID = DataSource.Id,
                            AvailableTime = shift.TotalWorkTime - shift.TotalBreakTime,
                            PlannedHaltTimes = shift.TotalBreakTime,
                            ShiftName = shift.Name,
                            StationName = station.Name,
                            StationCode = station.Code,
                            ShiftTime = shift.TotalWorkTime,
                            StationID = station.Id,
                            ShiftID = shift.Id,
                            ShiftOverTime = shift.Overtime,
                            ShiftOrder = shift.ShiftOrder,
                            Date_ = selectedDate.GetValueOrDefault(),
                            MaintenanceType = "Bakım 1", /*deneme*/
                            PlannedMaintenanceTime = 3600 /*deneme*/
                        };

                        LineGridList.Add(lineRecord);
                    }
                }

            }

            else
            {

                foreach (var shift in ShiftsList)
                {
                    foreach (var station in SelectedStations)
                    {
                        if (DataSource.SelectCalendarLinesDto.Where(t => t.StationID == station.Id).Count() != 0)
                        {
                            LineGridList.Add(DataSource.SelectCalendarLinesDto.Where(t => t.Date_ == selectedDate && t.StationID == station.Id && t.ShiftID == shift.Id).FirstOrDefault());
                        }
                        else
                        {
                            SelectCalendarLinesDto lineRecord = new SelectCalendarLinesDto
                            {
                                CalendarID = DataSource.Id,
                                AvailableTime = shift.TotalWorkTime - shift.TotalBreakTime,
                                PlannedHaltTimes = shift.TotalBreakTime,
                                ShiftName = shift.Name,
                                StationName = station.Name,
                                StationCode = station.Code,
                                ShiftTime = shift.TotalWorkTime,
                                StationID = station.Id,
                                ShiftID = shift.Id,
                                ShiftOverTime = shift.Overtime,
                                ShiftOrder = shift.ShiftOrder,
                                Date_ = selectedDate.GetValueOrDefault(),
                                MaintenanceType = "Bakım 1", /*deneme*/
                                PlannedMaintenanceTime = 3600 /*deneme*/
                            };

                            LineGridList.Add(lineRecord);
                        }

                    }
                }
            }

            LineModalVisible = true;
        }

        private void ShowLineWorkStatusModal()
        {
            lineWorkStatusVisible = true;
        }

        private void HideLineModal()
        {
            SelectedStations.Clear();
            LineGridList.Clear();
            LineModalVisible = false;
        }

        private void HideLineEditModal()
        {
            lineWorkStatusVisible = false;
        }

        public void ShowLineOverTimeModal()
        {
            overTimeModalVisible = true;
        }
        public void HideLineOverTimeModal()
        {
            overTimeModalVisible = false;
        }

        private async void LineModalSave()
        {
            DataSource.SelectCalendarLinesDto = LineGridList;

            var updatedEntity = ObjectMapper.Map<SelectCalendarsDto, UpdateCalendarsDto>(DataSource);

            var updatedResult = (await CalendarsService.UpdateWithoutDaysAsync(updatedEntity));

            HideLineModal();

            SchedularDaysList = (await CalendarsService.GetDaysListAsync(DataSource.Id)).Data.ToList();
            DataSourceEvent = ConvertToAppointmentData(SchedularDaysList);
            FinalDataSource = DataSourceEvent;


            await InvokeAsync(StateHasChanged);
        }

        private async void LineEditModalSave()
        {
            int selectedLineIndex = DataSource.SelectCalendarLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

            LineDataSource.WorkStatus = tempworkStatus;

            if (selectedLineIndex > -1)
            {
                DataSource.SelectCalendarLinesDto[selectedLineIndex] = LineDataSource;
            }

            await _LineGrid.Refresh();

            HideLineEditModal();
            await InvokeAsync(StateHasChanged);
        }

        private async void LineEditOverTimeModalSave()
        {
            int selectedLineIndex = DataSource.SelectCalendarLinesDto.FindIndex(t => t.Id == LineDataSource.Id);

            if (selectedLineIndex > -1)
            {
                DataSource.SelectCalendarLinesDto[selectedLineIndex] = LineDataSource;
            }

            await _LineGrid.Refresh();

            HideLineOverTimeModal();
            await InvokeAsync(StateHasChanged);
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                    if (permission)
                    {

                        switch (context.MenuName)
                        {
                            case "CalendarLineContextWorkStatus":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarLineContextWorkStatus"], Id = "workstatus" }); break;
                            case "CalendarLineContextOverTime":
                                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarLineContextOverTime"], Id = "overtime" }); break;
                            default: break;
                        }
                    }

                }
            }
        }

        protected void CreateLineMaintenaceInfosContextMenuItems()
        {
            if (MaintenanceGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MaintenaceInfosContextChange":
                                MaintenanceGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextChange"], Id = "change" }); break;
                            //case "MaintenaceInfosContextDelete":
                            //    MaintenanceGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextDelete"], Id = "delete" }); break;
                            //case "MaintenaceInfosContextRefresh":
                            //    MaintenanceGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected void CreateLineMaintenaceInfosLineContextMenuItems()
        {
            if (MaintenanceLineGridContextMenu.Count() == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "MaintenaceInfosContextChange":
                                MaintenanceLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextChange"], Id = "changed" }); break;
                            case "MaintenaceInfosContextDelete":
                                MaintenanceLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextDelete"], Id = "delete" }); break;
                            case "MaintenaceInfosContextRefresh":
                                MaintenanceLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["MaintenaceInfosContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public async void OnLineMaintenaceInfosContextMenuClick(ContextMenuClickEventArgs<ListPlannedMaintenancesDto> args)
        {
            switch (args.Item.Id)
            {
                case "change":

                    MaintenanceDataSource = (await PlannedMaintenancesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    PlannedMaintenanceLineList = MaintenanceDataSource.SelectPlannedMaintenanceLines;

                    #region ShowModal Metot
                    foreach (var item in status)
                    {
                        item.StatusName = L[item.StatusName];
                    }

                    if (MaintenanceDataSource != null)
                    {

                        if (MaintenanceDataSource.DataOpenStatus == true && MaintenanceDataSource.DataOpenStatus != null)
                        {
                            MaintenanceCrudModalVisible = false;
                            await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                            await InvokeAsync(StateHasChanged);
                        }
                        else
                        {
                            MaintenanceCrudModalVisible = true;
                            await InvokeAsync(StateHasChanged);
                        }
                    }
                    #endregion
                    await InvokeAsync(StateHasChanged);

                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageMaintenanceBase"]);
                    if (res == true)
                    {
                        await PlannedMaintenancesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                        foreach (var line in LineGridList)
                        {
                            var maintenance = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.PlannedDate == line.Date_ && t.StationID == line.StationID).FirstOrDefault();

                            PlannedMaintenanceList.Add(maintenance);
                        }

                        MaintenanceCrudModalVisible = false;
                        await _MaintenanceGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    foreach (var line in LineGridList)
                    {
                        var maintenance = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.PlannedDate == line.Date_ && t.StationID == line.StationID).FirstOrDefault();

                        PlannedMaintenanceList.Add(maintenance);
                    }
                    await _MaintenanceGrid.Refresh();
                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }
        }

        public void OnLineContextMenuClick(ContextMenuClickEventArgs<SelectCalendarLinesDto> args)
        {
            switch (args.Item.Id)
            {

                case "workstatus":
                    LineDataSource = args.RowInfo.RowData;

                    ShowLineWorkStatusModal();

                    break;

                case "overtime":
                    LineDataSource = args.RowInfo.RowData;

                    ShowLineOverTimeModal();

                    break;

                default:
                    break;
            }
        }

       

        public async void OnListMaintenaceInfosContextMenuClick(ContextMenuClickEventArgs<SelectPlannedMaintenanceLinesDto> args)
        {
            switch (args.Item.Id)
            {

                case "changed":
                    MaintenanceLineDataSource = args.RowInfo.RowData;
                    MaintenanceLineCrudModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            MaintenanceDataSource.SelectPlannedMaintenanceLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await PlannedMaintenancesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                                MaintenanceDataSource.SelectPlannedMaintenanceLines.Remove(line);
                            }
                            else
                            {
                                MaintenanceDataSource.SelectPlannedMaintenanceLines.Remove(line);
                            }
                        }

                        await _MaintenanceLineGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await _MaintenanceLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        private void WorkStatesValueChangeHandler(ChangeEventArgs<string, WorkStates> args)
        {
            int statusIndex = DataSource.SelectCalendarDaysDto.FindIndex(t => t.Date_ == selectedDate);
            switch (args.ItemData.ID)
            {
                case "State1":
                    DataSource.SelectCalendarDaysDto[statusIndex].CalendarDayStateEnum = 1;
                    foreach (var line in LineGridList)
                    {
                        line.WorkStatus = 1;
                    }
                    break;

                case "State2":
                    DataSource.SelectCalendarDaysDto[statusIndex].CalendarDayStateEnum = 2;
                    foreach (var line in LineGridList)
                    {
                        line.WorkStatus = 2;
                    }
                    break;

                case "State3":
                    DataSource.SelectCalendarDaysDto[statusIndex].CalendarDayStateEnum = 1;
                    foreach (var line in LineGridList)
                    {
                        line.WorkStatus = 3;
                    }
                    break;

                case "State4":
                    DataSource.SelectCalendarDaysDto[statusIndex].CalendarDayStateEnum = 1;

                    foreach (var line in LineGridList)
                    {
                        line.WorkStatus = 4;
                    }
                    break;

                case "State5":
                    DataSource.SelectCalendarDaysDto[statusIndex].CalendarDayStateEnum = 1;

                    foreach (var line in LineGridList)
                    {
                        line.WorkStatus = 5;
                    }
                    break;

            }


            _LineGrid.Refresh();
        }

        private void LineEditModalWorkStatesValueChangeHandler(ChangeEventArgs<string, WorkStates> args)
        {
            switch (args.ItemData.ID)
            {
                case "State1":

                    tempworkStatus = 1;
                    break;

                case "State2":

                    tempworkStatus = 2;
                    break;

                case "State3":

                    tempworkStatus = 3;
                    break;

                case "State4":

                    tempworkStatus = 4;
                    break;

                case "State5":

                    tempworkStatus = 5;
                    break;

            }
        }

        public async void ShowMaintenanceInformationsButtonClicked()
        {
            foreach (var line in LineGridList)
            {
                var maintenance = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.PlannedDate == line.Date_ && t.StationID == line.StationID).FirstOrDefault();

                if (maintenance != null && !PlannedMaintenanceList.Contains(maintenance))
                {
                    PlannedMaintenanceList.Add(maintenance);
                }
            }

            MaintenanceModalVisible = true;
        }

        public void HideMaintenanceModal()
        {
            MaintenanceModalVisible = false;
        }

        public void HideMaintenanceCrudModal()
        {
            MaintenanceCrudModalVisible = false;
        }

        public void HideMaintenanceLineModal()
        {
            MaintenanceLineCrudModalVisible = false;
        }

        public async Task OnMaintenanceLineSubmit()
        {
            if (MaintenanceLineDataSource.Amount == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase"]);
            }
            else
            {
                if (MaintenanceLineDataSource.Id == Guid.Empty)
                {
                    if (MaintenanceDataSource.SelectPlannedMaintenanceLines.Contains(MaintenanceLineDataSource))
                    {
                        int selectedLineIndex = MaintenanceDataSource.SelectPlannedMaintenanceLines.FindIndex(t => t.LineNr == MaintenanceLineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            MaintenanceDataSource.SelectPlannedMaintenanceLines[selectedLineIndex] = MaintenanceLineDataSource;
                        }
                    }
                    else
                    {
                        MaintenanceDataSource.SelectPlannedMaintenanceLines.Add(MaintenanceLineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = MaintenanceDataSource.SelectPlannedMaintenanceLines.FindIndex(t => t.Id == MaintenanceLineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        MaintenanceDataSource.SelectPlannedMaintenanceLines[selectedLineIndex] = MaintenanceLineDataSource;
                    }
                }

                PlannedMaintenanceLineList = MaintenanceDataSource.SelectPlannedMaintenanceLines;
                GetTotal();
                await _MaintenanceLineGrid.Refresh();

                HideMaintenanceLineModal();
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void OnMaintenanceSubmit()
        {
            var updateInput = ObjectMapper.Map<SelectPlannedMaintenancesDto, UpdatePlannedMaintenancesDto>(MaintenanceDataSource);

            await PlannedMaintenancesAppService.UpdateAsync(updateInput);

            PlannedMaintenanceList.Clear();

            HideMaintenanceCrudModal();

            foreach (var line in LineGridList)
            {
                var maintenance = (await PlannedMaintenancesAppService.GetListAsync(new ListPlannedMaintenancesParameterDto())).Data.Where(t => t.PlannedDate == line.Date_ && t.StationID == line.StationID).FirstOrDefault();

                if (maintenance != null && !PlannedMaintenanceList.Contains(maintenance))
                {
                    PlannedMaintenanceList.Add(maintenance);
                }
            }

            await _MaintenanceGrid.Refresh();

        }

        #endregion

        #region Public Class'lar

        public List<AppointmentData> DataSourceEvent = new List<AppointmentData> { };
        public List<AppointmentData> FinalDataSource = new List<AppointmentData>();
        public List<ResourceData> ResourceList { get; set; } = new List<ResourceData> {
        new ResourceData{ Text = "Çalışma Var", Id= 1, Color = CalendarColors.WorkExists },
        new ResourceData{ Text = "Çalışma Yok", Id= 2, Color = CalendarColors.WorkNotExists },
        new ResourceData{ Text = "Resmi Tatil", Id= 3, Color = CalendarColors.OfficialHoliday },
        new ResourceData{ Text = "Tatil", Id= 4, Color = CalendarColors.Holiday },
        new ResourceData{ Text = "Yarım Gün", Id= 5, Color = CalendarColors.HalfDay },
        new ResourceData{ Text = "Yükleme Günü", Id= 6, Color = CalendarColors.LoadingDay },
        new ResourceData{ Text = "Bakım", Id= 7, Color = CalendarColors.Maintenance } };

        public class ResourceData
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public string Color { get; set; }
        }

        public class AppointmentData
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Location { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Description { get; set; }
            public bool IsAllDay { get; set; }
            public string RecurrenceRule { get; set; }
            public string RecurrenceException { get; set; }
            public Nullable<int> RecurrenceID { get; set; }
            public string StartTimezone { get; set; }
            public string EndTimezone { get; set; }
            public int ResourceId { get; set; }
        }

        public class WorkStates
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<WorkStates> WorkStatesData = new List<WorkStates> {
         new WorkStates() { ID= "State1", Text= "Çalışma Var" },
         new WorkStates() { ID= "State2", Text= "Çalışma Yok" },
         new WorkStates() { ID= "State3", Text= "Üretim Kısıtı" },
         new WorkStates() { ID= "State4", Text= "Operatör Kısıtı" },
         new WorkStates() { ID= "State5", Text= "Arızi Kısıt" },
  };

        #endregion

        #region Combobox İşlemleri

        public IEnumerable<SelectPlannedMaintenancesDto> status = GetEnumDisplayStatusNames<PlannedMaintenanceStateEnum>();

        public static List<SelectPlannedMaintenancesDto> GetEnumDisplayStatusNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<PlannedMaintenanceStateEnum>()
                       .Select(x => new SelectPlannedMaintenancesDto
                       {
                           Status = x,
                           StatusName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;
        SfTextBox CodeMaintenanceButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async Task CodeMaintenanceOnCreateIcon()
        {
            var CodeMaintenanceButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeMaintenanceButtonClickEvent);
            await CodeMaintenanceButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodeMaintenanceButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CalendarChildMenu");
            await InvokeAsync(StateHasChanged);
        }

        public async void CodeMaintenanceButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PlannedMainChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Bakım Periyodu ButtonEdit 

        SfTextBox MaintenancePeriodsButtonEdit;
        //bool SelectMaintenancePeriodsPopupVisible = new();
        List<ListMaintenancePeriodsDto> MaintenancePeriodsList = new List<ListMaintenancePeriodsDto>();
        public async Task MaintenancePeriodsOnCreateIcon()
        {
            var MaintenancePeriodsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, MaintenancePeriodsButtonClickEvent);
            await MaintenancePeriodsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", MaintenancePeriodsButtonClick } });
        }

        public async void MaintenancePeriodsButtonClickEvent()
        {
            //SelectMaintenancePeriodsPopupVisible = true;
            await GetMaintenancePeriodsList();
            await InvokeAsync(StateHasChanged);
        }

        public void MaintenancePeriodsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                MaintenanceDataSource.PeriodID = Guid.Empty;
                MaintenanceDataSource.PeriodName = string.Empty;
            }
        }

        public async void MaintenancePeriodsDoubleClickHandler(RecordDoubleClickEventArgs<ListMaintenancePeriodsDto> args)
        {
            var selectedMaintenancePeriod = args.RowData;

            if (selectedMaintenancePeriod != null)
            {
                MaintenanceDataSource.PeriodID = selectedMaintenancePeriod.Id;
                MaintenanceDataSource.PeriodName = selectedMaintenancePeriod.Name;
                MaintenanceDataSource.PeriodTime = selectedMaintenancePeriod.PeriodTime;
                //SelectMaintenancePeriodsPopupVisible = false;
                await InvokeAsync(StateHasChanged);

                if (MaintenanceDataSource.PeriodID != null && MaintenanceDataSource.StationID != null)
                {
                    var instructionMaintenanceDataSource = (await MaintenanceInstructionsAppService.GetbyPeriodStationAsync(MaintenanceDataSource.StationID, MaintenanceDataSource.PeriodID)).Data;
                    var instructionGridLineList = instructionMaintenanceDataSource.SelectMaintenanceInstructionLines;

                    await GetProductsList();
                    await GetUnitSetsList();

                    foreach (var instructionline in instructionGridLineList)
                    {
                        SelectPlannedMaintenanceLinesDto plannedMaintenanceLine = new SelectPlannedMaintenanceLinesDto
                        {
                            Amount = instructionline.Amount,
                            InstructionDescription = instructionline.InstructionDescription,
                            LineNr = instructionline.LineNr,
                            PlannedMaintenanceID = MaintenanceDataSource.Id,
                            ProductCode = ProductsList.Where(t => t.Id == instructionline.ProductID).Select(t => t.Code).FirstOrDefault(),
                            ProductName = ProductsList.Where(t => t.Id == instructionline.ProductID).Select(t => t.Name).FirstOrDefault(),
                            ProductID = instructionline.ProductID,
                            UnitSetCode = UnitSetsList.Where(t => t.Id == instructionline.UnitSetID).Select(t => t.Code).FirstOrDefault(),
                            UnitSetID = instructionline.UnitSetID
                        };

                        PlannedMaintenanceLineList.Add(plannedMaintenanceLine);
                        await _MaintenanceLineGrid.Refresh();

                    }

                }
            }
        }

        #endregion

        #region GetList Metotları

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        private async Task GetStationsList()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        private async Task GetMaintenancePeriodsList()
        {
            MaintenancePeriodsList = (await MaintenancePeriodsAppService.GetListAsync(new ListMaintenancePeriodsParameterDto())).Data.ToList();
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
