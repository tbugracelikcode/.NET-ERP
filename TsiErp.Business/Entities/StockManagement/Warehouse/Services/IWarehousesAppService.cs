using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    public interface IWarehousesAppService : ICrudAppService<SelectWarehousesDto, ListWarehousesDto, CreateWarehousesDto, UpdateWarehousesDto, ListWarehousesParameterDto>
    {
    }
}
