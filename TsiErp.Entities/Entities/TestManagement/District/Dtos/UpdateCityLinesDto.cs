using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.TestManagement.District.Dtos
{
    public class UpdateCityLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Şehir ID
        /// </summary>
        public Guid CityID { get; set; }
        /// <summary>
        /// İlçe Adı
        /// </summary>
        public string DistrictName { get; set; }
        /// <summary>
        /// İlçe Nüfus
        /// </summary>
        public int DistrictPopulation { get; set; }
        /// <summary>
        /// Mahalle Sayısı
        /// </summary>
        public int NumberofTown { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Hastane İçeriyor
        /// </summary>
        public bool IsIncludeHospital { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }
        /// <summary>
        /// İlçe Talimatı
        /// </summary>
        public string DistrictInstruction { get; set; }
    }
}
