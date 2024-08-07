using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos
{
    public class CreateMaintenancePeriodsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Periyot Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Bakım Periyot Süresi
        /// </summary>
        public decimal PeriodTime { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Günlük Bakım
        /// </summary>
        public bool IsDaily { get; set; }
    }
}
