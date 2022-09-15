using DevExpress.Blazor;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;

namespace TsiErp.ErpUI.Pages.CalibrationVerification
{
    public partial class CalibrationVerificationsListPage
    {
        List<ListCalibrationVerificationsDto> Gridlist = new List<ListCalibrationVerificationsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;
        DateTime tarih = DateTime.Today;

        protected override async void OnInitialized()
        {
           Gridlist = (await CalibrationVerificationsService.GetListAsync(new ListCalibrationVerificationsParameterDto() )).Data.ToList();
        }

        void CalibrationVerificationsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
