using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

namespace TsiErp.ErpUI.Pages.HaltReason
{
    public partial class HaltReasonsListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = HaltReasonsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectHaltReasonsDto(){};

            EditPageVisible = true;

            return Task.CompletedTask;
        }
    }
}
