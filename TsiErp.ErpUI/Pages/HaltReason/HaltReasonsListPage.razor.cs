using TsiErp.Entities.Entities.HaltReason.Dtos;

namespace TsiErp.ErpUI.Pages.HaltReason
{
    public partial class HaltReasonsListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = HaltReasonsService;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectHaltReasonsDto(){};

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
