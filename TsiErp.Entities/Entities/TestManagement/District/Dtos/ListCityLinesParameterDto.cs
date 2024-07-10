using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.TestManagement.District.Dtos
{
    public class ListCityLinesParameterDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Şehir ID
        /// </summary>
        public Guid CityID { get; set; }
    }
}
