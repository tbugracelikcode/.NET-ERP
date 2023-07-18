using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos
{
    public class SelectPlanningManagementParametersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
    }
}
