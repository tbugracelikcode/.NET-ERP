using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos
{
    public class CreateProductionOrderChangeReportsDto : FullAuditedEntityDto
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
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid? LinkedProductionOrderID { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Hata Başlığı ID
        /// </summary>
        public Guid? UnsuitabilityItemsID { get; set; }
        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
    }
}
