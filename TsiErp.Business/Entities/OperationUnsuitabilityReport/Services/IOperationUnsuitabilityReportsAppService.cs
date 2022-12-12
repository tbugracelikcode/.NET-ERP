using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    public interface IOperationUnsuitabilityReportsAppService : ICrudAppService<SelectOperationUnsuitabilityReportsDto, ListOperationUnsuitabilityReportsDto, CreateOperationUnsuitabilityReportsDto, UpdateOperationUnsuitabilityReportsDto, ListOperationUnsuitabilityReportsParameterDto>
    {
    }
}
