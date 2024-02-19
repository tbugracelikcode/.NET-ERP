using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.ReportDtos.ProductionTrackingListReport;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.ProductionManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ReportPages
{
    public partial class ProductionTrackingListReportPage : IDisposable
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
            await GetEmployees();
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

        #region Employees

        List<ListEmployeesDto> Employees = new List<ListEmployeesDto>();
        List<Guid> BindingEmployees = new List<Guid>();

        private async Task GetEmployees()
        {
            Employees = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.ToList();
        }

        #endregion

        private async void CreateReport()
        {
            if (BindingProducts == null)
            {
                BindingProducts = new List<Guid>();
            }



            ProductionTrackingListReportParametersDto filters = new ProductionTrackingListReportParametersDto();
            filters.Products = BindingProducts;
            filters.ProductionOrders = BindingProductionOrders;
            filters.Stations = BindingStations;
            filters.Employees = BindingEmployees;


            var report = (await ProductionTrackingReportsAppService.GetProductionTrackingListReport(filters)).ToList();

            if (report.Count > 0)
            {
                Report = new ProductionTrackingListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new ProductionTrackingListReport();
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
}
