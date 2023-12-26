using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;

namespace TsiErp.ErpUI.Pages.FinanceManagement.PaymentPlan
{
    public partial class PaymentPlansListPage : IDisposable
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

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PaymentPlanContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PaymentPlanContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PaymentPlanContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["PaymentPlanContextRefresh"], Id = "refresh" });
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


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
