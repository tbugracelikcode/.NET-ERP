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
using TsiErp.Business.Entities.StockManagement.StockColumn.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockColumn;
using TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockShelf.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockColumns.Page;

namespace TsiErp.Business.Entities.StockColumn.Services
{
    [ServiceRegistration(typeof(IStockColumnsAppService), DependencyInjectionType.Scoped)]
    public class StockColumnsAppService : ApplicationService<StockColumnsResource>, IStockColumnsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public StockColumnsAppService(IStringLocalizer<StockColumnsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateStockColumnsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockColumnsDto>> CreateAsync(CreateStockColumnsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StockColumns).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<StockColumns>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StockColumns).Insert(new CreateStockColumnsDto
            {
                Code = input.Code,
                Name = input.Name,
                Description_ = input.Description_,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var StockColumns = queryFactory.Insert<SelectStockColumnsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StockColumnsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockColumns, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockColumnsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectStockColumnsDto>(StockColumns);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("StockColumnID", new List<string>
            {
                
                Tables.StockAddressLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.StockColumns).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var StockColumns = queryFactory.Update<SelectStockColumnsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockColumns, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockColumnsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectStockColumnsDto>(StockColumns);
            }

        }

        public async Task<IDataResult<SelectStockColumnsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StockColumns).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var StockColumn = queryFactory.Get<SelectStockColumnsDto>(query);


            LogsAppService.InsertLogToDatabase(StockColumn, StockColumn, LoginedUserService.UserId, Tables.StockColumns, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockColumnsDto>(StockColumn);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockColumnsDto>>> GetListAsync(ListStockColumnsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockColumns).Select<StockColumns>(s => new { s.Code, s.Name }).Where(null, "");
            var StockColumns = queryFactory.GetList<ListStockColumnsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStockColumnsDto>>(StockColumns);

        }


        [ValidationAspect(typeof(UpdateStockColumnsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockColumnsDto>> UpdateAsync(UpdateStockColumnsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockColumns).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<StockColumns>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StockColumns).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<StockColumns>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StockColumns).Update(new UpdateStockColumnsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                Description_ = input.Description_,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },  "");

            var StockColumns = queryFactory.Update<SelectStockColumnsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StockColumns, LoginedUserService.UserId, Tables.StockColumns, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["StockColumnsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectStockColumnsDto>(StockColumns);

        }

        public async Task<IDataResult<SelectStockColumnsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockColumns).Select("Id").Where(new { Id = id },  "");

            var entity = queryFactory.Get<StockColumns>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockColumns).Update(new UpdateStockColumnsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Description_ = entity.Description_,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var StockColumns = queryFactory.Update<SelectStockColumnsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockColumnsDto>(StockColumns);

        }
    }
}
