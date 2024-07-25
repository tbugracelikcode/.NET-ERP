using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos
{
    public class UpdateProductionTrackingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Takip Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid? WorkOrderID { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Operasyon Başlangıç Tarihi
        /// </summary>
        public DateTime OperationStartDate { get; set; }
        /// <summary>
        /// Başlangıç Saati
        /// </summary>
        public TimeSpan? OperationStartTime { get; set; }
        /// <summary>
        /// Operasyon Bitiş Tarihi
        /// </summary>
        public DateTime? OperationEndDate { get; set; }
        /// <summary>
        /// Bitiş Saati
        /// </summary>
        public TimeSpan? OperationEndTime { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Ayar Süresi
        /// </summary>
        public decimal AdjustmentTime { get; set; }
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// Hatalı Miktar
        /// </summary>
        public decimal FaultyQuantity { get; set; }

        /// <summary>
        /// Tamamlandı mı ?
        /// </summary>
        public bool IsFinished { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid? EmployeeID { get; set; }
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid? ShiftID { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }


        [NoDatabaseAction]

        /// <summary>
        /// Sipariş Satırları
        /// </summary>
        public List<SelectProductionTrackingHaltLinesDto> SelectProductionTrackingHaltLinesDto { get; set; }

    }
}
