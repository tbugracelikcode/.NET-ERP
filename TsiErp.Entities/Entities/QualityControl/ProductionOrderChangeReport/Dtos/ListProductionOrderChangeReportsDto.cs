using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos
{
    public class ListProductionOrderChangeReportsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Rapor Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid? LinkedProductionOrderID { get; set; }
        /// <summary>
        /// Bağlı Üretim Emri Fiş No
        /// </summary>
        public string LinkedProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Hata Başlığı ID
        /// </summary>
        public Guid? UnsuitabilityItemsID { get; set; }
        /// <summary>
        /// Hata Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemsName { get; set; }
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Satış Sipariş Fiş No
        /// </summary>
        public string SalesOrderFicheNo { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
    }
}
