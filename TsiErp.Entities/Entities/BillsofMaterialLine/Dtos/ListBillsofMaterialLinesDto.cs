using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.BillsofMaterialLine.Dtos
{
    public class ListBillsofMaterialLinesDto : FullAuditedEntityDto
    {
        
        /// <summary>
        /// Mamül Kodu
        /// </summary>
        public string FinishedProductCode { get; set; }
       
        /// <summary>
        /// Malzeme Türü
        /// </summary>
        public int MaterialType { get; set; }
        
        /// <summary>
        /// Ürün Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Miktar Formül
        /// </summary>
        public string QuantityFormula { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Birim Seti Kodu
        /// </summary>
        public string UnitSetCode { get; set; }
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
