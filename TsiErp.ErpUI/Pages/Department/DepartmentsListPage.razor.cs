using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.ErpUI.Pages.Department
{
    public partial class DepartmentsListPage
    {
        private SfGrid<ListDepartmentsDto> _grid;


        protected override async void OnInitialized()
        {
            BaseCrudService = DepartmentsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectDepartmentsDto()
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
