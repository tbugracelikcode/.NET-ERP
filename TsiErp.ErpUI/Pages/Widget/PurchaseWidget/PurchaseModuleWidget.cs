using Microsoft.Extensions.Localization;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.ErpUI.Pages.Widget.PurchaseWidget
{

    public class PurchaseModuleWidget
    {
        static List<StateWithPurchaseOrders> StateWithPurchaseOrdersList = new List<StateWithPurchaseOrders>();

        public static async Task<List<StateWithPurchaseOrders>> GetProductList(IPurchaseOrdersAppService Service,object localizer)
        {
            var loc = (IStringLocalizer)localizer;

            var orderList = (await Service.GetListAsync(new ListPurchaseOrdersParameterDto())).Data.GroupBy(t => t.PurchaseOrderState).ToList();

            foreach (var order in orderList)
            {
                if (order.Key == 0)
                {
                    continue;
                }

                var locKey = loc[Enum.GetName(typeof(PurchaseOrderStateEnum), order.Key)];

                StateWithPurchaseOrdersList.Add(new StateWithPurchaseOrders
                {
                    StateName = loc[GetEnumStringKey(locKey)],
                    Count = order.Count()
                });
            }

            return StateWithPurchaseOrdersList;
        }

        public static string GetEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "KismiTamamlandi":
                    result = "EnumInPartialCompleted";
                    break;
                case "Iptal":
                    result = "EnumCancel";
                    break;
                case "Tamamlandi":
                    result = "EnumCompleted";
                    break;
                case "Onaylandı":
                    result = "EnumApproved";
                    break;
                case "Beklemede":
                    result = "EnumWaiting";
                    break;
            }

            return result;
        }
    }

    public class StateWithPurchaseOrders
    {
        public string StateName { get; set; }

        public int Count { get; set; }
    }
}
