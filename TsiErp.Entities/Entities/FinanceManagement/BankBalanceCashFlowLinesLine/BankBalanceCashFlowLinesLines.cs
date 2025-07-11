﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine
{
    public class BankBalanceCashFlowLinesLines : FullAuditedEntity
    {
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowLineID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid BankAccountID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid CurrencyID { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Bakiye Türü
        /// </summary>
        public CashFlowPlansBalanceTypeEnum CashFlowPlansBalanceType { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Tarih
        /// </summary>
        public string TransactionDescription { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// İşlem Türü
        /// </summary>
        public CashFlowPlansTransactionTypeEnum CashFlowPlansTransactionType { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Kur Tutar
        /// </summary>
        public decimal ExchangeAmount_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// Tekrarlayan
        /// </summary>
        public bool isRecurrent { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Satırın Satırı
        /// </summary>
        public Guid LinkedBankBalanceCashFlowLinesLineID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tekrarlama Bitiş Tarihi
        /// </summary>
        public DateTime RecurrentEndTime { get; set; }
    }
}
