using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.ErpUI.Pages.Department
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
