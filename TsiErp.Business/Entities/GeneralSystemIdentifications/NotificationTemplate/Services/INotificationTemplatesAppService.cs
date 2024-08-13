using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services
{
    public interface INotificationTemplatesAppService : 
        ICrudAppService<SelectNotificationTemplatesDto, ListNotificationTemplatesDto, CreateNotificationTemplatesDto, UpdateNotificationTemplatesDto, ListNotificationTemplatesParameterDto>
    {
        string CreateCommandAsync(CreateNotificationTemplatesDto input);

        Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListbyModuleProcessAsync(string module, string process);
        Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListbyModuleProcessContextAsync(string module,  string context);
    }
}
