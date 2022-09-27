using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.Station.Dtos
{
    public class ListStationsParameterDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Aktiflik
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
