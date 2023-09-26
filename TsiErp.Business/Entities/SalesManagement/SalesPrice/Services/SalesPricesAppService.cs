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

        public SalesPricesAppService(IStringLocalizer<SalesPricesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }


        [ValidationAspect(typeof(CreateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> CreateAsync(CreateSalesPricesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<SalesPrices>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.SalesPrices).Insert(new CreateSalesPricesDto
                {
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    EndDate = input.EndDate,
                    IsApproved = input.IsApproved,
                    StartDate = input.StartDate,
                    WarehouseID = input.WarehouseID,
                    Code = input.Code,
                    CreationTime = DateTime.Now,
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
                        CurrentAccountCardID = item.CurrentAccountCardID,
                        CurrencyID = item.CurrencyID,
                        Linenr = item.Linenr,
                        Price = item.Price,
                        SalesPriceID = addedEntityId,
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
                        ProductID = item.ProductID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var salesPrice = queryFactory.Insert<SelectSalesPricesDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("SalesPricesChildMenu", input.Code);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesPrices, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Id = id }, true, true, "");

                var salesPrices = queryFactory.Get<SelectSalesPricesDto>(query);

                if (salesPrices.Id != Guid.Empty && salesPrices != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.SalesPrices).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.SalesPriceLines).Delete(LoginedUserService.UserId).Where(new { SalesPriceID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var salesPrice = queryFactory.Update<SelectSalesPricesDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPrices, LogType.Delete, id);
                    return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var salesPriceLines = queryFactory.Update<SelectSalesPriceLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.SalesPriceLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectSalesPriceLinesDto>(salesPriceLines);
                }
            }
        }

        public async Task<IDataResult<SelectSalesPricesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.SalesPrices)
                       .Select<SalesPrices>(sp=> new { sp.WarehouseID, sp.StartDate, sp.Name, sp.IsApproved, sp.IsActive, sp.Id, sp.EndDate, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.Code, sp.BranchID })
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
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name , CustomerCode  = ca.CustomerCode},
                            nameof(SalesPrices.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.SalesPrices);

                var salesPrices = queryFactory.Get<SelectSalesPricesDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.SalesPriceLines)
                       .Select<SalesPriceLines>(spl=> new { spl.StartDate, spl.SalesPriceID, spl.ProductID, spl.Price, spl.Linenr, spl.Id, spl.EndDate, spl.DataOpenStatusUserId, spl.DataOpenStatus, spl.CurrentAccountCardID, spl.CurrencyID })
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
                        .Where(new { SalesPriceID = id }, false, false, Tables.SalesPriceLines);

                var salesPriceLine = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

                salesPrices.SelectSalesPriceLines = salesPriceLine;

                LogsAppService.InsertLogToDatabase(salesPrices, salesPrices, LoginedUserService.UserId, Tables.SalesPrices, LogType.Get, id);

                return new SuccessDataResult<SelectSalesPricesDto>(salesPrices);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPricesDto>>> GetListAsync(ListSalesPricesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                        .From(Tables.SalesPrices)
                       .Select<SalesPrices>(sp => new { sp.WarehouseID, sp.StartDate, sp.Name, sp.IsApproved, sp.IsActive, sp.Id, sp.EndDate, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.Code, sp.BranchID })
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
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name, CustomerCode = ca.CustomerCode },
                            nameof(SalesPrices.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.SalesPrices);

                var salesPrices = queryFactory.GetList<ListSalesPricesDto>(query).ToList();
                return new SuccessDataResult<IList<ListSalesPricesDto>>(salesPrices);
            }
        }

        [ValidationAspect(typeof(UpdateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> UpdateAsync(UpdateSalesPricesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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
                        .Where(new { Id = input.Id }, true, true, Tables.SalesPrices);

                var entity = queryFactory.Get<SelectSalesPricesDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.SalesPriceLines)
                       .Select<SalesPriceLines>(spl => new { spl.StartDate, spl.SalesPriceID, spl.ProductID, spl.Price, spl.Linenr, spl.Id, spl.EndDate, spl.DataOpenStatusUserId, spl.DataOpenStatus, spl.CurrentAccountCardID, spl.CurrencyID })
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
                        .Where(new { SalesPriceID = input.Id }, false, false, Tables.SalesPriceLines);

                var salesPriceLines = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

                entity.SelectSalesPriceLines = salesPriceLines;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                                .From(Tables.SalesPrices)
                       .Select<SalesPrices>(sp => new { sp.WarehouseID, sp.StartDate, sp.Name, sp.IsApproved, sp.IsActive, sp.Id, sp.EndDate, sp.DataOpenStatusUserId, sp.DataOpenStatus, sp.CurrentAccountCardID, sp.CurrencyID, sp.Code, sp.BranchID })
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
                            ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(SalesPrices.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Code = input.Code }, false, false, Tables.SalesPrices);

                var list = queryFactory.GetList<ListSalesPricesDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.SalesPrices).Update(new UpdateSalesPricesDto
                {
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    EndDate = input.EndDate,
                    IsApproved = input.IsApproved,
                    StartDate = input.StartDate,
                    WarehouseID = input.WarehouseID,
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
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                    Name = input.Name,
                }).Where(new { Id = input.Id }, true, true, "");

                foreach (var item in input.SelectSalesPriceLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Insert(new CreateSalesPriceLinesDto
                        {
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CurrentAccountCardID = item.CurrentAccountCardID,
                            CurrencyID = item.CurrencyID,
                            Linenr = item.Linenr,
                            Price = item.Price,
                            SalesPriceID = input.Id,
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
                            ProductID = item.ProductID,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.SalesPriceLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectSalesPriceLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.SalesPriceLines).Update(new UpdateSalesPriceLinesDto
                            {
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                CurrentAccountCardID = item.CurrentAccountCardID,
                                CurrencyID = item.CurrencyID,
                                ProductID = item.ProductID,
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
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var salesPrice = queryFactory.Update<SelectSalesPricesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.SalesPrices, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectSalesPricesDto>(salesPrice);
            }
        }

        public async Task<IDataResult<IList<SelectSalesPriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var queryLines = queryFactory
                       .Query()
                       .From(Tables.SalesPriceLines)
                       .Select<SalesPriceLines>(spl => new { spl.StartDate, spl.SalesPriceID, spl.ProductID, spl.Price, spl.Linenr, spl.Id, spl.EndDate, spl.DataOpenStatusUserId, spl.DataOpenStatus, spl.CurrentAccountCardID, spl.CurrencyID })
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
                        .Where(new { ProductID = productId }, false, false, Tables.SalesPriceLines);

                var salesPriceLine = queryFactory.GetList<SelectSalesPriceLinesDto>(queryLines).ToList();

                return new SuccessDataResult<IList<SelectSalesPriceLinesDto>>(salesPriceLine);
            }
        }

        public async Task<IDataResult<SelectSalesPricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.SalesPrices).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<SalesPrices>(entityQuery);

                var query = queryFactory.Query().From(Tables.SalesPrices).Update(new UpdateSalesPricesDto
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
                }).Where(new { Id = id }, true, true, "");

                var salesPricesDto = queryFactory.Update<SelectSalesPricesDto>(query, "Id", true);
                return new SuccessDataResult<SelectSalesPricesDto>(salesPricesDto);

            }
        }
    }
}
