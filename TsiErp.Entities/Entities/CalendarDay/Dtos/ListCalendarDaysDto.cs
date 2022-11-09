using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

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
        /// Çalışma Var mı?
        /// </summary>
        public bool IsWorkDay { get; set; }
        /// <summary>
        /// Çalışma Yok mu?
        /// </summary>
        public bool IsNotWorkDay { get; set; }
        /// <summary>
        /// Resmi Tatil mi?
        /// </summary>
        public bool IsOfficialHoliday { get; set; }
        /// <summary>
        /// Tatil mi?
        /// </summary>
        public bool IsHoliday { get; set; }
        /// <summary>
        /// Yarım Gün mü?
        /// </summary>
        public bool IsHalfDay { get; set; }
        /// <summary>
        /// Yükleme Günü mü?
        /// </summary>
        public bool IsShipmentDay { get; set; }
        /// <summary>
        /// Bakım günü mü?
        /// </summary>
        public bool IsMaintenance { get; set; }
    }
}
