using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos
{
    public class SelectPlannedMaintenancesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kayıt No
        /// </summary>
        public string RegistrationNo { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid? StationID { get; set; }
        /// <summary>
        /// Data Concurrency Stamp
        /// </summary>
        public bool? DataOpenStatus { get; set; }

        /// <summary>
        /// Data Concurrency UserID
        /// </summary>
        public Guid? DataOpenStatusUserId { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Periyot ID
        /// </summary>
        public Guid? PeriodID { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string PeriodName { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public PlannedMaintenanceStateEnum Status { get; set; }
        /// <summary>
        /// Durum Adı
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Bakımı Yapan
        /// </summary>
        public string Caregiver { get; set; }
        /// <summary>
        /// Bakımı Yapan Kişi Sayısı
        /// </summary>
        public int NumberofCaregivers { get; set; }
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        /// <summary>
        /// Kalan Süre
        /// </summary>
        public decimal RemainingTime { get; set; }
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
        /// <summary>
        /// Planlanan Bakım Süre
        /// </summary>
        public decimal PlannedTime { get; set; }
        /// <summary>
        /// Gerçekleşen Bakım Süre
        /// </summary>
        public decimal OccuredTime { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Planlanan Bakım Tarihi
        /// </summary>
        public DateTime? PlannedDate { get; set; }
        /// <summary>
        ///  Tamamlama Tarihi
        /// </summary>
        public DateTime? CompletionDate { get; set; }
        [NoDatabaseAction]
        public List<SelectPlannedMaintenanceLinesDto> SelectPlannedMaintenanceLines { get; set; }
    }
}
