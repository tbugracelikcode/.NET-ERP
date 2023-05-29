using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos
{
    public class SelectProductionTrackingHaltLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Takip ID
        /// </summary>
        public Guid ProductionTrackingID { get; set; }
        /// <summary>
        /// Duruş Sebebi ID
        /// </summary>
        public Guid HaltID { get; set; }
        /// <summary>
        /// Duruş Sebebi Adı
        /// </summary>
        public string HaltName { get; set; }
        /// <summary>
        /// Duruş Sebebi Kodu
        /// </summary>
        public string HaltCode { get; set; }
        /// <summary>
        /// Planlı mı?
        /// </summary>
        public bool IsPlanned { get; set; }
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
    }
}
