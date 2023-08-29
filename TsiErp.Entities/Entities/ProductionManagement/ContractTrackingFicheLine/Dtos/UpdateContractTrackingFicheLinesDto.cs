using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos
{
    public class UpdateContractTrackingFicheLinesDto: FullAuditedEntityDto
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
        /// Ürün Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
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
