using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice
{
    public class PurchasePrices : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Onay
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
