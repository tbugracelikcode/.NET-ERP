using TsiErp.Entities.Entities.Currency.Dtos;


namespace TsiErp.ErpUI.Pages.Currency
{

    public partial class CurrenciesListPage
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
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
