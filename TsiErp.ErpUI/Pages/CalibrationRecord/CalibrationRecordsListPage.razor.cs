using DevExpress.Blazor;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationRecord
{
    public partial class CalibrationRecordsListPage
    {
        List<ListCalibrationRecordsDto> Gridlist = new List<ListCalibrationRecordsDto>();

        bool PopupVisible = false;
        DateTime tarih = DateTime.Today;

        protected override async void OnInitialized()
        {
            //Gridlist = (await CalibrationRecordsService.GetListAsync(new ListCalibrationRecordsParameterDto() { IsActive = true })).Data.ToList();
        }

        void CalibrationRecordsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
