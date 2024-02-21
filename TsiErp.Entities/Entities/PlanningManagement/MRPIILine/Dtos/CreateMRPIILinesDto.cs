using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.SqlDataTypeMappingUtilities;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos
{
    public class CreateMRPIILinesDto : FullAuditedEntityDto
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

        /// <summary>
        /// Satış Sipariş ID
        /// </summary>
        public Guid? SalesOrderID { get; set; }
        /// <summary>
        /// Sipariş Kabul ID
        /// </summary>
        public Guid? OrderAcceptanceID { get; set; }
    }
}
