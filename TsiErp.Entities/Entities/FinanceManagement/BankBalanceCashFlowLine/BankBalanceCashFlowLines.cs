using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine
{
    public class BankBalanceCashFlowLines: FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(MaxLength = 100, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ay Yıl
        /// </summary>
        public string MonthYear { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar Akbank TL
        /// </summary>
        public decimal AmountAkbankTL { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar Akbank EUR
        /// </summary>
        public decimal AmountAkbankEUR { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar İş Bankası TL
        /// </summary>
        public decimal AmountIsBankTL { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar İş Bankası EUR
        /// </summary>
        public decimal AmountIsBankEUR { get; set; }

        [SqlColumnType(Nullable = false, MaxLength =20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tarih Renk
        /// </summary>
        public string Date_Color { get; set; }
        [SqlColumnType(Nullable = false, MaxLength = 20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Ay Yıl Renk
        /// </summary>
        public string MonthYearColor { get; set; }
        [SqlColumnType(Nullable = false, MaxLength = 20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tutar Akbank TL Renk
        /// </summary>
        public string AmountAkbankTLColor { get; set; }
        [SqlColumnType(Nullable = false, MaxLength = 20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tutar Akbank EUR Renk
        /// </summary>
        public string AmountAkbankEURColor { get; set; }
        [SqlColumnType(Nullable = false, MaxLength = 20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tutar İş Bankası TL Renk
        /// </summary>
        public string AmountIsBankTLColor { get; set; }
        [SqlColumnType(Nullable = false, MaxLength = 20, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Tutar İş Bankası EUR Renk
        /// </summary>
        public string AmountIsBankEURColor { get; set; }
    }
}
