using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.TestManagement.Sector.Dtos;

namespace TsiErp.Business.Entities.TestManagement.Sector.Services
{
    public interface ISectorsAppService : ICrudAppService<SelectSectorsDto, ListSectorsDto, CreateSectorsDto, UpdateSectorsDto, ListSectorsParameterDto>
    {
    }
}
