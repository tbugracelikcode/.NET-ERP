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
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.SalesPrice.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesPrices.Page;

namespace TsiErp.Business.Entities.SalesPrice.Services
{
    [ServiceRegistration(typeof(ISalesPricesAppService), DependencyInjectionType.Scoped)]
    public class SalesPricesAppService : ApplicationService<SalesPricesResource>, ISalesPricesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public SalesPricesAppService(IStringLocalizer<SalesPricesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> CreateAsync(CreateSalesPricesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<SalesPrices>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.SalesPrices).Insert(new CreateSalesPricesDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                EndDate = input.EndDate,
                IsApproved = input.IsApproved,
                StartDate = input.StartDate,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
            });

            foreach (var item in input.SelectSalesPriceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Insert(new CreateSalesPriceLinesDto
                {
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    CurrencyID = item.CurrencyID.GetValueOrDefault(),
                    Linenr = item.Linenr,
                    Price = item.Price,
                    SalesPriceID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    ProductID = item.ProductID.GetValueOrDefault(),
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var salesPrice = queryFactory.Insert<SelectSalesPricesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("SalesPricesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesPrices, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Id = id },  "");

            var salesPrices = queryFactory.Get<SelectSalesPricesDto>(query);

            if (salesPrices.Id != Guid.Empty && salesPrices != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.SalesPrices).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.SalesPriceLines).Delete(LoginedUserService.UserId).Where(new { SalesPriceID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var salesPrice = queryFactory.Update<SelectSalesPricesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPrices, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
                var salesPriceLines = queryFactory.Update<SelectSalesPriceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPriceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectSalesPriceLinesDto>(salesPriceLines);
            }

        }

        public async Task<IDataResult<SelectSalesPricesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.SalesPrices)
                   .Select<SalesPrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesPrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(SalesPrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesPrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.SalesPrices);

            var salesPrices = queryFactory.Get<SelectSalesPricesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPriceLines)
                   .Select<SalesPriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(SalesPriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPriceID = id }, Tables.SalesPriceLines);

            var salesPriceLine = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

            salesPrices.SelectSalesPriceLines = salesPriceLine;

            LogsAppService.InsertLogToDatabase(salesPrices, salesPrices, LoginedUserService.UserId, Tables.SalesPrices, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPricesDto>(salesPrices);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPricesDto>>> GetListAsync(ListSalesPricesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.SalesPrices)
                   .Select<SalesPrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesPrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(SalesPrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesPrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.SalesPrices);

            var salesPrices = queryFactory.GetList<ListSalesPricesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListSalesPricesDto>>(salesPrices);

        }

        [ValidationAspect(typeof(UpdateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> UpdateAsync(UpdateSalesPricesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.SalesPrices)
                   .Select<SalesPrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(SalesPrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(SalesPrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(SalesPrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                        nameof(SalesPrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.SalesPrices);

            var entity = queryFactory.Get<SelectSalesPricesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPriceLines)
                   .Select<SalesPriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(SalesPriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPriceID = input.Id },  Tables.SalesPriceLines);

            var salesPriceLines = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

            entity.SelectSalesPriceLines = salesPriceLines;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.SalesPrices)
                   .Select<SalesPrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(SalesPrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(SalesPrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(SalesPrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(SalesPrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Code = input.Code }, Tables.SalesPrices);

            var list = queryFactory.GetList<ListSalesPricesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.SalesPrices).Update(new UpdateSalesPricesDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                EndDate = input.EndDate,
                IsApproved = input.IsApproved,
                StartDate = input.StartDate,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsActive = input.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectSalesPriceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Insert(new CreateSalesPriceLinesDto
                    {
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        CurrencyID = item.CurrencyID.GetValueOrDefault(),
                        Linenr = item.Linenr,
                        Price = item.Price,
                        SalesPriceID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        ProductID = item.ProductID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.SalesPriceLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectSalesPriceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Update(new UpdateSalesPriceLinesDto
                        {
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            CurrencyID = item.CurrencyID.GetValueOrDefault(),
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Linenr = item.Linenr,
                            Price = item.Price,
                            SalesPriceID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var salesPrice = queryFactory.Update<SelectSalesPricesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesPrices, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);

        }

        public async Task<IDataResult<IList<SelectSalesPriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPriceLines)
                   .Select<SalesPriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(SalesPriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productId },  Tables.SalesPriceLines);

            var salesPriceLine = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectSalesPriceLinesDto>>(salesPriceLine);

        }

        public async Task<IDataResult<SelectSalesPricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<SalesPrices>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesPrices).Update(new UpdateSalesPricesDto
            {
                BranchID = entity.BranchID.GetValueOrDefault(),
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID.GetValueOrDefault(),
                EndDate = entity.EndDate,
                IsApproved = entity.IsApproved,
                StartDate = entity.StartDate,
                WarehouseID = entity.WarehouseID.GetValueOrDefault(),
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var salesPricesDto = queryFactory.Update<SelectSalesPricesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPricesDto>(salesPricesDto);


        }

        public async Task<IDataResult<SelectSalesPricesDto>> GetbyCurrentAccountCurrencyDateAsync(Guid CurrentAccountID, Guid CurrencyID, DateTime LoadingDate)
        {
            var query = queryFactory
                  .Query()
                  .From(Tables.SalesPrices)
                  .Select<SalesPrices>(null)
                  .Join<Currencies>
                   (
                       c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                       nameof(SalesPrices.CurrencyID),
                       nameof(Currencies.Id),
                       JoinType.Left
                   )
                   .Join<Branches>
                   (
                       b => new { BranchID = b.Id, BranchCode = b.Code },
                       nameof(SalesPrices.BranchID),
                       nameof(Branches.Id),
                       JoinType.Left
                   )
                   .Join<Warehouses>
                   (
                       w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                       nameof(SalesPrices.WarehouseID),
                       nameof(Warehouses.Id),
                       JoinType.Left
                   )
                   .Join<CurrentAccountCards>
                   (
                       ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                       nameof(SalesPrices.CurrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left
                   )
                   .Where(new { CurrentAccountCardID = CurrentAccountID }, Tables.SalesPrices)
                   .Where(new { CurrencyID = CurrencyID }, Tables.SalesPrices);

            var salesPrices = queryFactory.GetList<SelectSalesPricesDto>(query).Where(t=>t.StartDate<=LoadingDate && t.EndDate >= LoadingDate).FirstOrDefault();

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.SalesPriceLines)
                   .Select<SalesPriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(SalesPriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(SalesPriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { SalesPriceID = salesPrices.Id },  Tables.SalesPriceLines);

            var salesPriceLine = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

            salesPrices.SelectSalesPriceLines = salesPriceLine;

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesPricesDto>(salesPrices);

        }
    }
}
