using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.OperationQuantityInformation.Services
{
    public interface IOperationQuantityInformationsAppService: ICrudAppService<SelectOperationQuantityInformationsDto, ListOperationQuantityInformationsDto, CreateOperationQuantityInformationsDto, UpdateOperationQuantityInformationsDto, ListOperationQuantityInformationsParameterDto>
    {
    }
}
