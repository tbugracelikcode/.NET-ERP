using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;

namespace TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos
{
    public class UpdatePackageFichesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Üretim Emri Ref No
        /// </summary>
        public string ProductionOrderReferenceNo { get; set; }

        /// <summary>
        /// Paket Fişi Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid? PackingListID { get; set; }
        /// <summary>
        /// Paket Fişi Tarihi
        /// </summary>
        public DateTime? Date_ { get; set; }
        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Cari Hesap ID
        /// </summary>
        public Guid? CurrentAccountID { get; set; }
        /// <summary>
        /// Koli Türü
        /// </summary>
        public string PackageType { get; set; }
        /// <summary>
        /// Koli İçeriği
        /// </summary>
        public int PackageContent { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid? ProductionOrderID { get; set; }
        /// <summary>
        /// Koli Sayısı
        /// </summary>
        public int NumberofPackage { get; set; }
        /// <summary>
        /// Palet No
        /// </summary>
        public string PalletNumber { get; set; }
        /// <summary>
        /// Ürün Palet Sırası
        /// </summary>
        public string ProductPalletOrder { get; set; }
        /// <summary>
        /// Birim Ağırlık
        /// </summary>
        public decimal UnitWeight { get; set; }

        [NoDatabaseAction]
        public List<SelectPackageFicheLinesDto> SelectPackageFicheLines { get; set; }
    }
}
