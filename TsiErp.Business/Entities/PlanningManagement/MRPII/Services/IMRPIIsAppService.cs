using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRPII.Services
{
    public interface IMRPIIsAppService : ICrudAppService<SelectMRPIIsDto, ListMRPIIsDto, CreateMRPIIsDto, UpdateMRPIIsDto, ListMRPIIsParameterDto>
    {
    }
}
