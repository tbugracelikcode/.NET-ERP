using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.ErpUI.Pages.ExchangeRate
{
    public partial class ExchangeRatesListPage
    {


        protected override async void OnInitialized()
        {
            BaseCrudService = ExchangeRatesService;
            await GetCurrenciesList();
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

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }


    }
}
