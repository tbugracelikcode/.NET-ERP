using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = BranchesService;

        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBranchesDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}