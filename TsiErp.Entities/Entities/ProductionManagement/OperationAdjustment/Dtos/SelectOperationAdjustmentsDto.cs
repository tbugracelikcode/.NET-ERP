using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos
{
    public class SelectOperationAdjustmentsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş Emri
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// İş Emri Numarası
        /// </summary>
        public string WorkOrderNr { get; set; }

        /// <summary>
        /// Ayar Yapan Kullanıcı
        /// </summary>
        public Guid AdjustmentUserId { get; set; }

        /// <summary>
        /// Ayar Yapan Kullanıcı Adı
        /// </summary>
        public string AdjustmentUserName { get; set; }

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
        /// Operator Id
        /// </summary>
        public string OperatorName { get; set; }

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
