using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Layouts;
using Syncfusion.Blazor;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using DevExpress.XtraCharts.Native;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using Microsoft.AspNetCore.Components;
using TsiErp.ErpUI.Components.Commons.Spinner;


namespace TsiErp.ErpUI.Pages
{
    public partial class Index
    {
        [Inject]
        SpinnerService SpinnerService { get; set; }


        #region Dashboard Değişkenleri

        SfDashboardLayout dashboardObject;
        double[] CellSpacing = new double[] { 5, 5 };
        int Columns = 6;
        double Ratio = 100 / 85;
        private Theme Theme { get; set; }
        public string[] ShapePropertyPath = { "continent" };
        public string ShapeDataPath = "Continent";
        SfGrid<ListNotificationsDto> _NotificationsGrid;
        SfGrid<OpenOrderBalances> _OpenOrderBalancesGrid;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };
        public List<ContextMenuItemModel> NotificationsGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        #endregion

        public List<ShipmentData> ShipmentDataList = new List<ShipmentData>();
        public List<SelectSalesOrderLinesDto> SalesOrderLineNotShippedList = new List<SelectSalesOrderLinesDto>();
        public List<NotShippedProductGroups> NotShippedProductGroupsList = new List<NotShippedProductGroups>();
        public List<NotShippedProductGroupsTooltip> NotShippedProductGroupsTooltipList = new List<NotShippedProductGroupsTooltip>();
        public List<OrdersbyMonths> OrdersbyMonthsList = new List<OrdersbyMonths>();
        public List<SelectSalesOrderLinesDto> SalesOrderLinesList = new List<SelectSalesOrderLinesDto>();
        public List<SelectPackingListPalletPackageLinesDto> PackingListPackageLineList = new List<SelectPackingListPalletPackageLinesDto>();
        public List<OpenOrderBalances> OpenOrderBalancesList = new List<OpenOrderBalances>();
        public List<ListNotificationsDto> NotificationsList = new List<ListNotificationsDto>();

        int PlannedQuantity = 0;
        int ShipmentQuantity = 0;
        int RestQuantity = 0;

        public SelectNotificationsDto DataSource { get; set; }

        #region Virtual Classes
        public class ShipmentData
        {
            public string QuantityType { get; set; }
            public int Count { get; set; }
            public double Percentage { get; set; }
        }

        public class NotShippedProductGroups
        {
            public string ProductGroupName { get; set; }
            public Guid ProductGroupID { get; set; }
            public int Amount { get; set; }
        }

        public class NotShippedProductGroupsTooltip
        {
            public string OrderNo { get; set; }
            public string CurrentAccountCode { get; set; }
            public DateTime RequestedDate { get; set; }
            public Guid ProductGroupID { get; set; }
            public string ProductGroupName { get; set; }
            public int Amount { get; set; }
        }

        public class OrdersbyMonths
        {
            public int NumberofOrder { get; set; }
            public string MonthName { get; set; }
        }

        public class OpenOrderBalances
        {
            public string OrderNo { get; set; }
            public string CustomerOrderNo { get; set; }
            public DateTime ShippingDate { get; set; }
            public int Amount { get; set; }
            public int Balance { get; set; }
            public decimal ShippingRate { get; set; }
        }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            #region Yorum

            // #region En Yakın Tarihli Yükleme

            // var shipmentPlanningListDto = (await ShipmentPlanningsAppService.GetListAsync(new ListShipmentPlanningsParameterDto())).Data.ToList();
            // shipmentPlanningListDto = shipmentPlanningListDto.OrderBy(t => t.ShipmentPlanningDate).ToList();

            // if (shipmentPlanningListDto.Count > 0)
            // {
            //     var shipmentPlanningLineList = (await ShipmentPlanningsAppService.GetAsync(shipmentPlanningListDto[0].Id)).Data.SelectShipmentPlanningLines.Count > 0 ? (await ShipmentPlanningsAppService.GetAsync(shipmentPlanningListDto[0].Id)).Data.SelectShipmentPlanningLines : new List<SelectShipmentPlanningLinesDto>();

