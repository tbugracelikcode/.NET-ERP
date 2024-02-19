using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos
{
    public class SelectMRPIILinesDto : FullAuditedEntityDto
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
        /// Stok Kodu
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Stok Açıklaması
        /// </summary>
        public string ProductName { get; set; }
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
        /// Bağlı Olduğu Stok Kodu
        /// </summary>
        public string LinkedProductCode { get; set; }
        /// <summary>
        /// Bağlı Olduğu Stok Açıklaması
        /// </summary>
        public string LinkedProductName { get; set; }
    }
}
