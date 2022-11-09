using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using System;
using System.ComponentModel.DataAnnotations;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.TemplateOperation.Services;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.Calendar
{
    public partial class CalendarsListPage
    {
        public DateTime officialHoliday;
        public List<int> YearList = new List<int>();
        public SfGrid<SelectCalendarDaysDto> _daysGrid;
        List<SelectCalendarDaysDto> GridDaysList = new List<SelectCalendarDaysDto>();
        SfComboBox<int, int> Years;

        public List<ContextMenuItemModel> LineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        [Inject]
        ModalManager ModalManager { get; set; }
        protected override async void OnInitialized()
        {
            BaseCrudService = CalendarsService;
            GetYearsList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalendarsDto() { };

            ShowEditPage();

            return Task.CompletedTask;
        }

        private void GetYearsList()
        {
            YearList = Enumerable.Range(1999, 65).ToList();
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
            return base.CreateAsync(input);
        }

        public async Task AddOfficialHoliday()
        {
            GridDaysList.Add(new SelectCalendarDaysDto()
            {
                Date_ = officialHoliday,
                IsOfficialHoliday = true
            });
            await InvokeAsync(StateHasChanged);
        }

        protected void CreateLineContextMenuItems()
        {
            if (LineGridContextMenu.Count() == 0)
            {
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                LineGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
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

                case "refresh":

                    await _daysGrid.Refresh();
                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }
        }

    }
}
