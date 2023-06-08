using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.ErpUI.Pages.ProductGroup
{
    public partial class ProductGroupsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductGroupsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
