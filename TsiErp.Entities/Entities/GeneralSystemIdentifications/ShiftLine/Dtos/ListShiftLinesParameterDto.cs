using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos
{
    public class ListShiftLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }
    }
}
