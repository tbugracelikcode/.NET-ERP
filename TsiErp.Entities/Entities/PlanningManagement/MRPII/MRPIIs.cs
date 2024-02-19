using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPII
{
    public class MRPIIs : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// MRPII Kodu
        /// </summary>
        public string Code { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Hesaplama Tarihi
        /// </summary>
        public DateTime CalculationDate { get; set; }
        [SqlColumnType( Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
