using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.BillsofMaterialLine
{
    /// <summary>
    /// Reçete Satırları
    /// </summary>
    public class BillsofMaterialLines : FullAuditedEntity
    {
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid BoMID { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid FinishedProductID { get; set; }
        /// <summary>
        /// Malzeme Türü
        /// </summary>
        public ProductTypeEnum MaterialType { get; set; }
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid ProductID { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid UnitSetID { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        [Precision(18, 6)]

        /// <summary>
        /// Boy
        /// </summary>
        public decimal Size { get; set; }

        public BillsofMaterials BillsofMaterials { get; set; }

        public UnitSets UnitSets { get; set; }

        public Products Products { get; set; }
    }
}
