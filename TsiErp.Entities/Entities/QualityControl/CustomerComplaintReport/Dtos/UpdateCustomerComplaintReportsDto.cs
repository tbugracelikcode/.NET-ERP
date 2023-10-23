using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos
{
    public class UpdateCustomerComplaintReportsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Rapor No
        /// </summary>
        public string ReportNo { get; set; }

        /// <summary>
        /// Rapor Tarihi
        /// </summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// Satış Siparişi ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// İmalat Referans Numarası
        /// </summary>
        public string ProductionReferanceNumber { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Şikayet Başlığı ID
        /// </summary>
        public Guid? UnsuitqabilityItemsID { get; set; }

        /// <summary>
        /// Domain
        /// </summary>
        public string Domain_ { get; set; }

        /// <summary>
        /// Dosya Yolu
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 8D Raporu Oluşturulacak
        /// </summary>
        public bool is8DReport { get; set; }

        /// <summary>
        /// Rapor Durumu
        /// </summary>
        public string ReportState { get; set; }

        /// <summary>
        /// Sevk Edilen Miktar
        /// </summary>
        public decimal DeliveredQuantity { get; set; }

        /// <summary>
        /// Hatalı Miktar
        /// </summary>
        public decimal DefectedQuantity { get; set; }

        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        /// <summary>
        /// Rapor Sonucu
        /// </summary>
        public string ReportResult { get; set; }
    }
}
