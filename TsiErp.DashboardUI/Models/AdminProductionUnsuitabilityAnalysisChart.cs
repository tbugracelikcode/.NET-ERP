namespace TsiErp.DashboardUI.Models
{
    public class AdminProductionUnsuitabilityAnalysisChart
    {
        
        /// <summary>
        /// Toplam 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Ay 
        /// </summary>
        public string Ay { get; set; }
        /// <summary>
        /// Oran 
        /// </summary>
        public decimal Percent { get; set; }

        public int UNSUITABILITY { get; set; }

        public decimal PRODUCTION { get; set; }

        public decimal DIFFUNS { get; set; }
    }
}
