using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.User
{
    /// <summary>
    /// Kullanıcılar
    /// </summary>
    public class Users : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kullanıcı Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 300, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string UserName { get; set; }
        [SqlColumnType(MaxLength = 300, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        ///  Adı Soyadı
        /// </summary>
        public string NameSurname { get; set; }
        [SqlColumnType(MaxLength = 300, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        ///  E-Posta
        /// </summary>
        public string Email { get; set; }
        
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        ///  Parola
        /// </summary>
        public string Password { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Grup ID
        /// </summary>
        public Guid GroupID { get; set; }
    }
}
