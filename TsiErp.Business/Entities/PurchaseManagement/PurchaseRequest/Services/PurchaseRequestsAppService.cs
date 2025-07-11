﻿using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.PurchaseRequest.Validations;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseRequests.Page;

namespace TsiErp.Business.Entities.PurchaseRequest.Services
{
    [ServiceRegistration(typeof(IPurchaseRequestsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseRequestsAppService : ApplicationService<PurchaseRequestsResource>, IPurchaseRequestsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;



        public PurchaseRequestsAppService(IStringLocalizer<PurchaseRequestsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }



        [ValidationAspect(typeof(CreatePurchaseRequestsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> CreateAsync(CreatePurchaseRequestsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseRequests).Select("FicheNo").Where(new { FicheNo = input.FicheNo },  "");
            var list = queryFactory.ControlList<PurchaseRequests>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.PurchaseRequests).Insert(new CreatePurchaseRequestsDto
            {
                FicheNo = input.FicheNo,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                MRPID = input.MRPID.GetValueOrDefault(),
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PropositionRevisionNo = input.PropositionRevisionNo,
                PurchaseRequestState = input.PurchaseRequestState,
                RevisionDate = input.RevisionDate,
                RevisionTime = input.RevisionTime,
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                ValidityDate_ = input.ValidityDate_,
                WarehouseID = input.WarehouseID,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectPurchaseRequestLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Insert(new CreatePurchaseRequestLinesDto
                {
                    DiscountAmount = item.DiscountAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    LineAmount = item.LineAmount,
                    LineDescription = item.LineDescription,
                    PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                    WaitingQuantity = item.WaitingQuantity,
                    LineTotalAmount = item.LineTotalAmount,
                    OrderConversionDate = item.OrderConversionDate,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseRequestLineState = (int)item.PurchaseRequestLineState,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseRequestID = addedEntityId,
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
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    SupplierReferenceNo = item.SupplierReferenceNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchaseRequest = queryFactory.Insert<SelectPurchaseRequestsDto>(query, "Id", true);

            StockMovementsService.InsertPurchaseRequests(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseRequestsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseRequests, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseRequestsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequest);

        }

        public async Task<IDataResult<SelectPurchaseRequestsDto>> ConvertToPurchaseRequestMRPAsync(CreatePurchaseRequestsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseRequests).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.ControlList<PurchaseRequests>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.PurchaseRequests).Insert(new CreatePurchaseRequestsDto
            {
                FicheNo = input.FicheNo,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                MRPID = input.MRPID.GetValueOrDefault(),
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                LinkedPurchaseRequestID = Guid.Empty,
                NetAmount = input.NetAmount,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PropositionRevisionNo = input.PropositionRevisionNo,
                PurchaseRequestState = input.PurchaseRequestState,
                RevisionDate = input.RevisionDate,
                RevisionTime = input.RevisionTime,
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                ValidityDate_ = input.ValidityDate_,
                WarehouseID = input.WarehouseID,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                PricingCurrency = input.PricingCurrency
            });

            foreach (var item in input.SelectPurchaseRequestLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Insert(new CreatePurchaseRequestLinesDto
                {
                    DiscountAmount = item.DiscountAmount,
                    TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                    TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                    TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                    TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                    TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                    DiscountRate = item.DiscountRate,
                    ExchangeRate = item.ExchangeRate,
                    LineAmount = item.LineAmount,
                    LineDescription = item.LineDescription,
                    PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                    WaitingQuantity = item.WaitingQuantity,
                    LineTotalAmount = item.LineTotalAmount,
                    OrderConversionDate = item.OrderConversionDate,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    PurchaseRequestLineState = (int)item.PurchaseRequestLineState,
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    PurchaseRequestID = addedEntityId,
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
                    Quantity = item.Quantity,
                    UnitSetID = item.UnitSetID.GetValueOrDefault(),
                    BranchID = input.BranchID.GetValueOrDefault(),
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    WarehouseID = input.WarehouseID.GetValueOrDefault(),
                    Date_ = input.Date_,
                    SupplierReferenceNo = item.SupplierReferenceNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchaseRequest = queryFactory.Insert<SelectPurchaseRequestsDto>(query, "Id", true);

            StockMovementsService.InsertPurchaseRequests(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseRequestsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseRequests, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequest);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;

            var query = queryFactory.Query().From(Tables.PurchaseRequests).Select("*").Where(new { Id = id }, "");

            var purchaseRequests = queryFactory.Get<SelectPurchaseRequestsDto>(query);

            if (purchaseRequests.Id != Guid.Empty && purchaseRequests != null)
            {
                StockMovementsService.DeletePurchaseRequests(purchaseRequests);

                var deleteQuery = queryFactory.Query().From(Tables.PurchaseRequests).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseRequestLines).Delete(LoginedUserService.UserId).Where(new { PurchaseRequestID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var purchaseRequest = queryFactory.Update<SelectPurchaseRequestsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseRequests, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseRequestsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = entity.FicheNo,
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
                                RecordNumber = entity.FicheNo,
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
                return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequest);
            }
            else
            {
                var queryLineGet = queryFactory.Query().From(Tables.PurchaseRequestLines).Select("*").Where(new { Id = id },  "");

                var purchaseRequestsLineGet = queryFactory.Get<SelectPurchaseRequestLinesDto>(queryLineGet);

                StockMovementsService.DeletePurchaseRequestLines(purchaseRequestsLineGet);

                var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var purchaseRequestLines = queryFactory.Update<SelectPurchaseRequestLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseRequestLines, LogType.Delete, id);
                await Task.CompletedTask;

                return new SuccessDataResult<SelectPurchaseRequestLinesDto>(purchaseRequestLines);
            }

        }

        public async Task<IDataResult<SelectPurchaseRequestsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequests)
                   .Select<PurchaseRequests>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseRequests.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequests.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequests.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseRequests.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseRequests.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequests.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseRequests.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { Id = id },  Tables.PurchaseRequests);

            var purchaseRequests = queryFactory.Get<SelectPurchaseRequestsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequestLines)
                   .Select<PurchaseRequestLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseRequestLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseRequestLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseRequestLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequestLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequestLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequestLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    ).Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseRequestLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseRequestID = id }, Tables.PurchaseRequestLines);

            var purchaseRequestLine = queryFactory.GetList<SelectPurchaseRequestLinesDto>(queryLines).ToList();

            purchaseRequests.SelectPurchaseRequestLines = purchaseRequestLine;

            LogsAppService.InsertLogToDatabase(purchaseRequests, purchaseRequests, LoginedUserService.UserId, Tables.PurchaseRequests, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequests);

        }

        public async Task<IDataResult<IList<ListPurchaseRequestsDto>>> GetListAsync(ListPurchaseRequestsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequests)
                   .Select<PurchaseRequests>(s => new { s.FicheNo,s.Date_, s.Id })
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseRequests.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<MRPs>
                    (
                        b => new { MRPID = b.Id, MRPCode = b.Code },
                        nameof(PurchaseOrders.MRPID),
                        nameof(MRPs.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequests.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequests.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseRequests.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseRequests.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CurrentAccountCardID = ca.Id },
                        nameof(PurchaseRequests.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        ca => new { ProductionOrderID = ca.Id, ProductionOrderFicheNo = ca.FicheNo },
                        nameof(PurchaseRequests.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(null, Tables.PurchaseRequests);

            var purchaseRequests = queryFactory.GetList<ListPurchaseRequestsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseRequestsDto>>(purchaseRequests);

        }

        [ValidationAspect(typeof(UpdatePurchaseRequestsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> UpdateAsync(UpdatePurchaseRequestsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.PurchaseRequests)
                   .Select<PurchaseRequests>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(PurchaseRequests.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequests.BranchID),
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
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequests.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchaseRequests.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(PurchaseRequests.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequests.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )

                    .Where(new { Id = input.Id }, Tables.PurchaseRequests);

            var entity = queryFactory.Get<SelectPurchaseRequestsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequestLines)
                   .Select<PurchaseRequestLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseRequestLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseRequestLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseRequestLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequestLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequestLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequestLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseRequestID = input.Id },  Tables.PurchaseRequestLines);

            var purchaseRequestLine = queryFactory.GetList<SelectPurchaseRequestLinesDto>(queryLines).ToList();

            entity.SelectPurchaseRequestLines = purchaseRequestLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PurchaseRequests)
                   .Select<PurchaseRequests>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(PurchaseRequests.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequests.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequests.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchaseRequests.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequests.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )

                            .Where(new { FicheNo = input.FicheNo }, Tables.PurchaseRequests);

            var list = queryFactory.GetList<ListPurchaseRequestsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.PurchaseRequests).Update(new UpdatePurchaseRequestsDto
            {
                FicheNo = input.FicheNo,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                LinkedPurchaseRequestID = input.LinkedPurchaseRequestID,
                NetAmount = input.NetAmount,
                MRPID = input.MRPID.GetValueOrDefault(),
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                PropositionRevisionNo = input.PropositionRevisionNo,
                PurchaseRequestState = input.PurchaseRequestState,
                RevisionDate = input.RevisionDate,
                RevisionTime = input.RevisionTime,
                SpecialCode = input.SpecialCode,
                Time_ = input.Time_,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                ValidityDate_ = input.ValidityDate_,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
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
                PricingCurrency = input.PricingCurrency
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPurchaseRequestLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Insert(new CreatePurchaseRequestLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        WaitingQuantity = item.WaitingQuantity,
                        PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                        LineTotalAmount = item.LineTotalAmount,
                        OrderConversionDate = item.OrderConversionDate,
                        PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        PurchaseRequestLineState = (int)item.PurchaseRequestLineState,
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        PurchaseRequestID = input.Id,
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
                        Quantity = item.Quantity,
                        UnitSetID = item.UnitSetID.GetValueOrDefault(),
                        BranchID = input.BranchID.GetValueOrDefault(),
                        CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                        Date_ = input.Date_,
                        WarehouseID = input.WarehouseID.GetValueOrDefault(),
                        SupplierReferenceNo = item.SupplierReferenceNo
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseRequestLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseRequestLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Update(new UpdatePurchaseRequestLinesDto
                        {
                            DiscountAmount = item.DiscountAmount,
                            DiscountRate = item.DiscountRate,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            ExchangeRate = item.ExchangeRate,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            LineTotalAmount = item.LineTotalAmount,
                            PurchaseReservedQuantity = item.PurchaseReservedQuantity,
                            WaitingQuantity = item.WaitingQuantity,
                            OrderConversionDate = item.OrderConversionDate,
                            PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PurchaseRequestLineState = (int)item.PurchaseRequestLineState,
                            UnitPrice = item.UnitPrice,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            PurchaseRequestID = input.Id,
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
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            BranchID = input.BranchID.GetValueOrDefault(),
                            CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                            Date_ = input.Date_,
                            WarehouseID = input.WarehouseID.GetValueOrDefault(),
                            SupplierReferenceNo = item.SupplierReferenceNo
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var purchaseRequest = queryFactory.Update<SelectPurchaseRequestsDto>(query, "Id", true);

            StockMovementsService.UpdatePurchaseRequests(entity, input);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseRequests, LogType.Update, purchaseRequest.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseRequestsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequest);

        }

        public async Task<IDataResult<SelectPurchaseRequestsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseRequests).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchaseRequests>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseRequests).Update(new UpdatePurchaseRequestsDto
            {
                FicheNo = entity.FicheNo,
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                Date_ = entity.Date_,
                MRPID = entity.MRPID,
                Description_ = entity.Description_,
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                ExchangeRate = entity.ExchangeRate,
                GrossAmount = entity.GrossAmount,
                LinkedPurchaseRequestID = entity.LinkedPurchaseRequestID,
                NetAmount = entity.NetAmount,
                PaymentPlanID = entity.PaymentPlanID,
                ProductionOrderID = entity.ProductionOrderID,
                PropositionRevisionNo = entity.PropositionRevisionNo,
                PurchaseRequestState = (int)entity.PurchaseRequestState,
                RevisionDate = entity.RevisionDate,
                RevisionTime = entity.RevisionTime,
                SpecialCode = entity.SpecialCode,
                Time_ = entity.Time_,
                TotalDiscountAmount = entity.TotalDiscountAmount,
                TotalVatAmount = entity.TotalVatAmount,
                TotalVatExcludedAmount = entity.TotalVatExcludedAmount,
                ValidityDate_ = entity.ValidityDate_,
                WarehouseID = entity.WarehouseID,
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
                PricingCurrency = (int)entity.PricingCurrency
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var purchaseRequestsDto = queryFactory.Update<SelectPurchaseRequestsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseRequestsDto>(purchaseRequestsDto);


        }

        public async Task UpdatePurchaseRequestLineState(List<SelectPurchaseOrderLinesDto> orderLineList, PurchaseRequestLineStateEnum lineState)
        {
            foreach (var item in orderLineList)
            {
                var entity = (await GetAsync(item.LinkedPurchaseRequestID.GetValueOrDefault())).Data;

                DateTime now = _GetSQLDateAppService.GetDateFromSQL();

                var query = queryFactory.Query().From(Tables.PurchaseRequests).Update(new UpdatePurchaseRequestsDto
                {
                    FicheNo = entity.FicheNo,
                    BranchID = entity.BranchID,
                    CurrencyID = entity.CurrencyID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
                    ExchangeRate = entity.ExchangeRate,
                    GrossAmount = entity.GrossAmount,
                    TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                    MRPID = entity.MRPID,
                    LinkedPurchaseRequestID = entity.LinkedPurchaseRequestID,
                    NetAmount = entity.NetAmount,
                    PaymentPlanID = entity.PaymentPlanID,
                    ProductionOrderID = entity.ProductionOrderID,
                    PropositionRevisionNo = entity.PropositionRevisionNo,
                    PurchaseRequestState = (int)entity.PurchaseRequestState,
                    RevisionDate = entity.RevisionDate,
                    RevisionTime = entity.RevisionTime,
                    SpecialCode = entity.SpecialCode,
                    Time_ = entity.Time_,
                    TotalDiscountAmount = entity.TotalDiscountAmount,
                    TotalVatAmount = entity.TotalVatAmount,
                    TotalVatExcludedAmount = entity.TotalVatExcludedAmount,
                    ValidityDate_ = entity.ValidityDate_,
                    WarehouseID = entity.WarehouseID,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = entity.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = now,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = entity.Id }, "");

                if (entity.SelectPurchaseRequestLines.Count > 0)
                {
                    foreach (var line in entity.SelectPurchaseRequestLines)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseRequestLines).Update(new UpdatePurchaseRequestLinesDto
                        {
                            DiscountAmount = line.DiscountAmount,
                            DiscountRate = line.DiscountRate,
                            ExchangeRate = line.ExchangeRate,
                            LineAmount = line.LineAmount,
                            LineDescription = line.LineDescription,
                            LineTotalAmount = line.LineTotalAmount,
                            OrderConversionDate = _GetSQLDateAppService.GetDateFromSQL(),
                            PaymentPlanID = line.PaymentPlanID,
                            ProductionOrderID = line.ProductionOrderID,
                            PurchaseRequestLineState = (int)lineState,
                            WaitingQuantity = line.WaitingQuantity,
                            PurchaseReservedQuantity = line.PurchaseReservedQuantity,
                            UnitPrice = line.UnitPrice,
                            VATamount = line.VATamount,
                            VATrate = line.VATrate,
                            PurchaseRequestID = line.PurchaseRequestID,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = line.Id,
                            IsDeleted = false,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = line.LineNr,
                            ProductID = line.ProductID,
                            Quantity = line.Quantity,
                            UnitSetID = line.UnitSetID,
                            BranchID = line.BranchID,
                            CurrentAccountCardID = line.CurrentAccountCardID,
                            WarehouseID = line.WarehouseID,
                            Date_ = line.Date_,
                            SupplierReferenceNo = line.SupplierReferenceNo
                        }).Where(new { Id = line.Id }, "");
                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                    }
                }

                var purchaseRequest = queryFactory.Update<SelectPurchaseRequestsDto>(query, "Id", true);
            }

        }

        public async Task<IDataResult<IList<SelectPurchaseRequestLinesDto>>> GetLineListAsync()
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseRequestLines)
                   .Select<PurchaseRequestLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseRequestLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(PurchaseRequestLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(PurchaseRequestLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(PurchaseRequestLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(PurchaseRequestLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchaseRequestLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchaseRequestLines);

            var purchaseRequestLine = queryFactory.GetList<SelectPurchaseRequestLinesDto>(queryLines).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<List<SelectPurchaseRequestLinesDto>>(purchaseRequestLine);
        }



        #region Stock Movement Transactions
        //private static async Task StockMovementDelete(UnitOfWork _uow, PurchaseRequests entity, PurchaseRequestLines line)
        //{
        //    #region ByDateStockMovement
        //    var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == line.ProductID && t.Date_ == entity.Date_);

        //    if (byDateStockMovement != null)
        //    {
        //        byDateStockMovement.TotalPurchaseRequest -= line.Quantity;
        //    }
        //    #endregion

        //    #region GrandTotalStockMovement
        //    var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == line.ProductID);

        //    if (grandTotalStockMovement != null)
        //    {
        //        grandTotalStockMovement.TotalPurchaseRequest -= line.Quantity;
        //    }
        //    #endregion
        //}

        //private static async Task StockMovementLineDelete(UnitOfWork _uow, PurchaseRequestLines lines, PurchaseRequests entity)
        //{
        //    #region ByDateStockMovement
        //    var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == lines.ProductID && t.Date_ == entity.Date_);

        //    if (byDateStockMovement != null)
        //    {
        //        byDateStockMovement.TotalPurchaseRequest -= lines.Quantity;
        //    }
        //    #endregion

        //    #region GrandTotalStockMovement
        //    var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == lines.ProductID);

        //    if (grandTotalStockMovement != null)
        //    {
        //        grandTotalStockMovement.TotalPurchaseRequest -= lines.Quantity;
        //    }
        //    #endregion
        //}

        //private static async Task StockMovementInsert(CreatePurchaseRequestsDto input, UnitOfWork _uow, PurchaseRequestLines lineEntity)
        //{
        //    #region ByDateStockMovement
        //    var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == input.BranchID && t.WarehouseID == input.WarehouseID && t.ProductID == lineEntity.ProductID && t.Date_ == input.Date_);

        //    if (byDateStockMovement == null)
        //    {
        //        await _uow.ByDateStockMovementsRepository.InsertAsync(new ByDateStockMovements
        //        {
        //            BranchID = input.BranchID.GetValueOrDefault(),
        //            Date_ = input.Date_,
        //            ProductID = lineEntity.ProductID,
        //            TotalPurchaseOrder = 0,
        //            WarehouseID = input.WarehouseID.GetValueOrDefault(),
        //            TotalSalesProposition = 0,
        //            TotalProduction = 0,
        //            TotalSalesOrder = 0,
        //            TotalWastage = 0,
        //            TotalPurchaseRequest = lineEntity.Quantity,
        //            TotalGoodsReceipt = 0,
        //            TotalGoodsIssue = 0,
        //            TotalConsumption = 0,
        //            Amount = 0

        //        });
        //    }
        //    else
        //    {
        //        byDateStockMovement.TotalPurchaseRequest = lineEntity.Quantity;
        //    }
        //    #endregion

        //    #region GrandTotalStockMovement
        //    var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == input.BranchID && t.WarehouseID == input.WarehouseID && t.ProductID == lineEntity.ProductID);

        //    if (grandTotalStockMovement == null)
        //    {
        //        await _uow.GrandTotalStockMovementsRepository.InsertAsync(new GrandTotalStockMovements
        //        {
        //            BranchID = input.BranchID.GetValueOrDefault(),
        //            ProductID = lineEntity.ProductID,
        //            TotalPurchaseOrder = 0,
        //            WarehouseID = input.WarehouseID.GetValueOrDefault(),
        //            TotalSalesProposition = 0,
        //            TotalProduction = 0,
        //            TotalSalesOrder = 0,
        //            TotalWastage = 0,
        //            TotalPurchaseRequest = lineEntity.Quantity,
        //            TotalGoodsReceipt = 0,
        //            TotalGoodsIssue = 0,
        //            TotalConsumption = 0,
        //            Amount = 0,
        //            TotalReserved = 0

        //        });
        //    }
        //    else
        //    {
        //        grandTotalStockMovement.TotalPurchaseRequest = lineEntity.Quantity;
        //    }
        //    #endregion
        //}

        //private static async Task StockMovementInsertOrUpdate(UpdatePurchaseRequestsDto input, UnitOfWork _uow, PurchaseRequests entity, SelectPurchaseRequestLinesDto item, PurchaseRequestLines lineEntity)
        //{
        //    var oldLine = entity.PurchaseRequestLines.FirstOrDefault(t => t.Id == item.Id);

        //    var branchId = input.BranchID == entity.BranchID ? entity.BranchID : input.BranchID;
        //    var warehouseId = input.WarehouseID == entity.WarehouseID ? entity.WarehouseID : input.WarehouseID;
        //    var date = input.Date_ == entity.Date_ ? entity.Date_ : input.Date_;
        //    var productId = lineEntity.ProductID == oldLine.ProductID ? oldLine.ProductID : lineEntity.ProductID;

        //    #region ByDateStockMovement
        //    var deletedByDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == productId && t.Date_ == entity.Date_);

        //    if (deletedByDateStockMovement != null)
        //    {
        //        deletedByDateStockMovement.TotalPurchaseRequest = deletedByDateStockMovement.TotalPurchaseRequest - lineEntity.Quantity;
        //    }

        //    var byDateStockMovement = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.BranchID == branchId && t.WarehouseID == warehouseId && t.ProductID == productId && t.Date_ == date);

        //    if (byDateStockMovement == null)
        //    {
        //        await _uow.ByDateStockMovementsRepository.InsertAsync(new ByDateStockMovements
        //        {
        //            BranchID = branchId.GetValueOrDefault(),
        //            Date_ = date,
        //            ProductID = productId,
        //            TotalPurchaseOrder = 0,
        //            WarehouseID = warehouseId.GetValueOrDefault(),
        //            TotalSalesProposition = 0,
        //            TotalProduction = 0,
        //            TotalSalesOrder = 0,
        //            TotalWastage = 0,
        //            TotalPurchaseRequest = lineEntity.Quantity,
        //            TotalGoodsReceipt = 0,
        //            TotalGoodsIssue = 0,
        //            TotalConsumption = 0,
        //            Amount = 0
        //        });
        //    }
        //    else
        //    {
        //        if (oldLine.Quantity > lineEntity.Quantity)
        //        {
        //            decimal lineValue = oldLine.Quantity - lineEntity.Quantity;
        //            var totalPurchaseRequest = byDateStockMovement.TotalPurchaseRequest - lineValue;
        //            byDateStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
        //        }

        //        if (oldLine.Quantity < lineEntity.Quantity)
        //        {
        //            decimal lineValue = lineEntity.Quantity - oldLine.Quantity;
        //            var totalPurchaseRequest = byDateStockMovement.TotalPurchaseRequest + lineValue;
        //            byDateStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
        //        }

        //    }
        //    #endregion

        //    #region GrandTotalStockMovement

        //    var deletedGrandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == entity.BranchID && t.WarehouseID == entity.WarehouseID && t.ProductID == productId);

        //    if (deletedGrandTotalStockMovement != null)
        //    {
        //        deletedGrandTotalStockMovement.TotalPurchaseRequest = deletedGrandTotalStockMovement.TotalPurchaseRequest - lineEntity.Quantity;
        //    }


        //    var grandTotalStockMovement = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.BranchID == branchId && t.WarehouseID == warehouseId && t.ProductID == productId);

        //    if (grandTotalStockMovement == null)
        //    {
        //        await _uow.GrandTotalStockMovementsRepository.InsertAsync(new GrandTotalStockMovements
        //        {
        //            BranchID = branchId.GetValueOrDefault(),
        //            ProductID = productId,
        //            TotalPurchaseOrder = 0,
        //            WarehouseID = warehouseId.GetValueOrDefault(),
        //            TotalSalesProposition = 0,
        //            TotalProduction = 0,
        //            TotalSalesOrder = 0,
        //            TotalWastage = 0,
        //            TotalPurchaseRequest = lineEntity.Quantity,
        //            TotalGoodsReceipt = 0,
        //            TotalGoodsIssue = 0,
        //            TotalConsumption = 0,
        //            Amount = 0,
        //            TotalReserved = 0
        //        });
        //    }
        //    else
        //    {
        //        if (oldLine.Quantity > lineEntity.Quantity)
        //        {
        //            decimal lineValue = oldLine.Quantity - lineEntity.Quantity;
        //            var totalPurchaseRequest = grandTotalStockMovement.TotalPurchaseRequest - lineValue;
        //            grandTotalStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
        //        }

        //        if (oldLine.Quantity < lineEntity.Quantity)
        //        {
        //            decimal lineValue = lineEntity.Quantity - oldLine.Quantity;
        //            var totalPurchaseRequest = grandTotalStockMovement.TotalPurchaseRequest + lineValue;
        //            grandTotalStockMovement.TotalPurchaseRequest = totalPurchaseRequest;
        //        }
        //    }
        //    #endregion
        //}
        #endregion
    }
}