namespace TsiErp.ErpUI.Pages.WorkOrder
{
    public partial class WorkOrdersListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;

        }

    }
}
