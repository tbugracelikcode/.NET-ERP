using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.UnitSet;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using SqlDataType = Tsi.Core.Utilities.SqlDataTypeMappingUtilities.SqlDataType;

namespace TsiErp.Entities.Entities.PlannedMaintenanceLine
{
    /// <summary>
    /// Önleyici / Planlı Bakım Kayıtları Satırları
    /// </summary>
    public class PlannedMaintenanceLines : FullAuditedEntity
    {
        /// <summary>
        /// Planlı Bakım ID
        /// </summary>
        public Guid PlannedMaintenanceID { get; set; }
        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Talimat Açıklaması
        /// </summary>
        public string InstructionDescription { get; set; }
        /// <summary>
        /// Bakım Notu
        /// </summary>
        public string MaintenanceNote { get; set; }
    }
}
