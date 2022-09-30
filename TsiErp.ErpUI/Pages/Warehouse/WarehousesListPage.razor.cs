using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.Warehouse
{
    public partial class WarehousesListPage
    {
        private SfGrid<ListWarehousesDto> _grid;

        protected override async void OnInitialized()
        {
            BaseCrudService = WarehousesService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectWarehousesDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }
    }
}
