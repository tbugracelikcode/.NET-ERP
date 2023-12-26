using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Schedule;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.CalendarColorConstant;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PlanningManagement.Calendar
{
    public partial class CalendarsListPage
    {
        SfSchedule<AppointmentData> ScheduleObj;

        SelectCalendarLinesDto LineDataSource;
        private CellClickEventArgs CellData { get; set; }
        public bool schedularVisible { get; set; } = false;
        public List<int> YearList = new List<int>();
        public List<string> StationGroupNameList = new List<string>();
        public List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();
        public SfGrid<SelectCalendarDaysDto> _daysGrid;
        private SfGrid<SelectCalendarLinesDto> _LineGrid;
        public List<SelectCalendarDaysDto> GridDaysList = new List<SelectCalendarDaysDto>();
        public List<SelectCalendarDaysDto> SchedularDaysList = new List<SelectCalendarDaysDto>();
        public List<ListStationsDto> StationsList = new List<ListStationsDto>();
        public List<ListStationsDto> SelectedStations = new List<ListStationsDto>();
        public List<ListShiftsDto> ShiftsList = new List<ListShiftsDto>();
        SfComboBox<int, int> Years;

        public List<ContextMenuItemModel> DayGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<SelectCalendarLinesDto> LineGridList = new List<SelectCalendarLinesDto>();
        public List<SelectCalendarLinesDto> StationWorkStatusList = new List<SelectCalendarLinesDto>();
        [Inject]
        ModalManager ModalManager { get; set; }

        #region Değişkenler

        private bool isCell;
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
        string cardbgcolor = "white";
        public DateTime officialHoliday = DateTime.Today;
        public DateTime CurrentDate = DateTime.Today;
        int selection = 0;
        public DateTime? selectedDate = null;
        public string selectedDateStr = string.Empty;
        public bool isAllStationsChecked = false;
        public string workStatus = string.Empty;
        public bool lineWorkStatusVisible = false;
        public int tempworkStatus = 0;

        #endregion

        protected override async void OnInitialized()
        {
            BaseCrudService = CalendarsService;
            _L = L;
            GetYearsList();
            CreateMainContextMenuItems();
            CreateLineContextMenuItems();
            CreateDayContextMenuItems();
            FinalDataSource = DataSourceEvent;
            base.OnInitialized();
                                                                       
        }

        #region Çalışma Takvimi Ekleme Modalı Metotları

        private void GetYearsList()
        {
            YearList = Enumerable.Range(DateTime.Today.Year - 2, 3).ToList();
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
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextAdd"], Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextShowCalendar"], Id = "schedular" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextChange"], Id = "changed" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextDelete"], Id = "delete" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarContextRefresh"], Id = "refresh" });
            }
        }

        public async override void ShowEditPage()
        {

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
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
                DayGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarDayContextDelete"], Id = "delete" });
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
                Code = FicheNumbersAppService.GetFicheNumberAsync("CalendarMenu")
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

        private async void LineModalSave()
        {
            DataSource.SelectCalendarLinesDto = LineGridList;


            var updatedEntity = ObjectMapper.Map<SelectCalendarsDto, UpdateCalendarsDto>(DataSource);

            var updatedResult = (await CalendarsService.UpdateAsync(updatedEntity));

            HideLineModal();

            SchedularDaysList = (await CalendarsService.GetDaysListAsync(DataSource.Id)).Data.ToList();

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

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarLineContextMaintenance"], Id = "maintenance" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = L["CalendarLineContextWorkStatus"], Id = "workstatus" });
            }
        }

        public void OnLineContextMenuClick(ContextMenuClickEventArgs<SelectCalendarLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "maintenance":


                    break;

                case "workstatus":
                    LineDataSource = args.RowInfo.RowData;

                    ShowLineWorkStatusModal();

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



        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CalendarMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion



    }
}
