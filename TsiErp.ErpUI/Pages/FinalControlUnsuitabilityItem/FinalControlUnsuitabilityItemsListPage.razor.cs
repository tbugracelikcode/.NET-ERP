using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.FinalControlUnsuitabilityItem
{
    public partial class FinalControlUnsuitabilityItemsListPage
    {

        private SfGrid<ListFinalControlUnsuitabilityItemsDto> _grid;

        protected override async void OnInitialized()
        {
            BaseCrudService = FinalControlUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectFinalControlUnsuitabilityItemsDto()
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
