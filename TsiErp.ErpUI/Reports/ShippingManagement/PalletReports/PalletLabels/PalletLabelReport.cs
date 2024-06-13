using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace TsiErp.ErpUI.Reports.ShippingManagement.PalletReports.PalletLabels
{
    public partial class PalletLabelReport : DevExpress.XtraReports.UI.XtraReport
    {

        public string PaletNo { get; set; }

        public PalletLabelReport()
        {
            InitializeComponent();
        }

        private void xrLabel28_BeforePrint(object sender, CancelEventArgs e)
        {
            PaletNo = (sender as XRLabel).Text;
        }

        private void xrBarcode_BeforePrint(object sender, CancelEventArgs e)
        {
            XRBarCode barcode = sender as XRBarCode;

            barcode.Text = PaletNo;
        }
    }
}
