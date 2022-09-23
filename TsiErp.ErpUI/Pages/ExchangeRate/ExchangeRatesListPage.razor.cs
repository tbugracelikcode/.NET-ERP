using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.ErpUI.Pages.ExchangeRate
{
    public partial class ExchangeRatesListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = ExchangeRatesService;
        }

    }
}
