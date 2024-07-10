using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.ReportDtos;

namespace TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.PackingList
{
    public partial class PackingListTrReport : DevExpress.XtraReports.UI.XtraReport
    {
        public List<PackingListPalletQuantityReportDto> PackingListPalletQuantityReportDto { get; set; }

        public PackingListTrReport()
        {
            InitializeComponent();

            PackingListPalletQuantityReportDto = new List<PackingListPalletQuantityReportDto>();
        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            ((XRSubreport)sender).ReportSource.DataSource = PackingListPalletQuantityReportDto;
        }
    }
}
