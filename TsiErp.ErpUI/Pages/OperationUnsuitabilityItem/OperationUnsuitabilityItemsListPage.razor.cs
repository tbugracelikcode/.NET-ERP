using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.OperationUnsuitabilityItem
{
    public partial class OperationUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = OperationUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectOperationUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}
