using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.ProductionTracking;

namespace TsiErp.Entities.Entities.ProductionTrackingHaltLine
{
    public class ProductionTrackingHaltLines : FullAuditedEntity
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
        /// Duruş Sebebi Kodu
        /// </summary>
        public string HaltCode { get; set; }
        /// <summary>
        /// Duruş Sebebi Adı
        /// </summary>
        public string HaltName { get; set; }
        [Precision(18, 6)]
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
        /// <summary>
        /// Duruş Sebepleri
        /// </summary>
        public HaltReasons HaltReasons { get; set; }
        public ProductionTrackings ProductionTrackings { get; set; }
    }
}
