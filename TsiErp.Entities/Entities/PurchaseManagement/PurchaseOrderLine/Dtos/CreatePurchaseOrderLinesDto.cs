﻿using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos
{
    public class CreatePurchaseOrderLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid PurchaseOrderID { get; set; }
        /// <summary>
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }
        /// <summary>
        /// Satın Alma Rezerve Miktarı
        /// </summary>
        public decimal PurchaseReservedQuantity { get; set; }
        /// <summary>
        /// Bekleyen Miktar
        /// </summary>
        public decimal WaitingQuantity { get; set; }
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
        /// Üretim Emri ID 
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Bağlı Satın Alma Talep Satır ID
        /// </summary>
        public Guid? LikedPurchaseRequestLineID { get; set; }
        /// <summary>
        /// Bağlı Satın Alma Talep ID
        /// </summary>
        public Guid? LinkedPurchaseRequestID { get; set; }

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
        public int PurchaseOrderLineStateEnum { get; set; }

        /// <summary>
        /// Satış Sipariş Satırı İrsaliye Durumu
        /// </summary>
        public int PurchaseOrderLineWayBillStatusEnum { get; set; }
        /// <summary>
        /// Üretim Emri Oluşturulma Tarihi
        /// </summary>
        public DateTime? WorkOrderCreationDate { get; set; }
        /// <summary>
        /// Temin Tarihi
        /// </summary>
        public DateTime? SupplyDate { get; set; }


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
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceID { get; set; }
        /// <summary>
        /// Sipariş Kabul Satır ID
        /// </summary>
        public Guid? OrderAcceptanceLineID { get; set; }
        /// <summary>
        /// Tedarikçi İrsaliye No
        /// </summary>
        public string SupplierWaybillNo { get; set; }
        /// <summary>
        /// Tedarikçi Fatura No
        /// </summary>
        public string SupplierBillNo { get; set; }

        /// <summary>
        /// Tedarikçi Referans No
        /// </summary>
        public string SupplierReferenceNo { get; set; }


        /// <summary>
        /// İşlem Dövizi Birim Fiyat
        /// </summary>
        public decimal TransactionExchangeUnitPrice { get; set; }
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
    }
}
