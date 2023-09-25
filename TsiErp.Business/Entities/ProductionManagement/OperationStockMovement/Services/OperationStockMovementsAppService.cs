using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Entities.Entities.ProductionManagement.OperationStockMovement.Dtos;
using TsiErp.Localizations.Resources.CurrentAccountCards.Page;
using TsiErp.Localizations.Resources.OperationStockMovement.Page;

namespace TsiErp.Business.Entities.ProductionManagement.OperationStockMovement.Services
{
    [ServiceRegistration(typeof(IOperationStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class OperationStockMovementsAppService : ApplicationService<OperationStockMovementsResources>, IOperationStockMovementsAppService
    {
        public OperationStockMovementsAppService(IStringLocalizer<OperationStockMovementsResources> l) : base(l)
        {
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> CreateAsync(CreateOperationStockMovementsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListOperationStockMovementsDto>>> GetListAsync(ListOperationStockMovementsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> UpdateAsync(UpdateOperationStockMovementsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
