using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos
{
    public class ListContractTrackingFichesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fiş No
        /// </summary>
        public string FicheNr { get; set; }
        /// <summary>
        /// Fiş Tarihi
        /// </summary>
        public DateTime FicheDate_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
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
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri No
        /// </summary>
        public string ProductionOrderNr { get; set; }
        /// <summary>
        /// Kalite Planı Cari Hesap ID
        /// </summary>
        public Guid? QualityPlanCurrentAccountCardID { get; set; }
        /// <summary>
        /// Kalite Planı Cari Hesap Kodu
        /// </summary>
        public string QualityPlanCurrentAccountCardCode { get; set; }

        /// <summary>
        /// Kalite Planı Cari Hesap Ünvanı
        /// </summary>
        public string QualityPlanCurrentAccountCardName { get; set; }
        /// <summary>
        /// Kalite Planı Müşteri Kodu
        /// </summary>
        public string QualityPlanCustomerCode { get; set; }

        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }

        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountCardName { get; set; }

        /// <summary>
        /// İş Tanımı Döküman No
        /// </summary>
        public string ContractQualityPlanDocumentNumber { get; set; }

        /// <summary>
        /// İş Tanımı Açıklama
        /// </summary>
        public string ContractQualityPlanDescription { get; set; }
        /// <summary>
        /// Adet
        /// </summary>
        public int Amount_ { get; set; }
        /// <summary>
        /// Gerçekleşen Adet
        /// </summary>
        public int OccuredAmount_ { get; set; }
        /// <summary>
        /// Tahmini Geliş Tarihi
        /// </summary>
        public DateTime EstimatedDate_ { get; set; }
    }
}
