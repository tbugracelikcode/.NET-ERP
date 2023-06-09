using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Branch
{
    public partial class BranchesListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = BranchesService;
            _L = L;
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