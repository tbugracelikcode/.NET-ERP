using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductWarehouseStatusReportDtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.StockManagement;

namespace TsiErp.ErpUI.Pages.StockManagement.ReportPages
{
    public partial class ProductWarehouseStatusReportPage : IDisposable
    {
        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);

        public DateTime EndDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        protected override async void OnInitialized()
        {
            await GetProducts();
            await GetWarehouses();
            GetProductTypeEnums();
        }

        #region Products

        List<ListProductsDto> Products = new List<ListProductsDto>();
        List<Guid> BindingProducts = new List<Guid>();

        private async Task GetProducts()
        {
            Products = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

        #region Warehouse

        List<ListWarehousesDto> Warehouses = new List<ListWarehousesDto>();
        List<Guid> BindingWarehouses = new List<Guid>();

        private async Task GetWarehouses()
        {
            Warehouses = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        #endregion

        #region Product Types

        List<ProductTypeEnumModel> ProductTypeEnumList = new List<ProductTypeEnumModel>();
        List<ProductTypeEnumModel> BindingProductTypeEnumList = new List<ProductTypeEnumModel>();

        private void GetProductTypeEnums()
        {
            var enumList = Enum.GetValues(typeof(ProductTypeEnum)).ToDynamicList<ProductTypeEnum>();

            foreach (var item in enumList)
            {
                var locKey = ProductLocalizer[Enum.GetName(typeof(ProductTypeEnum), item)];

                var locString = GetProductTypeEnumStringKey(locKey);

                int productTypeInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = ProductLocalizer.GetString(locString.Split('-')[1]);

                ProductTypeEnumList.Add(new ProductTypeEnumModel
                {
                    ProductTypeInt = productTypeInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetProductTypeEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "TM":
                    result = "1-EnumCommercialProduct";
                    break;
                case "HM":
                    result = "10-EnumMaterial";
                    break;
                case "YM":
                    result = "11-EnumSemiProduct";
                    break;
                case "MM":
                    result = "12-EnumProduct";
                    break;
                case "BP":
                    result = "30-EnumSparePart";
                    break;
                case "TK":
                    result = "40-EnumKit";
                    break;
                case "KLP":
                    result = "50-EnumMold";
                    break;
                case "APRT":
                    result = "60-EnumAparatus";
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

            if (BindingWarehouses == null)
            {
                BindingWarehouses = new List<Guid>();
            }

            if (BindingProductTypeEnumList == null)
            {
                BindingProductTypeEnumList = new List<ProductTypeEnumModel>();
            }

            ProductWarehouseStatusReportParametersDto filters = new ProductWarehouseStatusReportParametersDto();
            filters.ProductTypes = BindingProductTypeEnumList.Select(t => t.Value).ToList();
            filters.Warehouses = BindingWarehouses;
            filters.StockCards = BindingProducts;
            filters.StartDate = StartDate;
            filters.EndDate = EndDate;

            var report = (await ProductReportsAppService.GetProductWarehouseStatusReport(filters, ProductLocalizer)).ToList();
            Report = new ProductWarehouseStatusReport();
            Report.DataSource = report;

        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
