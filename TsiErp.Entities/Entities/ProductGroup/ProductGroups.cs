using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Product;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductGroup
{
    /// <summary>
    /// Ürün Grupları
    /// </summary>
    public class ProductGroups : FullAuditedEntity
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
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// StokID
        /// </summary>

        public ICollection<Products> Products { get; set; }
    }
}
