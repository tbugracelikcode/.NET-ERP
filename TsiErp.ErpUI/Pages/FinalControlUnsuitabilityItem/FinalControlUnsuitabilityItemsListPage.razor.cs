using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.FinalControlUnsuitabilityItem
{
    public partial class FinalControlUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = FinalControlUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectFinalControlUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
