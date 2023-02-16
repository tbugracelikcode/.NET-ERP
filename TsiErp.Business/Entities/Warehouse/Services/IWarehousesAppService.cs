using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.WareHouse.Dtos;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    public interface IWarehousesAppService : ICrudAppService<SelectWarehousesDto, ListWarehousesDto, CreateWarehousesDto, UpdateWarehousesDto, ListWarehousesParameterDto>
    {
    }
}
