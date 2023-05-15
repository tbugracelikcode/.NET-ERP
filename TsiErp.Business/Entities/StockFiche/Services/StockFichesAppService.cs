using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockFiche.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockFiche;
using TsiErp.Entities.Entities.StockFiche;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.StockFicheLine;
using TsiErp.Entities.Entities.StockFicheLine.Dtos;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Entities.StockFiche.Validations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Tsi.Core.Entities;

namespace TsiErp.Business.Entities.StockFiche.Services
{
    [ServiceRegistration(typeof(IStockFichesAppService), DependencyInjectionType.Scoped)]
    public class StockFichesAppService : ApplicationService<StockFichesResource>, IStockFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public StockFichesAppService(IStringLocalizer<StockFichesResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateStockFichesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockFichesDto>> CreateAsync(CreateStockFichesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.ControlList<StockFiches>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.StockFiches).Insert(new CreateStockFichesDto
                {
                    FicheNo = input.FicheNo,
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
                    CurrencyID = input.CurrencyID,
                    BranchID = input.BranchID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    FicheType = input.FicheType,
                    NetAmount = input.NetAmount,
                    ProductionOrderID = input.ProductionOrderID,
                    SpecialCode = input.SpecialCode,
                    Time_ = input.Time_,
                    WarehouseID = input.WarehouseID
                });

                foreach (var item in input.SelectStockFicheLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Insert(new CreateStockFicheLinesDto
                    {
                        StockFicheID = addedEntityId,
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
                        FicheType = item.FicheType,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        UnitPrice = item.UnitPrice
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var stockFiche = queryFactory.Insert<SelectStockFichesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Insert, stockFiche.Id);

                return new SuccessDataResult<SelectStockFichesDto>(stockFiche);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = id }, false, false, "");

                var StockFiches = queryFactory.Get<SelectStockFichesDto>(query);

                if (StockFiches.Id != Guid.Empty && StockFiches != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.StockFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.StockFicheLines).Delete(LoginedUserService.UserId).Where(new { StockFicheID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var stockFiche = queryFactory.Update<SelectStockFichesDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockFiches, LogType.Delete, id);
                    return new SuccessDataResult<SelectStockFichesDto>(stockFiche);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var stockFicheLines = queryFactory.Update<SelectStockFicheLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockFicheLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectStockFicheLinesDto>(stockFicheLines);
                }
            }
        }

        public async Task<IDataResult<SelectStockFichesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.StockFiches)
                       .Select<StockFiches>(b => new { b.Id, b.FicheNo, b.Date_, b.Description_, b.FicheType, b.NetAmount })
                       .Join<Branches>
                        (
                            pr => new { BranchCode = pr.Code },
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            pr => new { WarehouseCode = pr.Code },
                            nameof(StockFiches.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.StockFiches);

                var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StockFicheLines)
                       .Select<StockFicheLines>(b => new
                       {
                           b.Id,
                           b.StockFicheID,
                           b.LineNr,
                           b.ProductID,
                           b.UnitSetID,
                           b.Quantity,
                           b.UnitPrice,
                           b.LineAmount,
                           b.LineDescription,
                           b.FicheType,
                           b.CreatorId,
                           b.CreationTime,
                           b.LastModifierId,
                           b.LastModificationTime,
                           b.DeleterId,
                           b.DeletionTime,
                           b.IsDeleted,
                           b.DataOpenStatus,
                           b.DataOpenStatusUserId
                       })
                       .Join<Products>
                        (
                            p => new { ProductCode = p.Code, ProductName = p.Name },
                            nameof(StockFicheLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(StockFicheLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { StockFicheID = id }, false, false, Tables.StockFicheLines);

                var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

                stockFiches.SelectStockFicheLines = stockFicheLine;

                LogsAppService.InsertLogToDatabase(stockFiches, stockFiches, LoginedUserService.UserId, Tables.StockFiches, LogType.Get, id);

                return new SuccessDataResult<SelectStockFichesDto>(stockFiches);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStockFichesDto>>> GetListAsync(ListStockFichesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.StockFiches)
                       .Select<StockFiches>(b => new { b.FicheNo, b.Date_, b.Description_, b.FicheType, b.NetAmount })
                       .Join<Branches>
                        (
                            pr => new { BranchCode = pr.Code },
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            pr => new { WarehouseCode = pr.Code },
                            nameof(StockFiches.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(null, false, false, Tables.StockFiches);

                var stockFichesDto = queryFactory.GetList<ListStockFichesDto>(query).ToList();
                return new SuccessDataResult<IList<ListStockFichesDto>>(stockFichesDto);
            }
        }

        [ValidationAspect(typeof(UpdateStockFichesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStockFichesDto>> UpdateAsync(UpdateStockFichesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.StockFiches)
                       .Select<StockFiches>(b => new { b.Id, b.FicheNo, b.Date_, b.Description_, b.FicheType, b.NetAmount })
                       .Join<Branches>
                        (
                            pr => new { BranchCode = pr.Code },
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            pr => new { WarehouseCode = pr.Code },
                            nameof(StockFiches.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.StockFiches);

                var entity = queryFactory.Get<SelectStockFichesDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StockFicheLines)
                       .Select<StockFicheLines>(b => new
                       {
                           b.Id,
                           b.StockFicheID,
                           b.LineNr,
                           b.ProductID,
                           b.UnitSetID,
                           b.Quantity,
                           b.UnitPrice,
                           b.LineAmount,
                           b.LineDescription,
                           b.FicheType,
                           b.CreatorId,
                           b.CreationTime,
                           b.LastModifierId,
                           b.LastModificationTime,
                           b.DeleterId,
                           b.DeletionTime,
                           b.IsDeleted,
                           b.DataOpenStatus,
                           b.DataOpenStatusUserId
                       })
                       .Join<Products>
                        (
                            p => new { ProductCode = p.Code, ProductName = p.Name },
                            nameof(StockFicheLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(StockFicheLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { StockFicheID = input.Id }, false, false, Tables.StockFicheLines);

                var stockFicheLine = queryFactory.GetList<SelectStockFicheLinesDto>(queryLines).ToList();

                entity.SelectStockFicheLines = stockFicheLine;

                #region Update Control
                var listQuery = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { FicheNo = input.FicheNo, FicheType = input.FicheType }, false, false, "");

                var list = queryFactory.GetList<StockFiches>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNo != input.FicheNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.StockFiches).Update(new UpdateStockFichesDto
                {
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
                    BranchID = input.BranchID,
                    CurrencyID = input.CurrencyID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    ExchangeRate = input.ExchangeRate,
                    FicheNo = input.FicheNo,
                    FicheType = input.FicheType,
                    ProductionOrderID = input.ProductionOrderID,
                    SpecialCode = input.SpecialCode,
                    Time_ = input.Time_,
                    WarehouseID = input.WarehouseID

                }).Where(new { Id = input.Id }, false, false, "");

                foreach (var item in input.SelectStockFicheLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Insert(new CreateStockFicheLinesDto
                        {
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
                            FicheType = item.FicheType,
                            LineAmount = item.LineAmount,
                            LineDescription = item.LineDescription,
                            StockFicheID = input.Id,
                            UnitPrice = item.UnitPrice
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.StockFicheLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectStockFicheLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.StockFicheLines).Update(new UpdateStockFicheLinesDto
                            {
                                StockFicheID = input.Id,
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
                                FicheType = item.FicheType,
                                LineAmount = item.LineAmount,
                                LineDescription = item.LineDescription,
                                UnitPrice = item.UnitPrice
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var stockFiche = queryFactory.Update<SelectStockFichesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Update, stockFiche.Id);

                return new SuccessDataResult<SelectStockFichesDto>(stockFiche);
            }
        }

        public async Task<IDataResult<SelectStockFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<StockFiches>(entityQuery);

                var query = queryFactory.Query().From(Tables.StockFiches).Update(new UpdateStockFichesDto
                {
                    FicheNo = entity.FicheNo,
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
                    BranchID = entity.BranchID,
                    CurrencyID = entity.CurrencyID,
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
                    ExchangeRate = entity.ExchangeRate,
                    FicheType = entity.FicheType,
                    ProductionOrderID = entity.ProductionOrderID,
                    SpecialCode = entity.SpecialCode,
                    Time_ = entity.Time_,
                    WarehouseID = entity.WarehouseID

                }).Where(new { Id = id }, false, false, "");

                var stockFichesDto = queryFactory.Update<SelectStockFichesDto>(query, "Id", true);
                return new SuccessDataResult<SelectStockFichesDto>(stockFichesDto);
            }
        }
    }
}
