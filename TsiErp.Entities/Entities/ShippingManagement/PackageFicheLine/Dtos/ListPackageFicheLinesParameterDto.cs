using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos
{
    public class ListPackageFicheLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Paket Fişi ID
        /// </summary>
        public Guid PackageFicheID { get; set; }
    }
}
