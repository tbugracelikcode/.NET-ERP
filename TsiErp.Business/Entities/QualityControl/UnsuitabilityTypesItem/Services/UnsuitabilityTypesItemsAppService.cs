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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityTypesItem.Page;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityTypesItemsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityTypesItemsAppService : ApplicationService<UnsuitabilityTypesItemResources>, IUnsuitabilityTypesItemsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public UnsuitabilityTypesItemsAppService(IStringLocalizer<UnsuitabilityTypesItemResources> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreateUnsuitabilityTypesItemsValidator), Priority = 1)]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> CreateAsync(CreateUnsuitabilityTypesItemsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<UnsuitabilityTypesItems>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Insert(new CreateUnsuitabilityTypesItemsDto
            {
                Code = input.Code,
                Description_ = input.Description_,
                Name = input.Name,
                Id = GuidGenerator.CreateGuid(),
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                UnsuitabilityTypesDescription = input.UnsuitabilityTypesDescription
            });


            var unsuitabilityTypesItems = queryFactory.Insert<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnsTypesItemsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Insert, unsuitabilityTypesItems.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsTypesItemsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        [ValidationAspect(typeof(UpdateUnsuitabilityTypesItemsValidator), Priority = 1)]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateAsync(UpdateUnsuitabilityTypesItemsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = input.Id },"");
            var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<UnsuitabilityTypesItems>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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
                UnsuitabilityTypesDescription = input.UnsuitabilityTypesDescription
            }).Where(new { Id = input.Id }, "");

            var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsTypesItemsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("UnsuitabilityTypeID", new List<string>
            {
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
                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsTypesItemsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var unsuitabilityTypesItems = queryFactory.Get<SelectUnsuitabilityTypesItemsDto>(query);


            LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        public async Task<IDataResult<IList<ListUnsuitabilityTypesItemsDto>>> GetListAsync(ListUnsuitabilityTypesItemsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select<UnsuitabilityTypesItems>(s => new { s.Code, s.Name, s.Description_, s.Id }).Where(null, "");
            var unsuitabilityTypesItems = queryFactory.GetList<ListUnsuitabilityTypesItemsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnsuitabilityTypesItemsDto>>(unsuitabilityTypesItems);

        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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
                UnsuitabilityTypesDescription = entity.UnsuitabilityTypesDescription
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> GetWithUnsuitabilityItemDescriptionAsync(string description)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
            new
            {
                UnsuitabilityTypesDescription = description
            },  "");
            var unsuitabilityTypesItems = queryFactory.Get<SelectUnsuitabilityTypesItemsDto>(query);


            LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, unsuitabilityTypesItems.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

        }
    }
}
