using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Dtos
{
    public class SelectMaintenanceManagementParametersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
    }
}
