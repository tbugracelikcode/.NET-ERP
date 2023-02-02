using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlannedMaintenance.Dtos
{
    public class ListPlannedMaintenancesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kayıt No
        /// </summary>
        public string RegistrationNo { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string PeriodName { get; set; }
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
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Planlanan Bakım Tarihi
        /// </summary>
        public DateTime? PlannedDate { get; set; }
    }
}
