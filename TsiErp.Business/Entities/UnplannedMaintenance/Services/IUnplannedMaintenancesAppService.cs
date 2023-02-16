using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    public interface IUnplannedMaintenancesAppService : ICrudAppService<SelectUnplannedMaintenancesDto, ListUnplannedMaintenancesDto, CreateUnplannedMaintenancesDto, UpdateUnplannedMaintenancesDto, ListUnplannedMaintenancesParameterDto>
    {

    }
}
