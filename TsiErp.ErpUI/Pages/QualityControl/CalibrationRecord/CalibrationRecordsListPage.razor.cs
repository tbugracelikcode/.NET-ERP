using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.CalibrationRecord
{
    public partial class CalibrationRecordsListPage
    {

        protected override async void OnInitialized()
        {
            _L = L;
            BaseCrudService = CalibrationRecordsService;

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



        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalibrationRecordsDto()
            {
                Date_ = DateTime.Today,
                NextControl = DateTime.Today
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
