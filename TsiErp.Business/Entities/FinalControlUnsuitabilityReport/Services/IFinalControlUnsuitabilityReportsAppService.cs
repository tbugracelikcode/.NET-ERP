using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Services
{
    public interface IFinalControlUnsuitabilityReportsAppService : ICrudAppService<SelectFinalControlUnsuitabilityReportsDto, ListFinalControlUnsuitabilityReportsDto, CreateFinalControlUnsuitabilityReportsDto, UpdateFinalControlUnsuitabilityReportsDto, ListFinalControlUnsuitabilityReportsParameterDto>
    {
    }
}
