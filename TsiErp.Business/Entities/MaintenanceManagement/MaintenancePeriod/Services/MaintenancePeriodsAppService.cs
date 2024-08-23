using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MaintenancePeriod.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenancePeriods.Page;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    [ServiceRegistration(typeof(IMaintenancePeriodsAppService), DependencyInjectionType.Scoped)]
    public class MaintenancePeriodsAppService : ApplicationService<MaintenancePeriodsResource>, IMaintenancePeriodsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public MaintenancePeriodsAppService(IStringLocalizer<MaintenancePeriodsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreateMaintenancePeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> CreateAsync(CreateMaintenancePeriodsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<MaintenancePeriods>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Insert(new CreateMaintenancePeriodsDto
            {
                Code = input.Code,
                IsDaily = input.IsDaily,
                PeriodTime = input.PeriodTime,
                Description_ = input.Description_,
                Name = input.Name,
                Id = GuidGenerator.CreateGuid(),
                CreationTime =now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var maintenancePeriods = queryFactory.Insert<SelectMaintenancePeriodsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MainPeriodsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Insert, maintenancePeriods.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainPeriodsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainPeriodsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
        }

        public async Task<IDataResult<SelectMaintenancePeriodsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var maintenancePeriod = queryFactory.Get<SelectMaintenancePeriodsDto>(query);


            LogsAppService.InsertLogToDatabase(maintenancePeriod, maintenancePeriod, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriod);
        }

        public async Task<IDataResult<IList<ListMaintenancePeriodsDto>>> GetListAsync(ListMaintenancePeriodsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Select<MaintenancePeriods>(s => new { s.Code, s.Name, s.PeriodTime, s.Description_, s.Id }).Where(null, "");
            var maintenancePeriods = queryFactory.GetList<ListMaintenancePeriodsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMaintenancePeriodsDto>>(maintenancePeriods);
        }

        [ValidationAspect(typeof(UpdateMaintenancePeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateAsync(UpdateMaintenancePeriodsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<MaintenancePeriods>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<MaintenancePeriods>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Update(new UpdateMaintenancePeriodsDto
            {
                Code = input.Code,
                PeriodTime = input.PeriodTime,
                IsDaily = input.IsDaily,
                Description_ = input.Description_,
                Name = input.Name,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, maintenancePeriods, LoginedUserService.UserId, Tables.MaintenancePeriods, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MainPeriodsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
        }

        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.MaintenancePeriods).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<MaintenancePeriods>(entityQuery);

            var query = queryFactory.Query().From(Tables.MaintenancePeriods).Update(new UpdateMaintenancePeriodsDto
            {
                Code = entity.Code,
                IsDaily = entity.IsDaily,
                PeriodTime = entity.PeriodTime,
                Description_ = entity.Description_,
                Name = entity.Name,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var maintenancePeriods = queryFactory.Update<SelectMaintenancePeriodsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenancePeriodsDto>(maintenancePeriods);
        }
    }
}
