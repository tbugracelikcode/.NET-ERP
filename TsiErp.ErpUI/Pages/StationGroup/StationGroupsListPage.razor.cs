using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StationGroup
{
    public partial class StationGroupsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = StationGroupsService;
        }
    }
}
