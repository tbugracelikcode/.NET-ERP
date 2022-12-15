
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Entities.Entities.ProductsOperation
{
    public class ProductsOperations : FullAuditedEntity
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
        /// İş Merkezi ID
        /// </summary>
        public Guid WorkCenterID { get; set; }
        /// <summary>
        /// Şablon Operasyon ID
        /// </summary>
        public Guid TemplateOperationID { get; set; }
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        public ICollection<ProductsOperationLines> ProductsOperationLines { get; set; }

        public Products Products { get; set; }

        public ICollection<RouteLines> RouteLines { get; set; }

        public ICollection<WorkOrders> WorkOrders { get; set; }

        public ICollection<OperationUnsuitabilityReports> OperationUnsuitabilityReports { get; set; }
    }
}
