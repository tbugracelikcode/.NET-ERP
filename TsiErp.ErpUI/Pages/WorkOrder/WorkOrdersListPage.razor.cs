using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

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
