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
        protected override async void OnInitialized()
        {
            BaseCrudService = DepartmentsService;
        }
    }
}
