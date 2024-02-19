using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.StationOccupancy.Dtos
{
    public class UpdateStationOccupanciesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// Çalışacağı Süre
        /// </summary>
        public decimal TimeItWillWork { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// Çalıştığı Süre
        /// </summary>
        public decimal TimeItWorked { get; set; }
    }
}
