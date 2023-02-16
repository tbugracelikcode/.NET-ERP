using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.HaltReason.Dtos;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    public interface IHaltReasonsAppService : ICrudAppService<SelectHaltReasonsDto, ListHaltReasonsDto, CreateHaltReasonsDto, UpdateHaltReasonsDto, ListHaltReasonsParameterDto>
    {
    }
}