            //     PlannedQuantity = (int)shipmentPlanningLineList.Sum(t => t.PlannedQuantity);
            //     ShipmentQuantity = (int)shipmentPlanningLineList.Sum(t => t.ShipmentQuantity);
            //     RestQuantity = PlannedQuantity - ShipmentQuantity;

            //     ShipmentData shipmentDataShipment = new ShipmentData
            //         {
            //             Count = ShipmentQuantity,
            //             QuantityType = L["ClosestShipmentPackaged"],
            //             Percentage = (ShipmentQuantity / PlannedQuantity) * 100
            //         };
            //     ShipmentData shipmentDataRest = new ShipmentData
            //         {
            //             Count = RestQuantity,
            //             QuantityType = L["ClosestShipmentRemaining"],
            //             Percentage = (RestQuantity / PlannedQuantity) * 100
            //         };

            //     ShipmentDataList.Add(shipmentDataShipment);
            //     ShipmentDataList.Add(shipmentDataRest);
            // }


            // #endregion

            // #region Sevk Edilmeyen Ürün Grupları

            // SalesOrderLineNotShippedList = (await SalesOrdersAppService.GetLineSelectListAsync()).Data.ToList();

            // var groupedLineList = SalesOrderLineNotShippedList.GroupBy(t => new { t.ProductID, t.SalesOrderID }).Select(t => new { ProductID = t.Key.ProductID, SalesOrderID = t.Key.SalesOrderID, SalesOrderLine = t.ToList() }).ToList();

            // foreach (var salesOrderLine in groupedLineList)
            // {
            //     var productID = salesOrderLine.ProductID.GetValueOrDefault();
            //     var productGroupID = (await ProductsAppService.GetAsync(productID)).Data.ProductGrpID;
            //     var productGroup = (await ProductGroupsAppService.GetAsync(productGroupID)).Data;

            //     NotShippedProductGroups notShippedProductGroupModel = new NotShippedProductGroups
            //         {
            //             ProductGroupName = productGroup.Name,
            //             Amount = (int)salesOrderLine.SalesOrderLine.Sum(t => t.Quantity),
            //             ProductGroupID = productGroup.Id

            //         };


            //     NotShippedProductGroupsList.Add(notShippedProductGroupModel);

            //     var salesOrder = (await SalesOrdersAppService.GetAsync(salesOrderLine.SalesOrderLine.Select(t => t.SalesOrderID).FirstOrDefault())).Data;

            //     NotShippedProductGroupsTooltip tooltipModel = new NotShippedProductGroupsTooltip
            //         {
            //             CurrentAccountCode = salesOrder.CurrentAccountCardCode,
            //             OrderNo = salesOrder.FicheNo,
            //             ProductGroupID = productGroup.Id,
            //             ProductGroupName = productGroup.Name,
            //             RequestedDate = salesOrder.CustomerRequestedDate,
            //             Amount = (int)salesOrderLine.SalesOrderLine.Sum(t => t.Quantity)
            //         };

            //     NotShippedProductGroupsTooltipList.Add(tooltipModel);
            // }


            // #endregion

            // #region Aylara Göre Sipariş Sayısı


            // var salesOrderList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.Where(t => t.Date_.Year == GetSQLDateAppService.GetDateFromSQL().Year).ToList();

            // var groupedByDateList = salesOrderList.GroupBy(t => new { t.Date_.Month }).Select(t => new { Month = t.Key.Month, SalesOrder = t.ToList() }).ToList();

            // foreach (var monthOrder in groupedByDateList)
            // {
            //     string monthName = "";

