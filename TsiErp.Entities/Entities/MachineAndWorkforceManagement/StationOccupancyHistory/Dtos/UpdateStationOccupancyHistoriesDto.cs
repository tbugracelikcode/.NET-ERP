using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Dtos
{
    public class UpdateStationOccupancyHistoriesDto : IEntityDto
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
        /// <summary>
        /// Sevkiyat Planı ID
        /// </summary>
        public Guid ShipmentPlanningID { get; set; }
    }
}
