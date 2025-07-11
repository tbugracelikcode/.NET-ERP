﻿using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.HaltReason.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.HaltReasons.Page;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    [ServiceRegistration(typeof(IHaltReasonsAppService), DependencyInjectionType.Scoped)]
    public class HaltReasonsAppService : ApplicationService<HaltReasonsResource>, IHaltReasonsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public HaltReasonsAppService(IStringLocalizer<HaltReasonsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateHaltReasonsValidator), Priority = 1)]
        public async Task<IDataResult<SelectHaltReasonsDto>> CreateAsync(CreateHaltReasonsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.HaltReasons).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<HaltReasons>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.HaltReasons).Insert(new CreateHaltReasonsDto
            {
                Code = input.Code,
                Name = input.Name,
                IsMachine = input.IsMachine,
                IsIncidentalHalt = input.IsIncidentalHalt,
                IsManagement = input.IsManagement,
                IsOperator = input.IsOperator,
                IsPlanned = input.IsPlanned,
                Id = addedEntityId,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var haltReasons = queryFactory.Insert<SelectHaltReasonsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("HaltReasonsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.HaltReasons, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["HaltReasonsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);


        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("HaltID", new List<string>
            {
                Tables.ProductionTrackingHaltLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.HaltReasons).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.HaltReasons, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["HaltReasonsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains("*Not*"))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                     
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
                return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);
            }
        }

        public async Task<IDataResult<SelectHaltReasonsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var haltReason = queryFactory.Get<SelectHaltReasonsDto>(query);


            LogsAppService.InsertLogToDatabase(haltReason, haltReason, LoginedUserService.UserId, Tables.HaltReasons, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReason);

        }

        public async Task<IDataResult<IList<ListHaltReasonsDto>>> GetListAsync(ListHaltReasonsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.HaltReasons).Select<HaltReasons>(s => new { s.Code, s.Name, s.IsPlanned, s.IsMachine, s.IsOperator, s.IsManagement, s.IsIncidentalHalt, s.Id }).Where(null, "");
            var haltReasons = queryFactory.GetList<ListHaltReasonsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListHaltReasonsDto>>(haltReasons);
        }

        [ValidationAspect(typeof(UpdateHaltReasonsValidator), Priority = 1)]
        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateAsync(UpdateHaltReasonsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<HaltReasons>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<HaltReasons>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.HaltReasons).Update(new UpdateHaltReasonsDto
            {
                Code = input.Code,
                Name = input.Name,
                IsPlanned = input.IsPlanned,
                IsOperator = input.IsOperator,
                IsManagement = input.IsManagement,
                IsIncidentalHalt = input.IsIncidentalHalt,
                IsMachine = input.IsMachine,
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

            var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, haltReasons, LoginedUserService.UserId, Tables.HaltReasons, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["HaltReasonsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);


        }

        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<HaltReasons>(entityQuery);

            var query = queryFactory.Query().From(Tables.HaltReasons).Update(new UpdateHaltReasonsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                IsMachine = entity.IsMachine,
                IsManagement = entity.IsManagement,
                IsIncidentalHalt = entity.IsIncidentalHalt,
                IsOperator = entity.IsOperator,
                IsPlanned = entity.IsPlanned,
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

            var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);

        }
    }
}
