namespace TsiErp.ErpUI.Models.Dashboard
{
    public class AdminProductChart
    {
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

        public decimal SCRAP { get; set; }
    }
}
