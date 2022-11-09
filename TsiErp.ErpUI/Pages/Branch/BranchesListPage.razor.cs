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

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBranchesDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}