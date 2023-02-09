using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductReferanceNumber.Dtos
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
    }
}
