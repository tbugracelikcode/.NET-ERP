using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine.Dtos
{
    public class UpdateStationOccupancyLinesDto : IEntityDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationOccupancyID { get; set; }
        /// <summary>
        /// Satır No 
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Üretim Emri ID
        /// </summary>
        public Guid ProductionOrderID { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Ürün Operasyon ID
        /// </summary>
        public Guid ProductsOperationID { get; set; }
        /// <summary>
        /// Planlanan Başlangıç Tarihi
        /// </summary>
        public DateTime? PlannedStartDate { get; set; }
        /// <summary>
        /// Planlanan Bitiş Tarihi
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }
        /// <summary>
        /// Sevkiyat Planı ID
        /// </summary>
        public Guid ShipmentPlanningID { get; set; }
    }
}
