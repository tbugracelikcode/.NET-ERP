using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont.Dtos
{
    public class UpdateEximBankReeskontsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Yıl
        /// </summary>
        public int Year_ { get; set; }
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
        /// <summary>
        /// Anapara
        /// </summary>
        public decimal MainAmount { get; set; }
        /// <summary>
        /// Faiz
        /// </summary>
        public decimal InterestAmount { get; set; }
        /// <summary>
        /// Komisyon
        /// </summary>
        public decimal CommissionAmount { get; set; }
        /// <summary>
        /// Toplam
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Ödenen
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// Kalan
        /// </summary>
        public decimal RemainingAmount { get; set; }
    }
}
