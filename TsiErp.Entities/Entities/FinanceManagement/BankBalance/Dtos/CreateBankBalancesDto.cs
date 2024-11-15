using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos
{
    public class CreateBankBalancesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Banka ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        /// <summary>
        /// Ay Yıl
        /// </summary>
        public string MonthYear { get; set; }
    }
}
