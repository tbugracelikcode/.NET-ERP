using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StartingSalary.Services
{
    public interface IStartingSalariesAppService : ICrudAppService<SelectStartingSalariesDto, ListStartingSalariesDto, CreateStartingSalariesDto, UpdateStartingSalariesDto, ListStartingSalariesParameterDto>
    {
    }
}
