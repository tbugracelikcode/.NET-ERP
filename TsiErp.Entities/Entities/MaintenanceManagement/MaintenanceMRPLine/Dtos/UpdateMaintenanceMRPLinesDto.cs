using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos
{
    public class UpdateMaintenanceMRPLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Bakım MRP ID
        /// </summary>
        public Guid MaintenanceMRPID { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// İhtiyaç Miktar
        /// </summary>
        public int RequirementAmount { get; set; }
        /// <summary>
        /// Hesaplanacak
        /// </summary>
        public bool isCalculated { get; set; }
        /// <summary>
        /// Stoktan Kullanılacak
        /// </summary>
        public bool isStockUsage { get; set; }
    }
}
