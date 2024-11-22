using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine.Dtos
{
    public class SelectBankBalanceCashFlowLinesLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowID { get; set; }
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowLineID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Cari Hesap Ünvan
        /// </summary>
        public string CurrentAccountName { get; set; }
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
        /// <summary>
        /// Banka Hesap Adı
        /// </summary>
        public string BankAccountName { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        /// <summary>
        /// Bakiye Türü
        /// </summary>
        public CashFlowPlansBalanceTypeEnum CashFlowPlansBalanceType { get; set; }
        /// <summary>
        /// Bakiye Türü Adı
        /// </summary>
        public string CashFlowPlansBalanceTypeName { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public string TransactionDescription { get; set; }
        /// <summary>
        /// İşlem Türü
        /// </summary>
        public CashFlowPlansTransactionTypeEnum CashFlowPlansTransactionType { get; set; }
        /// <summary>
        /// İşlem Türü Adı
        /// </summary>
        public string CashFlowPlansTransactionTypeName { get; set; }
        /// <summary>
        /// Kur Tutar
        /// </summary>
        public decimal ExchangeAmount_ { get; set; }
        /// <summary>
        /// Tekrarlayan
        /// </summary>
        public bool isRecurrent { get; set; }
        /// <summary>
        /// Bağlı Satırın Satırı
        /// </summary>
        public Guid? LinkedBankBalanceCashFlowLinesLineID { get; set; }
        /// <summary>
        /// Tekrarlama Bitiş Tarihi
        /// </summary>
        public DateTime? RecurrentEndTime { get; set; }
    }
}
