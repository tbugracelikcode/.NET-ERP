using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Enums;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.CalendarDay
{
    public class CalendarDays : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Çalışma Takvimi ID
        /// </summary>
        public Guid CalendarID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Gün
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public int CalendarDayStateEnum { get; set; }
        [SqlColumnType(MaxLength = 50, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Renk Kodu
        /// </summary>
        public string ColorCode { get; set; }
    }
}
