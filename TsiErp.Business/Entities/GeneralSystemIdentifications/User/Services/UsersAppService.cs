using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.EntityContracts.User;
using TsiErp.Localizations.Resources.Users.Page;

namespace TsiErp.Business.Entities.User.Services
{
    [ServiceRegistration(typeof(IUsersAppService), DependencyInjectionType.Scoped)]
    public class UsersAppService : ApplicationService<UsersResource>, IUsersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;


        private readonly IUserPermissionsAppService _UserPermissionsAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public UsersAppService(IStringLocalizer<UsersResource> l, IFicheNumbersAppService ficheNumbersAppService, IUserPermissionsAppService userPermissionsAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _UserPermissionsAppService = userPermissionsAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateUsersValidator), Priority = 1)]
        public async Task<IDataResult<SelectUsersDto>> CreateAsync(CreateUsersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Users).Select("Code").Where(new { Code = input.Code, IsActive = false }, "");

            var list = queryFactory.ControlList<Users>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Users).Insert(new CreateUsersDto
            {
                Code = input.Code,
                Email = input.Email,
                GroupID = input.GroupID.GetValueOrDefault(),
                NameSurname = input.NameSurname,
                Password = input.Password,
                UserName = input.UserName,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var users = queryFactory.Insert<SelectUsersDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UsersChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Users, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UsersChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await _UserPermissionsAppService.AllPermissionsAddedUser(addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.Users).Delete(LoginedUserService.UserId).Where(new { Id = id, IsActive = true }, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Users, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UsersChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = entity.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.Users).Select<Users>(null)
                        .Join<UserGroups>
                        (
                            ug => new { GroupID = ug.Id, GroupName = ug.Name },
                            nameof(Users.GroupID),
                            nameof(UserGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id, IsActive = true }, Tables.Users);

            var user = queryFactory.Get<SelectUsersDto>(query);

            LogsAppService.InsertLogToDatabase(user, user, LoginedUserService.UserId, Tables.Users, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(user);
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsyncByUserNameAndPassword(string userName, string password)
        {
            var query = queryFactory.Query().From(Tables.Users).Select<Users>(null)
                        .Join<UserGroups>
                        (
                            ug => new { GroupID = ug.Id, GroupName = ug.Name },
                            nameof(Users.GroupID),
                            nameof(UserGroups.Id),
                            JoinType.Left
                        ).Where(new { UserName = userName, Password = password, IsActive = true }, Tables.Users);

            var user = queryFactory.Get<SelectUsersDto>(query);

            LogsAppService.InsertLogToDatabase(user, user, LoginedUserService.UserId, Tables.Users, LogType.Get, user.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(user);
        }

        public async Task<IDataResult<IList<ListUsersDto>>> GetListAsync(ListUsersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Users).Select<Users>(s => new { s.Code, s.UserName, s.NameSurname, s.Email, s.Id })
                       .Join<UserGroups>
                        (
                            ug => new { GroupID = ug.Id, GroupName = ug.Name },
                            nameof(Users.GroupID),
                            nameof(UserGroups.Id),
                            JoinType.Left
                        ).Where(null, Tables.Users);

            var users = queryFactory.GetList<ListUsersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUsersDto>>(users);

        }


        [ValidationAspect(typeof(UpdateUsersValidator), Priority = 1)]
        public async Task<IDataResult<SelectUsersDto>> UpdateAsync(UpdateUsersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Id = input.Id, IsActive = true }, "");
            var entity = queryFactory.Get<Users>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Code = input.Code, IsActive = false }, "");
            var list = queryFactory.GetList<Users>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Users).Update(new UpdateUsersDto
            {
                Code = input.Code,
                Id = input.Id,
                Email = input.Email,
                GroupID = input.GroupID.GetValueOrDefault(),
                NameSurname = input.NameSurname,
                Password = input.Password,
                UserName = input.UserName,
                IsActive = input.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id, IsActive = true }, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, users, LoginedUserService.UserId, Tables.Users, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UsersChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);
        }

        public async Task<IDataResult<SelectUsersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Id = id, IsActive = true }, "");
            var entity = queryFactory.Get<Users>(entityQuery);

            var query = queryFactory.Query().From(Tables.Users).Update(new UpdateUsersDto
            {
                Code = entity.Code,
                Email = entity.Email,
                GroupID = entity.GroupID,
                NameSurname = entity.NameSurname,
                Password = entity.Password,
                UserName = entity.UserName,
                IsActive = entity.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id, IsActive = true }, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);

        }
    }
}
