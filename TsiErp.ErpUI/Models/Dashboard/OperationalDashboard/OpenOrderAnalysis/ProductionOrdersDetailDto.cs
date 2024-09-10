namespace TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis
{
    public class ProductionOrdersDetailDto
    {
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }

        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// Mamül Kodu
        /// </summary>
        public string FinishedProductCode { get; set; }

        /// <summary>
        /// Mamül Açıklaması
        /// </summary>
        public string FinishedProductName { get; set; }

        /// <summary>
        /// Ürün Grubu
        /// </summary>
        public string ProductGroupName { get; set; }

        /// <summary>
        /// Üretim Emri Planlanan Miktar
        /// </summary>
        public int PlannedQuantity { get; set; }

        /// <summary>
        /// Teyit Edilen Yükleme Tarihi
        /// </summary>
        public DateTime ConfirmedLoadingDate { get; set; }

        /// <summary>
        /// Planlanan Sevk Tarihi
        /// </summary>
        public DateTime PlannedLoadingDate { get; set; }

        /// <summary>
        /// Aşık Son Operasyon
        /// </summary>
        public string AS { get; set; }

        /// <summary>
        /// Gövde Son Operasyon
        /// </summary>
        public string GV { get; set; }

        /// <summary>
        /// Mil Son Operasyon
        /// </summary>
        public string ML { get; set; }

        /// <summary>
        /// Burç Son Operasyon
        /// </summary>
        public string BR { get; set; }

        /// <summary>
        /// Pul Son Operasyon
        /// </summary>
        public string PL { get; set; }

        /// <summary>
        /// Sac Son Operasyon
        /// </summary>
        public string SC { get; set; }

    }
}
