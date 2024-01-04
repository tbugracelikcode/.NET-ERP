using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;


namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Currency
{

    public partial class CurrenciesListPage : IDisposable
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = CurrenciesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCurrenciesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("CurrenciesChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["CurrencyContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("CurrenciesChildMenu");
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
