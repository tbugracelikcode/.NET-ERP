using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos
{
    public class SelectCompanyChecksDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Seri No
        /// </summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// Vade Tarihi
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public CompanyChecksStateEnum CompanyChecksState { get; set; }
        /// <summary>
        /// Durum Adı
        /// </summary>
        public string CompanyChecksStateName { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Cari Hesap Kod
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Hesap Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
        /// <summary>
        /// Banka Hesap Adı
        /// </summary>
        public string BankAccountName { get; set; }
        /// <summary>
        /// Ödenme Durumu
        /// </summary>
        public CompanyChecksPaymentStateEnum CompanyChecksPaymentState { get; set; }
        /// <summary>
        /// Ödenme Durumu Adı
        /// </summary>
        public string CompanyChecksPaymentStateName { get; set; }
    }
}
