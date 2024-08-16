using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine.Dtos;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy.Dtos
{
    public class UpdateStationOccupanciesDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Boşa Çıkma Tarihi
        /// </summary>
        public DateTime? FreeDate { get; set; }

        [NoDatabaseAction]
        public List<SelectStationOccupancyLinesDto> SelectStationOccupancyLines { get; set; }
    }
}
