using Tsi.Core.Entities.Auditing;
using TSI.QueryBuilder.MappingAttributes;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos;

namespace TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos
{
    public class CreateMRPIIsDto : FullAuditedEntityDto
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

        [NoDatabaseAction]
        public List<SelectMRPIILinesDto> SelectMRPIILines { get; set; }
    }
}
