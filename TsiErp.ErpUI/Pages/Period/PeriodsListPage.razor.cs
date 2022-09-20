using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.ErpUI.Pages.Period
{
    public partial class PeriodsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = PeriodsService;
        }

    }
}
