using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = BranchesService;
        }

        public override void OnContextMenuClick(ContextMenuClickEventArgs<ListBranchesDto> args)
        {
            base.OnContextMenuClick(args);
        }
    }
}