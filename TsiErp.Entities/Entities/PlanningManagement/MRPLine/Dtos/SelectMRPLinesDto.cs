using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos
{
    public class SelectMRPLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// MRP ID
        /// </summary>
        public Guid MRPID { get; set; }
        /// <summary>
        /// Satış Siparişi ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Satış Siparişi Fiş Numarası
        /// </summary>
        public string SalesOrderFicheNo { get; set; }
        /// <summary>
        /// Satış Siparişi Satış ID
        /// </summary>
        public Guid? SalesOrderLineID { get; set; }

        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Şube Kodu
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Depo Kodu
        /// </summary>
        public string WarehouseCode { get; set; }
        /// <summary>
        /// Hesaplanacak
        /// </summary>
        public bool isCalculated { get; set; }
        /// <summary>
        /// Stoktan Kullanılacak
        /// </summary>
        public bool isStockUsage { get; set; }
        /// <summary>
        /// Satın Alınacak
        /// </summary>
        public bool isPurchase { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Satır No
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
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Birim Seti Kodu
        /// </summary>
        public string UnitSetCode { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public string State_ { get; set; }
        /// <summary>
        /// İhtiyaç Miktar
        /// </summary>
        public decimal RequirementAmount { get; set; }
        /// <summary>
        /// Stok Miktarı
        /// </summary>
        public decimal AmountOfStock { get; set; }
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceID { get; set; }
        /// <summary>
        /// Sipariş Kabul Satır ID
        /// </summary>
        public Guid? OrderAcceptanceLineID { get; set; }
    }
}
