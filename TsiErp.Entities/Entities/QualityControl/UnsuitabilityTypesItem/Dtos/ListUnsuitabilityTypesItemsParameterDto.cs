using Tsi.Core.Entities.Auditing;

namespace TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos
{
    public class ListUnsuitabilityTypesItemsParameterDto : FullAuditedEntityDto
    {
        public bool IsActive { get; set; } = true;
    }
}
