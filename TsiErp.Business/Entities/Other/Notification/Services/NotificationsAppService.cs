
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.TableConstant;
using TsiErp.Entities.Entities.Other.Notification;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Localizations.Resources.NotificationTemplates.Page;
using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Localizations.Resources.Notifications.Page;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu;


namespace TsiErp.Business.Entities.Other.Notification.Services
{
    [ServiceRegistration(typeof(INotificationsAppService), DependencyInjectionType.Scoped)]
    public class NotificationsAppService : ApplicationService<NotificationsResource>, INotificationsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public NotificationsAppService(IStringLocalizer<NotificationsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectNotificationsDto>> CreateAsync(CreateNotificationsDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Notifications).Insert(new CreateNotificationsDto
            {
                Id = addedEntityId,
                UserId = input.UserId,
                NotificationDate = input.NotificationDate,
                IsViewed = input.IsViewed,
                ViewDate = input.ViewDate,
                Message_ = input.Message_,
                ContextMenuName_ = input.ContextMenuName_,
                ModuleName_ = input.ModuleName_,
                ProcessName_ = input.ProcessName_,
                RecordNumber = input.RecordNumber
            });

            var notifications = queryFactory.Insert<SelectNotificationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Notifications, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationsDto>(notifications);

        }

        [CacheRemoveAspect("Get")]

        #region Get Method

        public async Task<IDataResult<SelectNotificationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Notifications).Select("*").Where(
            new
            {
                Id = id
            }, false, true, "").UseIsDelete(false);
            var notification = queryFactory.Get<SelectNotificationsDto>(query);

            LogsAppService.InsertLogToDatabase(notification, notification, LoginedUserService.UserId, Tables.Notifications, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationsDto>(notification);
        }
        public async Task<IDataResult<IList<SelectNotificationsDto>>> GetListbyUserIDAsync(Guid userID)
        {
            var query = queryFactory.Query().From(Tables.Notifications).Select("*").Where(
                new
                {
                    UserId = userID
                }, false, false, "").UseIsDelete(false);
            var notification = queryFactory.GetList<SelectNotificationsDto>(query).ToList();
            return new SuccessDataResult<IList<SelectNotificationsDto>>(notification);
        }

        public async Task<IDataResult<IList<ListNotificationsDto>>> GetListAsync(ListNotificationsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Notifications).Select("*").Where(null, false, true, "").UseIsDelete(false);
            var notification = queryFactory.GetList<ListNotificationsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationsDto>>(notification);
        }
        #endregion

        public string CreateCommandAsync(CreateNotificationsDto input)
        {

            var query = queryFactory.Query().From(Tables.Notifications).Insert(new CreateNotificationsDto
            {
                UserId = input.UserId,
                NotificationDate = input.NotificationDate,
                IsViewed = input.IsViewed,
                ViewDate = input.ViewDate,
                Message_ = input.Message_,
                ContextMenuName_ = input.ContextMenuName_,
                ModuleName_ = input.ModuleName_,
                ProcessName_ = input.ProcessName_,
                RecordNumber = input.RecordNumber
            });

            return query.Sql;

        }


        public async Task<IDataResult<SelectNotificationsDto>> UpdateAsync(UpdateNotificationsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Notifications).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Notifications>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Notifications).Select("*").Where(new { Code = input.Id }, false, false, "");
            var list = queryFactory.GetList<Notifications>(listQuery).ToList();

            if (list.Count > 0 && entity.Id != input.Id)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.Notifications).Update(new UpdateNotificationsDto
            {
                Id = input.Id,
                IsViewed = true,
                Message_ = input.Message_,
                NotificationDate = DateTime.Now,
                UserId = input.UserId,
                ViewDate = DateTime.Now,
                RecordNumber = input.RecordNumber,
                ProcessName_ = input.ProcessName_,
                ModuleName_ = input.ModuleName_,
                ContextMenuName_ = input.ContextMenuName_,

            }).Where(new { Id = input.Id }, true, true, "");

            var notification = queryFactory.Update<SelectNotificationsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, notification, LoginedUserService.UserId, Tables.Notifications, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationsDto>(notification);
        }
        public Task<IDataResult<SelectNotificationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();

        }

    }
}