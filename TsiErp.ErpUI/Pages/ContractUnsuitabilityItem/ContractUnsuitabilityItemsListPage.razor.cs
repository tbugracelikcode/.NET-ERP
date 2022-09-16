using DevExpress.Blazor;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.ContractUnsuitabilityItem
{
    public partial class ContractUnsuitabilityItemsListPage
    {
        List<ListContractUnsuitabilityItemsDto> Gridlist = new List<ListContractUnsuitabilityItemsDto>();

        bool PopupVisible = false;
        bool isActiveButton = false;
        bool isStationProductivityButton = false;
        bool isEmployeeProductivityButton = false;

        protected override async void OnInitialized()
        {
            //Gridlist = (await ContractUnsuitabilityItemsService.GetListAsync(new ListContractUnsuitabilityItemsParameterDto() )).Data.ToList();
        }

        void ContractUnsuitabilityItemsPopupClosing(PopupClosingEventArgs args)
        {
            PopupVisible = false;
        }

        public void OnPopupButtonClicked()
        {
            PopupVisible = true;
        }
    }
}
