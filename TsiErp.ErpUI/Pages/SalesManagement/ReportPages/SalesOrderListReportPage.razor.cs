using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Business.Entities.SalesManagement.SalesOrder.Reports;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.ReportDtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.ReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.SalesManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.ReportPages
{
    public partial class SalesOrderListReportPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        protected override async void OnInitialized()
        {
            await GetProducts();
            await GetCurrentAccountCards();
            GetSalesOrderLineStateEnums();
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

        #region Sales Order Line State

        List<SalesOrderLineStateEnumModel> SalesOrderLineStateEnumList = new List<SalesOrderLineStateEnumModel>();
        List<SalesOrderLineStateEnumModel> BindingSalesOrderLineStateEnumList = new List<SalesOrderLineStateEnumModel>();

        private void GetSalesOrderLineStateEnums()
        {
            var enumList = Enum.GetValues(typeof(SalesOrderLineStateEnum)).ToDynamicList<SalesOrderLineStateEnum>();

            foreach (var item in enumList)
            {
                var locKey = SalesOrdersLocalizer[Enum.GetName(typeof(SalesOrderLineStateEnum), item)];

                var locString = GetSalesOrderLineStateEnumStringKey(locKey);

                int SalesOrderLineStateInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = SalesOrdersLocalizer.GetString(locString.Split('-')[1]);

                SalesOrderLineStateEnumList.Add(new SalesOrderLineStateEnumModel
                {
                    SalesOrderLineStateInt = SalesOrderLineStateInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetSalesOrderLineStateEnumStringKey(LocalizedString stateString)
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
                case "UretimeVerildi":
                    result = "3-EnumInProduction";
                    break;
                case "Iptal":
                    result = "4-EnumCancel";
                    break;
                case "SevkEdildi":
                    result = "5-EnumTransfer";
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

            if (BindingSalesOrderLineStateEnumList == null)
            {
                BindingSalesOrderLineStateEnumList = new List<SalesOrderLineStateEnumModel>();
            }

            SalesOrderListReportParameterDto filters = new SalesOrderListReportParameterDto();
            filters.SalesOrderLineState = BindingSalesOrderLineStateEnumList.Select(t => t.Value).ToList();
            filters.Products = BindingProducts;
            filters.CurrentAccounts = BindingCurrentAccountCards;
            filters.StartDate = StartDate;
            filters.EndDate = EndDate;

            var report = (await SalesOrderReportsAppService.GetSalesOrderListReport(filters, ReportLocalizer)).ToList();

            if (report.Count > 0)
            {
                Report = new SalesOrderListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new SalesOrderListReport();
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

    public class SalesOrderLineStateEnumModel
    {
        public SalesOrderLineStateEnum Value { get; set; }

        public string Text { get; set; }

        public int SalesOrderLineStateInt { get; set; }
    }
}
