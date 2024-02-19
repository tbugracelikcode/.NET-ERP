using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.StationOccupancy.Dtos
{
    public class SelectStationOccupanciesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İstasyon ID
        /// </summary>
        public Guid StationID { get; set; }
        /// <summary>
        /// İstasyon Kodu
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// Çalışacağı Süre
        /// </summary>
        public decimal TimeItWillWork { get; set; }
        /// <summary>
        /// Sipariş ID
        /// </summary>
        public Guid SalesOrderID { get; set; }
        /// <summary>
        /// Sipariş No
        /// </summary>
        public string SalesOrderFicheNo { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid ProductID { get; set; }
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// İş Emri ID
        /// </summary>
        public Guid WorkOrderID { get; set; }
        /// <summary>
        /// İş Emri No
        /// </summary>
        public string WorkOrderFicheNo { get; set; }
        /// <summary>
        /// Çalıştığı Süre
        /// </summary>
        public decimal TimeItWorked { get; set; }
    }
}
