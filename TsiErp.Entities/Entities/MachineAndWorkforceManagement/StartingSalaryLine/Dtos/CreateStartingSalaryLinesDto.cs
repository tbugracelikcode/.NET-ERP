using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos
{
    public class CreateStartingSalaryLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Başlangıç Maaş ID
        /// </summary>
        public Guid StartingSalaryID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Kıdem ID
        /// </summary>
        public Guid? SeniorityID { get; set; }
        /// <summary>
        /// Mevcut Başlangıç Maaş
        /// </summary>
        public decimal CurrentStartingSalary { get; set; }
        /// <summary>
        /// Güncel Maaş Alt Sınır
        /// </summary>
        public decimal CurrentSalaryLowerLimit { get; set; }
        /// <summary>
        /// Güncel Maaş Üst Sınır
        /// </summary>
        public decimal CurrentSalaryUpperLimit { get; set; }
        /// <summary>
        /// Fark
        /// </summary>
        public decimal Difference { get; set; }
    }
}
