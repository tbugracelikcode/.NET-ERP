using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSI.QueryBuilder.BaseClasses;
using Tsi.Core.Utilities.Guids;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Logging.Services;
using Newtonsoft.Json;
using TsiErp.Entities.TableConstant;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services
{
    [ServiceRegistration(typeof(INotificationTemplatesAppService), DependencyInjectionType.Scoped)]
    public class NotificationTemplatesAppService 
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectNotificationTemplatesDto>> CreateAsync(CreateNotificationTemplatesDto input)
        {

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Insert(new CreateNotificationTemplatesDto
            {   
                Id = Guid.Empty,
                SourceDepartmentId = Guid.Empty,
                TargetDepartmentId = Guid.Empty,
                ProcessName_ = input.ProcessName_,
                ModuleName_ = input.ModuleName_,
                ContextMenuName_ = input.ContextMenuName_,
                QueryStr = input.QueryStr,
                IsActive = false,

            });

            var notificationTemplates = queryFactory.Insert<SelectNotificationTemplatesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var notificationTemplates = queryFactory.Update<SelectNotificationTemplatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }
    }
}