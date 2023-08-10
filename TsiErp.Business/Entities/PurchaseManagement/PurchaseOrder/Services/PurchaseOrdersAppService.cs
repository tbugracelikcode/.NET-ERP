using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchaseOrder.Validations;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseOrders.Page;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    [ServiceRegistration(typeof(IPurchaseOrdersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseOrdersAppService : ApplicationService<PurchaseOrdersResource>, IPurchaseOrdersAppService
    {
        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;

        QueryFactory queryFactory { get; set; } = new QueryFactory();


        public PurchaseOrdersAppService(IStringLocalizer<PurchaseOrdersResource> l, IPurchaseRequestsAppService PurchaseRequestsAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> CreateAsync(CreatePurchaseOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                string now = DateTime.Now.ToString();

                string[] timeSplit = now.Split(" ");

                string time = timeSplit[1];

                var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
                {
                    FicheNo = input.FicheNo,
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    GrossAmount = input.GrossAmount,
                    LinkedPurchaseRequestID = Guid.Empty,
                    NetAmount = input.NetAmount,
                    PaymentPlanID = input.PaymentPlanID,
                    ProductionOrderID = Guid.Empty,
                    PurchaseOrderState = input.PurchaseOrderState,
                    ShippingAdressID = Guid.Empty,
                    SpecialCode = input.SpecialCode,
                    Time_ = time,
                    TotalDiscountAmount = input.TotalDiscountAmount,
                    TotalVatAmount = input.TotalVatAmount,
                    TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                    WarehouseID = input.WarehouseID,
                    WorkOrderCreationDate = input.WorkOrderCreationDate,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        WorkOrderCreationDate = item.WorkOrderCreationDate,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPurchaseRequestLineID = Guid.Empty,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineTotalAmount = item.LineTotalAmount,
                        LinkedPurchaseRequestID = Guid.Empty,
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = Guid.Empty,
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = addedEntityId,
                        CreationTime = DateTime.Now,
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
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

                StockMovementsService.InsertPurchaseOrders(input);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);
            }
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
                {
                    FicheNo = input.FicheNo,
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    GrossAmount = input.GrossAmount,
                    LinkedPurchaseRequestID = Guid.Empty,
                    NetAmount = input.NetAmount,
                    PaymentPlanID = input.PaymentPlanID,
                    ProductionOrderID = input.ProductionOrderID,
                    PurchaseOrderState = input.PurchaseOrderState,
                    ShippingAdressID = Guid.Empty,
                    SpecialCode = input.SpecialCode,
                    Time_ = input.Time_,
                    TotalDiscountAmount = input.TotalDiscountAmount,
                    TotalVatAmount = input.TotalVatAmount,
                    TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                    WarehouseID = input.WarehouseID,
                    WorkOrderCreationDate = input.WorkOrderCreationDate,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        WorkOrderCreationDate = DateTime.Now,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LikedPurchaseRequestLineID = Guid.Empty,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineTotalAmount = item.LineTotalAmount,
                        LinkedPurchaseRequestID = Guid.Empty,
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = Guid.Empty,
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseOrderID = addedEntityId,
                        CreationTime = DateTime.Now,
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
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

                await _PurchaseRequestsAppService.UpdatePurchaseRequestLineState(input.SelectPurchaseOrderLinesDto, TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, purchaseOrder.Id);

                return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);
            }

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { Id = id }, false, false, "");

                var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

                if (purchaseOrders.Id != Guid.Empty && purchaseOrders != null)
                {
                    StockMovementsService.DeletePurchaseOrders(purchaseOrders);

                    var deleteQuery = queryFactory.Query().From(Tables.PurchaseOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Delete(LoginedUserService.UserId).Where(new { PurchaseOrderID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Delete, id);

                    return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);
                }
                else
                {
                    var queryLineGet = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = id }, false, false, "");

                    var purchaseOrdersLineGet = queryFactory.Get<SelectPurchaseOrderLinesDto>(queryLineGet);

                    StockMovementsService.DeletePurchaseOrderLines(purchaseOrdersLineGet);

                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var purchaseOrderLines = queryFactory.Update<SelectPurchaseOrderLinesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrderLines, LogType.Delete, id);

                    return new SuccessDataResult<SelectPurchaseOrderLinesDto>(purchaseOrderLines);
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.PurchaseOrders)
                       .Select<PurchaseOrders>(po => new { po.WorkOrderCreationDate,po.WarehouseID,po.TotalVatExcludedAmount,po.TotalVatAmount,po.TotalDiscountAmount,po.Time_,po.SpecialCode,po.ShippingAdressID,po.PurchaseOrderState,po.ProductionOrderID,po.PaymentPlanID,po.NetAmount,po.LinkedPurchaseRequestID,po.Id,po.GrossAmount,po.FicheNo,po.ExchangeRate,po.Description_,po.Date_,po.DataOpenStatusUserId,po.DataOpenStatus,po.CurrentAccountCardID,po.CurrencyID,po.BranchID})
                       .Join<PaymentPlans>
                        (
                            pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                            nameof(PurchaseOrders.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            JoinType.Left
                        )
                        .Join<Branches>
                        (
                            b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                            nameof(PurchaseOrders.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                            nameof(PurchaseOrders.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                         .Join<Currencies>
                        (
                            c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                            nameof(PurchaseOrders.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode  = ca.Code, CurrentAccountCardName  = ca.Name},
                            nameof(PurchaseOrders.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left)

                             .Join<ShippingAdresses>
                        (
                            sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName =sa.Name},
                            nameof(PurchaseOrders.ShippingAdressID),
                            nameof(ShippingAdresses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.PurchaseOrders);

                var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.PurchaseOrderLines)
                       .Select<PurchaseOrderLines>(pol => new { pol.WorkOrderCreationDate,pol.VATrate,pol.VATamount,pol.UnitSetID,pol.UnitPrice,pol.Quantity,pol.PurchaseOrderLineStateEnum,pol.PurchaseOrderID,pol.ProductionOrderID,pol.ProductID,pol.PaymentPlanID,pol.LinkedPurchaseRequestID,pol.LineTotalAmount,pol.LikedPurchaseRequestLineID,pol.Id,pol.ExchangeRate,pol.DiscountRate,pol.DiscountAmount,pol.DataOpenStatusUserId,pol.DataOpenStatus })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(PurchaseOrderLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(PurchaseOrderLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<PaymentPlans>
                        (
                            pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                            nameof(PurchaseOrderLines.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            JoinType.Left
                        )
                        .Where(new { PurchaseOrderID = id }, false, false, Tables.PurchaseOrderLines);

                var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

                purchaseOrders.SelectPurchaseOrderLinesDto = purchaseOrderLine;

                LogsAppService.InsertLogToDatabase(purchaseOrders, purchaseOrders, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Get, id);

                return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrders);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetListAsync(ListPurchaseOrdersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                        .From(Tables.PurchaseOrders)
                       .Select<PurchaseOrders>(po => new { po.WorkOrderCreationDate, po.WarehouseID, po.TotalVatExcludedAmount, po.TotalVatAmount, po.TotalDiscountAmount, po.Time_, po.SpecialCode, po.ShippingAdressID, po.PurchaseOrderState, po.ProductionOrderID, po.PaymentPlanID, po.NetAmount, po.LinkedPurchaseRequestID, po.Id, po.GrossAmount, po.FicheNo, po.ExchangeRate, po.Description_, po.Date_, po.DataOpenStatusUserId, po.DataOpenStatus, po.CurrentAccountCardID, po.CurrencyID, po.BranchID })
                       .Join<PaymentPlans>
                        (
                            pp => new { PaymentPlanName = pp.Name },
                            nameof(PurchaseOrders.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            JoinType.Left
                        )
                        .Join<Branches>
                        (
                            b => new {BranchCode = b.Code, BranchName = b.Name },
                            nameof(PurchaseOrders.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new { WarehouseCode = w.Code },
                            nameof(PurchaseOrders.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                         .Join<Currencies>
                        (
                            c => new { CurrencyCode = c.Code },
                            nameof(PurchaseOrders.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(PurchaseOrders.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left)

                             .Join<ShippingAdresses>
                        (
                            sa => new { ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                            nameof(PurchaseOrders.ShippingAdressID),
                            nameof(ShippingAdresses.Id),
                            JoinType.Left
                        )
                        .Where(null, false, false, Tables.PurchaseOrders);

                var purchaseOrders = queryFactory.GetList<ListPurchaseOrdersDto>(query).ToList();
                return new SuccessDataResult<IList<ListPurchaseOrdersDto>>(purchaseOrders);
            }
        }

        [ValidationAspect(typeof(UpdatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateAsync(UpdatePurchaseOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.PurchaseOrders)
                       .Select("*")
                       .Join<PaymentPlans>
                        (
                            pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                            nameof(PurchaseOrders.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            JoinType.Left
                        )
                        .Join<Branches>
                        (
                            b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                            nameof(PurchaseOrders.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                            nameof(PurchaseOrders.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                         .Join<Currencies>
                        (
                            c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                            nameof(PurchaseOrders.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(PurchaseOrders.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left)

                             .Join<ShippingAdresses>
                        (
                            sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                            nameof(PurchaseOrders.ShippingAdressID),
                            nameof(ShippingAdresses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.PurchaseOrders);

                var entity = queryFactory.Get<SelectPurchaseOrdersDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                        .From(Tables.PurchaseOrderLines)
                       .Select<PurchaseOrderLines>(pol => new { pol.WorkOrderCreationDate, pol.VATrate, pol.VATamount, pol.UnitSetID, pol.UnitPrice, pol.Quantity, pol.PurchaseOrderLineStateEnum, pol.PurchaseOrderID, pol.ProductionOrderID, pol.ProductID, pol.PaymentPlanID, pol.LinkedPurchaseRequestID, pol.LineTotalAmount, pol.LikedPurchaseRequestLineID, pol.Id, pol.ExchangeRate, pol.DiscountRate, pol.DiscountAmount, pol.DataOpenStatusUserId, pol.DataOpenStatus })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(PurchaseOrderLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(PurchaseOrderLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                         .Join<PaymentPlans>
                        (
                            pay => new { PaymentPlanID = pay.Id, PaymentPlanName = pay.Name },
                            nameof(PurchaseOrderLines.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            "PaymentPlanLine",
                            JoinType.Left
                        )
                        .Where(new { PurchaseOrderID = input.Id }, false, false, Tables.PurchaseOrderLines);

                var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

                entity.SelectPurchaseOrderLinesDto = purchaseOrderLine;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.PurchaseOrders)
                       .Select<PurchaseOrders>(po => new { po.WorkOrderCreationDate, po.WarehouseID, po.TotalVatExcludedAmount, po.TotalVatAmount, po.TotalDiscountAmount, po.Time_, po.SpecialCode, po.ShippingAdressID, po.PurchaseOrderState, po.ProductionOrderID, po.PaymentPlanID, po.NetAmount, po.LinkedPurchaseRequestID, po.Id, po.GrossAmount, po.FicheNo, po.ExchangeRate, po.Description_, po.Date_, po.DataOpenStatusUserId, po.DataOpenStatus, po.CurrentAccountCardID, po.CurrencyID, po.BranchID })
                       .Join<PaymentPlans>
                        (
                            pp => new {  PaymentPlanName = pp.Name },
                            nameof(PurchaseOrders.PaymentPlanID),
                            nameof(PaymentPlans.Id),
                            JoinType.Left
                        )
                        .Join<Branches>
                        (
                            b => new {  BranchCode = b.Code, BranchName = b.Name },
                            nameof(PurchaseOrders.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                         .Join<Warehouses>
                        (
                            w => new {  WarehouseCode = w.Code },
                            nameof(PurchaseOrders.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                         .Join<Currencies>
                        (
                            c => new {  CurrencyCode = c.Code },
                            nameof(PurchaseOrders.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(PurchaseOrders.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left)

                             .Join<ShippingAdresses>
                        (
                            sa => new {  ShippingAdressCode = sa.Code, ShippingAdressName = sa.Name },
                            nameof(PurchaseOrders.ShippingAdressID),
                            nameof(ShippingAdresses.Id),
                            JoinType.Left
                        )
                        .Where(new { FicheNo = input.FicheNo }, false, false, Tables.PurchaseOrders);

                var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNo != input.FicheNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
                {
                    FicheNo = input.FicheNo,
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    GrossAmount = input.GrossAmount,
                    LinkedPurchaseRequestID = input.LinkedPurchaseRequestID,
                    NetAmount = input.NetAmount,
                    PaymentPlanID = input.PaymentPlanID,
                    ProductionOrderID = input.ProductionOrderID,
                    PurchaseOrderState = input.PurchaseOrderState,
                    ShippingAdressID = input.ShippingAdressID,
                    SpecialCode = input.SpecialCode,
                    Time_ = input.Time_,
                    TotalDiscountAmount = input.TotalDiscountAmount,
                    TotalVatAmount = input.TotalVatAmount,
                    TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                    WarehouseID = input.WarehouseID,
                    WorkOrderCreationDate = input.WorkOrderCreationDate,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = input.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = input.Id }, false, false, "");

                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Insert(new CreatePurchaseOrderLinesDto
                        {
                            DiscountAmount = item.DiscountAmount,
                            WorkOrderCreationDate = item.WorkOrderCreationDate,
                            DiscountRate = item.DiscountRate,
                            ExchangeRate = item.ExchangeRate,
                            LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            LineTotalAmount = item.LineTotalAmount,
                            LinkedPurchaseRequestID = item.LinkedPurchaseRequestID,
                            PaymentPlanID = item.PaymentPlanID,
                            ProductionOrderID = item.ProductionOrderID,
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                            UnitPrice = item.UnitPrice,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            PurchaseOrderID = input.Id,
                            CreationTime = DateTime.Now,
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
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrderLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectPurchaseOrderLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.PurchaseOrderLines).Update(new UpdatePurchaseOrderLinesDto
                            {
                                DiscountAmount = item.DiscountAmount,
                                WorkOrderCreationDate = item.WorkOrderCreationDate,
                                DiscountRate = item.DiscountRate,
                                ExchangeRate = item.ExchangeRate,
                                LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID,
                                LineAmount = item.LineAmount,
                                LineDescription = item.LineDescription,
                                LineTotalAmount = item.LineTotalAmount,
                                LinkedPurchaseRequestID = item.LinkedPurchaseRequestID,
                                PaymentPlanID = item.PaymentPlanID,
                                ProductionOrderID = item.ProductionOrderID,
                                PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                                UnitPrice = item.UnitPrice,
                                VATamount = item.VATamount,
                                VATrate = item.VATrate,
                                PurchaseOrderID = input.Id,
                                CreationTime = line.CreationTime,
                                CreatorId = line.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = line.DeleterId.GetValueOrDefault(),
                                DeletionTime = line.DeletionTime.GetValueOrDefault(),
                                Id = item.Id,
                                IsDeleted = item.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = item.LineNr,
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                UnitSetID = item.UnitSetID,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

                StockMovementsService.UpdatePurchaseOrders(entity, input);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);

                return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<PurchaseOrders>(entityQuery);

                var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
                {
                    FicheNo = entity.FicheNo,
                    BranchID = entity.BranchID,
                    CurrencyID = entity.CurrencyID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
                    ExchangeRate = entity.ExchangeRate,
                    GrossAmount = entity.GrossAmount,
                    LinkedPurchaseRequestID = entity.LinkedPurchaseRequestID,
                    NetAmount = entity.NetAmount,
                    PaymentPlanID = entity.PaymentPlanID,
                    ProductionOrderID = entity.ProductionOrderID,
                    PurchaseOrderState = (int)entity.PurchaseOrderState,
                    ShippingAdressID = entity.ShippingAdressID,
                    SpecialCode = entity.SpecialCode,
                    Time_ = entity.Time_,
                    TotalDiscountAmount = entity.TotalDiscountAmount,
                    TotalVatAmount = entity.TotalVatAmount,
                    TotalVatExcludedAmount = entity.TotalVatExcludedAmount,
                    WarehouseID = entity.WarehouseID,
                    WorkOrderCreationDate = entity.WorkOrderCreationDate,
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
                }).Where(new { Id = id }, false, false, "");

                var purchaseOrdersDto = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);
                return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrdersDto);

            }
        }
    }
}
