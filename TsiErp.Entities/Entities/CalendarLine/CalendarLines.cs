using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.Station;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.CalendarLine
{
    /// <summary>
    /// Çalışma takvimi satırları
    /// </summary>
    public class CalendarLines : FullAuditedEntity
    {
        [Precision(18, 6)]

        /// <summary>
        /// Fazla Mesai Süresi
        /// </summary>
        public decimal ShiftOverTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Vardiya Süresi
        /// </summary>
        public decimal ShiftTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Planlanan Duruş Süresi
        /// </summary>
        public decimal PlannedHaltTimes { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Çalışılabilir Süre
        /// </summary>
        public decimal AvailableTime { get; set; }
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }
        /// <summary>
        /// Çalışma takvimi ID
        /// </summary>
        public Guid CalendarID { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Çalışma takvimi
        /// </summary>
        public Calendars Calendars { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
    }
}
