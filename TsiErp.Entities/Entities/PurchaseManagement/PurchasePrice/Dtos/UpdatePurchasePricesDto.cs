using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;

namespace TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice.Dtos
{
    public class UpdatePurchasePricesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Başlangıç Tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Bitiş Tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Para Birimi ID
        /// </summary>
        public Guid? CurrencyID { get; set; }
        // <summary>
        /// Cari ID
        /// </summary>
        public Guid? CurrentAccountCardID { get; set; }
        /// <summary>
        /// Şube ID
        /// </summary>
        public Guid? BranchID { get; set; }
        /// <summary>
        /// Depo ID
        /// </summary>
        public Guid? WarehouseID { get; set; }
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Onay
        /// </summary>
        public bool IsApproved { get; set; }
        [NoDatabaseAction]
        public List<SelectPurchasePriceLinesDto> SelectPurchasePriceLines { get; set; }
    }
}
