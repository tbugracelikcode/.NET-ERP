using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;

namespace TsiErp.ErpUI.Pages.FinanceManagement.PaymentPlan
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
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("PaymentPlansChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }



        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("PaymentPlansChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
