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
using TsiErp.Business.Entities.SalesProposition.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.StockManagement.Product;
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

        public SalesPropositionsAppService(IStringLocalizer<SalesPropositionsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> CreateAsync(CreateSalesPropositionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.ControlList<SalesPropositions>(listQuery).ToList();

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

                var query = queryFactory.Query().From(Tables.SalesPropositions).Insert(new CreateSalesPropositionsDto
                {
                    LinkedSalesPropositionID = Guid.Empty,
                    SalesPropositionState = input.SalesPropositionState,
                    FicheNo = input.FicheNo,
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    GrossAmount = input.GrossAmount,
                    NetAmount = input.NetAmount,
                    PaymentPlanID = input.PaymentPlanID,
                    PropositionRevisionNo = input.PropositionRevisionNo,
                    RevisionDate = input.RevisionDate,
                    RevisionTime = input.RevisionTime,
                    ShippingAdressID = input.ShippingAdressID,
                    SpecialCode = input.SpecialCode,
                    Time_ = time,
                    TotalDiscountAmount = input.TotalDiscountAmount,
                    TotalVatAmount = input.TotalVatAmount,
                    TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                    ValidityDate_ = input.ValidityDate_,
                    WarehouseID = input.WarehouseID,
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

                foreach (var item in input.SelectSalesPropositionLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Insert(new CreateSalesPropositionLinesDto
                    {
                        SalesPropositionLineState = (int)item.SalesPropositionLineState,
                        DiscountAmount = item.DiscountAmount,
                        DiscountRate = item.DiscountRate,
                        ExchangeRate = item.ExchangeRate,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        LineTotalAmount = item.LineTotalAmount,
                        OrderConversionDate = item.OrderConversionDate,
                        PaymentPlanID = item.PaymentPlanID.GetValueOrDefault(),
                        UnitPrice = item.UnitPrice,
                        VATamount = item.VATamount,
                        VATrate = item.VATrate,
                        SalesPropositionID = addedEntityId,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var salesProposition = queryFactory.Insert<SelectSalesPropositionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { Id = id }, false, false, "");

                var salesPropositions = queryFactory.Get<SelectSalesPropositionsDto>(query);

                if (salesPropositions.Id != Guid.Empty && salesPropositions != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.SalesPropositions).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.SalesPropositionLines).Delete(LoginedUserService.UserId).Where(new { SalesPropositionID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Delete, id);
                    return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var salesPropositionLines = queryFactory.Update<SelectSalesPropositionLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPropositionLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectSalesPropositionLinesDto>(salesPropositionLines);
                }
            }
        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.SalesPropositions)
                       .Select<SalesPropositions>(sp => new { sp.WarehouseID, sp.ValidityDate_, sp.TotalVatExcludedAmount, sp.TotalVatAmount, sp.TotalDiscountAmount, sp.Time_, sp.SpecialCode, sp.ShippingAdressID, sp.RevisionTime, sp.RevisionDate, sp.SalesPropositionState, sp.PropositionRevisionNo, sp.PaymentPlanID, sp.NetAmount, sp.LinkedSalesPropositionID, sp.Id, sp.GrossAmount, sp.FicheNo, sp.ExchangeRate, sp.Description_, sp.Date_, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.BranchID })
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
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
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
                        .Where(new { Id = id }, false, false, Tables.SalesPropositions);

                var salesPropositions = queryFactory.Get<SelectSalesPropositionsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.SalesPropositionLines)
                       .Select<SalesPropositionLines>(spl => new { spl.VATrate, spl.VATamount, spl.UnitSetID, spl.UnitPrice, spl.Quantity, spl.SalesPropositionLineState, spl.SalesPropositionID, spl.ProductID, spl.PaymentPlanID, spl.OrderConversionDate, spl.LineTotalAmount, spl.LineNr, spl.LineDescription, spl.LineAmount, spl.Id, spl.ExchangeRate, spl.DiscountRate, spl.DiscountAmount, spl.DataOpenStatusUserId, spl.DataOpenStatus })
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
                        .Where(new { SalesPropositionID = id }, false, false, Tables.SalesPropositionLines);

                var salesPropositionLine = queryFactory.GetList<SelectSalesPropositionLinesDto>(queryLines).ToList();

                salesPropositions.SelectSalesPropositionLines = salesPropositionLine;

                LogsAppService.InsertLogToDatabase(salesPropositions, salesPropositions, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Get, id);

                return new SuccessDataResult<SelectSalesPropositionsDto>(salesPropositions);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPropositionsDto>>> GetListAsync(ListSalesPropositionsParamaterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
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
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
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
                        .Where(null, false, false, Tables.SalesPropositions);

                var salesPropositions = queryFactory.GetList<ListSalesPropositionsDto>(query).ToList();
                return new SuccessDataResult<IList<ListSalesPropositionsDto>>(salesPropositions);
            }
        }

        [ValidationAspect(typeof(UpdateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateAsync(UpdateSalesPropositionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                     .From(Tables.SalesPropositions)
                       .Select<SalesPropositions>(sp => new { sp.WarehouseID, sp.ValidityDate_, sp.TotalVatExcludedAmount, sp.TotalVatAmount, sp.TotalDiscountAmount, sp.Time_, sp.SpecialCode, sp.ShippingAdressID, sp.RevisionTime, sp.RevisionDate, sp.SalesPropositionState, sp.PropositionRevisionNo, sp.PaymentPlanID, sp.NetAmount, sp.LinkedSalesPropositionID, sp.Id, sp.GrossAmount, sp.FicheNo, sp.ExchangeRate, sp.Description_, sp.Date_, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.BranchID })
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
                        .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
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
                        .Where(new { Id = input.Id }, false, false, Tables.SalesPropositions);

                var entity = queryFactory.Get<SelectSalesPropositionsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.SalesPropositionLines)
                       .Select<SalesPropositionLines>(spl => new { spl.VATrate, spl.VATamount, spl.UnitSetID, spl.UnitPrice, spl.Quantity, spl.SalesPropositionLineState, spl.SalesPropositionID, spl.ProductID, spl.PaymentPlanID, spl.OrderConversionDate, spl.LineTotalAmount, spl.LineNr, spl.LineDescription, spl.LineAmount, spl.Id, spl.ExchangeRate, spl.DiscountRate, spl.DiscountAmount, spl.DataOpenStatusUserId, spl.DataOpenStatus })
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
                        .Where(new { SalesPropositionID = input.Id }, false, false, Tables.SalesPropositionLines);

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
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
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
                     .Where(new { FicheNo = input.FicheNo }, false, false, Tables.SalesPropositions);

                var list = queryFactory.GetList<ListSalesPropositionsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNo != input.FicheNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
                {
                    LinkedSalesPropositionID = input.LinkedSalesPropositionID,
                    SalesPropositionState = input.SalesPropositionState,
                    FicheNo = input.FicheNo,
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    GrossAmount = input.GrossAmount,
                    NetAmount = input.NetAmount,
                    PaymentPlanID = input.PaymentPlanID,
                    PropositionRevisionNo = input.PropositionRevisionNo,
                    RevisionDate = input.RevisionDate,
                    RevisionTime = input.RevisionTime,
                    ShippingAdressID = input.ShippingAdressID,
                    SpecialCode = input.SpecialCode,
                    Time_ = input.Time_,
                    TotalDiscountAmount = input.TotalDiscountAmount,
                    TotalVatAmount = input.TotalVatAmount,
                    TotalVatExcludedAmount = input.TotalVatExcludedAmount,
                    ValidityDate_ = input.ValidityDate_,
                    WarehouseID = input.WarehouseID,
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

                foreach (var item in input.SelectSalesPropositionLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Insert(new CreateSalesPropositionLinesDto
                        {
                            DiscountAmount = item.DiscountAmount,
                            DiscountRate = item.DiscountRate,
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
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.SalesPropositionLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectSalesPropositionLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Update(new UpdateSalesPropositionLinesDto
                            {
                                DiscountAmount = item.DiscountAmount,
                                DiscountRate = item.DiscountRate,
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
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = item.LineNr,
                                ProductID = item.ProductID.GetValueOrDefault(),
                                Quantity = item.Quantity,
                                UnitSetID = item.UnitSetID.GetValueOrDefault(),
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesPropositions, LogType.Update, salesProposition.Id);

                return new SuccessDataResult<SelectSalesPropositionsDto>(salesProposition);
            }
        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.SalesPropositions).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<SalesPropositions>(entityQuery);

                var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
                {
                    FicheNo = entity.FicheNo,
                    BranchID = entity.BranchID,
                    CurrencyID = entity.CurrencyID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
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
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                }).Where(new { Id = id }, false, false, "");

                var salesPropositionDto = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);
                return new SuccessDataResult<SelectSalesPropositionsDto>(salesPropositionDto);

            }
        }

        public async Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                foreach (var item in orderLineList)
                {
                    var entity = (await GetAsync(item.LinkedSalesPropositionID.GetValueOrDefault())).Data;

                    var query = queryFactory.Query().From(Tables.SalesPropositions).Update(new UpdateSalesPropositionsDto
                    {
                        FicheNo = entity.FicheNo,
                        BranchID = entity.BranchID,
                        CurrencyID = entity.CurrencyID,
                        CurrentAccountCardID = entity.CurrentAccountCardID,
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
                        ShippingAdressID = entity.ShippingAdressID,
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
                        LastModificationTime = DateTime.Now,
                        LastModifierId = LoginedUserService.UserId,
                    }).Where(new { Id = entity.Id }, false, false, "");

                    if (entity.SelectSalesPropositionLines.Count > 0)
                    {
                        foreach (var line in entity.SelectSalesPropositionLines)
                        {
                            var queryLine = queryFactory.Query().From(Tables.SalesPropositionLines).Update(new UpdateSalesPropositionLinesDto
                            {
                                DiscountAmount = line.DiscountAmount,
                                DiscountRate = line.DiscountRate,
                                ExchangeRate = line.ExchangeRate,
                                LineAmount = line.LineAmount,
                                LineDescription = line.LineDescription,
                                LineTotalAmount = line.LineTotalAmount,
                                OrderConversionDate = DateTime.Now,
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
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = line.LineNr,
                                ProductID = line.ProductID.GetValueOrDefault(),
                                Quantity = line.Quantity,
                                UnitSetID = line.UnitSetID.GetValueOrDefault(),
                            }).Where(new { Id = line.Id }, false, false, "");
                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                        }
                    }

                    var salesProposition = queryFactory.Update<SelectSalesPropositionsDto>(query, "Id", true);
                }
            }

        }
    }

}
