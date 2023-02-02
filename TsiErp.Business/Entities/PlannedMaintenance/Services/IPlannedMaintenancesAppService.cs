using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    public interface IPlannedMaintenancesAppService : ICrudAppService<SelectPlannedMaintenancesDto, ListPlannedMaintenancesDto, CreatePlannedMaintenancesDto, UpdatePlannedMaintenancesDto, ListPlannedMaintenancesParameterDto>
    {

    }
}
