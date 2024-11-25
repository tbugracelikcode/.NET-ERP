using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.FinanceManagement.CompanyCheck
{
    public class CompanyChecks : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 200, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Seri No
        /// </summary>
        public string SerialNo { get; set; }
        [SqlColumnType( Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Vade Tarihi
        /// </summary>
        public DateTime DueDate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Durum
        /// </summary>
        public CompanyChecksStateEnum CompanyChecksState { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid BankAccountID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Ödenme Durumu
        /// </summary>
        public CompanyChecksPaymentStateEnum CompanyChecksPaymentState { get; set; }
    }
}
