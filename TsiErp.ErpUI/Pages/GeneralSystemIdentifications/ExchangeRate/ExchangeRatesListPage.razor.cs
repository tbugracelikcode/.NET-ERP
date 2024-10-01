using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Utilities.FinanceUtilities.TCMBExchange;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.ExchangeRate
{
    public partial class ExchangeRatesListPage : IDisposable
    {


        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }
        protected override async void OnInitialized()
        {
            BaseCrudService = ExchangeRatesService;

            _L = L;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ExRatesChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectExchangeRatesDto()
            {
                Date = GetSQLDateAppService.GetDateFromSQL().Date
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {
                foreach (var context in contextsList)
                {
                 var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                        case "ExchangeRateContextAdd":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextAdd"], Id = "new" }); break;
                        case "ExchangeRateContextChange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextChange"], Id = "changed" }); break;
                        case "ExchangeRateContextDelete":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextDelete"], Id = "delete" }); break;
                        case "ExchangeRateContextRefresh":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextRefresh"], Id = "refresh" }); break;
                        case "ExchangeRateContextCentralBankExchange":
                            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ExchangeRateContextCentralBankExchange"], Id = "bankexchange" }); break;
                        default: break;
                        }
                    }
                }
            }

                
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListExchangeRatesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    if (args.RowInfo.RowData != null)
                    {

                        IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "delete":

                    if (args.RowInfo.RowData != null)
                    {

                        var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "bankexchange":
                    if (args.RowInfo.RowData != null)
                    {

                        var date = GetSQLDateAppService.GetDateFromSQL();

                    var controlDate = new DateTime(date.Year, date.Month, date.Day);

                    var currencyList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();

                    foreach (var currency in currencyList)
                    {
                        string symbol = currency.CurrencySymbol;

                        Guid currencyId = currency.Id;

                        var currencyControl = (await ExchangeRatesService.GetListAsync(new ListExchangeRatesParameterDto())).Data.Where(t => t.Date == controlDate && t.CurrencyId == currencyId).ToList();

                        int currencyCode = GetCurrencyCode(symbol);

                        if (currencyControl.Count == 0)
                        {
                            if (currencyCode > -1)
                            {
                                var currencyRates = TCMBCurrenciesExchange.GetHistoricalExchangeRates((TCMBCurrencyCode)currencyCode, date);

                                CreateExchangeRatesDto exRate = new CreateExchangeRatesDto
                                {
                                    BuyingRate = Convert.ToDecimal(currencyRates.ForexBuying),
                                    CurrencyID = currencyId,
                                    Date = new DateTime(date.Year, date.Month, date.Day),
                                    EffectiveBuyingRate = Convert.ToDecimal(currencyRates.BanknoteBuying),
                                    EffectiveSaleRate = Convert.ToDecimal(currencyRates.BanknoteSelling),
                                    SaleRate = Convert.ToDecimal(currencyRates.ForexSelling)
                                };

                                await ExchangeRatesService.CreateAsync(exRate);

                            }
                        }
                        else
                        {
                            if (currencyCode > -1)
                            {
                                var currencyRates = TCMBCurrenciesExchange.GetHistoricalExchangeRates((TCMBCurrencyCode)currencyCode, date);

                                UpdateExchangeRatesDto exRate = new UpdateExchangeRatesDto
                                {
                                    BuyingRate = Convert.ToDecimal(currencyRates.ForexBuying),
                                    CurrencyID = currencyId,
                                    Date = new DateTime(date.Year, date.Month, date.Day),
                                    EffectiveBuyingRate = Convert.ToDecimal(currencyRates.BanknoteBuying),
                                    EffectiveSaleRate = Convert.ToDecimal(currencyRates.BanknoteSelling),
                                    SaleRate = Convert.ToDecimal(currencyRates.ForexSelling),
                                    Id = currencyControl.LastOrDefault().Id
                                };

                                await ExchangeRatesService.UpdateAsync(exRate);

                            }
                        }
                    }
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    }

                    break;

                default:
                    break;
            }


            if (args.RowInfo.RowData != null)
            {
                args.RowInfo.RowData = null;
            }
        }

        private int GetCurrencyCode(string symbol)
        {
            int code = -1;

            switch (symbol)
            {
                case "USD":
                    code = 0;
                    break;
                case "EUR":
                    code = 1;
                    break;
                case "GBP":
                    code = 2;
                    break;
                case "TRY":
                    code = 3;
                    break;

                default:
                    break;
            }

            return code;
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
