using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.OperationUnsuitabilityItem
{
    public partial class OperationUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = OperationUnsuitabilityItemsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
