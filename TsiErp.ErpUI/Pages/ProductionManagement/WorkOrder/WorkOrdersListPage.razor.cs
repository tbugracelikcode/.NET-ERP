namespace TsiErp.ErpUI.Pages.ProductionManagement.WorkOrder
{
    public partial class WorkOrdersListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
            _L = L;

        }

    }
}
