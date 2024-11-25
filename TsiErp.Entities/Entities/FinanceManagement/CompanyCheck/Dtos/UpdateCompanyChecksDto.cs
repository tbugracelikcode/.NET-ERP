using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos
{
    public class UpdateCompanyChecksDto : FullAuditedEntityDto
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
        public int CompanyChecksState { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
        /// <summary>
        /// Ödenme Durumu
        /// </summary>
        public int CompanyChecksPaymentState { get; set; }
    }
}
