using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine.Dtos
{
    public class SelectCPRMaterialCostLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// CPR ID
        /// </summary>
        public Guid CPRID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Malzeme Tanımlaması
        /// </summary>
        public string MaterialDesignation { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Stok Kod
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Adı
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Geri Ödeme
        /// </summary>
        public string Reimbursement { get; set; }
        /// <summary>
        /// Part Başına Net Ağırlık
        /// </summary>
        public decimal NetWeightperPart { get; set; }
        /// <summary>
        /// Part Başına Brüt Ağırlık
        /// </summary>
        public decimal GrossWeightperPart { get; set; }
        /// <summary>
        /// Temel Malzeme Fiyatı
        /// </summary>
        public decimal BaseMaterialPrice { get; set; }
        /// <summary>
        /// Malzeme Fiyatı Ek Ücretler
        /// </summary>
        public decimal SurchargesMaterialPrice { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Malzeme Giderleri
        /// </summary>
        public decimal MaterialOverhead { get; set; }
        /// <summary>
        /// Hurda Oranı
        /// </summary>
        public decimal ScrapRate { get; set; }
        /// <summary>
        /// Hurda Maliyeti
        /// </summary>
        public decimal ScrapCost { get; set; }
        /// <summary>
        /// Malzeme Maliyeti
        /// </summary>
        public decimal MaterialCost { get; set; }
    }
}
