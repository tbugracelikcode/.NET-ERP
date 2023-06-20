using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItem
{
    partial class UnsuitabilityItemListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = UnsuitabilityItemsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
