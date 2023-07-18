using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Dtos
{
    public class ListMachineAndWorkforceManagementParametersDto : FullAuditedEntityDto
    {
        /// <summary>
        /// İleri Zamanlı Tarih Parametresi
        /// </summary>
        public bool FutureDateParameter { get; set; }
    }
}
