using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class Employees : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İsim
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Soyisim
        /// </summary>
        public string Surname { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Departman
        /// </summary>
        public Guid DepartmentID { get; set; }
        [SqlColumnType(MaxLength = 11, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// TC Kimlik No
        /// </summary>
        public string IDnumber { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Doğum Günü
        /// </summary>
        public DateTime? Birthday { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Kan Grubu
        /// </summary>
        public BloodTypeEnum BloodType { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Adres
        /// </summary>
        public string Address { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// İlçe
        /// </summary>
        public string District { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Şehir
        /// </summary>
        public string City { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ev Telefonu
        /// </summary>
        public string HomePhone { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cep Telefonu
        /// </summary>
        public string CellPhone { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// EPosta
        /// </summary>
        public string Email { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Üretim Ekran Kullanıcısı
        /// </summary>
        public bool IsProductionScreenUser { get; set; }

        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretim Ekran Şifresi
        /// </summary>
        public string ProductionScreenPassword { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Üretim Ekran Ayar Kullanıcısı
        /// </summary>
        public bool IsProductionScreenSettingUser { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid SeniorityID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Eğitim Seviyesi ID
        /// </summary>
        public Guid EducationLevelID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Mevcut Maaş
        /// </summary>
        public decimal CurrentSalary { get; set; }
        [SqlColumnType( SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Görev Tanımı
        /// </summary>
        public string TaskDefinition { get; set; }

        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// İşe Alınma Tarihi
        /// </summary>
        public DateTime HiringDate { get; set; }

    }
}
