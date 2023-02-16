using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    public interface IMaintenancePeriodsAppService : ICrudAppService<SelectMaintenancePeriodsDto, ListMaintenancePeriodsDto, CreateMaintenancePeriodsDto, UpdateMaintenancePeriodsDto, ListMaintenancePeriodsParameterDto>
    {
    }
}
