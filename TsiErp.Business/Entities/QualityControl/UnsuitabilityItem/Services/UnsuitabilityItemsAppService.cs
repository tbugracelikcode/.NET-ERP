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
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityItemsAppService : ApplicationService<UnsuitabilityItemsResource>, IUnsuitabilityItemsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public UnsuitabilityItemsAppService(IStringLocalizer<UnsuitabilityItemsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateUnsuitabilityItemsValidator), Priority = 1)]
        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> CreateAsync(CreateUnsuitabilityItemsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<UnsuitabilityItems>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Insert(new CreateUnsuitabilityItemsDto
            {
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = GuidGenerator.CreateGuid(),
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
                Detectability = input.Detectability,
                ExtraCost = input.ExtraCost,
                IntensityCoefficient = input.IntensityCoefficient,
                IntensityRange = input.IntensityRange,
                LifeThreatening = input.LifeThreatening,
                LossOfPrestige = input.LossOfPrestige,
                ProductLifeShortening = input.ProductLifeShortening,
                ToBeUsedAs = input.ToBeUsedAs,
                UnsuitabilityTypesItemsId = input.UnsuitabilityTypesItemsId,
                StationGroupId = input.StationGroupId,
                isEmployeeProductivityAnalysis = input.isEmployeeProductivityAnalysis,
                isStationProductivityAnalysis = input.isStationProductivityAnalysis,
            });

            var unsuitabilityItem = queryFactory.Insert<SelectUnsuitabilityItemsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnsItemsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Insert, unsuitabilityItem.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsItemsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("UnsuitabilityItemID", new List<string>
            {
                Tables.PFMEAs,
                Tables.UnsuitabilityItemSPCLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsItemsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.UnsuitabilityItems).Select<UnsuitabilityItems>(null)
                        .Join<UnsuitabilityTypesItems>
                        (
                            b => new { UnsuitabilityTypesItemsName = b.Name, UnsuitabilityTypesItemsId = b.Id },
                            nameof(UnsuitabilityItems.UnsuitabilityTypesItemsId),
                            nameof(UnsuitabilityTypesItems.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { StationGroupName = sg.Name, StationGroupId = sg.Id },
                            nameof(UnsuitabilityItems.StationGroupId),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.UnsuitabilityItems);

            var unsuitabilityItem = queryFactory.Get<SelectUnsuitabilityItemsDto>(query);

            LogsAppService.InsertLogToDatabase(unsuitabilityItem, unsuitabilityItem, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);


        }

        public async Task<IDataResult<IList<ListUnsuitabilityItemsDto>>> GetListAsync(ListUnsuitabilityItemsParameterDto input)
        {
            var query = queryFactory
                    .Query().From(Tables.UnsuitabilityItems).Select<UnsuitabilityItems>(s => new { s.Code, s.Name, s.Description_, s.IntensityCoefficient, s.IntensityRange, s.Id })
                        .Join<UnsuitabilityTypesItems>
                        (
                            b => new { UnsuitabilityTypesItemsName = b.Name, UnsuitabilityTypesItemsId = b.Id },
                            nameof(UnsuitabilityItems.UnsuitabilityTypesItemsId),
                            nameof(UnsuitabilityTypesItems.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { StationGroupName = sg.Name, StationGroupId = sg.Id },
                            nameof(UnsuitabilityItems.StationGroupId),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(null, Tables.UnsuitabilityItems);

            var unsuitabilityItems = queryFactory.GetList<ListUnsuitabilityItemsDto>(query).ToList();


            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnsuitabilityItemsDto>>(unsuitabilityItems);

        }

        [ValidationAspect(typeof(UpdateUnsuitabilityItemsValidator), Priority = 1)]
        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> UpdateAsync(UpdateUnsuitabilityItemsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<UnsuitabilityItems>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<UnsuitabilityItems>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Update(new UpdateUnsuitabilityItemsDto
            {
                Code = input.Code,
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
                LastModifierId = LoginedUserService.UserId,
                Detectability = input.Detectability,
                ExtraCost = input.ExtraCost,
                IntensityCoefficient = input.IntensityCoefficient,
                IntensityRange = input.IntensityRange,
                LifeThreatening = input.LifeThreatening,
                LossOfPrestige = input.LossOfPrestige,
                ProductLifeShortening = input.ProductLifeShortening,
                ToBeUsedAs = input.ToBeUsedAs,
                UnsuitabilityTypesItemsId = input.UnsuitabilityTypesItemsId,
                StationGroupId = input.StationGroupId,
                isStationProductivityAnalysis = input.isStationProductivityAnalysis,
                isEmployeeProductivityAnalysis = input.isEmployeeProductivityAnalysis
            }).Where(new { Id = input.Id }, "");

            var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, unsuitabilityItem, LoginedUserService.UserId, Tables.UnsuitabilityItems, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsItemsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);

        }

        public async Task<IDataResult<SelectUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityItems).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<UnsuitabilityItems>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnsuitabilityItems).Update(new UpdateUnsuitabilityItemsDto
            {
                Code = entity.Code,
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
                DataOpenStatusUserId = userId,
                Detectability = entity.Detectability,
                ExtraCost = entity.ExtraCost,
                IntensityCoefficient = entity.IntensityCoefficient,
                IntensityRange = entity.IntensityRange,
                LifeThreatening = entity.LifeThreatening,
                LossOfPrestige = entity.LossOfPrestige,
                ProductLifeShortening = entity.ProductLifeShortening,
                ToBeUsedAs = entity.ToBeUsedAs,
                UnsuitabilityTypesItemsId = entity.UnsuitabilityTypesItemsId,
                StationGroupId = entity.StationGroupId,
                isEmployeeProductivityAnalysis = entity.isEmployeeProductivityAnalysis,
                isStationProductivityAnalysis = entity.isStationProductivityAnalysis,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var unsuitabilityItem = queryFactory.Update<SelectUnsuitabilityItemsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityItemsDto>(unsuitabilityItem);


        }
    }
}
