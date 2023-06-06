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
        /// <summary>
        /// Cihaz Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Cihaz Tanımı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ölçme Aralığı
        /// </summary>
        public string MeasuringRange { get; set; }
        /// <summary>
        /// Ölçme Hassasiyeti
        /// </summary>
        public string MeasuringAccuracy { get; set; }
        /// <summary>
        /// Departman ID
        /// </summary>
        public Guid Department { get; set; }
        /// <summary>
        /// Sorumlu
        /// </summary>
        public string Responsible { get; set; }
        /// <summary>
        /// Sıklık
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// Cihaz Seri No
        /// </summary>
        public string EquipmentSerialNo { get; set; }
        /// <summary>
        /// Kayıt Tarihi
        /// </summary>
        public DateTime RecordDate { get; set; }
        /// <summary>
        /// İptal
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// İptal Tarihi
        /// </summary>
        public DateTime? CancellationDate { get; set; }
        /// <summary>
        /// İptal Nedeni
        /// </summary>
        public string CancellationReason { get; set; }

        public bool IsActive { get; set; }


    }
}
