namespace TsiErp.Business.Models.AdminDashboard
{
    public class AdminPurchaseUnsuitabilityChart
    {

        public int YEAR { get; set; }

        public string MONTH { get; set; }

        public decimal TOTALOCCUREDQUANTITY { get; set; }

        public decimal TOTALUNSUITABILITYQUANTITY { get; set; }

        public decimal TOTALSUPPLIERCONTACTQUANTITY { get; set; }

        public decimal TOTALREJECTQUANTITY { get; set; }

        public decimal TOTALCORRECTIONQUANTITY { get; set; }

        public decimal TOTALTOBEUSEDASQUANTITY { get; set; }

        public decimal UNSUITABILITYPERCENT { get; set; }
    }
}
