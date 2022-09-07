namespace TsiErp.DashboardUI.Models
{
    public class AdminContractUnsuitabilityAnalysisChart
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
        public double Percent { get; set; }
        /// <summary>
        /// Fason Tedarikçi
        /// </summary>
        public string ContractSupplier { get; set; }
        /// <summary>
        /// Fason Tedarikçi ID
        /// </summary>
        public int ContractSupplierID { get; set; }
        /// <summary>
        /// Fason Fiş Adet
        /// </summary>
        public int ContractReceiptQuantity { get; set; }
    }
}
