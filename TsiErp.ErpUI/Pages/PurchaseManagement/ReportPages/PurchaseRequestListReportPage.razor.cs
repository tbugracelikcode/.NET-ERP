using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.ReportDtos.PurchaseRequestListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.PurchaseManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.PurchaseManagement.ReportPages
{
    public partial class PurchaseRequestListReportPage : IDisposable
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
            GetPurchaseRequestLineStateEnums();
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


        #region Purchase Request Line State

        List<PurchaseRequestLineStateEnumModel> PurchaseRequestLineStateEnumList = new List<PurchaseRequestLineStateEnumModel>();
        List<PurchaseRequestLineStateEnumModel> BindingPurchaseRequestLineStateEnumList = new List<PurchaseRequestLineStateEnumModel>();

        private void GetPurchaseRequestLineStateEnums()
        {
            var enumList = Enum.GetValues(typeof(PurchaseRequestLineStateEnum)).ToDynamicList<PurchaseRequestLineStateEnum>();

            foreach (var item in enumList)
            {
                var locKey = PurchaseRequestsLocalizer[Enum.GetName(typeof(PurchaseRequestLineStateEnum), item)];

                var locString = GetPurchaseRequestLineStateEnumStringKey(locKey);

                int purchaseRequestLineStateInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = PurchaseRequestsLocalizer.GetString(locString.Split('-')[1]);

                PurchaseRequestLineStateEnumList.Add(new PurchaseRequestLineStateEnumModel
                {
                    PurchaseRequestLineStateInt = purchaseRequestLineStateInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetPurchaseRequestLineStateEnumStringKey(LocalizedString stateString)
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
                case "SatinAlma":
                    result = "3-EnumPurchase";
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

            if (BindingPurchaseRequestLineStateEnumList == null)
            {
                BindingPurchaseRequestLineStateEnumList = new List<PurchaseRequestLineStateEnumModel>();
            }

            PurchaseRequestListReportParameterDto filters = new PurchaseRequestListReportParameterDto();
            filters.PurchaseRequestLineState = BindingPurchaseRequestLineStateEnumList.Select(t => t.Value).ToList();
            filters.Products = BindingProducts;
            filters.CurrentAccounts = BindingCurrentAccountCards;
            filters.StartDate = StartDate;
            filters.EndDate = EndDate;

            var report = (await PurchaseRequestReportsAppService.GetPurchaseRequestListReport(filters,ReportLocalizer ,ProductLocalizer)).ToList();

            if (report.Count > 0)
            {
                Report = new PurchaseRequsetListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new PurchaseRequsetListReport();
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

    public class PurchaseRequestLineStateEnumModel
    {
        public PurchaseRequestLineStateEnum Value { get; set; }

        public string Text { get; set; }

        public int PurchaseRequestLineStateInt { get; set; }
    }

}
