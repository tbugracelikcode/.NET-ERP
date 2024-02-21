using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using System.Linq.Dynamic.Core;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.ReportDtos.WorkOrderListReport;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.ProductionManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ReportPages
{
    public partial class WorkOrderListReportPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        protected override async void OnInitialized()
        {
            await GetProducts();
            await GetProductionOrders();
            await GetStations();
            await GetSalesOrders();
            await GetWorkOrderStateEnums();
        }

        #region Products

        List<ListProductsDto> Products = new List<ListProductsDto>();
        List<Guid> BindingProducts = new List<Guid>();

        private async Task GetProducts()
        {
            List<ProductTypeEnum> productTypeEnums = new List<ProductTypeEnum> { ProductTypeEnum.MM, ProductTypeEnum.YM };

            Products = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.Where(t => productTypeEnums.Contains(t.ProductType)).ToList();
        }

        #endregion

        #region Production Orders

        List<ListProductionOrdersDto> ProductionOrders = new List<ListProductionOrdersDto>();
        List<Guid> BindingProductionOrders = new List<Guid>();

        private async Task GetProductionOrders()
        {
            ProductionOrders = (await ProductionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.ToList();
        }

        #endregion

        #region Stations

        List<ListStationsDto> Stations = new List<ListStationsDto>();
        List<Guid> BindingStations = new List<Guid>();

        private async Task GetStations()
        {
            Stations = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
        }

        #endregion

        #region Sales Order

        List<ListSalesOrderDto> SalesOrders = new List<ListSalesOrderDto>();
        List<Guid> BindingSalesOrders = new List<Guid>();

        private async Task GetSalesOrders()
        {
            SalesOrders = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();
        }

        #endregion

        #region Work Order States

        List<WorkOrderStateEnumModel> WorkOrderStateEnumList = new List<WorkOrderStateEnumModel>();
        List<WorkOrderStateEnumModel> BindingWorkOrderStateEnumList = new List<WorkOrderStateEnumModel>();

        private async Task GetWorkOrderStateEnums()
        {
            var enumList = Enum.GetValues(typeof(WorkOrderStateEnum)).ToDynamicList<WorkOrderStateEnum>();

            foreach (var item in enumList)
            {
                var locString = GetWorkOrderEnumStringKey(item);

                int workOrderStateInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = locString.Split('-')[1];

                WorkOrderStateEnumList.Add(new WorkOrderStateEnumModel
                {
                    WorkOrderStateInt = workOrderStateInt,
                    Text = text,
                    Value = item
                });
            }

            await Task.CompletedTask;
        }

        public static string GetWorkOrderEnumStringKey(WorkOrderStateEnum stateEnum)
        {
            string result = "";

            switch (stateEnum)
            {
                case WorkOrderStateEnum.Baslamadi:
                    result = "1-Başlamadı";
                    break;
                case WorkOrderStateEnum.Durduruldu:
                    result = "2-Durduruldu";
                    break;
                case WorkOrderStateEnum.Iptal:
                    result = "3-İptal";
                    break;
                case WorkOrderStateEnum.DevamEdiyor:
                    result = "4-Devam Ediyor";
                    break;
                case WorkOrderStateEnum.Tamamlandi:
                    result = "5-Tamamlandı";
                    break;
                case WorkOrderStateEnum.FasonaGonderildi:
                    result = "6-Fasona Gönderildi";
                    break;
            }

            return result;
        }
        #endregion

        private async void CreateReport()
        {
            if (BindingProducts == null)
            {
                BindingProducts = new List<Guid>();
            }

            if (BindingProductionOrders == null)
            {
                BindingProductionOrders = new List<Guid>();
            }

            if (BindingStations == null)
            {
                BindingStations = new List<Guid>();
            }

            if (BindingSalesOrders == null)
            {
                BindingSalesOrders = new List<Guid>();
            }

            if (BindingWorkOrderStateEnumList == null)
            {
                BindingWorkOrderStateEnumList = new List<WorkOrderStateEnumModel>();
            }

            WorkOrderListReportParameterDto filters = new WorkOrderListReportParameterDto();
            filters.Products = BindingProducts;
            filters.ProductionOrders = BindingProductionOrders;
            filters.Stations = BindingStations;
            filters.Orders = BindingSalesOrders;
            filters.WorkOrderStates = BindingWorkOrderStateEnumList.Select(t => t.Value).ToList();

            var report = (await WorkOrderReportsAppService.GetWorkOrderListReport(filters)).ToList();

            if (report.Count > 0)
            {
                Report = new WorkOrderListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new WorkOrderListReport();
                Report.DataSource = null;
                await ModalManager.MessagePopupAsync(ReportLocalizer["ReportMessageTitle"], ReportLocalizer["ReportRecordNotFound"]);
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }

    public class WorkOrderStateEnumModel
    {
        public WorkOrderStateEnum Value { get; set; }

        public string Text { get; set; }

        public int WorkOrderStateInt { get; set; }
    }
}
