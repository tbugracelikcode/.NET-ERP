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

        private SfGrid<ListOperationUnsuitabilityItemsDto> _grid;

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

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }
    }
}
