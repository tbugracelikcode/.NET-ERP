using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.StockManagement.ProductRelatedProductProperty.Dtos
{
    public class SelectProductRelatedProductPropertiesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Ürün Grubu ID
        /// </summary>
        public Guid ProductGroupID { get; set; }
        /// <summary>
        /// Ürün Özellik ID
        /// </summary>
        public Guid ProductPropertyID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        ///Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Özellik Açıklaması
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Özellik Değeri
        /// </summary>
        public string PropertyValue { get; set; }
        /// <summary>
        /// Kalite Kontrol Kriteri
        /// </summary>
        public bool IsQualityControlCriterion { get; set; }
        /// <summary>
        /// Satın Alma Dökümü
        /// </summary>
        public bool isPurchaseBreakdown { get; set; }
    }
}
