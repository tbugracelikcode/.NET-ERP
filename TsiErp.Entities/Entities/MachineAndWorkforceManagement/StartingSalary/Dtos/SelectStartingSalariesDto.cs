using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos
{
    public class SelectStartingSalariesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ad
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public DateTime? Year_ { get; set; }

        [NoDatabaseAction]
        public List<SelectStartingSalaryLinesDto> SelectStartingSalaryLines { get; set; }
    }
}
