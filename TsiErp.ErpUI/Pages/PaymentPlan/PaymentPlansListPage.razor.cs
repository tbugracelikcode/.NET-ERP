using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.ErpUI.Pages.PaymentPlan
{
    public partial class PaymentPlansListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = PaymentPlansService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPaymentPlansDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
