using DevExpress.Blazor;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.ErpUI.Pages.Period
{
    public partial class PeriodsListPage
    {
        List<ListPeriodsDto> Gridlist = new List<ListPeriodsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await PeriodsService.GetListAsync(new ListPeriodsParameterDto() { IsActive = true })).Data.ToList();
        }

        void PeriodsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }

    }
}
