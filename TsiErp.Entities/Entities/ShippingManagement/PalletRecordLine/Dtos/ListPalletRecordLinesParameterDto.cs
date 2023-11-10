using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos
{
    public class ListPalletRecordLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Palet Kaydı ID
        /// </summary>
        public Guid PalletRecordID { get; set; }
    }
}
