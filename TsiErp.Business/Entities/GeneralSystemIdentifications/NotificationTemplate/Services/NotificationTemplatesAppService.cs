using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;
using TsiErp.Localizations.Resources.NotificationTemplates.Page;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Localizations.Resources.UnsuitabilityItems.Page;

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
                 Message_ = input.Message_,

            });

            var notificationTemplates = queryFactory.Insert<SelectNotificationTemplatesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplates);

        }

        public string CreateCommandAsync(CreateNotificationTemplatesDto input)
        {

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
                  
                

            });

            return query.Sql;

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

        public async Task<IDataResult<SelectNotificationTemplatesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(
            new
            {
                Id = id
            }, false, true, "").UseIsDelete(false);
            var notificationTemplate = queryFactory.Get<SelectNotificationTemplatesDto>(query);

            LogsAppService.InsertLogToDatabase(notificationTemplate, notificationTemplate, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Get, id);

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

        public async Task<IDataResult<IList<ListNotificationTemplatesDto>>> GetListbyModuleProcessAsync(string module, string process)
        {
            var query = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { ProcessName_ = process }, false, true, "").Where(new { ModuleName_ = module }, false, true, "").UseIsDelete(false);
            var notificationTemplate = queryFactory.GetList<ListNotificationTemplatesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListNotificationTemplatesDto>>(notificationTemplate);
        }

        public async Task<IDataResult<SelectNotificationTemplatesDto>> UpdateAsync(UpdateNotificationTemplatesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<NotificationTemplates>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.NotificationTemplates).Select("*").Where(new { Code = input.Id }, false, false, "");
            var list = queryFactory.GetList<NotificationTemplates>(listQuery).ToList();

            if (list.Count > 0 && entity.Id != input.Id)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.NotificationTemplates).Update(new UpdateNotificationTemplatesDto
            {
                Id = input.Id,
                TargetUsersId = input.TargetUsersId,
                SourceDepartmentId = input.SourceDepartmentId,
                TargetDepartmentId = input.TargetDepartmentId,
                QueryStr = input.QueryStr,
                ContextMenuName_ = input.ContextMenuName_,
                IsActive = input.IsActive,
                ModuleName_ = input.ModuleName_,
                Name = input.Name,
                ProcessName_ = input.ProcessName_,
                 Message_ = input.Message_,
            }).Where(new { Id = input.Id }, true, true, "");

            var notificationTemplate = queryFactory.Update<SelectNotificationTemplatesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, notificationTemplate, LoginedUserService.UserId, Tables.NotificationTemplates, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectNotificationTemplatesDto>(notificationTemplate);
        }

        public Task<IDataResult<SelectNotificationTemplatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}