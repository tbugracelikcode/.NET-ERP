using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos
{
    public class ListPackingListPalletCubageLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Çeki Listesi ID
        /// </summary>
        public Guid PackingListID { get; set; }
    }
}