            //     switch (monthOrder.Month)
            //     {
            //         case 1: monthName = L["Month1"]; break;
            //         case 2: monthName = L["Month2"]; break;
            //         case 3: monthName = L["Month3"]; break;
            //         case 4: monthName = L["Month4"]; break;
            //         case 5: monthName = L["Month5"]; break;
            //         case 6: monthName = L["Month6"]; break;
            //         case 7: monthName = L["Month7"]; break;
            //         case 8: monthName = L["Month8"]; break;
            //         case 9: monthName = L["Month9"]; break;
            //         case 10: monthName = L["Month10"]; break;
            //         case 11: monthName = L["Month11"]; break;
            //         case 12: monthName = L["Month12"]; break;
            //         default: break;
            //     }

            //     OrdersbyMonths ordersbyMonthsModel = new OrdersbyMonths
            //         {
            //             MonthName = monthName,
            //             NumberofOrder = monthOrder.SalesOrder.Count
            //         };

            //     OrdersbyMonthsList.Add(ordersbyMonthsModel);
            // }

            // #endregion

            #region Açık Sipariş Bakiyeleri

            // SalesOrderLinesList = (await SalesOrdersAppService.GetLineSelectListAsync()).Data.ToList();

            // PackingListPackageLineList = (await PackingListsAppService.GetLinePalletPackageListAsync()).Data.ToList();

            // foreach (var package in PackingListPackageLineList)
            // {
            //     var salesOrderLine = SalesOrderLinesList.Where(t => t.Id == package.SalesOrderLineID).FirstOrDefault();

            //     if (salesOrderLine != null && salesOrderLine.Id != Guid.Empty && salesOrderLine.Quantity - package.TotalAmount > 0) //Satır varsa ve bakiye 0'dan büyükse (açık sipariş senaryosu)
            //     {
            //         var salesOrder = (await SalesOrdersAppService.GetAsync(salesOrderLine.SalesOrderID)).Data;

            //         var packingList = (await PackingListsAppService.GetAsync(package.PackingListID)).Data;

            //         OpenOrderBalances openOrderBalanceModel = new OpenOrderBalances
            //             {
            //                 OrderNo = salesOrder.FicheNo,
            //                 ShippingDate = packingList.LoadingDate.GetValueOrDefault(),
            //                 Amount = (int)salesOrderLine.Quantity,
            //                 Balance = (int)salesOrderLine.Quantity - (int)package.TotalAmount,
            //                 ShippingRate = (((int)salesOrderLine.Quantity - (int)package.TotalAmount) / (int)salesOrderLine.Quantity) * 100
            //             };

            //         OpenOrderBalancesList.Add(openOrderBalanceModel);
            //     }
            // }

            // OpenOrderBalancesList = OpenOrderBalancesList.OrderBy(t => t.ShippingDate).ToList();

            OpenOrderBalances openOrderBalanceModel1 = new OpenOrderBalances
            {
                OrderNo = "0001",
                CustomerOrderNo = "0002",
                ShippingDate = DateTime.Today,
                Amount = 330,
                Balance = 300,
                ShippingRate = 55
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel1);

            OpenOrderBalances openOrderBalanceModel2 = new OpenOrderBalances
            {
                OrderNo = "0002",
                CustomerOrderNo = "0003",
                ShippingDate = DateTime.Today,
                Amount = 569,
                Balance = 255,
                ShippingRate = 23
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel2);

            OpenOrderBalances openOrderBalanceModel3 = new OpenOrderBalances
            {
                OrderNo = "0042",
                CustomerOrderNo = "0004",
                ShippingDate = DateTime.Today,
                Amount = 999,
                Balance = 123,
                ShippingRate = 56
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel3);

            OpenOrderBalances openOrderBalanceModel4 = new OpenOrderBalances
            {
                OrderNo = "0045",
                CustomerOrderNo = "0005",
                ShippingDate = DateTime.Today,
                Amount = 1220,
                Balance = 220,
                ShippingRate = 80
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel4);

