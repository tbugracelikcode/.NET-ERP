using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos
{
    public class SelectBankAccountsDto : FullAuditedEntityDto
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
        /// Banka Talimat Açıklaması
        /// </summary>
        public string BankInstructionDescription { get; set; }
        /// <summary>
        /// SWIFT Kodu
        /// </summary>
        public string SWIFTCode { get; set; }
        /// <summary>
        /// Adres
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BankBranchName { get; set; }
        /// <summary>
        /// TL Hesap No
        /// </summary>
        public string TLAccountNo { get; set; }
        /// <summary>
        /// TL Hesap IBAN No
        /// </summary>
        public string TLAccountIBAN { get; set; }
        /// <summary>
        /// USD Hesap No
        /// </summary>
        public string USDAccountNo { get; set; }
        /// <summary>
        /// USD Hesap IBAN No
        /// </summary>
        public string USDAccountIBAN { get; set; }
        /// <summary>
        /// Euro Hesap No
        /// </summary>
        public string EuroAccountNo { get; set; }
        /// <summary>
        /// Euro Hesap IBAN No
        /// </summary>
        public string EuroAccountIBAN { get; set; }
        /// <summary>
        /// GBP Hesap No
        /// </summary>
        public string GBPAccountNo { get; set; }
        /// <summary>
        /// GBP Hesap IBAN No
        /// </summary>
        public string GBPAccountIBAN { get; set; }
    }
}
