using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial
{
    /// <summary>
    /// Reçeteler
    /// </summary>
    public class BillsofMaterials : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Reçete Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Reçete Adı
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid FinishedProductID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Genel Açıklama
        /// </summary>
        public string _Description { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }


    }
}
