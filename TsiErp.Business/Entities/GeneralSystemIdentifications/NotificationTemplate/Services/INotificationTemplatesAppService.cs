using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services
{
    public interface INotificationTemplatesAppService : 
        ICrudAppService<SelectNotificationTemplatesDto, ListNotificationTemplatesDto, CreateNotificationTemplatesDto, UpdateNotificationTemplatesDto, ListNotificationTemplatesParameterDto>
    {
    }
}
