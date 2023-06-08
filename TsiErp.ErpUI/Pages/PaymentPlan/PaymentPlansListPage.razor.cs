using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;

namespace TsiErp.ErpUI.Pages.PaymentPlan
{
    public partial class PaymentPlansListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = PaymentPlansService;
            _L = L;
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
