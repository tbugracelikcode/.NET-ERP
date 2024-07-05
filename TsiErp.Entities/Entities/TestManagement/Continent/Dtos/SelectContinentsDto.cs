using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos;

namespace TsiErp.Entities.Entities.TestManagement.Continent.Dtos
{
    public class SelectContinentsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Kıta Kod
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Kıta Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nüfus
        /// </summary>
        public int Population_ { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

        [NoDatabaseAction]
        public List<SelectContinentLinesDto> SelectContinentLines { get; set; }
    }
}
