using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Department
{
    public partial class DepartmentsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = DepartmentsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectDepartmentsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

    }
}
