using DevExpress.Blazor;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.ErpUI.Pages.Station
{
    public partial class StationsListPage
    {
        List<ListStationsDto> Gridlist = new List<ListStationsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;
        DateTime tarih = DateTime.Today;

        protected override async void OnInitialized()
        {
             //Gridlist = (await StationsService.GetListAsync(new ListStationsParameterDto() { IsActive = true })).Data.ToList();
        }

        void StationsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }

    }
}
