using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos
{
    public class ListMRPIIsDto : FullAuditedEntityDto
    {

        /// <summary>
        /// MRPII Kodu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Hesaplama Tarihi
        /// </summary>
        public DateTime CalculationDate { get; set; }
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description_ { get; set; }

    }
}
