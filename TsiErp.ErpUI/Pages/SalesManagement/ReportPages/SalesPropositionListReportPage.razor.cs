using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.ReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Reports.SalesManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.SalesManagement.ReportPages
{
    public partial class SalesPropositionListReportPage : IDisposable
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
            GetSalesPropositionLineStateEnums();
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

        List<SalesPropositionLineStateEnumModel> SalesPropositionLineStateEnumList = new List<SalesPropositionLineStateEnumModel>();
        List<SalesPropositionLineStateEnumModel> BindingSalesPropositionLineStateEnumList = new List<SalesPropositionLineStateEnumModel>();

        private void GetSalesPropositionLineStateEnums()
        {
            var enumList = Enum.GetValues(typeof(SalesPropositionLineStateEnum)).ToDynamicList<SalesPropositionLineStateEnum>();

            foreach (var item in enumList)
            {
                var locKey = SalesPropositionsLocalizer[Enum.GetName(typeof(SalesPropositionLineStateEnum), item)];

                var locString = GetSalesPropositionLineStateEnumStringKey(locKey);

                int SalesPropositionLineStateInt = Convert.ToInt32(locString.Split('-')[0]);

                string text = SalesPropositionsLocalizer.GetString(locString.Split('-')[1]);

                SalesPropositionLineStateEnumList.Add(new SalesPropositionLineStateEnumModel
                {
                    SalesPropositionLineStateInt = SalesPropositionLineStateInt,
                    Text = text,
                    Value = item
                });
            }
        }

        public static string GetSalesPropositionLineStateEnumStringKey(LocalizedString stateString)
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
                case "Siparis":
                    result = "3-EnumOrder";
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

            if (BindingSalesPropositionLineStateEnumList == null)
            {
                BindingSalesPropositionLineStateEnumList = new List<SalesPropositionLineStateEnumModel>();
            }

            SalesPropositionListReportParameterDto filters = new SalesPropositionListReportParameterDto();
            filters.SalesPropositionLineState = BindingSalesPropositionLineStateEnumList.Select(t => t.Value).ToList();
            filters.Products = BindingProducts;
            filters.CurrentAccounts = BindingCurrentAccountCards;
            filters.StartDate = StartDate;
            filters.EndDate = EndDate;

            var report = (await SalesPropositionReportsAppService.GetSalesPropositionListReport(filters, ReportLocalizer)).ToList();

            if (report.Count > 0)
            {
                Report = new SalesPropositionListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new SalesPropositionListReport();
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

    public class SalesPropositionLineStateEnumModel
    {
        public SalesPropositionLineStateEnum Value { get; set; }

        public string Text { get; set; }

        public int SalesPropositionLineStateInt { get; set; }
    }
}
