using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning
{
    public class ShipmentPlannings : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Sevkiyat Planlama Tarihi
        /// </summary>
        /// 
        public DateTime ShipmentPlanningDate { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Net KG
        /// </summary>
        public decimal TotalNetKG { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam Brüt KG
        /// </summary>
        public decimal TotalGrossKG { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int TotalAmount { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Toplam Adet
        /// </summary>
        public int PlannedAmount { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Planlanan Yükleme Tarihi
        /// </summary>
        /// 
        public DateTime? PlannedLoadingTime { get; set; }
    }
}
