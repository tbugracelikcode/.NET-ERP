using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.UnitSet
{
    public partial class UnitSetsListPage
    {

        private SfGrid<ListUnitSetsDto> _grid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };


        protected override async void OnInitialized()
        {
            BaseCrudService = UnitSetsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnitSetsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

    }
}
