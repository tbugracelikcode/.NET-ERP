using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.OrderAcceptanceRecord.Services;
using TsiErp.Business.Entities.PurchaseOrder.Validations;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
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

        QueryFactory queryFactory { get; set; } = new QueryFactory();


        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;
        private readonly IOrderAcceptanceRecordsAppService _OrderAcceptanceRecordsAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }


        public PurchaseOrdersAppService(IStringLocalizer<PurchaseOrdersResource> l, IPurchaseRequestsAppService PurchaseRequestsAppService, IFicheNumbersAppService ficheNumbersAppService, IOrderAcceptanceRecordsAppService orderAcceptanceRecordsAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _OrderAcceptanceRecordsAppService = orderAcceptanceRecordsAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> CreateAsync(CreatePurchaseOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
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
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                MaintenanceMRPID = input.MaintenanceMRPID,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                MRPID = input.MRPID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
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

            if(input.OrderAcceptanceID != null && input.OrderAcceptanceID != Guid.Empty)
            {
                var OrderAcceptance = (await _OrderAcceptanceRecordsAppService.GetAsync(input.OrderAcceptanceID.GetValueOrDefault())).Data;

                OrderAcceptance.OrderAcceptanceRecordState = TsiErp.Entities.Enums.OrderAcceptanceRecordStateEnum.SiparisOlusturuldu;

                var updatedEntity = ObjectMapper.Map<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>(OrderAcceptance);

                await _OrderAcceptanceRecordsAppService.UpdateAsync(updatedEntity);
            }

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
                    OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                    OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                    LinkedPurchaseRequestID = Guid.Empty,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    ProductionOrderID = Guid.Empty,
                    PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                    SupplyDate = item.SupplyDate,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                if(item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                {
                    if(item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                    {
                        await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                    }
                }
            }

            var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

            StockMovementsService.InsertPurchaseOrders(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.ControlList<PurchaseOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Insert(new CreatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                MRPID = input.MRPID.GetValueOrDefault(),
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
                ShippingAdressID = Guid.Empty,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
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
                    SupplyDate = item.SupplyDate,
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
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchaseOrder = queryFactory.Insert<SelectPurchaseOrdersDto>(query, "Id", true);

            await _PurchaseRequestsAppService.UpdatePurchaseRequestLineState(input.SelectPurchaseOrderLinesDto, TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Insert, purchaseOrder.Id);

            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("OrderID", new List<string>
            {
                Tables.PurchaseUnsuitabilityReports
            });

            DeleteControl.ControlList.Add("PurchaseOrderID", new List<string>
            {
                Tables.StockFiches,
                Tables.StockFicheLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
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

                    await Task.CompletedTask;
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

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectPurchaseOrderLinesDto>(purchaseOrderLines);
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
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
                    .Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
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
                    .Where(new { Id = id }, false, false, Tables.PurchaseOrders);

            var purchaseOrders = queryFactory.Get<SelectPurchaseOrdersDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
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
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrderID = id }, false, false, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();

            purchaseOrders.SelectPurchaseOrderLinesDto = purchaseOrderLine;

            LogsAppService.InsertLogToDatabase(purchaseOrders, purchaseOrders, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrders);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetListAsync(ListPurchaseOrdersParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
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
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseOrdersDto>>(purchaseOrders);

        }


        [ValidationAspect(typeof(UpdatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateAsync(UpdatePurchaseOrdersDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrders)
                   .Select<PurchaseOrders>(null)
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
                    ).Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
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
                   .Select<PurchaseOrderLines>(null)
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
                    ).Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
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
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseOrders.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
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
                    .Where(new { FicheNo = input.FicheNo }, false, false, Tables.PurchaseOrders);

            var list = queryFactory.GetList<ListPurchaseOrdersDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PurchaseOrders).Update(new UpdatePurchaseOrdersDto
            {
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                OrderAcceptanceID = input.OrderAcceptanceID.GetValueOrDefault(),
                MRPID = input.MRPID,
                MaintenanceMRPID = input.MaintenanceMRPID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID.GetValueOrDefault(),
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PurchaseOrderState = input.PurchaseOrderState,
                ShippingAdressID = input.ShippingAdressID.GetValueOrDefault(),
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
                        LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID.GetValueOrDefault(),
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineTotalAmount = item.LineTotalAmount,
                        OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                        OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                        LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                        PaymentPlanID = item.PaymentPlanID,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                        SupplyDate = item.SupplyDate,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        BranchID = input.BranchID.GetValueOrDefault(),
                        CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                        WarehouseID = input.WarehouseID.GetValueOrDefault(),
                        Date_ = input.Date_
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                    if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                    {
                        if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                        {
                            await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                        }
                    }
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
                            LikedPurchaseRequestLineID = item.LikedPurchaseRequestLineID.GetValueOrDefault(),
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            OrderAcceptanceLineID = item.OrderAcceptanceLineID.GetValueOrDefault(),
                            OrderAcceptanceID = item.OrderAcceptanceID.GetValueOrDefault(),
                            LineTotalAmount = item.LineTotalAmount,
                            LinkedPurchaseRequestID = item.LinkedPurchaseRequestID.GetValueOrDefault(),
                            PaymentPlanID = item.PaymentPlanID,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseOrderLineStateEnum = (int)item.PurchaseOrderLineStateEnum,
                            UnitPrice = item.UnitPrice,
                            SupplyDate = item.SupplyDate,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            BranchID = input.BranchID.GetValueOrDefault(),
                            CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                            WarehouseID = input.WarehouseID.GetValueOrDefault(),
                            Date_ = input.Date_
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                        if (item.OrderAcceptanceID != null && item.OrderAcceptanceID != Guid.Empty) // Sipariş kabul kaydının temin tarihini güncelleme
                        {
                            if (item.OrderAcceptanceLineID != null && item.OrderAcceptanceLineID != Guid.Empty)
                            {
                                await _OrderAcceptanceRecordsAppService.UpdateLineAsync(item.OrderAcceptanceLineID.GetValueOrDefault(), item.SupplyDate.GetValueOrDefault());
                            }
                        }
                    }
                }
            }

            var purchaseOrder = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);

            StockMovementsService.UpdatePurchaseOrders(entity, input);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrders, LogType.Update, purchaseOrder.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrder);

        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
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
                OrderAcceptanceID = entity.OrderAcceptanceID.GetValueOrDefault(),
                GrossAmount = entity.GrossAmount,
                MaintenanceMRPID = entity.MaintenanceMRPID,
                MRPID = entity.MRPID,
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
                LastModifierId = entity.LastModifierId.GetValueOrDefault()
            }).Where(new { Id = id }, false, false, "");

            var purchaseOrdersDto = queryFactory.Update<SelectPurchaseOrdersDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersDto>(purchaseOrdersDto);


        }

        public async Task<IDataResult<IList<SelectPurchaseOrderLinesDto>>> GetLineListAsync()
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrderLines)
                   .Select<PurchaseOrderLines>(null)
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
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseOrderLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseOrderLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseOrderLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseOrderLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, false, false, Tables.PurchaseOrderLines);

            var purchaseOrderLine = queryFactory.GetList<SelectPurchaseOrderLinesDto>(queryLines).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<List<SelectPurchaseOrderLinesDto>>(purchaseOrderLine);
        }
    }
}
