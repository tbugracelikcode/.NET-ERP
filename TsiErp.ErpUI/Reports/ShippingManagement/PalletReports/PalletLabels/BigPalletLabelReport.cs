using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace TsiErp.ErpUI.Reports.ShippingManagement.PalletReports.PalletLabels
{
    public partial class BigPalletLabelReport : DevExpress.XtraReports.UI.XtraReport
    {
        public string PackageNo { get; set; }
        public string MarkingRefNo { get; set; }
        public string BarcodeNo { get; set; }

        public BigPalletLabelReport()
        {
            InitializeComponent();
        }

        private void xrLabel28_BeforePrint(object sender, CancelEventArgs e)
        {
            PackageNo = (sender as XRLabel).Text;
        }

        private void xrBarcode_BeforePrint(object sender, CancelEventArgs e)
        {
            XRBarCode barcode = sender as XRBarCode;

            barcode.Text = PackageNo;
        }

        private void xrBarCode3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRBarCode barcode = sender as XRBarCode;

            barcode.Text = BarcodeNo;
        }

        private void xrLabel34_BeforePrint(object sender, CancelEventArgs e)
        {
            MarkingRefNo = (sender as XRLabel).Text;
        }
    }
}
