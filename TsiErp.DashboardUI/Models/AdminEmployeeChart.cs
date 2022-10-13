namespace TsiErp.DashboardUI.Models
{
    public class AdminEmployeeChart
    {


        public List<string> PERSONELLER { get; set; }

        public decimal KULLANILABILIRLIK { get; set; }

        public decimal PERFORMANS { get; set; }

        public decimal KALITE { get; set; }

        public decimal OEE { get; set; }

        public string AY { get; set; }

        public decimal DIFFOEE { get; set; }

        public decimal DIFFAVA { get; set; }

        public decimal DIFFQUA { get; set; }

        public decimal DIFFPER { get; set; }
    }
}
