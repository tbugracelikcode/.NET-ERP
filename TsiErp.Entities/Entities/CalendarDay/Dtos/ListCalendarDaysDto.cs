using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.CalendarDay.Dtos
{
    public class ListCalendarDaysDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çalışma Takvimi ID
        /// </summary>
        public Guid CalendarID { get; set; }
        /// <summary>
        /// Gün
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public int CalendarDayStateEnum { get; set; }
        /// <summary>
        /// Renk Kodu
        /// </summary>
        public string ColorCode { get; set; }
    }
}
