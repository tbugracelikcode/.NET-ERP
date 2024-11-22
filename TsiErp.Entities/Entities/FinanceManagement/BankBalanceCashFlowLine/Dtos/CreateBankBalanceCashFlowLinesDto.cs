using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLinesLine.Dtos;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos
{
    public class CreateBankBalanceCashFlowLinesDto : FullAuditedEntityDto
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

        [NoDatabaseAction]
        public List<SelectBankBalanceCashFlowLinesLinesDto> SelectBankBalanceCashFlowLinesLines {  get; set; }  
    }
}
