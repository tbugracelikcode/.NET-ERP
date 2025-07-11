﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos
{
    public class UpdateWorkOrdersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderNo { get; set; }
        /// <summary>
        /// Operasyon Uygunsuzluk İş Emri
        /// </summary>
        public bool IsOperationUnsuitabilityWorkOrder { get; set; }
        /// <summary>
        /// Fason Uygunsuzluk İş Emri
        /// </summary>
        public bool IsContractUnsuitabilityWorkOrder { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans ID
        /// </summary>
        public Guid? ProductionDateReferenceID { get; set; }
        /// <summary>
        /// Parçalama Miktarı
        /// </summary>
        public int SplitQuantity { get; set; }
        /// <summary>
        /// İptal
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public int WorkOrderState { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public decimal AdjustmentAndControlTime { get; set; }

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
        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

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
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid? PropositionID { get; set; }
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid? RouteID { get; set; }
        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid? ProductsOperationID { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// İş İstasyonu Grup ID
        /// </summary>
        public Guid? StationGroupID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }

        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid OrderID { get; set; }
    }
}
