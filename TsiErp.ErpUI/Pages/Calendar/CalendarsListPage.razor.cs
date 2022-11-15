using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Schedule;
using System;
using System.ComponentModel.DataAnnotations;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.SalesOrder.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.TemplateOperation.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Calendar
{
    public partial class CalendarsListPage
    {
        public bool schedularVisible { get; set; } = false;
        public DateTime officialHoliday = DateTime.Today;
        public List<int> YearList = new List<int>();
        public SfGrid<SelectCalendarDaysDto> _daysGrid;
        public List<SelectCalendarDaysDto> GridDaysList = new List<SelectCalendarDaysDto>();
        public List<SelectCalendarDaysDto> SchedularDaysList = new List<SelectCalendarDaysDto>();
        SfComboBox<int, int> Years;
        public DateTime CurrentDate = DateTime.Today;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        [Inject]
        ModalManager ModalManager { get; set; }
        public void OnPopupOpen(PopupOpenEventArgs<AppointmentData> args)
        {
            if (args.Type == PopupType.Editor || args.Type == PopupType.QuickInfo)
            {
                args.Cancel = true;
            }
        }
        protected override async void OnInitialized()
        {
            BaseCrudService = CalendarsService;
            GetYearsList();
            CreateLineContextMenuItems();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalendarsDto(){};
            ShowEditPage();

            return Task.CompletedTask;
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
                    CalendarDayStateEnum = CalendarDayStateEnum.ResmiTatil
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
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "changed":
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    EditPageVisible = true;
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


    }
}
