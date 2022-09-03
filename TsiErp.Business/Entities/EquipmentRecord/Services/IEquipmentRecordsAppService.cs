using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    public interface IEquipmentRecordsAppService : ICrudAppService<EquipmentRecords, SelectEquipmentRecordsDto, ListEquipmentRecordsDto, CreateEquipmentRecordsDto, UpdateEquipmentRecordsDto, ListEquipmentRecordsParameterDto>
    {
    }
}
