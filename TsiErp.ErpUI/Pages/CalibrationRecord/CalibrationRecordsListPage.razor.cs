using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationRecord
{
    public partial class CalibrationRecordsListPage
    {
        SfComboBox<string, ListEquipmentRecordsDto> EquipmentRecordsComboBox;
        List<ListEquipmentRecordsDto> EquipmentRecordsList = new List<ListEquipmentRecordsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationRecordsService;
            await GetEquipmentRecordsList();
        }

        public async Task EquipmentFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await EquipmentRecordsComboBox.FilterAsync(EquipmentRecordsList, query);
        }

        private async Task GetEquipmentRecordsList()
        {
            EquipmentRecordsList = (await EquipmentRecordsService.GetListAsync(new ListEquipmentRecordsParameterDto())).Data.ToList();
        }

        public async Task EquipmentRecordValueChangeHandler(ChangeEventArgs<string, ListEquipmentRecordsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.EquipmentID = args.ItemData.Id;
                DataSource.Equipment = args.ItemData.Name;
            }
            else
            {
                DataSource.EquipmentID = Guid.Empty;
                DataSource.Equipment = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalibrationRecordsDto()
            {
                Date = DateTime.Today,
                NextControl = DateTime.Today
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
