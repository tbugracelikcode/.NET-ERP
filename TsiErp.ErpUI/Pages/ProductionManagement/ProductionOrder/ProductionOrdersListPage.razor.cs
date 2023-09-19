using Syncfusion.Blazor.Grids;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionOrder
{
    public partial class ProductionOrdersListPage
    {

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        protected override void OnInitialized()
        {
            BaseCrudService = ProductionOrdersAppService;
            CreateMainContextMenuItems();
            _L = L;

        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextWorkOrders"], Id = "workorders" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextOccuredAmountEntry"], Id = "occuredamountentry" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ContextConsumptionReceipt"], Id = "consumptionreceipt" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "workorders":

                    break;

                case "occuredamountentry":
                   
                    break;

                case "consumptionreceipt":
                   
                    break;

              

                default:
                    break;
            }
        }


    }
}
