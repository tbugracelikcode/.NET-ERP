using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.CalendarLine.Dtos
{
    public class ListCalendarLinesDto : FullAuditedEntityDto
    {
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
        /// Çalışma Takvimi Satırı Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Çalışma Takvimi Satırı Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Vardiya Adı
        /// </summary>
        public string ShiftName { get; set; }
        /// <summary>
        /// Vardiya Sırası
        /// </summary>
        public int ShiftOrder { get; set; }
        /// <summary>
        /// İstasyon Adı
        /// </summary>
        public string StationName { get; set; }
    }
}
