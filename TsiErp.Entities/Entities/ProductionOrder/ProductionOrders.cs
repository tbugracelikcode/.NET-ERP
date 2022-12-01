using Microsoft.EntityFrameworkCore;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionOrder 
{
    /// <summary>
    /// Üretim Emirleri
    /// </summary>
    public class ProductionOrders : FullAuditedEntity
    {
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// İptal
        /// </summary>
        public bool Cancel_ { get;set; }
        /// <summary>
        /// Üretim Emri Durumu
        /// </summary>
        public ProductionOrderStateEnum ProductionOrderState { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }
        /// <summary>
        /// Sipariş Satır ID
        /// </summary>
        public Guid OrderLineID { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid FinishedProductID { get; set; }
        /// <summary>
        /// Bağlı Ürün ID
        /// </summary>
        public Guid LinkedProductID { get; set; }
        /// <summary>
        /// Birim Seti
        /// </summary>
        public Guid UnitSetID { get; set; }
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid BOMID { get; set; }
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        /// <summary>
        /// Ürün Ağacı ID
        /// </summary>
        public Guid ProductTreeID { get; set; }
        /// <summary>
        /// Ürün Ağacı Satır ID
        /// </summary>
        public Guid ProductTreeLineID { get; set; }
        /// <summary>
        /// Teklif ID
        /// </summary>
        public Guid PropositionID { get; set; }
        /// <summary>
        /// Teklif Satır ID
        /// </summary>
        public Guid PropositionLineID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountID { get; set; }
        /// <summary>
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid LinkedProductionOrderID { get; set; }

        public SalesOrders SalesOrders { get; set; }
        public SalesOrderLines SalesOrderLines { get; set; }
        public Products Products { get; set; }
        public UnitSets UnitSets { get; set; }
        public BillsofMaterials BillsofMaterials { get; set; }
        public Routes Routes { get; set; }
        public SalesPropositions SalesPropositions { get; set; }
        public SalesPropositionLines SalesPropositionLines { get; set; }
        public CurrentAccountCards CurrentAccountCards { get; set; }


    }
}
