using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos
{
    public class UpdateMRPIILinesDto : FullAuditedEntityDto
    {
        /// <summary>
        /// MRPII ID
        /// </summary>
        public Guid MRPIIID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? ProductID { get; set; }
        /// <summary>
        /// Tahmini Satın Alma Temin Tarihi
        /// </summary>
        public DateTime EstimatedPurchaseSupplyDate { get; set; }
        /// <summary>
        /// Tahmini Satın Alma Temin Tarihi
        /// </summary>
        public DateTime EstimatedProductionStartDate { get; set; }
        /// <summary>
        /// Tahmini Satın Alma Temin Tarihi
        /// </summary>
        public DateTime EstimatedProductionEndDate { get; set; }
        /// <summary>
        /// Stok ID
        /// </summary>
        public Guid? LinkedProductID { get; set; }
    }
}
