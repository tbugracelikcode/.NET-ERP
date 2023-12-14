using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos
{
    public class UpdateOperationAdjustmentsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş Emri
        /// </summary>
        public Guid? WorkOrderId { get; set; }

        /// <summary>
        /// Ayar Yapan Kullanıcı
        /// </summary>
        public Guid? AdjustmentUserId { get; set; }

        /// <summary>
        /// Ayar Başlangıç Tarihi
        /// </summary>
        public DateTime? AdjustmentStartDate { get; set; }

        /// <summary>
        /// Toplam Ayar Süresi
        /// </summary>
        public int TotalAdjustmentTime { get; set; }

        /// <summary>
        /// Toplam Kalite Kontrol Onay Süresi
        /// </summary>
        public int TotalQualityControlApprovedTime { get; set; }

        /// <summary>
        /// Operator Id
        /// </summary>
        public Guid OperatorId { get; set; }

        /// <summary>
        /// Hurda Adet
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        /// <summary>
        /// Onaylanan Adet
        /// </summary>
        public decimal ApprovedQuantity { get; set; }
    }
}
