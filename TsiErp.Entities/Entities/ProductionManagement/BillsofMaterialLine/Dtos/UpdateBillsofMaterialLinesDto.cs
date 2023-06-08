using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos
{
    public class UpdateBillsofMaterialLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Reçete ID
        /// </summary>
        public Guid? BoMID { get; set; }
        /// <summary>
        /// Mamül ID
        /// </summary>
        public Guid? FinishedProductID { get; set; }
        /// <summary>
        /// Malzeme Türü
        /// </summary>
        public ProductTypeEnum MaterialType { get; set; }
        /// <summary>
        /// Ürün ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string _Description { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Boy
        /// </summary>
        public decimal Size { get; set; }
    }
}
