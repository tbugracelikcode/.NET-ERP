using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos
{
    public class ListShipmentPlanningLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Yükleme Planlama ID
        /// </summary>
        public Guid? ShipmentPlanningID { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans ID
        /// </summary>
        public Guid? ProductionDateReferenceID { get; set; }
        /// <summary>
        /// Üretim Tarihi Referans Kod
        /// </summary>
        public string ProductionDateReferenceNo { get; set; }

        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Planlanan Başlangıç Tarihi
        /// </summary>
        /// 
        public DateTime? PlannedStartDate { get; set; }

        /// <summary>
        /// Planlanan Bitiş Tarihi
        /// </summary>
        /// 
        public DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// Müşteri Sipariş No
        /// </summary>
        public string CustomerOrderNr { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }

        /// <summary>
        /// Müşterinin İstediği Yükleme Tarihi
        /// </summary>
        /// 
        public DateTime RequestedLoadingDate { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }

        /// <summary>
        /// Planlanan Miktar
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        /// <summary>
        /// Gönderilen Miktar
        /// </summary>
        public decimal SentQuantity { get; set; }

        /// <summary>
        /// Yükleme Miktarı
        /// </summary>
        public decimal ShipmentQuantity { get; set; }

        /// <summary>
        /// Birim Ağırlık
        /// </summary>
        public decimal UnitWeightKG { get; set; }

        /// <summary>
        /// Net KG
        /// </summary>
        public decimal NetWeightKG { get; set; }

        /// <summary>
        /// Brüt KG
        /// </summary>
        public decimal GrossWeightKG { get; set; }

        /// <summary>
        /// Satır Açıklama
        /// </summary>
        public string LineDescription_ { get; set; }
    }
}
