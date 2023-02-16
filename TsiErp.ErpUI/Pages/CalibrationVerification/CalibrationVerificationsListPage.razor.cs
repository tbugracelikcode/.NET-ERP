using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationVerification
{
    public partial class CalibrationVerificationsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationVerificationsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalibrationVerificationsDto()
            {
                Date = DateTime.Today,
                NextControl = DateTime.Today
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #region Ekipman ButtonEdit

        SfTextBox EquipmentRecordButtonEdit;
        bool SelectEquipmentRecordPopupVisible = false;
        List<ListEquipmentRecordsDto> EquipmentRecordList = new List<ListEquipmentRecordsDto>();

        public async Task EquipmentRecordOnCreateIcon()
        {
            var EquipmentRecordButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EquipmentRecordButtonClickEvent);
            await EquipmentRecordButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EquipmentRecordButtonClick } });
        }

        public async void EquipmentRecordButtonClickEvent()
        {
            SelectEquipmentRecordPopupVisible = true;
            EquipmentRecordList = (await EquipmentRecordsService.GetListAsync(new ListEquipmentRecordsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EquipmentRecordOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EquipmentID = Guid.Empty;
                DataSource.Equipment = string.Empty;
            }
        }

        public async void EquipmentRecordDoubleClickHandler(RecordDoubleClickEventArgs<ListEquipmentRecordsDto> args)
        {
            var selectedEquipmentRecord = args.RowData;

            if (selectedEquipmentRecord != null)
            {
                DataSource.EquipmentID = selectedEquipmentRecord.Id;
                DataSource.Equipment = selectedEquipmentRecord.Name;
                SelectEquipmentRecordPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        private async Task GetEquipmentRecordsList()
        {
            EquipmentRecordList = (await EquipmentRecordsService.GetListAsync(new ListEquipmentRecordsParameterDto())).Data.ToList();
        }

    
    }
}
