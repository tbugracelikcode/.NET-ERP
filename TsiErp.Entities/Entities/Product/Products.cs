using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.SalesPropositionLine;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.Route;
using Microsoft.EntityFrameworkCore;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Enums;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Entities.Entities.ContractProductionTracking;

namespace TsiErp.Entities.Entities.Product
{
    /// <summary>
    /// Stok Kodları
    /// </summary>
    public class Products : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Temin Şekli
        /// </summary>
        public ProductSupplyFormEnum SupplyForm { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Stok Boyu
        /// </summary>
        public decimal ProductSize { get; set; }
        /// <summary>
        /// GTIP
        /// </summary>
        public string GTIP { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Testere Fire
        /// </summary>
        public decimal SawWastage { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Confirmation { get; set; }
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalConfirmation { get; set; }
        /// <summary>
        /// Stok Türü
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// Ürün Grup ID
        /// </summary>
        public Guid ProductGrpID { get; set; }
        /// <summary>
        /// Üretici Firma Kodu
        /// </summary>
        public string ManufacturerCode { get; set; }
        /// <summary>
        /// Satış KDV
        /// </summary>
        public int SaleVAT { get; set; }
        /// <summary>
        /// Satın Alma KDV
        /// </summary>
        public int PurchaseVAT { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        /// <summary>
        /// Özellik Set ID
        /// </summary>
        public Guid FeatureSetID { get; set; }
        /// <summary>
        /// İngilizce Tanım
        /// </summary>
        public string EnglishDefinition { get; set; }
        /// <summary>
        /// İhracat Kategori No
        /// </summary>
        public string ExportCatNo { get; set; }
        /// <summary>
        /// OemRefNo
        /// </summary>
        public string OemRefNo { get; set; }
        /// <summary>
        /// OemRefNo2
        /// </summary>
        public string OemRefNo2 { get; set; }
        /// <summary>
        /// OemRefNo3
        /// </summary>
        public string OemRefNo3 { get; set; }
        /// <summary>
        /// Planlanan Fire
        /// </summary>
        public int PlannedWastage { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Kaplama Ağırlığı
        /// </summary>
        public decimal CoatingWeight { get; set; }


        public UnitSets UnitSets { get; set; }
        public ICollection<Routes> Routes { get; set; }
        public ProductGroups ProductGroups { get; set; }
        public ICollection<SalesPropositionLines> SalesPropositionLines { get; set; }
        public ICollection<SalesOrderLines> SalesOrderLines { get; set; }
        public ICollection<ProductsOperations> ProductsOperations { get; set; }
        public ICollection<BillsofMaterialLines> BillsofMaterialLines { get; set; }
        public ICollection<BillsofMaterials> BillsofMaterials { get; set; }
        public ICollection<RouteLines> RouteLines { get; set; }
        public ICollection<ProductionOrders> ProductionOrders { get; set; }
        public ICollection<WorkOrders> WorkOrders { get; set; }
        public ICollection<PurchaseOrderLines> PurchaseOrderLines { get; set; }
        public ICollection<PurchaseRequestLines> PurchaseRequestLines { get; set; }
        public ICollection<PurchaseUnsuitabilityReports> PurchaseUnsuitabilityReports { get; set; }
        public ICollection<OperationUnsuitabilityReports> OperationUnsuitabilityReports { get; set; }
        public ICollection<ForecastLines> ForecastLines { get; set; }
        public ICollection<SalesPriceLines> SalesPriceLines { get; set; }
        public ICollection<PurchasePriceLines> PurchasePriceLines { get; set; }
        public ICollection<FinalControlUnsuitabilityReports> FinalControlUnsuitabilityReports { get; set; }
        public ICollection<MaintenanceInstructionLines> MaintenanceInstructionLines { get; set; }
        public ICollection<PlannedMaintenanceLines> PlannedMaintenanceLines { get; set; }
        public ICollection<UnplannedMaintenanceLines> UnplannedMaintenanceLines { get; set; }
        public ICollection<ByDateStockMovements> ByDateStockMovements { get; set; }
        public ICollection<GrandTotalStockMovements> GrandTotalStockMovements { get; set; }
        public ICollection<TechnicalDrawings> TechnicalDrawings { get; set; }
        public ICollection<ProductReferanceNumbers> ProductReferanceNumbers { get; set; }
        public ICollection<ContractProductionTrackings> ContractProductionTrackings { get; set; }

    }
}
