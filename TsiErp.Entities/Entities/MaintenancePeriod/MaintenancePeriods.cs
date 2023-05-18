using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenance;

namespace TsiErp.Entities.Entities.MaintenancePeriod
{
    /// <summary>
    /// Bakım Periyotları
    /// </summary>
    public class MaintenancePeriods : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string Name { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Bakım Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Günlük Bakım
        /// </summary>
        public bool IsDaily { get; set; }


    }
}
