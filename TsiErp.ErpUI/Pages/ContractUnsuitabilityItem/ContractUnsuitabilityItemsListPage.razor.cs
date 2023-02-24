using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.ContractUnsuitabilityItem
{
    public partial class ContractUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ContractUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectContractUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

       
    }
}
