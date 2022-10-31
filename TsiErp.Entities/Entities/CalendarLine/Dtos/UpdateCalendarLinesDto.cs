using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CalendarLine.Dtos
{
    public class UpdateCalendarLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çalışma takvimi ID
        /// </summary>
        public Guid? CalenderID { get; set; }
    }
}
