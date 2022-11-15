using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CalendarLine.Dtos
{
    public class SelectCalendarLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal ShiftOverTime { get; set; }
        /// <summary>
        /// Vardiya Süresi
        /// </summary>
        public decimal ShiftTime { get; set; }
        /// <summary>
        /// Planlanan Duruş Süresi
        /// </summary>
        public decimal PlannedHaltTimes { get; set; }
        /// <summary>
        /// Çalışılabilir Süre
        /// </summary>
        public decimal AvailableTime { get; set; }
        /// <summary>
        /// Çalışma takvimi ID
        /// </summary>
        public Guid? CalendarID { get; set; }
        /// <summary>
        /// Vardiya Adı
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// Vardiya Sırası
        /// </summary>
        public int ShiftOrder { get; set; }
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid? ShiftID { get; set; }
        /// <summary>
        /// İstasyon Adı
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
    }
}
