using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services
{
    [ServiceRegistration(typeof(INotificationTemplatesAppService), DependencyInjectionType.Scoped)]
    public class NotificationTemplatesAppService : INotificationTemplatesAppService
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

        #region Unused Methods

        public Task<IDataResult<SelectNotificationTemplatesDto>> UpdateAsync(UpdateNotificationTemplatesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectNotificationTemplatesDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListAsync(ListNotificationTemplatesParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectNotificationTemplatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}