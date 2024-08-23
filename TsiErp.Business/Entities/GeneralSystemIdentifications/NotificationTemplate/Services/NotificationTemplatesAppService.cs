using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.TableConstant;
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

        public async Task<IDataResult<SelectNotificationTemplatesDto>> CreateAsync(CreateNotificationTemplatesDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Insert(new CreateNotificationTemplatesDto
            {
                Id = addedEntityId,
                SourceDepartmentId = input.SourceDepartmentId,
                TargetDepartmentId = input.TargetDepartmentId,
                ProcessName_ = input.ProcessName_,
                ModuleName_ = input.ModuleName_,
                ContextMenuName_ = input.ContextMenuName_,
                Name = input.Name,
                IsActive = input.IsActive,
                TargetUsersId = input.TargetUsersId,
                Message_ = input.Message_,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,

            });

            var notificationTemplates = queryFactory.Insert<SelectNotificationTemplatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }

        public string CreateCommandAsync(CreateNotificationTemplatesDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Insert(new CreateNotificationTemplatesDto
            {
                SourceDepartmentId = input.SourceDepartmentId,
                TargetDepartmentId = input.TargetDepartmentId,
                ProcessName_ = input.ProcessName_,
                ModuleName_ = input.ModuleName_,
                ContextMenuName_ = input.ContextMenuName_,
                Name = input.Name,
                IsActive = input.IsActive,
                TargetUsersId = input.TargetUsersId,
                Message_ = input.Message_,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });
            return query.Sql;
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).UseIsDelete(false).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var notificationTemplates = queryFactory.Delete(query, true);

            //LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>("");

        }

        #region Unused Method

        public async Task<IDataResult<SelectNotificationTemplatesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select<NotificationTemplates>(null)
                   .Join<UserGroups>
                    (
                        p => new { SourceDepartmentId = p.Id, SourceDepartmentName = p.Name },
                        nameof(NotificationTemplates.SourceDepartmentId),
                        nameof(UserGroups.Id),
                        JoinType.Left
                    ).Where(new { Id = id, IsActive = true }, Tables.NotificationTemplates);
            var notificationTemplate = queryFactory.Get<SelectNotificationTemplatesDto>(query);

            LogsAppService.InsertLogToDatabase(notificationTemplate, notificationTemplate, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplate);
        }

        public async Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListAsync(ListNotificationTemplatesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select<NotificationTemplates>(s => new { s.ModuleName_, s.Name, s.ProcessName_, s.ContextMenuName_, s.IsActive, s.Id }).Where(null, "");
            var notificationTemplate = queryFactory.GetList<ListNotificationTemplatesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationTemplatesDto>>(notificationTemplate);
        }

        public async Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListbyModuleProcessAsync(string module, string process)
        {
            //var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { ProcessName_ = process }, false, false, "").Where(new { ModuleName_ = module }, false, true, "").UseIsDelete(false);

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { ProcessName_ = process, ModuleName_ = module }, "");


            var notificationTemplate = queryFactory.GetList<ListNotificationTemplatesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationTemplatesDto>>(notificationTemplate);
        }

        public async Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListbyModuleProcessContextAsync(string module,  string context)
        {
            //var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { ProcessName_ = process }, false, false, "").Where(new { ModuleName_ = module }, false, true, "").UseIsDelete(false);

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { ModuleName_ = module, ContextMenuName_ = context }, "");


            var notificationTemplate = queryFactory.GetList<ListNotificationTemplatesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationTemplatesDto>>(notificationTemplate);
        }

        public async Task<IDataResult<SelectNotificationTemplatesDto>> UpdateAsync(UpdateNotificationTemplatesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<SelectNotificationTemplatesDto>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { Code = input.Id }, "");
            var list = queryFactory.GetList<NotificationTemplates>(listQuery).ToList();

            if (list.Count > 0 && entity.Id != input.Id)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Update(new UpdateNotificationTemplatesDto
            {
                Id = input.Id,
                TargetUsersId = input.TargetUsersId,
                SourceDepartmentId = input.SourceDepartmentId,
                TargetDepartmentId = input.TargetDepartmentId,
                ContextMenuName_ = input.ContextMenuName_,
                IsActive = input.IsActive,
                ModuleName_ = input.ModuleName_,
                Name = input.Name,
                ProcessName_ = input.ProcessName_,
                Message_ = input.Message_,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },"");

            var notificationTemplate = queryFactory.Update<SelectNotificationTemplatesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, notificationTemplate, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplate);
        }

        public async Task<IDataResult<SelectNotificationTemplatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<NotificationTemplates>(entityQuery);

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Update(new UpdateNotificationTemplatesDto
            {
                ProcessName_ = entity.ProcessName_,
                Message_ = entity.Message_,
                ContextMenuName_ = entity.ContextMenuName_,
                IsActive = entity.IsActive,
                ModuleName_ = entity.ModuleName_,
                Name = entity.Name,
                SourceDepartmentId = entity.SourceDepartmentId,
                TargetDepartmentId = entity.TargetDepartmentId,
                TargetUsersId = entity.TargetUsersId,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                Id = entity.Id,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var NotificationTemplatesDto = queryFactory.Update<SelectNotificationTemplatesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(NotificationTemplatesDto);
        }
    }
    #endregion
}
