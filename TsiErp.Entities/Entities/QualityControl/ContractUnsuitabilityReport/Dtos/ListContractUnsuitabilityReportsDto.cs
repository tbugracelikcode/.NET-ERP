using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos
{
    public class ListContractUnsuitabilityReportsDto : FullAuditedEntityDto
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
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        /// <summary>
        /// Uygunsuzluk İş Emri Oluşacak
        /// </summary>
        public bool IsUnsuitabilityWorkOrder { get; set; }


        /// <summary>
        /// Hata Başlığı ID
        /// </summary>
        public Guid UnsuitabilityItemsID { get; set; }

        /// <summary>
        /// Hata Başlığı Adı
        /// </summary>
        public string UnsuitabilityItemsName { get; set; }

        /// <summary>
        /// Uygun Olmayan Miktar
        /// </summary>
        public decimal UnsuitableAmount { get; set; }

        /// <summary>
        /// Aksiyon
        /// </summary>
        public string Action_ { get; set; }

        /// <summary>
        /// İş Emri Fiş No
        /// </summary>
        public string WorkOrderFicheNr { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }

        /// <summary>
        /// Fason Takip Fiş No
        /// </summary>
        public string ContractTrackingFicheNr { get; set; }

        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNr { get; set; }

        /// <summary>
        /// Cari Hesap Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }

        /// <summary>
        /// Cari Hesap Ünvanı
        /// </summary>
        public string CurrentAccountCardName { get; set; }
    }
}
