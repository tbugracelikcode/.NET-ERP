using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos
{
    public class ListContractTrackingFicheLinesDto:FullAuditedEntityDto
    {
        
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNr { get; set; }
        /// <summary>
        /// İş İstasyonu Kodu
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// Ürün Operasyon Kodu
        /// </summary>
        public string OperationCode { get; set; }

        /// <summary>
        /// Ürün Operasyon Adı
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// İş İstasyonu Açıklaması
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Gönderilecek
        /// </summary>
        public bool IsSent { get; set; }
        /// <summary>
        /// Satır no
        /// </summary>
        public int LineNr { get; set; }
    }
}
