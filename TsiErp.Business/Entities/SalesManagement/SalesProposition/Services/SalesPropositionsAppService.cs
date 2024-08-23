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
using TsiErp.Business.Entities.SalesProposition.Validations;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesPropositions.Page;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    [ServiceRegistration(typeof(ISalesPropositionsAppService), DependencyInjectionType.Scoped)]
    public class SalesPropositionsAppService : ApplicationService<SalesPropositionsResource>, ISalesPropositionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public SalesPropositionsAppService(IStringLocalizer<SalesPropositionsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateSalesPropositionsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectSalesPropositionsDto>> CreateAsync(CreateSalesPropositionsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.SalesPropositions).Select("FicheNo").Where(new { FicheNo = input.FicheNo },  "");
            var list = queryFactory.ControlList<SalesPropositions>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.SalesPropositions).Insert(new CreateSalesPropositionsDto
            {
                LinkedSalesPropositionID = Guid.Empty,
                SalesPropositionState = input.SalesPropositionState,
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                PropositionRevisionNo = input.PropositionRevisionNo,
                RevisionDate = input.RevisionDate,
                RevisionTime = input.RevisionTime,
                ShippingAdressID = input.ShippingAdressID.GetValueOrDefault(),
                SpecialCode = input.SpecialCode,
                Time_ = time,
                TotalDiscountAmount = input.TotalDiscountAmount,
                TotalVatAmount = input.TotalVatAmount,
                TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                ValidityDate_ = input.ValidityDate_,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
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

            foreach (var item in input.SelectSalesPropositionLines)
            {
                var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Insert(new CreateSalesPropositionLinesDto
                {
                    SalesPropositionLineState = (int)item.SalesPropositionLineState,
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
                    LineTotalAmount = item.LineTotalAmount,
                    OrderConversionDate = item.OrderConversionDate,
                    PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                    UnitPrice = item.UnitPrice,
                    VATamount = item.VATamount,
                    VATrate = item.VATrate,
                    SalesPropositionID = addedEntityId,
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
                    Date_ = input.Date_
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var salesProposition = queryFactory.Insert<SelectSalesPropositionsDto>(query, "Id", true);

            StockMovementsService.InsertSalesPropositions(input);

            await FicheNumbersAppService.UpdateFicheNumberAsync("SalesPropositionsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesPropositionsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                            Message_ = notTemplate.Message_,
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

            return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { Id = id },  "");

            var salesPropositions = queryFactory.Get<SelectSalesPropositionsDto>(query);

            if (salesPropositions.Id != Guid.Empty && salesPropositions != null)
            {
                StockMovementsService.DeleteSalesPropositions(salesPropositions);

                var deleteQuery = queryFactory.Query().From(Tables.SalesPropositions).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.SalesPropositionLines).Delete(LoginedUserService.UserId).Where(new { SalesPropositionID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesPropositionsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                Message_ = notTemplate.Message_,
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
                return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);
            }
            else
            {
                var queryLineGet = queryFactory.Query().From(Tables.SalesPropositionLines).Select("*").Where(new { Id = id },  "");

                var salesPropositionsLineGet = queryFactory.Get<SelectSalesPropositionLinesDto>(queryLineGet);

                StockMovementsService.DeleteSalesPropositionLines(salesPropositionsLineGet);

                var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var salesPropositionLines = queryFactory.Update<SelectSalesPropositionLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPropositionLines, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesPropositionLinesDto>(salesPropositionLines);
            }

        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.SalesPropositions)
                   .Select<SalesPropositions>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesPropositions.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositions.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositions.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesPropositions.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                      .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesPropositions.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPropositions.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code },
                        nameof(SalesPropositions.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.SalesPropositions);

            var salesPropositions = queryFactory.Get<SelectSalesPropositionsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPropositionLines)
                   .Select<SalesPropositionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPropositionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesPropositionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(SalesPropositionLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositionLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositionLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesPropositionLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPropositionID = id },  Tables.SalesPropositionLines);

            var salesPropositionLine = queryFactory.GetList<SelectSalesPropositionLinesDto>(queryLines).ToList();

            salesPropositions.SelectSalesPropositionLines = salesPropositionLine;

            LogsAppService.InsertLogToDatabase(salesPropositions, salesPropositions, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPropositionsDto>(salesPropositions);

        }

        public async Task<IDataResult<IList<ListSalesPropositionsDto>>> GetListAsync(ListSalesPropositionsParamaterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.SalesPropositions)
                   .Select<SalesPropositions>(s => new { s.FicheNo, s.Date_, s.Id })
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(SalesPropositions.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositions.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositions.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesPropositions.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesPropositions.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPropositions.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code },
                        nameof(SalesPropositions.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.SalesPropositions);

            var salesPropositions = queryFactory.GetList<ListSalesPropositionsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListSalesPropositionsDto>>(salesPropositions);

        }

        [ValidationAspect(typeof(UpdateSalesPropositionsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateAsync(UpdateSalesPropositionsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                 .From(Tables.SalesPropositions)
                   .Select<SalesPropositions>(null)
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanID = pp.Id, PaymentPlanName = pp.Name },
                        nameof(SalesPropositions.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositions.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositions.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesPropositions.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        w => new { TransactionExchangeCurrencyCode = w.Code, TransactionExchangeCurrencyID = w.Id },
                        nameof(SalesPropositions.TransactionExchangeCurrencyID),
                        nameof(Currencies.Id),
                        "TransactionExchangeCurrency",
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPropositions.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressID = sa.Id, ShippingAdressCode = sa.Code },
                        nameof(SalesPropositions.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.SalesPropositions);

            var entity = queryFactory.Get<SelectSalesPropositionsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPropositionLines)
                   .Select<SalesPropositionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPropositionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesPropositionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(SalesPropositionLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositionLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositionLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesPropositionLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPropositionID = input.Id },  Tables.SalesPropositionLines);

            var salesPropositionLine = queryFactory.GetList<SelectSalesPropositionLinesDto>(queryLines).ToList();

            entity.SelectSalesPropositionLines = salesPropositionLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                     .From(Tables.SalesPropositions)
                   .Select<SalesPropositions>(sp => new { sp.WarehouseID, sp.ValidityDate_, sp.TotalVatExcludedAmount, sp.TotalVatAmount, sp.TotalDiscountAmount, sp.Time_, sp.SpecialCode, sp.ShippingAdressID, sp.RevisionTime, sp.RevisionDate, sp.SalesPropositionState, sp.PropositionRevisionNo, sp.PaymentPlanID, sp.NetAmount, sp.LinkedSalesPropositionID, sp.Id, sp.GrossAmount, sp.FicheNo, sp.ExchangeRate, sp.Description_, sp.Date_, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.BranchID })
                   .Join<PaymentPlans>
                    (
                        pp => new { PaymentPlanName = pp.Name },
                        nameof(SalesPropositions.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositions.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositions.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesPropositions.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPropositions.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<ShippingAdresses>
                    (
                        sa => new { ShippingAdressCode = sa.Code },
                        nameof(SalesPropositions.ShippingAdressID),
                        nameof(ShippingAdresses.Id),
                        JoinType.Left
                    )
                 .Where(new { FicheNo = input.FicheNo }, Tables.SalesPropositions);

            var list = queryFactory.GetList<ListSalesPropositionsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
            {
                LinkedSalesPropositionID = input.LinkedSalesPropositionID,
                SalesPropositionState = input.SalesPropositionState,
                FicheNo = input.FicheNo,
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                TransactionExchangeCurrencyID = input.TransactionExchangeCurrencyID.GetValueOrDefault(),
                ExchangeRate = input.ExchangeRate,
                GrossAmount = input.GrossAmount,
                NetAmount = input.NetAmount,
                PaymentPlanID = input.PaymentPlanID.GetValueOrDefault(),
                PropositionRevisionNo = input.PropositionRevisionNo,
                RevisionDate = input.RevisionDate,
                RevisionTime = input.RevisionTime,
                TransactionExchangeGrossAmount = input.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = input.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = input.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = input.TransactionExchangeTotalVatAmount,
                TransactionExchangeTotalVatExcludedAmount = input.TransactionExchangeTotalVatExcludedAmount,
                ShippingAdressID = input.ShippingAdressID.GetValueOrDefault(),
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
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
                PricingCurrency = input.PricingCurrency
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectSalesPropositionLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Insert(new CreateSalesPropositionLinesDto
                    {
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                        TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                        TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                        TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                        TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineTotalAmount = item.LineTotalAmount,
                        OrderConversionDate = item.OrderConversionDate,
                        PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                        SalesPropositionLineState = (int)item.SalesPropositionLineState,
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        SalesPropositionID = input.Id,
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
                        WarehouseID = input.WarehouseID.GetValueOrDefault()
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.SalesPropositionLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectSalesPropositionLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Update(new UpdateSalesPropositionLinesDto
                        {
                            DiscountAmount = item.DiscountAmount,
                            DiscountRate = item.DiscountRate,
                            TransactionExchangeVATamount = item.TransactionExchangeVATamount,
                            TransactionExchangeUnitPrice = item.TransactionExchangeUnitPrice,
                            TransactionExchangeLineTotalAmount = item.TransactionExchangeLineTotalAmount,
                            TransactionExchangeLineAmount = item.TransactionExchangeLineAmount,
                            TransactionExchangeDiscountAmount = item.TransactionExchangeDiscountAmount,
                            ExchangeRate = item.ExchangeRate,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            LineTotalAmount = item.LineTotalAmount,
                            OrderConversionDate = item.OrderConversionDate,
                            PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                            SalesPropositionLineState = (int)item.SalesPropositionLineState,
                            UnitPrice = item.UnitPrice,
                            VATamount = item.VATamount,
                            VATrate = item.VATrate,
                            SalesPropositionID = input.Id,
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
                            Quantity = item.Quantity,
                            UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            BranchID = input.BranchID.GetValueOrDefault(),
                            CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                            Date_ = input.Date_,
                            WarehouseID = input.WarehouseID.GetValueOrDefault()
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);

            StockMovementsService.UpdateSalesPropositions(entity, input);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Update, salesProposition.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["SalesPropositionsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                            Message_ = notTemplate.Message_,
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
            return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);

        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<SalesPropositions>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
            {
                FicheNo = entity.FicheNo,
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                ExchangeRate = entity.ExchangeRate,
                GrossAmount = entity.GrossAmount,
                LinkedSalesPropositionID = entity.LinkedSalesPropositionID,
                NetAmount = entity.NetAmount,
                PaymentPlanID = entity.PaymentPlanID,
                PropositionRevisionNo = entity.PropositionRevisionNo,
                SalesPropositionState = (int)entity.SalesPropositionState,
                RevisionDate = entity.RevisionDate,
                RevisionTime = entity.RevisionTime,
                ShippingAdressID = entity.ShippingAdressID,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var salesPropositionDto = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPropositionsDto>(salesPropositionDto);

        }

        public async Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState)
        {
            foreach (var item in orderLineList)
            {
                var entity = (await GetAsync(item.LinkedSalesPropositionID.GetValueOrDefault())).Data;

                DateTime now = _GetSQLDateAppService.GetDateFromSQL();

                var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
                {
                    FicheNo = entity.FicheNo,
                    BranchID = entity.BranchID,
                    CurrencyID = entity.CurrencyID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    TransactionExchangeTotalVatAmount = entity.TransactionExchangeTotalVatAmount,
                    TransactionExchangeTotalDiscountAmount = entity.TransactionExchangeTotalDiscountAmount,
                    TransactionExchangeNetAmount = entity.TransactionExchangeNetAmount,
                    TransactionExchangeGrossAmount = entity.TransactionExchangeGrossAmount,
                    TransactionExchangeTotalVatExcludedAmount = entity.TransactionExchangeTotalVatExcludedAmount,
                    TransactionExchangeCurrencyID = entity.TransactionExchangeCurrencyID.GetValueOrDefault(),
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
                    ExchangeRate = entity.ExchangeRate,
                    GrossAmount = entity.GrossAmount,
                    LinkedSalesPropositionID = entity.LinkedSalesPropositionID,
                    NetAmount = entity.NetAmount,
                    PaymentPlanID = entity.PaymentPlanID,
                    PropositionRevisionNo = entity.PropositionRevisionNo,
                    SalesPropositionState = (int)entity.SalesPropositionState,
                    RevisionDate = entity.RevisionDate,
                    RevisionTime = entity.RevisionTime,
                    ShippingAdressID = entity.ShippingAdressID.GetValueOrDefault(),
                    SpecialCode = entity.SpecialCode,
                    Time_ = entity.Time_,
                    TotalDiscountAmount = entity.TotalDiscountAmount,
                    TotalVatAmount = entity.TotalVatAmount,
                    TotalVatExcludedAmount = entity.TotalVatExcludedAmount,
                    ValidityDate_ = entity.ValidityDate_.GetValueOrDefault(),
                    WarehouseID = entity.WarehouseID,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = entity.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime =now,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = entity.Id }, "");

                if (entity.SelectSalesPropositionLines.Count > 0)
                {
                    foreach (var line in entity.SelectSalesPropositionLines)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Update(new UpdateSalesPropositionLinesDto
                        {
                            DiscountAmount = line.DiscountAmount,
                            DiscountRate = line.DiscountRate,
                            ExchangeRate = line.ExchangeRate,
                            TransactionExchangeDiscountAmount = line.TransactionExchangeDiscountAmount,
                            TransactionExchangeLineAmount = line.TransactionExchangeLineAmount,
                            TransactionExchangeLineTotalAmount = line.TransactionExchangeLineTotalAmount,
                            TransactionExchangeUnitPrice = line.TransactionExchangeUnitPrice,
                            TransactionExchangeVATamount = line.TransactionExchangeVATamount,
                            LineAmount = line.LineAmount,
                            LineDescription = line.LineDescription,
                            LineTotalAmount = line.LineTotalAmount,
                            OrderConversionDate =now,
                            PaymentPlanID = line.PaymentPlanID.GetValueOrDefault(),
                            SalesPropositionLineState = (int)lineState,
                            UnitPrice = line.UnitPrice,
                            VATamount = line.VATamount,
                            VATrate = line.VATrate,
                            SalesPropositionID = line.SalesPropositionID,
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
                            ProductID = line.ProductID.GetValueOrDefault(),
                            Quantity = line.Quantity,
                            UnitSetID = line.UnitSetID.GetValueOrDefault(),
                            BranchID = line.BranchID,
                            CurrentAccountCardID = line.CurrentAccountCardID,
                            Date_ = line.Date_,
                            WarehouseID = line.WarehouseID
                        }).Where(new { Id = line.Id },  "");
                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                    }
                }

                var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);
            }
        }

        public async Task<IDataResult<IList<SelectSalesPropositionLinesDto>>> GetLineListAsync()
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPropositionLines)
                   .Select<SalesPropositionLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPropositionLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(SalesPropositionLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                     .Join<PaymentPlans>
                    (
                        ppl => new { PaymentPlanID = ppl.Id, PaymentPlanName = ppl.Name },
                        nameof(SalesPropositionLines.PaymentPlanID),
                        nameof(PaymentPlans.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code, BranchName = b.Name },
                        nameof(SalesPropositionLines.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseName = w.Name, WarehouseCode = w.Code },
                        nameof(SalesPropositionLines.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesPropositionLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.SalesPropositionLines);

            var salesPropositionLine = queryFactory.GetList<SelectSalesPropositionLinesDto>(queryLines).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<List<SelectSalesPropositionLinesDto>>(salesPropositionLine);
        }
    }

}
