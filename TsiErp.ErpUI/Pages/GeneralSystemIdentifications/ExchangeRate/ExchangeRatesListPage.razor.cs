using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.ExchangeRate
{
    public partial class ExchangeRatesListPage : IDisposable
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = ExchangeRatesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectExchangeRatesDto()
            {
                Date = DateTime.Today
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextRefresh"], Id = "refresh" });
        }

        #region Para Birimi ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrenciesPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrenciesPopupVisible = true;
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrencies = args.RowData;

            if (selectedCurrencies != null)
            {
                DataSource.CurrencyID = selectedCurrencies.Id;
                DataSource.CurrencyCode = selectedCurrencies.Code;
                SelectCurrenciesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
