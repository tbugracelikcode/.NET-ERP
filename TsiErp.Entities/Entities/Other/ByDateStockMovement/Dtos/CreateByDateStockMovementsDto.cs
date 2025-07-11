﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos
{
    public class CreateByDateStockMovementsDto : FullAuditedEntityDto
    {
        /// <summary>
        ///  Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        ///  Depo Giriş Toplamı
        /// </summary>
        public decimal TotalWarehouseInput { get; set; }
        /// <summary>
        ///  Depo Çıkış Toplamı
        /// </summary>
        public decimal TotalWarehouseOutput { get; set; }
        /// <summary>
        ///  Satın Alma Talep Toplamı
        /// </summary>
        public decimal TotalPurchaseRequest { get; set; }
        /// <summary>
        ///  Satın Alma Sipariş Toplamı
        /// </summary>
        public decimal TotalPurchaseOrder { get; set; }
        /// <summary>
        ///  Verilen Teklif Toplamı
        /// </summary>
        public decimal TotalSalesProposition { get; set; }
        /// <summary>
        ///  Satış Sipariş Toplamı
        /// </summary>
        public decimal TotalSalesOrder { get; set; }
        /// <summary>
        ///  Sarf Toplamı
        /// </summary>
        public decimal TotalConsumption { get; set; }
        /// <summary>
        ///  Fire Toplamı
        /// </summary>
        public decimal TotalWastage { get; set; }
        /// <summary>
        ///  Üretim Toplamı
        /// </summary>
        public decimal TotalProduction { get; set; }
        /// <summary>
        ///  Stok Giriş Toplamı
        /// </summary>
        public decimal TotalGoodsReceipt { get; set; }
        /// <summary>
        ///  Stok Çıkış Toplamı
        /// </summary>
        public decimal TotalGoodsIssue { get; set; }
        /// <summary>
        ///  Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        ///  Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        ///  Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        ///  Stok Miktarı
        /// </summary>
        public decimal Amount { get; set; }
    }
}
