using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ForecastLine.Dtos
{
    public class ListForecastLinesParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Forecast ID
        /// </summary>
        public Guid ForecastID { get; set; }
    }
}
