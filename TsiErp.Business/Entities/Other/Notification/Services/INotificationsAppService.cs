using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;

namespace TsiErp.Business.Entities.Other.Notification.Services
{
    public interface INotificationsAppService : ICrudAppService<SelectNotificationsDto, ListNotificationsDto, CreateNotificationsDto, UpdateNotificationsDto, ListNotificationsParameterDto>
    {
        string CreateCommandAsync(CreateNotificationsDto input);

        Task<IDataResult<IList<SelectNotificationsDto>>> GetListbyUserIDAsync(Guid userID);
        Task<IDataResult<IList<ListNotificationsDto>>> GetListbyUserIDListDtoAsync(Guid userID);

    }
}
