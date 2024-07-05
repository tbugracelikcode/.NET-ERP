using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.TestManagement.SectorLine.Dtos
{
    public class CreateSectorLinesDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Sektör ID
        /// </summary>
        public Guid SectorID { get; set; }
 
        /// <summary>
        /// Firma Adı
        /// </summary>
        public string CompanyName { get; set; }
 
        /// <summary>
        /// Firma no
        /// </summary>
        public int CompanyNo { get; set; }

        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Şahıs Şirketi
        /// </summary>
        public bool IsSoleProprietorship { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
