using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont
{
    public class EximBankReeskonts : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid BankAccountID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Anapara
        /// </summary>
        public decimal MainAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Faiz
        /// </summary>
        public decimal InterestAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Komisyon
        /// </summary>
        public decimal CommissionAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Toplam
        /// </summary>
        public decimal TotalAmount { get; set; }

        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Ödenen
        /// </summary>
        public decimal PaidAmount { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kalan
        /// </summary>
        public decimal RemainingAmount { get; set; }
    }
}
