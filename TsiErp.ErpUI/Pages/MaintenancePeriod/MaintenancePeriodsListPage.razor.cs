using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;

namespace TsiErp.ErpUI.Pages.MaintenancePeriod
{
    public partial class MaintenancePeriodsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = MaintenancePeriodsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectMaintenancePeriodsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
