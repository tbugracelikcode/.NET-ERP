using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos
{
    public class SelectContractTrackingFicheLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fason Takip Fişi ID
        /// </summary>
        public Guid ContractTrackingFicheID { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid? WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNr { get; set; }
        /// <summary>
        /// Ürün Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }

        /// <summary>
        /// Ürün Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Ürün Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// Satır no
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// İş İstasyonu Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// İş İstasyonu Açıklaması
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Gönderilecek
        /// </summary>
        public bool IsSent { get; set; }
    }
}
