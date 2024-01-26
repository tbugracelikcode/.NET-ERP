using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.StockManagement;

namespace TsiErp.ErpUI.Pages.StockManagement.ReportPages
{
    public partial class ProductListReportPage : IDisposable
    {

        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        protected override async void OnInitialized()
        {
            await GetProductGroups();
            GetProductTypeEnums();
            GetProductSupplyFormEnums();
        }

        #region Product Groups

        List<ListProductGroupsDto> ProductGoups = new List<ListProductGroupsDto>();
        List<Guid> BindingProductGroups = new List<Guid>();

        private async Task GetProductGroups()
        {
            ProductGoups = (await ProductGroupsAppService.GetListAsync(new ListProductGroupsParameterDto())).Data.ToList();
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


        #region Product Supply Form

        List<ProductSupplyFormEnumModel> ProductSupplyFormEnumList = new List<ProductSupplyFormEnumModel>();
        List<ProductSupplyFormEnumModel> BindingProductSupplyFormEnumList = new List<ProductSupplyFormEnumModel>();

        private void GetProductSupplyFormEnums()
        {
            var enumList = Enum.GetValues(typeof(ProductSupplyFormEnum)).ToDynamicList<ProductSupplyFormEnum>();

            foreach (var item in enumList)
            {
                var locKey = ProductLocalizer[Enum.GetName(typeof(ProductSupplyFormEnum), item)];

                var locString = GetProductSupplyFormEnumStringKey(locKey);

                int productSupplyInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = ProductLocalizer.GetString(locString.Split('-')[1]);

                ProductSupplyFormEnumList.Add(new ProductSupplyFormEnumModel
                {
                    ProductSupplyFormInt = productSupplyInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetProductSupplyFormEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "Satınalma":
                    result = "1-EnumPurchase";
                    break;
                case "Üretim":
                    result = "2-EnumProduction";
                    break;
            }

            return result;
        }
        #endregion

        private async void CreateReport()
        {
            if(BindingProductGroups==null)
            {
                BindingProductGroups = new List<Guid>();
            }

            if(BindingProductTypeEnumList==null)
            {
                BindingProductTypeEnumList = new List<ProductTypeEnumModel>();
            }

            if(BindingProductSupplyFormEnumList == null)
            {
                BindingProductSupplyFormEnumList = new List<ProductSupplyFormEnumModel>();
            }

            ProductListReportParametersDto filters = new ProductListReportParametersDto();
            filters.ProductGroups = BindingProductGroups;
            filters.ProductTypes = BindingProductTypeEnumList.Select(t=>t.Value).ToList();
            filters.ProductSupplyForms = BindingProductSupplyFormEnumList.Select(t => t.Value).ToList();

            var report = (await ProductReportsAppService.GetProductListReport(filters, ProductLocalizer)).ToList();

            Report = new ProductListReport();
            Report.DataSource = report;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }

    public class ProductTypeEnumModel
    {
        public ProductTypeEnum Value { get; set; }

        public string Text { get; set; }

        public int ProductTypeInt { get; set; }
    }

    public class ProductSupplyFormEnumModel
    {
        public ProductSupplyFormEnum Value { get; set; }

        public string Text { get; set; }

        public int ProductSupplyFormInt { get; set; }
    }
}
