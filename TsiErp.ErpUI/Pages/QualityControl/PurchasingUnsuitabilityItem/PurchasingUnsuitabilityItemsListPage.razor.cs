using TsiErp.Entities.Entities.QualityControl.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.PurchasingUnsuitabilityItem
{
    public partial class PurchasingUnsuitabilityItemsListPage
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = PurchasingUnsuitabilityItemsService;
            _L = L;
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
