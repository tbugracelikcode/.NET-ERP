using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.CostManagement.CostPeriodLine
{
    public class CostPeriodLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Maliyet Periyodu ID
        /// </summary>
        public Guid CostPeriodID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Başlık
        /// </summary>
        public string Title_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount { get; set; }

    }
}
