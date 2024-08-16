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
using TsiErp.Business.Entities.PlanningManagement.MRP.Services;
using TsiErp.Business.Entities.PlanningManagement.MRP.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MRPs.Page;

namespace TsiErp.Business.Entities.MRP.Services
{
    [ServiceRegistration(typeof(IMRPsAppService), DependencyInjectionType.Scoped)]
    public class MRPsAppService : ApplicationService<MRPsResource>, IMRPsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public MRPsAppService(IStringLocalizer<MRPsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        [ValidationAspect(typeof(CreateMRPsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPsDto>> CreateAsync(CreateMRPsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<MRPs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.MRPs).Insert(new CreateMRPsDto
            {
                Date_ = input.Date_,
                IsMaintenanceMRP = input.IsMaintenanceMRP,
                ReferanceDate = input.ReferanceDate,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                Description_ = input.Description_,
                State_ = input.State_,
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,

            });

            foreach (var item in input.SelectMRPLines)
            {
                var queryLine = queryFactory.Query().From(Tables.MRPLines).Insert(new CreateMRPLinesDto
                {
                    State_ = item.State_,
                    isCalculated = item.isCalculated,
                    isPurchase = item.isPurchase,
                    LineNr = item.LineNr,
                    PurchaseReservedAmount = item.PurchaseReservedAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    Amount = item.Amount,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    RequirementAmount = item.RequirementAmount,
                    CurrencyID = item.CurrencyID,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    ReservedAmount = item.ReservedAmount,
                    SupplyDate = item.SupplyDate,
                    UnitPrice = item.UnitPrice,
                    BranchID = item.BranchID.GetValueOrDefault(),
                    WarehouseID = item.WarehouseID.GetValueOrDefault(),
                    isStockUsage = item.isStockUsage,
                    SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                    SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    MRPID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var MRP = queryFactory.Insert<SelectMRPsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MRPChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MRPs, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MRPChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMRPsDto>(MRP);

        }


        public async Task<IDataResult<SelectMRPsDto>> ConvertMRPMaintenanceMRPAsync(CreateMRPsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MRPs).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<MRPs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.MRPs).Insert(new CreateMRPsDto
            {
                Date_ = input.Date_,
                IsMaintenanceMRP = input.IsMaintenanceMRP,
                ReferanceDate = input.ReferanceDate,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                Description_ = input.Description_,
                State_ = input.State_,
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,

            });

            foreach (var item in input.SelectMRPLines)
            {
                var queryLine = queryFactory.Query().From(Tables.MRPLines).Insert(new CreateMRPLinesDto
                {
                    State_ = item.State_,
                    isCalculated = item.isCalculated,
                    isPurchase = item.isPurchase,
                    LineNr = item.LineNr,
                    PurchaseReservedAmount = item.PurchaseReservedAmount,
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    Amount = item.Amount,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    RequirementAmount = item.RequirementAmount,
                    CurrencyID = item.CurrencyID,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    ReservedAmount = item.ReservedAmount,
                    SupplyDate = item.SupplyDate,
                    UnitPrice = item.UnitPrice,
                    BranchID = item.BranchID.GetValueOrDefault(),
                    WarehouseID = item.WarehouseID.GetValueOrDefault(),
                    isStockUsage = item.isStockUsage,
                    SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                    SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    MRPID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var MRP = queryFactory.Insert<SelectMRPsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MRPChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MRPs, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["MainMatReqPlanningChildMenu"], L["MaintenanceMRPsContextConvertMRP"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["MaintenanceMRPsContextConvertMRP"],
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
                            ContextMenuName_ = L["MaintenanceMRPsContextConvertMRP"],
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
            return new SuccessDataResult<SelectMRPsDto>(MRP);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("MRPID", new List<string>
            {
                Tables.PurchaseRequests
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Id = id }, "");

                var MRPs = queryFactory.Get<SelectMRPsDto>(query);

                if (MRPs.Id != Guid.Empty && MRPs != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.MRPs).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.MRPLines).Delete(LoginedUserService.UserId).Where(new { MRPID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var MRP = queryFactory.Update<SelectMRPsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPs, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MRPChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectMRPsDto>(MRP);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.MRPLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
                    var MRPLines = queryFactory.Update<SelectMRPLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectMRPLinesDto>(MRPLines);
                }
            }
        }

        public async Task<IDataResult<SelectMRPsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.MRPs).Select<MRPs>(null)
                .Join<MaintenanceMRPs>
                        (
                            pr => new { MaintenanceMRPCode = pr.Code, MaintenanceMRPID = pr.Id },
                            nameof(MRPs.MaintenanceMRPID),
                            nameof(MaintenanceMRPs.Id),
                            JoinType.Left
                        )
                .Where(new { Id = id }, Tables.MRPs);
            var MRP = queryFactory.Get<SelectMRPsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MRPLines)
                   .Select<MRPLines, GrandTotalStockMovements>
              (
                  null
                , t => t.Amount
                , Tables.GrandTotalStockMovements
                , true
                , nameof(GrandTotalStockMovements.ProductID) + "=" + Tables.MRPLines + "." + nameof(MRPLines.ProductID))
                   .Join<Products>
                    (
                        s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                        nameof(MRPLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        s => new { WarehouseID = s.Id, WarehouseCode = s.Code },
                        nameof(MRPLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        s => new { CurrentAccountCardID = s.Id, CurrentAccountCardCode = s.Code, CurrentAccountCardName = s.Name },
                        nameof(MRPLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Branches>
                    (
                        s => new { BranchID = s.Id, BranchCode = s.Code },
                        nameof(MRPLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        s => new { CurrencyID = s.Id, CurrencyCode = s.Code },
                        nameof(MRPLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        sh => new { UnitSetID = sh.Id, UnitSetCode = sh.Code },
                        nameof(MRPLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        sh => new { SalesOrderID = sh.Id, SalesOrderFicheNo = sh.FicheNo },
                        nameof(MRPLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                      .Join<SalesOrderLines>
                    (
                        sh => new { SalesOrderLineID = sh.Id },
                        nameof(MRPLines.SalesOrderLineID),
                        nameof(SalesOrderLines.Id),
                        JoinType.Left
                    )
                    .Where(new { MRPID = id }, Tables.MRPLines);

            var MRPLine = queryFactory.GetList<SelectMRPLinesDto>(queryLines).ToList();

            MRP.SelectMRPLines = MRPLine;

            LogsAppService.InsertLogToDatabase(MRP, MRP, LoginedUserService.UserId, Tables.MRPs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPsDto>(MRP);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMRPsDto>>> GetListAsync(ListMRPsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.MRPs).Select<MRPs>(s => new { s.Code, s.Date_, s.State_, s.Description_, s.Id })
                .Join<MaintenanceMRPs>
                        (
                            pr => new { MaintenanceMRPCode = pr.Code, MaintenanceMRPID = pr.Id},
                            nameof(MRPs.MaintenanceMRPID),
                            nameof(MaintenanceMRPs.Id),
                            JoinType.Left
                        ).Where(null, Tables.MRPs);
            var mRPs = queryFactory.GetList<ListMRPsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMRPsDto>>(mRPs);
        }

        [ValidationAspect(typeof(UpdateMRPsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPsDto>> UpdateAsync(UpdateMRPsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.MRPs).Select<MRPs>(null)
                .Join<MaintenanceMRPs>
                        (
                            pr => new { MaintenanceMRPCode = pr.Code, MaintenanceMRPID = pr.Id },
                            nameof(MRPs.MaintenanceMRPID),
                            nameof(MaintenanceMRPs.Id),
                            JoinType.Left
                        ).Where(
          new
          {
              Id = input.Id
          }, Tables.MRPs);
            var entity = queryFactory.Get<SelectMRPsDto>(entityQuery);

            var queryLines = queryFactory
                 .Query()
                 .From(Tables.MRPLines)
                 .Select<MRPLines>(null)
                 .Join<Products>
                  (
                      s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                      nameof(MRPLines.ProductID),
                      nameof(Products.Id),
                      JoinType.Left
                  )
                    .Join<Warehouses>
                    (
                        s => new { WarehouseID = s.Id, WarehouseCode = s.Code },
                        nameof(MRPLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                     .Join<Branches>
                    (
                        s => new { BranchID = s.Id, BranchCode = s.Code },
                        nameof(MRPLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                 .Join<UnitSets>
                  (
                      sh => new { UnitSetID = sh.Id, UnitSetCode = sh.Code },
                      nameof(MRPLines.UnitSetID),
                      nameof(UnitSets.Id),
                      JoinType.Left
                  )
                   .Join<SalesOrders>
                  (
                      sh => new { SalesOrderID = sh.Id, SalesOrderFicheNo = sh.FicheNo },
                      nameof(MRPLines.SalesOrderID),
                      nameof(SalesOrders.Id),
                      JoinType.Left
                  )
                    .Join<SalesOrderLines>
                  (
                      sh => new { SalesOrderLineID = sh.Id },
                      nameof(MRPLines.SalesOrderLineID),
                      nameof(SalesOrderLines.Id),
                      JoinType.Left
                  )
                  .Where(new { MRPID = input.Id }, Tables.MRPLines);

            var MRPLine = queryFactory.GetList<SelectMRPLinesDto>(queryLines).ToList();

            entity.SelectMRPLines = MRPLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.MRPs).Select<MRPs>(null)
                .Join<MaintenanceMRPs>
                        (
                            pr => new { MaintenanceMRPCode = pr.Code, MaintenanceMRPID = pr.Id },
                            nameof(MRPs.MaintenanceMRPID),
                            nameof(MaintenanceMRPs.Id),
                            JoinType.Left
                        ).Where(new { Code = input.Code }, Tables.MRPs);

            var list = queryFactory.GetList<ListMRPsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.MRPs).Update(new UpdateMRPsDto
            {
                Date_ = input.Date_,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                IsMaintenanceMRP = input.IsMaintenanceMRP,
                ReferanceDate = input.ReferanceDate,
                Description_ = input.Description_,
                State_ = input.State_,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectMRPLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.MRPLines).Insert(new CreateMRPLinesDto
                    {
                        State_ = item.State_,
                        LineNr = item.LineNr,
                        isCalculated = item.isCalculated,
                        isPurchase = item.isPurchase,
                        Amount = item.Amount,
                        WarehouseID = item.WarehouseID.GetValueOrDefault(),
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        BranchID = item.BranchID.GetValueOrDefault(),
                        isStockUsage = item.isStockUsage,
                        CurrencyID = item.CurrencyID.GetValueOrDefault(),
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        ReservedAmount = item.ReservedAmount,
                        SupplyDate = item.SupplyDate,
                        PurchaseReservedAmount = item.PurchaseReservedAmount,
                        UnitPrice = item.UnitPrice,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        RequirementAmount = item.RequirementAmount,
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        MRPID = input.Id,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.MRPLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectMRPLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MRPLines).Update(new UpdateMRPLinesDto
                        {
                            State_ = item.State_,
                            LineNr = item.LineNr,
                            Amount = item.Amount,
                            isStockUsage = item.isStockUsage,
                            isCalculated = item.isCalculated,
                            OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                            OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                            isPurchase = item.isPurchase,
                            BranchID = item.BranchID.GetValueOrDefault(),
                            WarehouseID = item.WarehouseID.GetValueOrDefault(),
                            CurrencyID = item.CurrencyID.GetValueOrDefault(),
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            ReservedAmount = item.ReservedAmount,
                            SupplyDate = item.SupplyDate,
                            PurchaseReservedAmount = item.PurchaseReservedAmount,
                            UnitPrice = item.UnitPrice,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            RequirementAmount = item.RequirementAmount,
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            SalesOrderLineID = item.SalesOrderLineID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            MRPID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var MRP = queryFactory.Update<SelectMRPsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MRPs, LogType.Update, input.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["MRPChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectMRPsDto>(MRP);

        }

        public async Task<IDataResult<SelectMRPsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.MRPs).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<MRPs>(entityQuery);

            var query = queryFactory.Query().From(Tables.MRPs).Update(new UpdateMRPsDto
            {
                State_ = entity.State_,
                IsMaintenanceMRP = entity.IsMaintenanceMRP,
                MaintenanceMRPID = entity.MaintenanceMRPID,
                ReferanceDate = entity.ReferanceDate,
                Date_ = entity.Date_,
                OrderAcceptanceID = entity.OrderAcceptanceID.GetValueOrDefault(),
                Description_ = entity.Description_,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var MRPsDto = queryFactory.Update<SelectMRPsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPsDto>(MRPsDto);


        }
    }
}
