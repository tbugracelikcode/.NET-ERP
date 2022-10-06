using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ProductGroup.Dtos;

namespace TsiErp.ErpUI.Pages.ProductGroup
{
    public partial class ProductGroupsListPage
    {

        private SfGrid<ListProductGroupsDto> _grid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductGroupsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
