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
using TsiErp.Business.Entities.Warehouse.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Warehouses.Page;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService<WarehousesResource>, IWarehousesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public WarehousesAppService(IStringLocalizer<WarehousesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<Warehouses>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Warehouses).Insert(new CreateWarehousesDto
            {
                Code = input.Code,
                Name = input.Name,
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


            var warehouses = queryFactory.Insert<SelectWarehousesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("WarehousesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Warehouses, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WarehousesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectWarehousesDto>(warehouses);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("WarehouseID", new List<string>
            {
                Tables.ByDateStockMovements,
                Tables.GrandTotalStockMovements,
                Tables.MRPLines,
                Tables.ProductionOrders,
                Tables.PurchaseOrders,
                Tables.PurchasePrices,
                Tables.PurchaseRequests,
                Tables.SalesOrders,
                Tables.SalesPrices,
                Tables.SalesPropositions,
                Tables.StockFiches
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.Warehouses).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Warehouses, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WarehousesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectWarehousesDto>(warehouses);
            }
        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var warehouse = queryFactory.Get<SelectWarehousesDto>(query);


            LogsAppService.InsertLogToDatabase(warehouse, warehouse, LoginedUserService.UserId, Tables.Warehouses, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectWarehousesDto>(warehouse);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Warehouses).Select<Warehouses>(s => new { s.Code, s.Name, s.Id }).Where(null, "");
            var warehouses = queryFactory.GetList<ListWarehousesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListWarehousesDto>>(warehouses);

        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<Warehouses>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<Warehouses>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Warehouses).Update(new UpdateWarehousesDto
            {
                Code = input.Code,
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

            var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, warehouses, LoginedUserService.UserId, Tables.Warehouses, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WarehousesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectWarehousesDto>(warehouses);

        }

        public async Task<IDataResult<SelectWarehousesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Warehouses).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Warehouses>(entityQuery);

            var query = queryFactory.Query().From(Tables.Warehouses).Update(new UpdateWarehousesDto
            {
                Code = entity.Code,
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

            var warehouses = queryFactory.Update<SelectWarehousesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectWarehousesDto>(warehouses);

        }
    }
}
