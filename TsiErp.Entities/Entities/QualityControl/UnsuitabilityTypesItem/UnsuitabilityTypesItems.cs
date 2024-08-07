using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem
{
    public class UnsuitabilityTypesItems : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Başlık Kodu
        /// </summary>
        public string Code { get; set; }

        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Başlık 
        /// </summary>
        public string Name { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama 
        /// </summary>
        public string Description_ { get; set; }

        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Uygunsuzluk Türü Açıklaması
        /// </summary>
        public string UnsuitabilityTypesDescription { get; set; }
    }
}
