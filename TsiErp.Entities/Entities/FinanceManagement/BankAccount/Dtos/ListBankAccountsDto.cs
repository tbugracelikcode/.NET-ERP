using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos
{
    public class ListBankAccountsDto : FullAuditedEntityDto
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
        ///  Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        ///  Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// SWIFT Kodu
        /// </summary>
        public string SWIFTCode { get; set; }
        /// <summary>
        /// Adres
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Banka Talimat Açıklaması
        /// </summary>
        public string BankInstructionDescription { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BankBranchName { get; set; }
        /// <summary>
        ///  Hesap No
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        ///  Hesap IBAN No
        /// </summary>
        public string AccountIBAN { get; set; }
        /// <summary>
        ///  Banka Sıra No
        /// </summary>
        public int BankOrderNo { get; set; }
        /// <summary>
        ///  Banka Grup Adı
        /// </summary>
        public string BankGroupName { get; set; }
    }
}
