using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.ErpUI.Pages.ExchangeRate
{
    public partial class ExchangeRatesListPage
    {
        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;

        private SfGrid<ListExchangeRatesDto> _grid;

        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();


        protected override async void OnInitialized()
        {
            BaseCrudService = ExchangeRatesService;
            await GetCurrenciesList();
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }


        #region Para Birimleri
        public async Task CurrencyFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await CurrenciesComboBox.FilterAsync(CurrenciesList, query);
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        public async Task CurrencyValueChangeHandler(ChangeEventArgs<string, ListCurrenciesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.CurrencyID = args.ItemData.Id;
                DataSource.CurrencyCode = args.ItemData.Code;
            }
            else
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.CurrencyCode = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion


    }
}
