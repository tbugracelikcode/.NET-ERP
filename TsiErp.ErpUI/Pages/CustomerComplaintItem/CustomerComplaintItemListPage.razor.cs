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
    }
}
