using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    public interface ITechnicalDrawingsAppService : ICrudAppService<SelectTechnicalDrawingsDto, ListTechnicalDrawingsDto, CreateTechnicalDrawingsDto, UpdateTechnicalDrawingsDto, ListTechnicalDrawingsParameterDto>
    {
        Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId);
    }
}
