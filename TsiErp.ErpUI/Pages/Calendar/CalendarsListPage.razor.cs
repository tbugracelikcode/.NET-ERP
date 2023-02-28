using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Schedule;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Calendar
{
    public partial class CalendarsListPage
    {
        SfSchedule<AppointmentData> ScheduleObj;
        private CellClickEventArgs CellData { get; set; }
        public bool schedularVisible { get; set; } = false;
        public List<int> YearList = new List<int>();
        public SfGrid<SelectCalendarDaysDto> _daysGrid;
        public List<SelectCalendarDaysDto> GridDaysList = new List<SelectCalendarDaysDto>();
        public List<SelectCalendarDaysDto> SchedularDaysList = new List<SelectCalendarDaysDto>();
        public List<ListStationsDto> StationsList = new List<ListStationsDto>();
        public List<ListStationsDto> SelectedStations = new List<ListStationsDto>();
        public List<ListShiftsDto> ShiftsList = new List<ListShiftsDto>();
        SfComboBox<int, int> Years;
        
        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> StationShiftGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        public List<StationShiftInfos> StationShiftList = new List<StationShiftInfos>();
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
        private bool modal1visible = false;
        private bool modal2visible = false;
        string imageURL = "images/Stations/";
        string cardbgcolor = "white";
        public DateTime CurrentDate = DateTime.Today;
        public DateTime officialHoliday = DateTime.Today;

        #endregion

        public List<AppointmentData> DataSourceEvent = new List<AppointmentData> { };
        public List<AppointmentData> FinalDataSource = new List<AppointmentData>();
        public List<ResourceData> ResourceList { get; set; } = new List<ResourceData> {
        new ResourceData{ Text = "Çalışma Var", Id= 1, Color = "#00ff14" },
        new ResourceData{ Text = "Çalışma Yok", Id= 2, Color = "#ff0000" },
        new ResourceData{ Text = "Resmi Tatil", Id= 3, Color = "#6c757d" },
        new ResourceData{ Text = "Tatil", Id= 4, Color = "#b75050ed" },
        new ResourceData{ Text = "Yarım Gün", Id= 5, Color = "#0089ff" },
        new ResourceData{ Text = "Yükleme Günü", Id= 6, Color = "#ffeb00" },
        new ResourceData{ Text = "Bakım", Id= 7, Color = "#ff8d00" }
    };

        protected override void OnInitialized()
        {
            BaseCrudService = CalendarsService;
            GetYearsList();
            CreateMainContextMenuItems();
            FinalDataSource = DataSourceEvent;
            base.OnInitialized();
        }

        protected override Task BeforeInsertAsync()
        {
            GridDaysList.Clear();
            DataSource = new SelectCalendarsDto() { };
            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public void OnPopupOpen(PopupOpenEventArgs<AppointmentData> args)
        {
            if (args.Type == PopupType.Editor)
            {
                args.Cancel = true;
            }
        }

        public void customChange(string switchName)
        {
            DataSourceEvent = FinalDataSource;
            List<int> selectedResource = new List<int>();
            List<AppointmentData> filteredData = new List<AppointmentData>();
            if (chcCalismaVar) { selectedResource.Add(0); }
            if (chcCalismaYok) { selectedResource.Add(1); }
            if (chcResmiTatil) { selectedResource.Add(2); }
            if (chcTatil) { selectedResource.Add(3); }
            if (chcYarimGun) { selectedResource.Add(4); }
            if (chcYuklemeGunu) { selectedResource.Add(5); }
            if (chcBakim) { selectedResource.Add(6); }
            foreach (int resource in selectedResource)
            {
                List<AppointmentData> data = FinalDataSource.Where(x => ResourceList[resource].Id == x.ResourceId).ToList();
                filteredData = filteredData.Concat(data).ToList();
            }
            DataSourceEvent = filteredData;
            if (switchName == "chcTumu") { chcCalismaVar = false; chcCalismaYok = false; chcResmiTatil = false; chcTatil = false; chcYarimGun = false; chcYuklemeGunu = false; chcBakim = false; } else { chcTumu = false; }

            if (chcTumu) { DataSourceEvent = FinalDataSource; }
        }

        private void GetYearsList()
        {
            YearList = Enumerable.Range(DateTime.Today.Year - 35, 65).ToList();
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
                    CalendarDayStateEnum = 7
                });
                await _daysGrid.Refresh();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await ModalManager.WarningPopupAsync("Dikkat", "Seçtiğiniz tarih daha önce eklenmiştir.");
            }
        }

        protected void CreateMainContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "new" });
                MainGridContextMenu.Add(new ContextMenuItemModel
                {
                    Text = "Takvimi Görünt" +
                    "üle",
                    Id = "schedular"
                });
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

                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz ?");


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

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
            }
        }

        public async void OnListContextMenuClick(ContextMenuClickEventArgs<SelectCalendarDaysDto> args)
        {
            switch (args.Item.Id)
            {
                case "delete":

                    var res = await ModalManager.ConfirmationAsync("Dikkat", "Seçtiğiniz satır kalıcı olarak silinecektir.");

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


        #region İş İstasyonları Modalı İşlemleri

        private async void ShowModal1()
        {
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
            modal1visible = true;
        }

        private void HideModal1()
        {
            modal1visible = false;
            SelectedStations.Clear();
        }

        private async void OnSelectStation(ListStationsDto station)
        {
            if (SelectedStations.Contains(station))
            {
                SelectedStations.Remove(station);
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

        #endregion

        #region Vardiya Bilgileri Modalı İşlemleri

        private void ShowModal2()
        {
           
            //ShiftsList = (await ShiftsAppService.GetListAsync(new ListShiftsParameterDto())).Data.ToList();

            //var lineList = (await CalendarsService.GetLineListAsync(DataSource.Id)).Data.Where(t=>t.Date_ == selectedDay.).ToList();

            //foreach (var station in SelectedStations)
            //{
            //    lineList.Where(t=>t.)

            //    foreach (var line in lineList)
            //    {
            //        StationShiftInfos stationShiftModel = new StationShiftInfos
            //        {
            //            Station = station.Code,
            //            Overtime = line.ShiftOverTime,
            //            BreakTime = line.PlannedHaltTimes,
            //            ShiftOrder = line.ShiftOrder,
            //            TotalWorkTime = line.AvailableTime
            //        };

            //        StationShiftList.Add(stationShiftModel);
            //    }

            //}

            modal2visible = true;
        }

        private void HideModal2()
        {
            modal2visible = false;
        }

        private void Modal2Save()
        {
            HideModal2();
        }

        protected void CreateStationShiftContextMenuItems()
        {
            if (StationShiftGridContextMenu.Count() == 0)
            {
                StationShiftGridContextMenu.Add(new ContextMenuItemModel { Text = "Bakım Bilgileri", Id = "maintenance" });
            }
        }

        public void OnStationShiftContextMenuClick(ContextMenuClickEventArgs<StationShiftInfos> args)
        {
            switch (args.Item.Id)
            {
                case "maintenance":


                    break;

                default:
                    break;
            }
        }

        #endregion


        public async Task OnItemSelected(MenuEventArgs<MenuItem> args)
        {
            var SelectedMenuItem = args.Item.Id;

            switch (SelectedMenuItem)
            {
                case "Edit":
                    ShowModal1();
                    break;
                default:
                    break;
            }
        }

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

        public class StationShiftInfos
        {
            public decimal TotalWorkTime { get; set; }

            public decimal BreakTime { get; set; }

            public decimal Overtime { get; set; }

            public string Station { get; set; }

            public int ShiftOrder { get; set; }
        }

    }
}
