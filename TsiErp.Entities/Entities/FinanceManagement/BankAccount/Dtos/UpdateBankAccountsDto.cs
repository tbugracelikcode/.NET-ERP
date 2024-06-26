using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos
{
    public class UpdateBankAccountsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Banka Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Banka Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Banka Hesap Adı
        /// </summary>
        public string BankAccountName { get; set; }
        /// <summary>
        /// Hesap No
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// IBAN No
        /// </summary>
        public string IBANNo { get; set; }
        /// <summary>
        /// SWIFT Kodu
        /// </summary>
        public string SWIFTCode { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BankBranchName { get; set; }
        /// <summary>
        /// Şube Numarası
        /// </summary>
        public string BankBranchNo { get; set; }
        /// <summary>
        /// Adres
        /// </summary>
        public string Address { get; set; }
    }
}
