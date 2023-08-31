using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCComparing
{
    public class UnsuitabilityItemSPCComparings : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Uygunsuzluk Başlık ID
        /// </summary>
        public Guid UnsuitabilityItemID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih 1
        /// </summary>
        public DateTime Date1 { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih 2
        /// </summary>
        public DateTime Date2 { get; set; }
    }
}
