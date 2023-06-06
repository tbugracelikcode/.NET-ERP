using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.Department;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.SalesPrice;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.Employee
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

    }
}
