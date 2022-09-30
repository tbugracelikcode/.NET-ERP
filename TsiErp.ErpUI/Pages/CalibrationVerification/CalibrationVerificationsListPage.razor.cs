using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationVerification
{
    public partial class CalibrationVerificationsListPage
    {
        SfComboBox<string, ListEquipmentRecordsDto> EquipmentRecordsComboBox;

        List<ListEquipmentRecordsDto> EquipmentRecordsList = new List<ListEquipmentRecordsDto>();

        private SfGrid<ListCalibrationVerificationsDto> _grid;

        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationVerificationsService;
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

        public async Task EquipmentGroupOpened(PopupEventArgs args)
        {
            if (EquipmentRecordsList.Count == 0)
            {
                await GetEquipmentRecordsList();
            }
        }

        private void EquipmentValueChanged(ChangeEventArgs<string, ListEquipmentRecordsDto> args)
        {
            DataSource.EquipmentID = args.ItemData.Id;
            DataSource.Equipment = args.ItemData.Name;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }
    }
}
