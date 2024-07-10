using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos;
using TsiErp.Entities.Entities.TestManagement.SectorLine.Dtos;

namespace TsiErp.Entities.Entities.TestManagement.Sector.Dtos
{
    public class CreateSectorsDto : FullAuditedEntityDto
    {
       
        /// <summary>
        /// Sektör Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Sektör Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tür
        /// </summary>
        public string Type_ { get; set; }
        /// <summary>
        /// Özel Sektör
        /// </summary>
        public bool IsPrivateSector { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectSectorLinesDto> SelectSectorLines { get; set; }
    }
}
