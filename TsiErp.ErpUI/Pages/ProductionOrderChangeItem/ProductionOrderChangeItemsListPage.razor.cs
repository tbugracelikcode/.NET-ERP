using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionOrderChangeItem
{
    public partial class ProductionOrderChangeItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrderChangeItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionOrderChangeItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
