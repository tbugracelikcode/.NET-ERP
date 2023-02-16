using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    public interface IEquipmentRecordsAppService : ICrudAppService<SelectEquipmentRecordsDto, ListEquipmentRecordsDto, CreateEquipmentRecordsDto, UpdateEquipmentRecordsDto, ListEquipmentRecordsParameterDto>
    {
    }
}
