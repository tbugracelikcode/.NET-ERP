using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos
{
    public class ListContinentLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kıta ID
        /// </summary>
        public Guid ContinentID { get; set; }
    }
}
