
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos
{
    public class ListProductionTrackingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Takip Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri Kodu
        /// </summary>
        public string WorkOrderCode { get; set; }
        /// <summary>
        /// Müşteri Kodu
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

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
        public DateTime OperationEndDate { get; set; }
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
        /// İş İstasyonu Kody
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Çalışan Adı
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Vardiya Kodu
        /// </summary>
        public string ShiftCode { get; set; }
        /// <summary>
        /// İş İstasyonu ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Çalışan ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// Vardiya ID
        /// </summary>
        public Guid ShiftID { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }

        /// <summary>
        /// Üretim Emri No
        /// </summary>
        public string ProductionOrderCode { get; set; }

        /// <summary>
        /// Ürüne Özel Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }

        /// <summary>
        /// Operasyon Adı
        /// </summary>
        public string ProductOperationName { get; set; }



        public List<SelectProductionTrackingHaltLinesDto> SelectProductionTrackingHaltLines { get; set; }
    }
}
