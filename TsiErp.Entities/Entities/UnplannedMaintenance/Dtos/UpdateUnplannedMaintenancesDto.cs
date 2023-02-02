using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine.Dtos;

namespace TsiErp.Entities.Entities.UnplannedMaintenance.Dtos
{
    public class UpdateUnplannedMaintenancesDto : FullAuditedEntityDto
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
        /// Periyot ID
        /// </summary>
        public Guid? PeriodID { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Bakımı Yapan
        /// </summary>
        public string Caregiver { get; set; }
        /// <summary>
        /// Bakımı Yapan Kişi Sayısı
        /// </summary>
        public int NumberofCaregivers { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Kalan Süre
        /// </summary>
        public decimal RemainingTime { get; set; }
        /// <summary>
        /// Not
        /// </summary>
        public string Note_ { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Planlanan Bakım Süre
        /// </summary>
        public decimal UnplannedTime { get; set; }
        [Precision(18, 6)]
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
        public DateTime? UnplannedDate { get; set; }
        /// <summary>
        ///  Tamamlama Tarihi
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        public List<SelectUnplannedMaintenanceLinesDto> SelectUnplannedMaintenanceLines { get; set; }
    }
}
