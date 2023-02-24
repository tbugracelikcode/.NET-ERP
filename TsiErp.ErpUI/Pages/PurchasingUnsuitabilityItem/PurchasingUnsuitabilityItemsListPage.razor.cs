using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.PurchasingUnsuitabilityItem
{
    public partial class PurchasingUnsuitabilityItemsListPage
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = PurchasingUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchasingUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
