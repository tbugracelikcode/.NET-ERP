using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos
{
    public class ListContractTrackingFicheLinesParameterDto:FullAuditedEntityDto
    {
        /// <summary>
        /// Fason Takip Fişi ID
        /// </summary>
        public Guid ContractTrackingFicheID { get; set; }
    }
}
