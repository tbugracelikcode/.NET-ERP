﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos
{
    public class SelectPurchaseRequestLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İşlem Dövizi Birim Fiyat
        /// </summary>
        public decimal TransactionExchangeUnitPrice { get; set; }
        /// <summary>
        /// Satın Alma Rezerve Miktarı
        /// </summary>
        public decimal PurchaseReservedQuantity { get; set; }
        /// <summary>
        /// Bekleyen Miktar
        /// </summary>
        public decimal WaitingQuantity { get; set; }
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
        /// Satın Alma Talep ID
        /// </summary>
        public Guid? PurchaseRequestID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Adı
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Birim Seti Kodu
        /// </summary>
        public string UnitSetCode { get; set; }


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
        /// Ödeme Planı Adı
        /// </summary>
        public string PaymentPlanName { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }
        /// <summary>
        /// Satış Teklif Satırı Durumu
        /// </summary>
        public PurchaseRequestLineStateEnum PurchaseRequestLineState { get; set; }
        /// <summary>
        /// Siparişe Çevirilme Tarihi
        /// </summary>
        public DateTime? OrderConversionDate { get; set; }

        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid BranchID { get; set; }

        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Şube Adı
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid WarehouseID { get; set; }


        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Depo Adı
        /// </summary>
        public string WarehouseName { get; set; }


        /// <summary>
        /// Cari Hesap Kartı ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }

        /// <summary>
        /// Cari Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
        /// <summary>
        /// Cari Ünvan
        /// </summary>
        public string CurrentAccountCardName { get; set; }

        /// <summary>
        /// Tedarikçi Referans No
        /// </summary>
        public string SupplierReferenceNo { get; set; }
    }
}
