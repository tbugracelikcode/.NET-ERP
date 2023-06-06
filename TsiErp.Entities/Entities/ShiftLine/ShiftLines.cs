using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Enums;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.ShiftLine
{
    /// <summary>
    /// Vardiya Satırları
    /// </summary>
    public class ShiftLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Time)]
        /// <summary>
        /// Başlangıç Saati
        /// </summary>
        public TimeSpan? StartHour { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Time)]
        /// <summary>
        /// Bitiş Saati
        /// </summary>
        public TimeSpan? EndHour { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Tür Enum
        /// </summary>
        public ShiftLinesTypeEnum Type { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Katsayı
        /// </summary>
        public decimal Coefficient { get; set; }


    }
}
