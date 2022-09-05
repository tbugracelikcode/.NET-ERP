using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.EquipmentRecord.Dtos
{
    public class ListEquipmentRecordsDto : FullAuditedEntityDto
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
        /// Departman
        /// </summary>
        public string DepartmentName { get; set; }
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
        public string? CancellationReason { get; set; }
    }
}
