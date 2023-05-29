using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.Version.Dtos;

namespace TsiErp.Business.Entities.ProgVersion.Services
{
    public interface IProgVersionsAppService : ICrudAppService<SelectProgVersionsDto, ListProgVersionsDto, CreateProgVersionsDto, UpdateProgVersionsDto, ListProgVersionsParameterDto>
    {
        Task<bool> CheckVersion(string progVersion);

        Task<bool> UpdateDatabase(string versionToBeUpdated);
    }
}
