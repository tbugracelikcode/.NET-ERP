using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos
{
    public class SelectContractProductionTrackingsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Stok Açıklama
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Operasyon Başlangıç Tarihi
        /// </summary>
        public DateTime? OperationStartDate { get; set; }
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
        /// Tamamlandı mı ?
        /// </summary>
        public bool IsFinished { get; set; }
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public decimal PlannedQuantity { get; set; }
        /// <summary>
        /// İş İstasyonu Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Cari Kodu
        /// </summary>
        public string CurrentAccountCardCode { get; set; }
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
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        public string WorkOrderCode { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
    }
}
