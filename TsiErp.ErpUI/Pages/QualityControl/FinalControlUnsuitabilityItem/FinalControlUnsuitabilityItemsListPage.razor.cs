using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.FinalControlUnsuitabilityItem
{
    public partial class FinalControlUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = FinalControlUnsuitabilityItemsService;
            _L = L;
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
