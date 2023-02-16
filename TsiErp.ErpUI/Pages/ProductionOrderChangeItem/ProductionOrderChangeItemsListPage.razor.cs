using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionOrderChangeItem
{
    public partial class ProductionOrderChangeItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrderChangeItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductionOrderChangeItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

    }
}
