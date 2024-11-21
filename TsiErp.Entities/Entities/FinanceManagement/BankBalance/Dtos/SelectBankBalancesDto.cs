using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;

namespace TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos
{
    public class SelectBankBalancesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Banka ID
        /// </summary>
        public Guid? BankAccountID { get; set; }

        [NoDatabaseAction]
        /// <summary>
        /// Banka Adı
        /// </summary>
        public string BankAccountName { get; set; }
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
