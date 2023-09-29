using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos
{
    public class UpdateMRPLinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// MRP ID
        /// </summary>
        public Guid MRPID { get; set; }
        /// <summary>
        /// Satış Siparişi ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Satış Siparişi Satış ID
        /// </summary>
        public Guid? SalesOrderLineID { get; set; }
        /// <summary>
        /// Hesaplanacak
        /// </summary>
        public bool isCalculated { get; set; }
        /// <summary>
        /// Satın Alınacak
        /// </summary>
        public bool isPurchase { get; set; }
        /// <summary>
        /// Miktar
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Birim Seti ID
        /// </summary>
        public Guid? UnitSetID { get; set; }
        /// <summary>
        /// Durum
        /// </summary>
        public string State_ { get; set; }
        /// <summary>
        /// İhtiyaç Miktar
        /// </summary>
        public int RequirementAmount { get; set; }
    }
}
