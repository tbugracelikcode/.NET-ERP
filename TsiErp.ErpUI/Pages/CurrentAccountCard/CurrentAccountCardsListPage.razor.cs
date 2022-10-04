using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Department.Services;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;

namespace TsiErp.ErpUI.Pages.CurrentAccountCard
{
    public partial class CurrentAccountCardsListPage
    {

        private SfGrid<ListCurrentAccountCardsDto> _grid;

        SfComboBox<string, ListShippingAdressesDto> ShippingAdressesComboBox;
        List<ListShippingAdressesDto> ShippingAdressesList = new List<ListShippingAdressesDto>();

        SfComboBox<string, ListCurrenciesDto> CurrenciesComboBox;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();
        protected override async void OnInitialized()
        {
            BaseCrudService = CurrentAccountCardsService;
            await GetCurrenciesList();
            await GetShippingAdressesList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCurrentAccountCardsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
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
                DataSource.Currency = args.ItemData.Name;
            }
            else
            {
                DataSource.CurrencyID = Guid.Empty;
                DataSource.Currency = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Sevkiyat Adresleri
        public async Task ShippingAdressFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await ShippingAdressesComboBox.FilterAsync(ShippingAdressesList, query);
        }

        private async Task GetShippingAdressesList()
        {
            ShippingAdressesList = (await ShippingAdressesAppService.GetListAsync(new ListShippingAdressesParameterDto())).Data.ToList();
        }

        public async Task ShippingAdressValueChangeHandler(ChangeEventArgs<string, ListShippingAdressesDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.ShippingAddress = args.ItemData.Name;
            }
            else
            {
                DataSource.ShippingAddress = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

        #endregion

    }
}
