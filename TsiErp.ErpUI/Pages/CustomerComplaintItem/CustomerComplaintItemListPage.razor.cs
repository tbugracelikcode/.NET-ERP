using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.ErpUI.Pages.CustomerComplaintItem
{
    public partial class CustomerComplaintItemListPage
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = CustomerComplaintItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCustomerComplaintItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
