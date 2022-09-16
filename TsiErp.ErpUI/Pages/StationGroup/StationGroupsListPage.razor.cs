using DevExpress.Blazor;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StationGroup
{
    public partial class StationGroupsListPage
    {
        List<ListStationGroupsDto> Gridlist = new List<ListStationGroupsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto() { IsActive = true })).Data.ToList();
        }

        void StationGroupsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
