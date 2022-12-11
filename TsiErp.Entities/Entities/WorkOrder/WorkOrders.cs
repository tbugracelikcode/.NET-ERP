using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.WorkOrder
{
    /// <summary>
    /// İş Emirleri
    /// </summary>
    public class WorkOrders: FullAuditedEntity
    {
        /// <summary>
        /// İş Emri Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// İptal
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public WorkOrderStateEnum WorkOrderState { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public decimal AdjustmentAndControlTime { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Gerçekleşen Başlangıç Tarihi
        /// </summary>
        public DateTime? OccuredStartDate { get; set; }
        /// <summary>
        /// Gerçekleşen Bitiş Tarihi
        /// </summary>
        public DateTime? OccuredFinishDate { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Gerçekleşen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Bağlı İş Emri ID
        /// </summary>
        public Guid? LinkedWorkOrderID { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid PropositionID { get; set; }
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// İş İstasyonu Grup ID
        /// </summary>
        public Guid StationGroupID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }

        public ProductionOrders ProductionOrders { get; set; }

        public SalesPropositions SalesPropositions { get; set; }

        public Routes Routes { get; set; }

        public ProductsOperations ProductsOperations { get; set; }

        public Stations Stations { get; set; }

        public StationGroups StationGroups { get; set; }

        public Products Products { get; set; }

        public CurrentAccountCards CurrentAccountCards { get; set; }

        public ICollection<OperationUnsuitabilityReports> OperationUnsuitabilityReports { get; set; }


    }
}
