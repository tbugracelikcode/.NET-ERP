using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.TestManagement.SectorLine.Dtos
{
    public class ListSectorLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Sektör ID
        /// </summary>
        public Guid SectorID { get; set; }
    }
}
