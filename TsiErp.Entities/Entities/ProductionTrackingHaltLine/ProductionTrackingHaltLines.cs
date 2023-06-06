using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.ProductionTracking;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

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
        /// <summary>
        /// Duruş Süresi
        /// </summary>
        public decimal HaltTime { get; set; }
    }
}
