using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    public interface IEquipmentRecordsAppService : ICrudAppService<SelectEquipmentRecordsDto, ListEquipmentRecordsDto, CreateEquipmentRecordsDto, UpdateEquipmentRecordsDto, ListEquipmentRecordsParameterDto>
    {
    }
}
