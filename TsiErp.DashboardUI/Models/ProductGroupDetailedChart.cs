namespace TsiErp.DashboardUI.Models
{
    public class ProductGroupDetailedChart
    {
        /// <summary>
        /// Sebep
        /// </summary>
        public string ScrapCauseName { get; set; }
        /// <summary>
        /// PPM
        /// </summary>
        public decimal PPM { get; set; }
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public int TotalScrap { get; set; }
        /// <summary>
        /// Üretilen Adet 
        /// </summary>
        public int TotalProduction { get; set; }
    }
}
