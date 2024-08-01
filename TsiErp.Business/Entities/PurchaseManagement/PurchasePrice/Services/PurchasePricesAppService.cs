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
using TsiErp.Business.Entities.PurchasePrice.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchasePrices.Page;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    [ServiceRegistration(typeof(IPurchasePricesAppService), DependencyInjectionType.Scoped)]
    public class PurchasePricesAppService : ApplicationService<PurchasePricesResource>, IPurchasePricesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PurchasePricesAppService(IStringLocalizer<PurchasePricesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> CreateAsync(CreatePurchasePricesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchasePrices).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<PurchasePrices>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PurchasePrices).Insert(new CreatePurchasePricesDto
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

            foreach (var item in input.SelectPurchasePriceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Insert(new CreatePurchasePriceLinesDto
                {
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                    CurrencyID = item.CurrencyID.GetValueOrDefault(),
                    SupplyDateDay = item.SupplyDateDay,
                    Linenr = item.Linenr,
                    Price = item.Price,
                    PurchasePriceID = addedEntityId,
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

            var purchasePrice = queryFactory.Insert<SelectPurchasePricesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchasePricesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PurchasePrices).Select("*").Where(new { Id = id }, true, true, "");

            var purchasePrices = queryFactory.Get<SelectPurchasePricesDto>(query);

            if (purchasePrices.Id != Guid.Empty && purchasePrices != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PurchasePrices).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PurchasePriceLines).Delete(LoginedUserService.UserId).Where(new { PurchasePriceID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var purchasePrice = queryFactory.Update<SelectPurchasePricesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var billOfMaterialLines = queryFactory.Update<SelectPurchasePriceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchasePriceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPurchasePriceLinesDto>(billOfMaterialLines);
            }

        }

        public async Task<IDataResult<SelectPurchasePricesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, true, true, Tables.PurchasePrices);

            var purchasePrices = queryFactory.Get<SelectPurchasePricesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )

                    .Join<CurrentAccountCards>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                    .Where(new { PurchasePriceID = id }, false, false, Tables.PurchasePriceLines);

            var purchasePriceLine = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            purchasePrices.SelectPurchasePriceLines = purchasePriceLine;

            LogsAppService.InsertLogToDatabase(purchasePrices, purchasePrices, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrices);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchasePricesDto>>> GetListAsync(ListPurchasePricesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, true, true, Tables.PurchasePrices);

            var purchasePrices = queryFactory.GetList<ListPurchasePricesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchasePricesDto>>(purchasePrices);

        }

        [ValidationAspect(typeof(UpdatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateAsync(UpdatePurchasePricesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, true, true, Tables.PurchasePrices);

            var entity = queryFactory.Get<SelectPurchasePricesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchasePriceID = input.Id }, false, false, Tables.PurchasePriceLines);

            var purchasePriceLines = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            entity.SelectPurchasePriceLines = purchasePriceLines;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Code = input.Code }, false, false, Tables.PurchasePrices);

            var list = queryFactory.GetList<ListPurchasePricesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PurchasePrices).Update(new UpdatePurchasePricesDto
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
            }).Where(new { Id = input.Id }, true, true, "");

            foreach (var item in input.SelectPurchasePriceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Insert(new CreatePurchasePriceLinesDto
                    {
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                        CurrencyID = item.CurrencyID.GetValueOrDefault(),
                        Linenr = item.Linenr,
                        SupplyDateDay = item.SupplyDateDay,
                        Price = item.Price,
                        PurchasePriceID = input.Id,
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
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchasePriceLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectPurchasePriceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Update(new UpdatePurchasePriceLinesDto
                        {
                            StartDate = item.StartDate,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            EndDate = item.EndDate,
                            SupplyDateDay = item.SupplyDateDay,
                            CurrentAccountCardID = item.CurrentAccountCardID.GetValueOrDefault(),
                            CurrencyID = item.CurrencyID.GetValueOrDefault(),
                            Linenr = item.Linenr,
                            Price = item.Price,
                            PurchasePriceID = input.Id,
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
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var purchasePrice = queryFactory.Update<SelectPurchasePricesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);

        }

        public async Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productId }, false, false, Tables.PurchasePriceLines);

            var purchasePriceLine = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectPurchasePriceLinesDto>>(purchasePriceLine);

        }

        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchasePrices).Select("*").Where(new { Id = id }, true, true, "");

            var entity = queryFactory.Get<PurchasePrices>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchasePrices).Update(new UpdatePurchasePricesDto
            {
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                EndDate = entity.EndDate,
                IsApproved = entity.IsApproved,
                StartDate = entity.StartDate,
                WarehouseID = entity.WarehouseID,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, true, true, "");

            var purchasePricesDto = queryFactory.Update<SelectPurchasePricesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePricesDto);

        }
    }
}
