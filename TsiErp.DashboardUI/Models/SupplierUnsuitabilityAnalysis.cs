namespace TsiErp.DashboardUI.Models
{
    public class SupplierUnsuitabilityAnalysis
    {
        /// <summary>
        /// Tedarikçi ID
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// Tedarikçi
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// Tedarikçi Uygunsuzluk ID 
        /// </summary>
        public int SupplierUnsuitabilityID { get; set; }
        /// <summary>
        /// Red Miktar 
        /// </summary>
        public int RefuseQuantity { get; set; }
        /// <summary>
        /// Uygunsuzluk Sebebi 
        /// </summary>
        public string UnsuitabilityReason { get; set; }
        /// <summary>
        /// Düzeltme
        /// </summary>
        public int Correction { get; set; }
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public int ToBeUsedAs { get; set; }
        /// <summary>
        /// Tedarikçi ile İrtibat
        /// </summary>
        public int ContactWithSupplier { get; set; }
        /// <summary>
        /// Toplam 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Toplam Sipariş
        /// </summary>
        public int TotalOrder { get; set; }
        /// <summary>
        /// Hata ID 
        /// </summary>
        public int ErrorID { get; set; }
        /// <summary>
        /// Oran 
        /// </summary>
        public double Percent { get; set; }
    }
}
