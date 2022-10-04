using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Currency.Dtos;


namespace TsiErp.ErpUI.Pages.Currency
{

    public partial class CurrenciesListPage
    {

        private SfGrid<ListCurrenciesDto> _grid;

        protected override async void OnInitialized()
        {
            BaseCrudService = CurrenciesService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCurrenciesDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }
    }
}
