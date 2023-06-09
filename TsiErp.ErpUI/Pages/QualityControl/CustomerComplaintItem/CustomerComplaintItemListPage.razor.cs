using TsiErp.Entities.Entities.QualityControl.CustomerComplaintItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.CustomerComplaintItem
{
    public partial class CustomerComplaintItemListPage
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = CustomerComplaintItemsService;
            _L = L;
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
