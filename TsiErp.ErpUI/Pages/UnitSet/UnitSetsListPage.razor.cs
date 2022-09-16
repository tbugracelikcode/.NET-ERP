using DevExpress.Blazor;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.UnitSet
{
    public partial class UnitSetsListPage
    {
        List<ListUnitSetsDto> Gridlist = new List<ListUnitSetsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
             //Gridlist = (await UnitSetsService.GetListAsync(new ListUnitSetsParameterDto() { IsActive = true })).Data.ToList();
        }

        void UnitSetsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
