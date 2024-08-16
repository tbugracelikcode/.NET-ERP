using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.EmployeeAnnualSeniorityDifference.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EmployeeAnnualSeniorityDifferences.Page;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    [ServiceRegistration(typeof(IEmployeeAnnualSeniorityDifferencesAppService), DependencyInjectionType.Scoped)]
    public class EmployeeAnnualSeniorityDifferencesAppService : ApplicationService<EmployeeAnnualSeniorityDifferencesResource>, IEmployeeAnnualSeniorityDifferencesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public EmployeeAnnualSeniorityDifferencesAppService(IStringLocalizer<EmployeeAnnualSeniorityDifferencesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateEmployeeAnnualSeniorityDifferencesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> CreateAsync(CreateEmployeeAnnualSeniorityDifferencesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Insert(new CreateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = input.Description_,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                Difference = input.Difference,
                Code = input.Code,
                Year_ = input.Year_,
                Id = addedEntityId,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
            });


            var EmployeeAnnualSeniorityDifferences = queryFactory.Insert<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EmployeeAnnualSeniorityDifferencesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeAnnualSeniorityDifferencesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var EmployeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeAnnualSeniorityDifferencesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }


        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.EmployeeAnnualSeniorityDifferences);
            var EmployeeSeniority = queryFactory.Get<SelectEmployeeAnnualSeniorityDifferencesDto>(query);

            LogsAppService.InsertLogToDatabase(EmployeeSeniority, EmployeeSeniority, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeSeniority);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeeAnnualSeniorityDifferencesDto>>> GetListAsync(ListEmployeeAnnualSeniorityDifferencesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(s => new { s.Code, s.Difference, s.Year_, s.Id })
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(null, Tables.EmployeeAnnualSeniorityDifferences);

            var employeeAnnualSeniorityDifferences = queryFactory.GetList<ListEmployeeAnnualSeniorityDifferencesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEmployeeAnnualSeniorityDifferencesDto>>(employeeAnnualSeniorityDifferences);
        }


        [ValidationAspect(typeof(UpdateEmployeeAnnualSeniorityDifferencesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> UpdateAsync(UpdateEmployeeAnnualSeniorityDifferencesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select<EmployeeAnnualSeniorityDifferences>(null)
                        .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(EmployeeAnnualSeniorityDifferences.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        ).Where(new { Id = input.Id }, Tables.EmployeeAnnualSeniorityDifferences);
            var entity = queryFactory.Get<EmployeeAnnualSeniorityDifferences>(entityQuery);


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Update(new UpdateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = input.Description_,
                Year_ = input.Year_,
                Difference = input.Difference,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                 Code = entity.Code,
            }).Where(new { Id = input.Id }, "");

            var employeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, employeeAnnualSeniorityDifferences, LoginedUserService.UserId, Tables.EmployeeAnnualSeniorityDifferences, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeeAnnualSeniorityDifferencesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(employeeAnnualSeniorityDifferences);
        }

        public async Task<IDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<EmployeeAnnualSeniorityDifferences>(entityQuery);

            var query = queryFactory.Query().From(Tables.EmployeeAnnualSeniorityDifferences).Update(new UpdateEmployeeAnnualSeniorityDifferencesDto
            {
                Description_ = entity.Description_,
                SeniorityID = entity.SeniorityID,
                Difference = entity.Difference,
                Year_ = entity.Year_,
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
                 Code = entity.Code  

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var EmployeeAnnualSeniorityDifferences = queryFactory.Update<SelectEmployeeAnnualSeniorityDifferencesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeeAnnualSeniorityDifferencesDto>(EmployeeAnnualSeniorityDifferences);
        }
    }
}
