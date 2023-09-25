using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;

namespace TsiErp.ErpUI.Pages.ProductionManagement.WorkOrder
{
    public partial class WorkOrdersListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = WorkOrdersAppService;
            _L = L;

        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WorkOrderContextRefresh"], Id = "refresh" });
        }

    }
}