            OpenOrderBalances openOrderBalanceModel5 = new OpenOrderBalances
            {
                OrderNo = "0777",
                CustomerOrderNo = "0006",
                ShippingDate = DateTime.Today,
                Amount = 777,
                Balance = 77,
                ShippingRate = 77
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel5);

            OpenOrderBalances openOrderBalanceModel6 = new OpenOrderBalances
            {
                OrderNo = "0008",
                CustomerOrderNo = "0007",
                ShippingDate = DateTime.Today,
                Amount = 554,
                Balance = 12,
                ShippingRate = 94
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel6);

            OpenOrderBalances openOrderBalanceModel7 = new OpenOrderBalances
            {
                OrderNo = "0007",
                CustomerOrderNo = "0008",
                ShippingDate = DateTime.Today,
                Amount = 613,
                Balance = 72,
                ShippingRate = 34
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel7);

            OpenOrderBalances openOrderBalanceModel8 = new OpenOrderBalances
            {
                OrderNo = "0013",
                CustomerOrderNo = "0009",
                ShippingDate = DateTime.Today,
                Amount = 817,
                Balance = 64,
                ShippingRate = 21
            };

            OpenOrderBalancesList.Add(openOrderBalanceModel8);


            #endregion

            #endregion

            #region Bildirimler

            GetNotificationsList();
            CreateNotificationsContextMenuItems();

            #endregion


            //NotificationService.OnChange += StateHasChanged;

            await InvokeAsync(StateHasChanged);
        }


        public async void GetNotificationsList()
        {
            NotificationsList = (await NotificationsAppService.GetListbyUserIDListDtoAsync(LoginedUserService.UserId)).Data.ToList();
        }

        protected void CreateNotificationsContextMenuItems()
        {
            if (NotificationsGridContextMenu.Count == 0)
            {

                NotificationsGridContextMenu.Add(new ContextMenuItemModel { Text = L["NotificationsContextMultiSelect"], Id = "markallread" });

            }
        }

        public async void NotificationsContextMenuClick(ContextMenuClickEventArgs<ListNotificationsDto> args)
        {
            switch (args.Item.Id)
            {
                case "markallread":
                    SpinnerService.Show();
                    await Task.Delay(100);
                    foreach (var notification in NotificationsList.Where(t => t.IsViewed == false))
                    {

                        notification.IsViewed = true;

                        var updatedInput = ObjectMapper.Map<ListNotificationsDto, UpdateNotificationsDto>(notification);
                        await NotificationsAppService.UpdateAsync(updatedInput);
                    }
                    SpinnerService.Hide();
                    GetNotificationsList();

                    await _NotificationsGrid.Refresh();

                    await MediatorAppService.Publish("Notification");

                    await InvokeAsync(StateHasChanged);

                    break;

                default:
                    break;
            }

        }

        public async Task ViewedClick(Guid notificationId)
        {
            var notification = (await NotificationsAppService.GetAsync(notificationId)).Data;

            notification.IsViewed = true;

            var updatedInput = ObjectMapper.Map<SelectNotificationsDto, UpdateNotificationsDto>(notification);
            await NotificationsAppService.UpdateAsync(updatedInput);

            GetNotificationsList();
            await _NotificationsGrid.Refresh();

            await MediatorAppService.Publish("Notification");

            await InvokeAsync(StateHasChanged);
        }

        private async Task UnViewedClick(Guid notificationId)
        {
            var notification = (await NotificationsAppService.GetAsync(notificationId)).Data;
            notification.IsViewed = false;

            var updatedInput = ObjectMapper.Map<SelectNotificationsDto, UpdateNotificationsDto>(notification);
            await NotificationsAppService.UpdateAsync(updatedInput);

            GetNotificationsList();
            await _NotificationsGrid.Refresh();

            await MediatorAppService.Publish("Notification");

            await InvokeAsync(StateHasChanged);
        }
    }

}

