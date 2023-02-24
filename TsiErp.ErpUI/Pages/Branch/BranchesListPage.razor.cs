using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Branch
{
    public partial class BranchesListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = BranchesService;

        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBranchesDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}