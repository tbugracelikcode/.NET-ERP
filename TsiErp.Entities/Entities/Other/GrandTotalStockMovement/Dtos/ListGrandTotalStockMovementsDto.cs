using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos
{
    public class ListGrandTotalStockMovementsDto : FullAuditedEntityDto
    {
        /// <summary>
        ///  Rezerve Toplamı
        /// </summary>
        public decimal TotalReserved { get; set; }
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
        ///  Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        ///  Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        ///  Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        ///  Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        ///  Stok Miktarı
        /// </summary>
        public decimal Amount { get; set; }
    }
}
