﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos
{
    public class UpdateStockFicheLinesDto : FullAuditedEntity
    {
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }

        /// <summary>
        /// Satış Sipariş Satır ID
        /// </summary>
        public Guid? SalesOrderLineID { get; set; }
        /// <summary>
        /// Satış Fatura ID
        /// </summary>
        public Guid? SalesInvoiceID { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans ID
        /// </summary>
        public Guid? ProductionDateReferenceID { get; set; }

        /// <summary>
        /// Satın Alma Fatura ID
        /// </summary>
        public Guid? PurchaseInvoiceID { get; set; }

        /// <summary>
        /// Satış Fatura Satır ID
        /// </summary>
        public Guid? SalesInvoiceLineID { get; set; }

        /// <summary>
        /// Satın Alma Fatura Satır ID
        /// </summary>
        public Guid? PurchaseInvoiceLineID { get; set; }

        /// <summary>
        /// Giriş Çıkış Kodu
        /// </summary>
        public int InputOutputCode { get; set; }


        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Parti No
        /// </summary>
        public string PartyNo { get; set; }
        /// <summary>
        /// İşlem Dövizi Birim Fiyat
        /// </summary>
        public decimal TransactionExchangeUnitPrice { get; set; }

        /// <summary>
        /// İşlem Dövizi Satır Tutarı
        /// </summary>
        public decimal TransactionExchangeLineAmount { get; set; }
        /// <summary>
        /// Stok Fiş ID
        /// </summary>
        public Guid StockFicheID { get; set; }

        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Satır ID
        /// </summary>
        public Guid? PurchaseOrderLineID { get; set; }

        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }

        /// <summary>
        /// Fiş Türü
        /// </summary>
        public int FicheType { get; set; }

        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime Date_ { get; set; }

        /// <summary>
        /// Çıkış Birim Maliyeti
        /// </summary>
        public decimal UnitOutputCost { get; set; }
        /// <summary>
        /// MRP ID
        /// </summary>
        public Guid? MRPID { get; set; }
        /// <summary>
        /// MRP Satır ID
        /// </summary>
        public Guid? MRPLineID { get; set; }
    }
}
