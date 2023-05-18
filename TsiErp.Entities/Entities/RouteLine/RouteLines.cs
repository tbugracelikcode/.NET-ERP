using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.Route;

namespace TsiErp.Entities.Entities.RouteLine
{
    /// <summary>
    /// Rota Satırları
    /// </summary>
    public class RouteLines : FullAuditedEntity
    {
        
        /// <summary>
        /// Rota ID
        /// </summary>
        public Guid RouteID { get; set; }
        /// <summary>
        /// Ana Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Ürün Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        /// <summary>
        /// Üretim Havuz ID
        /// </summary>
        public Guid ProductionPoolID { get; set; }
        /// <summary>
        /// Üretim Havuz Açıklama
        /// </summary>
        public string ProductionPoolDescription { get; set; }
        /// <summary>
        /// Ayar ve Kontrol Süresi
        /// </summary>
        public int AdjustmentAndControlTime { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Operasyon Süresi
        /// </summary>
        public decimal OperationTime { get; set; }
        /// <summary>
        /// Öncelik
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Satır Numarası
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Operasyon Resmi
        /// </summary>
        public byte[] OperationPicture { get; set; }

        public ProductsOperations ProductsOperations { get; set; }

        public Products Products { get; set; }

        public Routes Routes { get; set; }
    }
}
