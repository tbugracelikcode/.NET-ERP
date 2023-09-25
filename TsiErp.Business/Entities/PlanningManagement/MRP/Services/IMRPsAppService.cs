using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.MRP.Services
{
    public interface IMRPsAppService : ICrudAppService<SelectMRPsDto, ListMRPsDto, CreateMRPsDto, UpdateMRPsDto, ListMRPsParameterDto>
    {
    }
}
