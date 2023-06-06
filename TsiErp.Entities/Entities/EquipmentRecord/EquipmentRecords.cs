using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.Department;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.EquipmentRecord
{
    /// <summary>
    /// Cihaz Kayıtları
    /// </summary>
    public class EquipmentRecords : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cihaz Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cihaz Tanımı
        /// </summary>
        public string Name { get; set; }
        [SqlColumnType(MaxLength = 150, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ölçme Aralığı
        /// </summary>
        public string MeasuringRange { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ölçme Hassasiyeti
        /// </summary>
        public string MeasuringAccuracy { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Departman ID
        /// </summary>
        public Guid Department { get; set; }
        [SqlColumnType(MaxLength = 150, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Sorumlu
        /// </summary>
        public string Responsible { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Sıklık
        /// </summary>
        public string Frequency { get; set; }
        [SqlColumnType(MaxLength = 250, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Cihaz Seri No
        /// </summary>
        public string EquipmentSerialNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Kayıt Tarihi
        /// </summary>
        public DateTime RecordDate { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// İptal
        /// </summary>
        public bool Cancel { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// İptal Tarihi
        /// </summary>
        public DateTime? CancellationDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// İptal Nedeni
        /// </summary>
        public string CancellationReason { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        public bool IsActive { get; set; }


    }
}
