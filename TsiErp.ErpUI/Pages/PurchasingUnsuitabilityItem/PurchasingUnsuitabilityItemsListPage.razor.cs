using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.PurchasingUnsuitabilityItem
{
    public partial class PurchasingUnsuitabilityItemsListPage
    {
        private SfGrid<ListPurchasingUnsuitabilityItemsDto> _grid;


        protected override async void OnInitialized()
        {
            BaseCrudService = PurchasingUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPurchasingUnsuitabilityItemsDto()
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
