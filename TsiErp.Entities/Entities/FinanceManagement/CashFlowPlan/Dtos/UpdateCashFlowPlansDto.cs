using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos
{
    public class UpdateCashFlowPlansDto : IEntityDto
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
        /// Alıcı Banka Hesap ID
        /// </summary>
        public Guid? RecieverBankAccountID { get; set; }
        /// <summary>
        /// Gönderici Banka Hesap ID
        /// </summary>
        public Guid? TransmitterBankAccountID { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
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
