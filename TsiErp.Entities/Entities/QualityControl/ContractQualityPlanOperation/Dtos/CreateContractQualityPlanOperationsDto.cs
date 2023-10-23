using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos
{
    public class CreateContractQualityPlanOperationsDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Fason Kalite Planı ID
        /// </summary>
        public Guid ContractQualityPlanID { get; set; }
        /// <summary>
        /// Operasyon ID
        /// </summary>
        public Guid? OperationID { get; set; }
        /// <summary>
        /// Satır No
        /// </summary>
        public int LineNr { get; set; }
    }
}
