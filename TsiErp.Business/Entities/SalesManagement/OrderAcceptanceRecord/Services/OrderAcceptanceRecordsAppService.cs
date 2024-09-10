using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Entities;
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
using TsiErp.Business.Entities.SalesManagement.OrderAcceptanceRecord.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OrderAcceptanceRecords.Page;

namespace TsiErp.Business.Entities.OrderAcceptanceRecord.Services
{
    [ServiceRegistration(typeof(IOrderAcceptanceRecordsAppService), DependencyInjectionType.Scoped)]
    public class OrderAcceptanceRecordsAppService : ApplicationService<OrderAcceptanceRecordsResource>, IOrderAcceptanceRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public OrderAcceptanceRecordsAppService(IStringLocalizer<OrderAcceptanceRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateOrderAcceptanceRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> CreateAsync(CreateOrderAcceptanceRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Select("Code").Where(new { Code = input.Code },  "");
            var list = queryFactory.ControlList<OrderAcceptanceRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Insert(new CreateOrderAcceptanceRecordsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Description_ = input.Description_,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime =now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                 PaymentPlanID = input.PaymentPlanID
                 
            });

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                {
                    OrderReferanceNo = item.OrderReferanceNo,
                    CustomerBarcodeNo = item.CustomerBarcodeNo,
                    CustomerReferanceNo = item.CustomerReferanceNo,
                    DefinedUnitPrice = item.DefinedUnitPrice,
                    Description_ = item.Description_,
                    PurchaseSupplyDate = item.PurchaseSupplyDate,
                    LineAmount = item.LineAmount,
                    MinOrderAmount = item.MinOrderAmount,
                    OrderAmount = item.OrderAmount,
                    OrderUnitPrice = item.OrderUnitPrice,
                    ProductReferanceNumberID = item.ProductReferanceNumberID,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    OrderAcceptanceRecordID = addedEntityId,
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
                    LineNr = item.LineNr,
                    ProductID = item.ProductID.GetValueOrDefault(),
                     ProductCode = item.ProductCode,
                    PaymentPlanID = item.PaymentPlanID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var OrderAcceptanceRecord = queryFactory.Insert<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("OrderAcceptanceRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OrderAcceptanceRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Select("*").Where(new { Id = id },  "");

            var OrderAcceptanceRecords = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(query);

            if (OrderAcceptanceRecords.Id != Guid.Empty && OrderAcceptanceRecords != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Delete(LoginedUserService.UserId).Where(new { OrderAcceptanceRecordID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OrderAcceptanceRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
                var OrderAcceptanceRecordLines = queryFactory.Update<SelectOrderAcceptanceRecordLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OrderAcceptanceRecordLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectOrderAcceptanceRecordLinesDto>(OrderAcceptanceRecordLines);
            }

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(OrderAcceptanceRecords.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.OrderAcceptanceRecords);

            var orderAcceptanceRecords = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(OrderAcceptanceRecordLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name, VATrate=pr.SaleVAT },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Join<UnitSets>
                    (
                        pr => new { UnitSetCode=pr.Code },
                        nameof(OrderAcceptanceRecordLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = id },  Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            orderAcceptanceRecords.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            LogsAppService.InsertLogToDatabase(orderAcceptanceRecords, orderAcceptanceRecords, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(orderAcceptanceRecords);

        }

        public async Task<IDataResult<IList<ListOrderAcceptanceRecordsDto>>> GetListAsync(ListOrderAcceptanceRecordsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(s => new { s.Code, s.Date_, s.CustomerOrderNo, s.CustomerRequestedDate, s.ConfirmedLoadingDate, s.ProductionOrderLoadingDate, s.OrderAcceptanceRecordState, s.Id })
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id ,PaymentPlanName = pp.Name },
                        nameof(OrderAcceptanceRecords.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.OrderAcceptanceRecords);

            var orderAcceptanceRecords = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOrderAcceptanceRecordsDto>>(orderAcceptanceRecords);

        }

        [ValidationAspect(typeof(UpdateOrderAcceptanceRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateAsync(UpdateOrderAcceptanceRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(OrderAcceptanceRecords.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id },  Tables.OrderAcceptanceRecords);

            var entity = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                     .Join<PaymentPlans>
                    (
                        pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                        nameof(OrderAcceptanceRecordLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = input.Id },  Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            entity.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OrderAcceptanceRecords)
                           .Select<OrderAcceptanceRecords>(null)
                           .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                             .Where(new { Code = input.Code },  Tables.OrderAcceptanceRecords);

            var list = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                Description_ = input.Description_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                PaymentPlanID = input.PaymentPlanID
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                    {
                        OrderReferanceNo = item.OrderReferanceNo,
                        CustomerBarcodeNo = item.CustomerBarcodeNo,
                        CustomerReferanceNo = item.CustomerReferanceNo,
                        DefinedUnitPrice = item.DefinedUnitPrice,
                        Description_ = item.Description_,
                        LineAmount = item.LineAmount,
                        MinOrderAmount = item.MinOrderAmount,
                        OrderAmount = item.OrderAmount,
                        OrderUnitPrice = item.OrderUnitPrice,
                        ProductReferanceNumberID = item.ProductReferanceNumberID,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        OrderAcceptanceRecordID = input.Id,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
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
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                         ProductCode = item.ProductCode,
                        PaymentPlanID = item.PaymentPlanID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
                        {
                            OrderReferanceNo = item.OrderReferanceNo,
                            CustomerBarcodeNo = item.CustomerBarcodeNo,
                            CustomerReferanceNo = item.CustomerReferanceNo,
                            DefinedUnitPrice = item.DefinedUnitPrice,
                            Description_ = item.Description_,
                            LineAmount = item.LineAmount,
                            MinOrderAmount = item.MinOrderAmount,
                            OrderAmount = item.OrderAmount,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            OrderUnitPrice = item.OrderUnitPrice,
                            ProductReferanceNumberID = item.ProductReferanceNumberID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime =now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                             ProductCode    = item.ProductCode,
                            PaymentPlanID = item.PaymentPlanID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OrderAcceptanceRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateTechApprovalAsync(UpdateOrderAcceptanceRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.OrderAcceptanceRecords);

            var entity = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = input.Id }, Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            entity.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OrderAcceptanceRecords)
                           .Select<OrderAcceptanceRecords>(null)
                           .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                             .Where(new { Code = input.Code }, Tables.OrderAcceptanceRecords);

            var list = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                Description_ = input.Description_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                 PaymentPlanID = entity.PaymentPlanID
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                    {
                        OrderReferanceNo = item.OrderReferanceNo,
                        CustomerBarcodeNo = item.CustomerBarcodeNo,
                        CustomerReferanceNo = item.CustomerReferanceNo,
                        DefinedUnitPrice = item.DefinedUnitPrice,
                        Description_ = item.Description_,
                        LineAmount = item.LineAmount,
                        MinOrderAmount = item.MinOrderAmount,
                        OrderAmount = item.OrderAmount,
                        OrderUnitPrice = item.OrderUnitPrice,
                        ProductReferanceNumberID = item.ProductReferanceNumberID,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        OrderAcceptanceRecordID = input.Id,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
                        CreationTime =now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                         ProductCode    = item.ProductCode,
                          PaymentPlanID = item.PaymentPlanID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
                        {
                            OrderReferanceNo = item.OrderReferanceNo,
                            CustomerBarcodeNo = item.CustomerBarcodeNo,
                            CustomerReferanceNo = item.CustomerReferanceNo,
                            DefinedUnitPrice = item.DefinedUnitPrice,
                            Description_ = item.Description_,
                            LineAmount = item.LineAmount,
                            MinOrderAmount = item.MinOrderAmount,
                            OrderAmount = item.OrderAmount,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            OrderUnitPrice = item.OrderUnitPrice,
                            ProductReferanceNumberID = item.ProductReferanceNumberID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = input.Id,
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
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                             ProductCode = item.ProductCode,
                              PaymentPlanID = item.PaymentPlanID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["OrderAcceptanceRecordsChildMenu"],  L["OrderAcceptanceRecordsContextTechnicalApproval"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["OrderAcceptanceRecordsContextTechnicalApproval"],
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
                            ContextMenuName_ = L["OrderAcceptanceRecordsContextTechnicalApproval"],
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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateOrderApprovalAsync(UpdateOrderAcceptanceRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.OrderAcceptanceRecords);

            var entity = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = input.Id }, Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            entity.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OrderAcceptanceRecords)
                           .Select<OrderAcceptanceRecords>(null)
                           .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                             .Where(new { Code = input.Code }, Tables.OrderAcceptanceRecords);

            var list = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                Description_ = input.Description_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                 PaymentPlanID = entity.PaymentPlanID
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                    {
                        OrderReferanceNo = item.OrderReferanceNo,
                        CustomerBarcodeNo = item.CustomerBarcodeNo,
                        CustomerReferanceNo = item.CustomerReferanceNo,
                        DefinedUnitPrice = item.DefinedUnitPrice,
                        Description_ = item.Description_,
                        LineAmount = item.LineAmount,
                        MinOrderAmount = item.MinOrderAmount,
                        OrderAmount = item.OrderAmount,
                        OrderUnitPrice = item.OrderUnitPrice,
                        ProductReferanceNumberID = item.ProductReferanceNumberID,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        OrderAcceptanceRecordID = input.Id,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
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
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                         ProductCode    = item.ProductCode,
                        PaymentPlanID = item.PaymentPlanID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
                        {
                            OrderReferanceNo = item.OrderReferanceNo,
                            CustomerBarcodeNo = item.CustomerBarcodeNo,
                            CustomerReferanceNo = item.CustomerReferanceNo,
                            DefinedUnitPrice = item.DefinedUnitPrice,
                            Description_ = item.Description_,
                            LineAmount = item.LineAmount,
                            MinOrderAmount = item.MinOrderAmount,
                            OrderAmount = item.OrderAmount,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            OrderUnitPrice = item.OrderUnitPrice,
                            ProductReferanceNumberID = item.ProductReferanceNumberID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = input.Id,
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
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                             ProductCode    = item.ProductCode,
                            PaymentPlanID = item.PaymentPlanID
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["OrderAcceptanceRecordsChildMenu"],  L["OrderAcceptanceRecordsContextOrderApproval"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["OrderAcceptanceRecordsContextOrderApproval"],
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
                            ContextMenuName_ = L["OrderAcceptanceRecordsContextOrderApproval"],
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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdatePendingAsync(UpdateOrderAcceptanceRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.OrderAcceptanceRecords);

            var entity = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = input.Id }, Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            entity.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OrderAcceptanceRecords)
                           .Select<OrderAcceptanceRecords>(null)
                           .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                             .Where(new { Code = input.Code }, Tables.OrderAcceptanceRecords);

            var list = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                Description_ = input.Description_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
                 PaymentPlanID = entity.PaymentPlanID
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                    {
                        OrderReferanceNo = item.OrderReferanceNo,
                        CustomerBarcodeNo = item.CustomerBarcodeNo,
                        CustomerReferanceNo = item.CustomerReferanceNo,
                        DefinedUnitPrice = item.DefinedUnitPrice,
                        Description_ = item.Description_,
                        LineAmount = item.LineAmount,
                        MinOrderAmount = item.MinOrderAmount,
                        OrderAmount = item.OrderAmount,
                        OrderUnitPrice = item.OrderUnitPrice,
                        ProductReferanceNumberID = item.ProductReferanceNumberID,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        OrderAcceptanceRecordID = input.Id,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
                        CreationTime =now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                         ProductCode = item.ProductCode,
                        PaymentPlanID = item.PaymentPlanID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
                        {
                            OrderReferanceNo = item.OrderReferanceNo,
                            CustomerBarcodeNo = item.CustomerBarcodeNo,
                            CustomerReferanceNo = item.CustomerReferanceNo,
                            DefinedUnitPrice = item.DefinedUnitPrice,
                            Description_ = item.Description_,
                            LineAmount = item.LineAmount,
                            MinOrderAmount = item.MinOrderAmount,
                            OrderAmount = item.OrderAmount,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            OrderUnitPrice = item.OrderUnitPrice,
                            ProductReferanceNumberID = item.ProductReferanceNumberID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = input.Id,
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
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                             ProductCode = item.ProductCode,
                            PaymentPlanID = item.PaymentPlanID
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["OrderAcceptanceRecordsChildMenu"],  L["OrderAcceptanceRecordsContextPending"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["OrderAcceptanceRecordsContextPending"],
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
                            ContextMenuName_ = L["OrderAcceptanceRecordsContextPending"],
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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateAcceptanceOrderAsync(UpdateOrderAcceptanceRecordsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecords)
                   .Select<OrderAcceptanceRecords>(null)
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.OrderAcceptanceRecords);

            var entity = queryFactory.Get<SelectOrderAcceptanceRecordsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                   .Select<OrderAcceptanceRecordLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { OrderAcceptanceRecordID = input.Id }, Tables.OrderAcceptanceRecordLines);

            var OrderAcceptanceRecordLine = queryFactory.GetList<SelectOrderAcceptanceRecordLinesDto>(queryLines).ToList();

            entity.SelectOrderAcceptanceRecordLines = OrderAcceptanceRecordLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OrderAcceptanceRecords)
                           .Select<OrderAcceptanceRecords>(null)
                           .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardCustomerCode = ca.CustomerCode },
                        nameof(OrderAcceptanceRecords.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<Currencies>
                    (
                        ca => new { CurrenyID = ca.Id, CurrenyCode = ca.Code },
                        nameof(OrderAcceptanceRecords.CurrenyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                             .Where(new { Code = input.Code }, Tables.OrderAcceptanceRecords);

            var list = queryFactory.GetList<ListOrderAcceptanceRecordsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ConfirmedLoadingDate = input.ConfirmedLoadingDate,
                CurrenyID = input.CurrenyID.GetValueOrDefault(),
                CustomerOrderNo = input.CustomerOrderNo,
                Description_ = input.Description_,
                CustomerRequestedDate = input.CustomerRequestedDate,
                Date_ = input.Date_,
                ExchangeRateAmount = input.ExchangeRateAmount,
                OrderAcceptanceRecordState = input.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = input.ProductionOrderLoadingDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
                 PaymentPlanID = entity.PaymentPlanID
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOrderAcceptanceRecordLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Insert(new CreateOrderAcceptanceRecordLinesDto
                    {
                        OrderReferanceNo = item.OrderReferanceNo,
                        CustomerBarcodeNo = item.CustomerBarcodeNo,
                        CustomerReferanceNo = item.CustomerReferanceNo,
                        DefinedUnitPrice = item.DefinedUnitPrice,
                        Description_ = item.Description_,
                        LineAmount = item.LineAmount,
                        MinOrderAmount = item.MinOrderAmount,
                        OrderAmount = item.OrderAmount,
                        OrderUnitPrice = item.OrderUnitPrice,
                        ProductReferanceNumberID = item.ProductReferanceNumberID,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        OrderAcceptanceRecordID = input.Id,
                        PurchaseSupplyDate = item.PurchaseSupplyDate,
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
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                         ProductCode    = item.ProductCode,
                        PaymentPlanID = item.PaymentPlanID
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
                        {
                            OrderReferanceNo = item.OrderReferanceNo,
                            CustomerBarcodeNo = item.CustomerBarcodeNo,
                            CustomerReferanceNo = item.CustomerReferanceNo,
                            DefinedUnitPrice = item.DefinedUnitPrice,
                            Description_ = item.Description_,
                            LineAmount = item.LineAmount,
                            MinOrderAmount = item.MinOrderAmount,
                            OrderAmount = item.OrderAmount,
                            PurchaseSupplyDate = item.PurchaseSupplyDate,
                            OrderUnitPrice = item.OrderUnitPrice,
                            ProductReferanceNumberID = item.ProductReferanceNumberID.GetValueOrDefault(),
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            OrderAcceptanceRecordID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime =now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                             ProductCode = item.ProductCode,
                            PaymentPlanID = item.PaymentPlanID
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OrderAcceptanceRecord = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OrderAcceptanceRecords, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["OrderAcceptanceRecordsChildMenu"],  L["OrderAcceptanceRecordsContextConverttoOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["OrderAcceptanceRecordsContextConverttoOrder"],
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
                            ContextMenuName_ = L["OrderAcceptanceRecordsContextConverttoOrder"],
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
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecord);

        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<OrderAcceptanceRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecords).Update(new UpdateOrderAcceptanceRecordsDto
            {

                CurrentAccountCardID = entity.CurrentAccountCardID,
                ConfirmedLoadingDate = entity.ConfirmedLoadingDate,
                CurrenyID = entity.CurrenyID,
                CustomerOrderNo = entity.CustomerOrderNo,
                Description_ = entity.Description_,
                CustomerRequestedDate = entity.CustomerRequestedDate,
                Date_ = entity.Date_,
                ExchangeRateAmount = entity.ExchangeRateAmount,
                OrderAcceptanceRecordState = (int)entity.OrderAcceptanceRecordState,
                ProductionOrderLoadingDate = entity.ProductionOrderLoadingDate,
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
                PaymentPlanID = entity.PaymentPlanID,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var OrderAcceptanceRecordsDto = queryFactory.Update<SelectOrderAcceptanceRecordsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectOrderAcceptanceRecordsDto>(OrderAcceptanceRecordsDto);


        }

        public async Task<IDataResult<SelectOrderAcceptanceRecordLinesDto>> UpdateLineAsync(Guid lineID, DateTime supplyDate)
        {
            var entityLineQuery = queryFactory
                   .Query()
                   .From(Tables.OrderAcceptanceRecordLines)
                     .Select<OrderAcceptanceRecordLines>(null)
                  .Join<Products>
                    (
                        pr => new { ProductID = pr.Id, ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(OrderAcceptanceRecordLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductReferanceNumbers>
                    (
                        pr => new { ProductReferanceNumberID = pr.Id, OrderReferanceNo = pr.OrderReferanceNo, CustomerReferanceNo = pr.CustomerReferanceNo, CustomerBarcodeNo = pr.CustomerBarcodeNo, MinOrderAmount = pr.MinOrderAmount },
                        nameof(OrderAcceptanceRecordLines.ProductReferanceNumberID),
                        nameof(ProductReferanceNumbers.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = lineID }, Tables.OrderAcceptanceRecordLines);

            var entityLine = queryFactory.Get<SelectOrderAcceptanceRecordLinesDto>(entityLineQuery);

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OrderAcceptanceRecordLines).Update(new UpdateOrderAcceptanceRecordLinesDto
            {
                OrderReferanceNo = entityLine.OrderReferanceNo,
                CustomerBarcodeNo = entityLine.CustomerBarcodeNo,
                CustomerReferanceNo = entityLine.CustomerReferanceNo,
                DefinedUnitPrice = entityLine.DefinedUnitPrice,
                Description_ = entityLine.Description_,
                LineAmount = entityLine.LineAmount,
                MinOrderAmount = entityLine.MinOrderAmount,
                OrderAmount = entityLine.OrderAmount,
                PurchaseSupplyDate = supplyDate,
                OrderUnitPrice = entityLine.OrderUnitPrice,
                ProductReferanceNumberID = entityLine.ProductReferanceNumberID,
                UnitSetID = entityLine.UnitSetID,
                OrderAcceptanceRecordID = entityLine.OrderAcceptanceRecordID,
                CreationTime = entityLine.CreationTime,
                CreatorId = entityLine.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entityLine.DeleterId.GetValueOrDefault(),
                DeletionTime = entityLine.DeletionTime.GetValueOrDefault(),
                Id = entityLine.Id,
                IsDeleted = entityLine.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                LineNr = entityLine.LineNr,
                ProductID = entityLine.ProductID,
                 ProductCode    = entityLine.ProductCode,
                PaymentPlanID = entityLine.PaymentPlanID,
            }).Where(new { Id = lineID },  "");

            var OrderAcceptanceRecordLine = queryFactory.Update<SelectOrderAcceptanceRecordLinesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOrderAcceptanceRecordLinesDto>(OrderAcceptanceRecordLine);
        }
    }
}
