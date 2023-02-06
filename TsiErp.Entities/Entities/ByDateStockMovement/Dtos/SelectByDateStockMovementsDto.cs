using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ByDateStockMovement.Dtos
{
    public class SelectByDateStockMovementsDto : FullAuditedEntityDto
    {
        /// <summary>
        ///  Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Satın Alma Talep Toplamı
        /// </summary>
        public decimal TotalPurchaseRequest { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Satın Alma Sipariş Toplamı
        /// </summary>
        public decimal TotalPurchaseOrder { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Verilen Teklif Toplamı
        /// </summary>
        public decimal TotalSalesProposition { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Satış Sipariş Toplamı
        /// </summary>
        public decimal TotalSalesOrder { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Sarf Toplamı
        /// </summary>
        public decimal TotalConsumption { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Fire Toplamı
        /// </summary>
        public decimal TotalWastage { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Üretim Toplamı
        /// </summary>
        public decimal TotalProduction { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Stok Giriş Toplamı
        /// </summary>
        public decimal TotalGoodsReceipt { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Stok Çıkış Toplamı
        /// </summary>
        public decimal TotalGoodsIssue { get; set; }
        /// <summary>
        ///  Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        ///  Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        ///  Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        ///  Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        ///  Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        ///  Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        ///  Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        [Precision(18, 6)]
        /// <summary>
        ///  Stok Miktarı
        /// </summary>
        public decimal Amount { get; set; }
    }
}
