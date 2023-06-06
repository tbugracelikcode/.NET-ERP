using Microsoft.EntityFrameworkCore;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesPropositionLine;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionOrder 
{
    /// <summary>
    /// Üretim Emirleri
    /// </summary>
    public class ProductionOrders : FullAuditedEntity
    {
        [SqlColumnType(MaxLength = 17, Nullable = false, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Üretim Emri Fiş No
        /// </summary>
        public string FicheNo { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Tarih
        /// </summary>
        public DateTime? Date_ { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Bit)]
        /// <summary>
        /// İptal
        /// </summary>
        public bool Cancel_ { get;set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.Int)]
        /// <summary>
        /// Üretim Emri Durumu
        /// </summary>
        public ProductionOrderStateEnum ProductionOrderState { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.DateTime)]
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        [SqlColumnType(SqlDbType = SqlDataType.Decimal, Precision = 6, Scale = 18)]
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.NVarCharMax)]
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        [SqlColumnType(MaxLength = 200, Nullable = true, SqlDbType = SqlDataType.NVarChar)]
        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNo { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Sipariş Satır ID
        /// </summary>
        public Guid OrderLineID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid FinishedProductID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Ürün ID
        /// </summary>
        public Guid LinkedProductID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Birim Seti
        /// </summary>
        public Guid UnitSetID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid BOMID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün Ağacı ID
        /// </summary>
        public Guid ProductTreeID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Ürün Ağacı Satır ID
        /// </summary>
        public Guid ProductTreeLineID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Teklif ID
        /// </summary>
        public Guid PropositionID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Teklif Satır ID
        /// </summary>
        public Guid PropositionLineID { get; set; }
        [SqlColumnType(Nullable = false, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountID { get; set; }
        [SqlColumnType(Nullable = true, SqlDbType = SqlDataType.UniqueIdentifier)]
        /// <summary>
        /// Bağlı Üretim Emri ID
        /// </summary>
        public Guid LinkedProductionOrderID { get; set; }

    }
}
