using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Version.Dtos;

namespace TsiErp.Business.Entities.ProgVersion.Services
{
    public interface IProgVersionsAppService : ICrudAppService<SelectProgVersionsDto, ListProgVersionsDto, CreateProgVersionsDto, UpdateProgVersionsDto, ListProgVersionsParameterDto>
    {
        Task<bool> CheckVersion(string progVersion);

        Task<bool> UpdateDatabase(string versionToBeUpdated);

        Task<bool> TableAddColumn();
    }
}
