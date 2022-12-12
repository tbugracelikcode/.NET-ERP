using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.Route
{
    /// <summary>
    /// Rotalar
    /// </summary>
    public class Routes : FullAuditedEntity
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
        /// Ana Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Üretim Başlangıç
        /// </summary>
        public string ProductionStart { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// Teknik Onay
        /// </summary>
        public bool TechnicalApproval { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        public ICollection<RouteLines> RouteLines { get; set; }
        public Products Products { get; set; }
        public ICollection<ProductionOrders> ProductionOrders { get; set; }
        public ICollection<WorkOrders> WorkOrders { get; set; }
    }
}
