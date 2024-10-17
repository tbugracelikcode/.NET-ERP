using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos
{
    public class CreateSalesOrderLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İşlem Dövizi Birim Fiyat
        /// </summary>
        public decimal TransactionExchangeUnitPrice { get; set; }
        /// <summary>
        /// Stok Grup ID
        /// </summary>
        public Guid? ProductGroupID { get; set; }
        /// <summary>
        /// İşlem Dövizi KDV Tutarı
        /// </summary>
        public decimal TransactionExchangeVATamount { get; set; }
        /// <summary>
        /// İşlem Dövizi Satır Tutarı
        /// </summary>
        public decimal TransactionExchangeLineAmount { get; set; }
        /// <summary>
        /// İşlem Dövizi Satır Toplar Tutarı
        /// </summary>
        public decimal TransactionExchangeLineTotalAmount { get; set; }
        ///<summary>
        /// İşlem Dövizi İndirim Tutarı
        /// </summary>
        public decimal TransactionExchangeDiscountAmount { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Bağlı Teklif Satır ID
        /// </summary>
        public Guid LikedPropositionLineID { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// İndirim Oranı
        /// </summary>
        public decimal DiscountRate { get; set; }

        ///<summary>
        /// İndirim Tutarı
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        /// <summary>
        /// Satır Toplar Tutarı
        /// </summary>
        public decimal LineTotalAmount { get; set; }
        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }
        /// <summary>
        /// KDV Oranı
        /// </summary>
        public int VATrate { get; set; }

        /// <summary>
        /// KDV Tutarı
        /// </summary>
        public decimal VATamount { get; set; }

        /// <summary>
        /// Kur Turarı
        /// </summary>
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Ödeme Planı ID
        /// </summary>
        public Guid? PaymentPlanID { get; set; }
        /// <summary>
        /// Satış Sipariş Satırı Durumu
        /// </summary>
        public int SalesOrderLineStateEnum { get; set; }
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }

        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }

        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }


        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        /// <summary>
        /// Satın Alma Temin Tarihi
        /// </summary>
        public DateTime? PurchaseSupplyDate { get; set; }
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceRecordID { get; set; }
        /// <summary>
        /// Sipariş Kabul Satır ID
        /// </summary>
        public Guid? OrderAcceptanceRecordLineID { get; set; }
    }
}
