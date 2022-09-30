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
        private SfGrid<ListContractUnsuitabilityItemsDto> _grid;

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

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }
    }
}
