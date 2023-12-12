using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment
{
    public class OperationAdjustments : FullAuditedEntity
    {
      
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// İş Emri
        /// </summary>
        public Guid WorkOrderId { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ayar Yapan Kullanıcı
        /// </summary>
        public Guid AdjustmentUserId { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Ayar Başlangıç Tarihi
        /// </summary>
        public DateTime? AdjustmentStartDate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Ayar Süresi
        /// </summary>
        public int TotalAdjustmentTime { get; set; }


        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Onaylanan Adet
        /// </summary>
        public decimal ApprovedQuantity { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Hurda Adet
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Operator Id
        /// </summary>
        public Guid OperatorId { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Kalite Kontrol Onay Süresi
        /// </summary>
        public int TotalQualityControlApprovedTime { get; set; }

    }
}
