using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.CustomDbAttributes;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockFicheLine
{
    [DatabaseTable]
    public class StockFicheLines : FullAuditedEntity
    {
        /// <summary>
        /// Stok Fiş ID
        /// </summary>
        public Guid StockFicheID { get; set; }

        /// <summary>
        /// Satır Nr
        /// </summary>
        public int LineNr { get; set; }

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }

        /// <summary>
        /// Birim Set ID
        /// </summary>
        public Guid UnitSetID { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Quantity { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Birim Fiyat
        /// </summary>
        public decimal UnitPrice { get; set; }

        [Precision(18, 6)]
        /// <summary>
        /// Satır Tutarı
        /// </summary>
        public decimal LineAmount { get; set; }

        /// <summary>
        /// Satır Açıklaması
        /// </summary>
        public string LineDescription { get; set; }

        /// <summary>
        /// Fiş Türü
        /// </summary>
        public StockFicheTypeEnum FicheType { get; set; }
    }
}
