using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos
{
    public class ListStockFicheLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Stok Adı
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Birim Set Kodu
        /// </summary>
        public string UnitSetCode { get; set; }

        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }

        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }
    }
}
