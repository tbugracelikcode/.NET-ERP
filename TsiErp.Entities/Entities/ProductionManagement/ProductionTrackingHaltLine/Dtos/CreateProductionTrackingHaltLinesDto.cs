using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos
{
    public class CreateProductionTrackingHaltLinesDto : FullAuditedEntityDto
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
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
    }
}
