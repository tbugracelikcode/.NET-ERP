namespace TsiErp.ErpUI.Models.Dashboard
{
    public class ProductGroupsAnalysis
    {
        /// <summary>
        /// Ürün Grup ID 
        /// </summary>
        public Guid ProductGroupID { get; set; }
        /// <summary>
        /// Ürün Grubu 
        /// </summary>
        public string ProductGroupName { get; set; }
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public int PlannedQuantity { get; set; }
        /// <summary>
        /// Üretilen Adet
        /// </summary>
        public int TotalProduction { get; set; }
        /// <summary>
        /// Hurda Adet 
        /// </summary>
        public int TotalScrap { get; set; }
        /// <summary>
        /// Oran 
        /// </summary>
        public double Quality { get; set; }
        /// <summary>
        /// OEE 
        /// </summary>
        public decimal OEE { get; set; }
    }
}
