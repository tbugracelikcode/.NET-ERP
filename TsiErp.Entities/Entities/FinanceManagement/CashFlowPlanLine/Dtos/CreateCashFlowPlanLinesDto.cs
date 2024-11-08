using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos
{
    public class CreateCashFlowPlanLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid CashFlowPlanID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Banka Hesap ID
        /// </summary>
        public Guid? BankAccountID { get; set; }
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
        /// Kur Tutar
        /// </summary>
        public decimal ExchangeAmount_ { get; set; }
    }
}
