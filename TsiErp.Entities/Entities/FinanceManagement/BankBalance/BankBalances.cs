using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalance
{
    public class BankBalances : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Banka ID
        /// </summary>
        public Guid BankAccountID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ay Yıl
        /// </summary>
        public string MonthYear { get; set; }
    }
}
