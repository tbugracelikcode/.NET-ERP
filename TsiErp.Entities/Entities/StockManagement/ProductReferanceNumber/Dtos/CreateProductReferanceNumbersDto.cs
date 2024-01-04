using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos
{
    public class CreateProductReferanceNumbersDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Cari ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Ürün Referans Numarası
        /// </summary>
        public string ReferanceNo { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// Sipariş Referans No
        /// </summary>
        public string OrderReferanceNo { get; set; }
        /// <summary>
        /// Müşteri Referans No
        /// </summary>
        public string CustomerReferanceNo { get; set; }
        /// <summary>
        /// Müşteri Barkod No
        /// </summary>
        public string CustomerBarcodeNo { get; set; }
        /// <summary>
        /// Minimum Sipariş Miktarı
        /// </summary>
        public decimal MinOrderAmount { get; set; }
    }
}
