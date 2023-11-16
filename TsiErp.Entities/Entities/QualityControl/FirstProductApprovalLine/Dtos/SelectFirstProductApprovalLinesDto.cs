using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos
{
    public class SelectFirstProductApprovalLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İlk Ürün Onay ID
        /// </summary>
        public Guid FirstProductApprovalID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Kritik Ölçü
        /// </summary>
        public bool IsCriticalMeasurement { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Olması Gereken Ölçü
        /// </summary>
        public decimal IdealMeasure { get; set; }
        /// <summary>
        /// İlk Üürn Onay Kodu
        /// </summary>
        public string MeasurementValue { get; set; }
        /// <summary>
        /// Alt Tolerans
        /// </summary>
        public decimal BottomTolerance { get; set; }
        /// <summary>
        /// Üst Tolerans
        /// </summary>
        public decimal UpperTolerance { get; set; }
    }
}
