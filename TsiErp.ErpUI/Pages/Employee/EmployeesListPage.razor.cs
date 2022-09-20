using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Employee.Dtos;

namespace TsiErp.ErpUI.Pages.Employee
{
    public partial class EmployeesListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeesService;
        }


    }
}
