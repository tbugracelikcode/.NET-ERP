using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos
{
    public class UpdatePackageFicheLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Paket Fişi ID
        /// </summary>
        public Guid PackageFicheID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Paketleme Tarihi
        /// </summary>
        public DateTime? PackingDate { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public string Status_ { get; set; }
        /// <summary>
        /// Koli İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        /// <summary>
        /// Koli Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        /// <summary>
        /// Üretim Emri Referans No
        /// </summary>
        public string ProductionOrderFicheNo { get; set; }
    }
}
