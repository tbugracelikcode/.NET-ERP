namespace TsiErp.DashboardUI.Models
{
    public class AdminProductChart
    {
        /// <summary>
        /// Verimlilik
        /// </summary>
        public decimal OEE { get; set; }
        /// <summary>
        /// Ay
        /// </summary>
        public string Ay { get; set; }
        /// <summary>
        /// Hurda Yüzde 
        /// </summary>
        public double ScrapPercent { get; set; }

        public decimal DIFFSCRAPPERCENT { get; set; }

        public decimal PRODUCTION { get; set; }

        public int SCRAP { get; set; }
    }
}
