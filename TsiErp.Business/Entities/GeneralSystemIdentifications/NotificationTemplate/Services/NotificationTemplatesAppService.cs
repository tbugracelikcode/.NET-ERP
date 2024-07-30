using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;
using TsiErp.Localizations.Resources.NotificationTemplates.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services
{
    [ServiceRegistration(typeof(INotificationTemplatesAppService), DependencyInjectionType.Scoped)]
    public class NotificationTemplatesAppService : ApplicationService<NotificationTemplatesResource>, INotificationTemplatesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public NotificationTemplatesAppService(IStringLocalizer<NotificationTemplatesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectNotificationTemplatesDto>> CreateAsync(CreateNotificationTemplatesDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Insert(new CreateNotificationTemplatesDto
            {
                Id = addedEntityId,
                SourceDepartmentId = input.SourceDepartmentId,
                TargetDepartmentId = input.TargetDepartmentId,
                ProcessName_ = input.ProcessName_,
                ModuleName_ = input.ModuleName_,
                ContextMenuName_ = input.ContextMenuName_,
                Name = input.Name,
                QueryStr = input.QueryStr,
                IsActive = input.IsActive,
                TargetUsersId = input.TargetUsersId,

            });

            var notificationTemplates = queryFactory.Insert<SelectNotificationTemplatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, true, "").UseIsDelete(false);

            var notificationTemplates = queryFactory.Update<SelectNotificationTemplatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }


        [CacheRemoveAspect("Get")]
        #region Unused Method
        public Task<IDataResult<SelectNotificationTemplatesDto>> UpdateAsync(UpdateNotificationTemplatesDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectNotificationTemplatesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(
            new
            {
                Id = id
            }, false, true, "").UseIsDelete(false);
            var notificationTemplate = queryFactory.Get<SelectNotificationTemplatesDto>(query);

            LogsAppService.InsertLogToDatabase(notificationTemplate, notificationTemplate, LoginedUserService.UserId, Tables.Currencies, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplate);
        }

        public async Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListAsync(ListNotificationTemplatesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(null, false, true, "").UseIsDelete(false);
            var notificationTemplate = queryFactory.GetList<ListNotificationTemplatesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationTemplatesDto>>(notificationTemplate);
        }

        public Task<IDataResult<SelectNotificationTemplatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }


       

        #endregion
    }
}