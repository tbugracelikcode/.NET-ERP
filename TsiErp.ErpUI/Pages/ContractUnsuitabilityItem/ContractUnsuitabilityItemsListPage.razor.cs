using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.ErpUI.Pages.ContractUnsuitabilityItem
{
    public partial class ContractUnsuitabilityItemsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ContractUnsuitabilityItemsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectContractUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

       
    }
}
