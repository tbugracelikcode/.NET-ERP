using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos
{
    public class ListCalendarLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal ShiftOverTime { get; set; }
        /// <summary>
        /// İş Merkezi ID
        /// </summary>
        public Guid? WorkCenterID { get; set; }
        /// <summary>
        /// Vardiya Süresi
        /// </summary>
        public decimal ShiftTime { get; set; }
        /// <summary>
        /// Çalışma Durumu
        /// </summary>
        public int WorkStatus { get; set; }
        /// <summary>
        /// Planlanan Bakım Süresi
        /// </summary>
        public decimal PlannedMaintenanceTime { get; set; }

        /// <summary>
        /// Bakım Türü
        /// </summary>
        public string MaintenanceType { get; set; }
        /// <summary>
        /// Planlanan Duruş Süresi
        /// </summary>
        public decimal PlannedHaltTimes { get; set; }
        /// <summary>
        /// Çalışılabilir Süre
        /// </summary>
        public decimal AvailableTime { get; set; }
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
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
    }
}
