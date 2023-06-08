using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos
{
    public class ListCalendarLinesParameterDto : FullAuditedEntityDto
    {
        public Guid CalendarID { get; set; }
    }
}
