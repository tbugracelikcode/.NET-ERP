using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring
{
    public class EmployeeScorings : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Puantaj Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Ay
        /// </summary>
        public int Month_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Puantaj Durumu
        /// </summary>
        public EmployeeScoringsStateEnum ScoringState { get; set; }

        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime EndDate { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
