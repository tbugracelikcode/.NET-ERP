using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.UnitSet
{
    public partial class UnitSetsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = UnitSetsService;
        }
    }
}
