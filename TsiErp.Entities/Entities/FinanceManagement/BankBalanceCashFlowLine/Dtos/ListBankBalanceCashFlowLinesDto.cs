using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos
{
    public class ListBankBalanceCashFlowLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Nakit Akış ID
        /// </summary>
        public Guid BankBalanceCashFlowID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Ay Yıl
        /// </summary>
        public string MonthYear { get; set; }
        /// <summary>
        /// Tutar Akbank TL
        /// </summary>
        public decimal AmountAkbankTL { get; set; }
        /// <summary>
        /// Tutar Akbank EUR
        /// </summary>
        public decimal AmountAkbankEUR { get; set; }
        /// <summary>
        /// Tutar İş Bankası TL
        /// </summary>
        public decimal AmountIsBankTL { get; set; }
        /// <summary>
        /// Tutar İş Bankası EUR
        /// </summary>
        public decimal AmountIsBankEUR { get; set; }
        /// <summary>
        /// Tarih Renk
        /// </summary>
        public string Date_Color { get; set; }
        /// <summary>
        /// Ay Yıl Renk
        /// </summary>
        public string MonthYearColor { get; set; }
        /// <summary>
        /// Tutar Akbank TL Renk
        /// </summary>
        public string AmountAkbankTLColor { get; set; }
        /// <summary>
        /// Tutar Akbank EUR Renk
        /// </summary>
        public string AmountAkbankEURColor { get; set; }
        /// <summary>
        /// Tutar İş Bankası TL Renk
        /// </summary>
        public string AmountIsBankTLColor { get; set; }
        /// <summary>
        /// Tutar İş Bankası EUR Renk
        /// </summary>
        public string AmountIsBankEURColor { get; set; }
    }
}
