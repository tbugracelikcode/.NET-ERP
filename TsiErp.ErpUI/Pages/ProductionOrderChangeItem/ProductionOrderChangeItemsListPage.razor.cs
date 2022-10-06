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

        private SfGrid<ListProductionOrderChangeItemsDto> _grid;
        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

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

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
