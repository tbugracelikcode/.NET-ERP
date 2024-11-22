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
    }
}
