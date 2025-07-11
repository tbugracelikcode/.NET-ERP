﻿using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services
{
    public interface IOperationStockMovementsAppService : ICrudAppService<SelectOperationStockMovementsDto, ListOperationStockMovementsDto, CreateOperationStockMovementsDto, UpdateOperationStockMovementsDto, ListOperationStockMovementsParameterDto>
    {

        Task<IDataResult<SelectOperationStockMovementsDto>> GetByProductionOrderIdAsync(Guid productionOrderId,Guid productOperationId);
    }
}
