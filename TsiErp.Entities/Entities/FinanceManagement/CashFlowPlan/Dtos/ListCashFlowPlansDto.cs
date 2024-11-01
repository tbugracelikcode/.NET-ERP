using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos
{
    public class ListCashFlowPlansDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Cari Hesap Ünvan
        /// </summary>
        public string CurrentAccountName { get; set; }
        /// <summary>
        /// Alıcı Banka Hesap ID
        /// </summary>
        public Guid? RecieverBankAccountID { get; set; }
        /// <summary>
        /// Alıcı Banka Hesap Adı
        /// </summary>
        public string RecieverBankAccountName { get; set; }
        /// <summary>
        /// Gönderici Banka Hesap ID
        /// </summary>
        public Guid? TransmitterBankAccountID { get; set; }
        /// <summary>
        /// Gönderici Banka Hesap Adı
        /// </summary>
        public string TransmitterBankAccountName { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        /// <summary>
        /// Para Birimi Kodu
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Tutar
        /// </summary>
        public decimal Amount_ { get; set; }
        /// <summary>
        /// Bakiye Türü
        /// </summary>
        public int CashFlowPlansBalanceType { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public string TransactionDescription { get; set; }
        /// <summary>
        /// İşlem Türü
        /// </summary>
        public int CashFlowPlansTransactionType { get; set; }
        /// <summary>
        /// Masraf Tutar
        /// </summary>
        public decimal ExpenseAmount_ { get; set; }
    }
}
