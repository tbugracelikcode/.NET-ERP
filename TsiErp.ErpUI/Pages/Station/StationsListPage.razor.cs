using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.ErpUI.Pages.Station
{
    public partial class StationsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = StationsService;
        }
    }
}
