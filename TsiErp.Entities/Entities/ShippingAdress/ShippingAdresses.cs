using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ShippingAdress
{
    /// <summary>
    /// Sevkiyat Adresleri
    /// </summary>
    public class ShippingAdresses : FullAuditedEntity
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
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Müşteri Kartı ID
        /// </summary>
        public Guid? CustomerCardID { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Adres 1
        /// </summary>
        public string Adress1 { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Adres 2
        /// </summary>
        public string Adress2 { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ülke
        /// </summary>
        public string Country { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Posta Kodu
        /// </summary>
        public string PostCode { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Telefon
        /// </summary>
        public string Phone { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// E-Posta
        /// </summary>
        public string EMail { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Faks
        /// </summary>
        public string Fax { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Varsayılan
        /// </summary>
        public bool _Default { get; set; }
    }
}
