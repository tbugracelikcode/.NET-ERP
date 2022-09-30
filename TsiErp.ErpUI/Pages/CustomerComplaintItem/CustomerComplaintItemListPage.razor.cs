using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.ErpUI.Pages.CustomerComplaintItem
{
    public partial class CustomerComplaintItemListPage
    {

        private SfGrid<ListCustomerComplaintItemsDto> _grid;


        protected override async void OnInitialized()
        {
            BaseCrudService = CustomerComplaintItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectCustomerComplaintItemsDto()
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
