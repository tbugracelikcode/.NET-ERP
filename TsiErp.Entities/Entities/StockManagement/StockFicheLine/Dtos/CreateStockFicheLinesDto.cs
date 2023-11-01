using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos
{
    public class CreateStockFicheLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Stok Fiş ID
        /// </summary>
        public Guid StockFicheID { get; set; }
        /// <summary>
        /// Üretim Tarihi Referansı
        /// </summary>
        public string ProductionDateReferance { get; set; }

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
        /// <summary>
        /// Satın Alma Sipariş ID
        /// </summary>
        public Guid? PurchaseOrderID { get; set; }
        /// <summary>
        /// Satın Alma Sipariş Satır ID
        /// </summary>
        public Guid? PurchaseOrderLineID { get; set; }

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
        /// Fiş Türü
        /// </summary>
        public int FicheType { get; set; }
    }
}
