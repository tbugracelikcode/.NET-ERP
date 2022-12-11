using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.StationGroup
{
    /// <summary>
    /// İş İstasyonları Grup
    /// </summary>
    public class StationGroups : FullAuditedEntity
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Toplam Çalışan
        /// </summary>
        public int TotalEmployees { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        public ICollection<Stations> Stations { get; set; }

        public ICollection<WorkOrders> WorkOrders { get; set; }

        public ICollection<OperationUnsuitabilityReports> OperationUnsuitabilityReports { get; set; }
    }
}
