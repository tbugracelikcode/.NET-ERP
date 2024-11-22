using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum CashFlowPlansTransactionTypeEnum
    {
        [Display(Name = "TransactionTypeIncome")]
        GelenOdeme = 1,
        [Display(Name = "TransactionTypePayment")]
        YapilacakOdeme = 2,
        [Display(Name = "TransactionTypeTransferBetweenBanks")]
        BankaHesaplarArasiVirman = 3,
        [Display(Name = "TransactionTypeExchangePurchase")]
        DovizAlis = 4,
        [Display(Name = "TransactionTypeExchangeSales")]
        DovizSatis = 5,
        [Display(Name = "TransactionTypeExpenseAmount")]
        MasrafTutari = 6,
        [Display(Name = "TransactionTypeCreditPayment")]
        KrediOdemesi = 7,
    }
}
