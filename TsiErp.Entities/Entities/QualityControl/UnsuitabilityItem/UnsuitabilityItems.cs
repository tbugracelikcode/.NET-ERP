using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem
{
    public class UnsuitabilityItems : FullAuditedEntity
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


        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Uygunsuzluk Türü Başlığı 
        /// </summary>
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        public Guid UnsuitabilityTypesItemsId { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Hayati Tehlike
        /// </summary>
        public string LifeThreatening { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Prestij Kaybı
        /// </summary>
        public string LossOfPrestige { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ekstra Maliyet
        /// </summary>
        public string ExtraCost { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ürün Ömrü Kısalması
        /// </summary>
        public string ProductLifeShortening { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Farkedilebilirlik
        /// </summary>
        public string Detectability { get; set; }

        [SqlColumnType(MaxLength = 5, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Olduğu Gibi Kullanılacak
        /// </summary>
        public string ToBeUsedAs { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Şiddet Aralığı
        /// </summary>
        public int IntensityRange { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Şiddet Kat Sayısı
        /// </summary>
        public int IntensityCoefficient { get; set; }
    }
}
