namespace TsiErp.DashboardUI.Models
{
    public class AdminMachineChart
    {
        public List<string> ISTASYONLAR { get; set; }

        public decimal KULLANILABILIRLIK { get; set; }

        public decimal PERFORMANS { get; set; }

        public decimal KALITE { get; set; }

        public decimal OEE { get; set; }

        public string AY { get; set; }
    }
}
