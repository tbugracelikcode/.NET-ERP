namespace TsiErp.DashboardUI.Models
{
    public class AdminEmployeeChart
    {
        
        
        /// <summary>
        /// Üretim Adet 
        /// </summary>
        public decimal TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public decimal TotalScrap { get; set; }
        /// <summary>
        /// Planlanan Adet 
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Verimlilik
        /// </summary>
        public decimal Productivity { get; set; }
        /// <summary>
        /// Ay
        /// </summary>
        public string Ay { get; set; }
        /// <summary>
        /// Gerçekleşen Birim Süre
        /// </summary>
        public int OccuredUnitTime { get; set; }
        /// <summary>
        /// Planlanan Birim Süre
        /// </summary>
        public int PlannedUnitTime { get; set; }
    }
}
