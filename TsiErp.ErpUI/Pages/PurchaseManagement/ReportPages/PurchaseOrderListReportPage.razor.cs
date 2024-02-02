using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Business.Entities.PurchaseManagement.PurchaseOrder.Reports;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.ReportDtos.PurchaseOrderListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.PurchaseManagement;

namespace TsiErp.ErpUI.Pages.PurchaseManagement.ReportPages
{
    public partial class PurchaseOrderListReportPage : IDisposable
    {
        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        protected override async void OnInitialized()
        {
            await GetProducts();
            await GetCurrentAccountCards();
            GetPurchaseOrderLineStateEnums();
        }


        #region Products

        List<ListProductsDto> Products = new List<ListProductsDto>();
        List<Guid> BindingProducts = new List<Guid>();

        private async Task GetProducts()
        {
            Products = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        #endregion

        #region Current Accounts

        List<ListCurrentAccountCardsDto> CurrentAccountCards = new List<ListCurrentAccountCardsDto>();
        List<Guid> BindingCurrentAccountCards = new List<Guid>();

        private async Task GetCurrentAccountCards()
        {
            CurrentAccountCards = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.ToList();
        }

        #endregion

        #region Purchase Order Line State

        List<PurchaseOrderLineStateEnumModel> PurchaseOrderLineStateEnumList = new List<PurchaseOrderLineStateEnumModel>();
        List<PurchaseOrderLineStateEnumModel> BindingPurchaseOrderLineStateEnumList = new List<PurchaseOrderLineStateEnumModel>();

        private void GetPurchaseOrderLineStateEnums()
        {
            var enumList = Enum.GetValues(typeof(PurchaseOrderLineStateEnum)).ToDynamicList<PurchaseOrderLineStateEnum>();

            foreach (var item in enumList)
            {
                var locKey = PurchaseOrdersLocalizer[Enum.GetName(typeof(PurchaseOrderLineStateEnum), item)];

                var locString = GetPurchaseOrderLineStateEnumStringKey(locKey);

                int purchaseOrderLineStateInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = PurchaseOrdersLocalizer.GetString(locString.Split('-')[1]);

                PurchaseOrderLineStateEnumList.Add(new PurchaseOrderLineStateEnumModel
                {
                    PurchaseOrderLineStateInt = purchaseOrderLineStateInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetPurchaseOrderLineStateEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "Beklemede":
                    result = "1-EnumWaiting";
                    break;
                case "Onaylandı":
                    result = "2-EnumApproved";
                    break;
                case "Tamamlandi":
                    result = "3-EnumCompleted";
                    break;
                case "Iptal":
                    result = "4-EnumCancel";
                    break;
                case "KismiTamamlandi":
                    result = "5-EnumInPartialCompleted";
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

            if (BindingCurrentAccountCards == null)
            {
                BindingCurrentAccountCards = new List<Guid>();
            }

            if (BindingPurchaseOrderLineStateEnumList == null)
            {
                BindingPurchaseOrderLineStateEnumList = new List<PurchaseOrderLineStateEnumModel>();
            }

            PurchaseOrderListReportParameterDto filters = new PurchaseOrderListReportParameterDto();
            filters.PurchaseOrderLineState = BindingPurchaseOrderLineStateEnumList.Select(t => t.Value).ToList();
            filters.Products = BindingProducts;
            filters.CurrentAccounts = BindingCurrentAccountCards;
            filters.StartDate = StartDate;
            filters.EndDate = EndDate;

            var report = (await PurchaseOrderReportsAppService.GetPurchaseOrderListReport(filters, ReportLocalizer, ProductLocalizer)).ToList();
            Report = new PurchaseOrderListReport();
            Report.DataSource = report;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }

    public class PurchaseOrderLineStateEnumModel
    {
        public PurchaseOrderLineStateEnum Value { get; set; }

        public string Text { get; set; }

        public int PurchaseOrderLineStateInt { get; set; }
    }
}
