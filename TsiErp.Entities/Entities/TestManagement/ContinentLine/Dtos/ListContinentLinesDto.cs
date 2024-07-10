using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos
{
    public class ListContinentLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Kıta ID
        /// </summary>
        public Guid ContinentID { get; set; }
        /// <summary>
        /// Ülke Adı
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// Ülke Nüfus
        /// </summary>
        public int CountryPopulation { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// NATO Üyesi
        /// </summary>
        public bool IsNATOMember { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
    }
}
