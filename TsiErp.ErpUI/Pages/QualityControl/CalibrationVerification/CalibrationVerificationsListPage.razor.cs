using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.CalibrationVerification
{
    public partial class CalibrationVerificationsListPage : IDisposable
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = CalibrationVerificationsService;
            _L = L;

        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCalibrationVerificationsDto()
            {
                Date_ = DateTime.Today,
                NextControl = DateTime.Today,
                Code = FicheNumbersAppService.GetFicheNumberAsync("CalVerificationsChildMenu")
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

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationVerificationContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationVerificationContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationVerificationContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CalibrationVerificationContextRefresh"], Id = "refresh" });
        }

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CalVerificationsChildMenu");
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
