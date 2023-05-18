using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.ContractProductionTracking
{
    public class ContractProductionTrackings : FullAuditedEntity
    {
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Üretilen Miktar
        /// </summary>
        public decimal ProducedQuantity { get; set; }
        [Precision(18, 6)]
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
        /// Tamamlandı mı ?
        /// </summary>
        public bool IsFinished { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Planlanan Adet
        /// </summary>
        public decimal PlannedQuantity { get; set; }
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
        /// Cari Hesap ID
        /// </summary>
        public Guid CurrentAccountCardID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }

    }
}
