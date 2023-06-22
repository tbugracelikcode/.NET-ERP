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
using TsiErp.Business.Entities.StockFiche.Validations;
using TsiErp.Business.Entities.StockMovement;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Enums;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockFiche;

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

                if(input.FicheType != 25)
                {
                    switch (input.FicheType)
                    {
                        case 12: input.InputOutputCode = 1; break;
                        case 11: input.InputOutputCode = 1; break;
                        case 13: input.InputOutputCode = 0; break;
                        case 50: input.InputOutputCode = 0; break;
                        case 51: input.InputOutputCode = 1; break;
                    }
                }
               

                var query = queryFactory.Query().From(Tables.StockFiches).Insert(new CreateStockFichesDto
                {
                    FicheNo = input.FicheNo,
                    InputOutputCode = input.InputOutputCode,
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
                    ProductionOrderID = Guid.Empty,
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
                        FicheType = (int)item.FicheType,
                        LineAmount = item.LineAmount,
                        LineDescription = item.LineDescription,
                        UnitPrice = item.UnitPrice
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var stockFiche = queryFactory.Insert<SelectStockFichesDto>(query, "Id", true);

                #region Stock Movement Service

                switch (input.FicheType)
                {
                    case 12: StockMovementsService.InsertTotalWastages(input); ; break;
                    case 11: StockMovementsService.InsertTotalConsumptions(input); ; break;
                    case 13: StockMovementsService.InsertTotalProductions(input); ; break;
                    case 50: StockMovementsService.InsertTotalGoods(input); ; break;
                    case 51: StockMovementsService.InsertTotalGoodIssues(input); ; break;
                    case 25: StockMovementsService.InsertTotalWarehouseShippings(input); break;
                }

                #endregion

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Insert, addedEntityId);

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
                    #region Stock Movement Service

                    switch (StockFiches.FicheType)
                    {
                        case StockFicheTypeEnum.FireFisi: StockMovementsService.DeleteTotalWastages(StockFiches); ; break;
                        case StockFicheTypeEnum.SarfFisi: StockMovementsService.DeleteTotalConsumptions(StockFiches); ; break;
                        case StockFicheTypeEnum.UretimdenGirisFisi: StockMovementsService.DeleteTotalProductions(StockFiches); ; break;
                        case StockFicheTypeEnum.StokGirisFisi: StockMovementsService.DeleteTotalGoods(StockFiches); ; break;
                        case StockFicheTypeEnum.StokCikisFisi: StockMovementsService.DeleteTotalGoodIssues(StockFiches); ; break;
                        case StockFicheTypeEnum.DepoSevkFisi: StockMovementsService.DeleteTotalWarehouseShippings(StockFiches); break;
                    }

                    #endregion

                    var deleteQuery = queryFactory.Query().From(Tables.StockFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.StockFicheLines).Delete(LoginedUserService.UserId).Where(new { StockFicheID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var stockFiche = queryFactory.Update<SelectStockFichesDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StockFiches, LogType.Delete, id);

                    return new SuccessDataResult<SelectStockFichesDto>(stockFiche);
                }
                else
                {
                    var queryLineGet = queryFactory.Query().From(Tables.StockFicheLines).Select("*").Where(new { Id = id }, false, false, "");

                    var stockFichesLineGet = queryFactory.Get<SelectStockFicheLinesDto>(queryLineGet);

                    var queryDeleteEntity = queryFactory.Query().From(Tables.StockFiches).Select("*").Where(new { Id = stockFichesLineGet.StockFicheID }, false, false, "");

                    var StockFichesDeleteEntity = queryFactory.Get<SelectStockFichesDto>(queryDeleteEntity);

                    #region Stock Movement Service

                    switch (StockFichesDeleteEntity.FicheType)
                    {
                        case StockFicheTypeEnum.FireFisi: StockMovementsService.DeleteTotalWastageLines(stockFichesLineGet); ; break;
                        case StockFicheTypeEnum.SarfFisi: StockMovementsService.DeleteTotalConsumptionLines(stockFichesLineGet); ; break;
                        case StockFicheTypeEnum.UretimdenGirisFisi: StockMovementsService.DeleteTotalProductionLines(stockFichesLineGet); ; break;
                        case StockFicheTypeEnum.StokGirisFisi: StockMovementsService.DeleteTotalGoodLines(stockFichesLineGet); ; break;
                        case StockFicheTypeEnum.StokCikisFisi: StockMovementsService.DeleteTotalGoodIssueLines(stockFichesLineGet); ; break;
                        case StockFicheTypeEnum.DepoSevkFisi: StockMovementsService.DeleteTotalWarehouseShippingLines(stockFichesLineGet); break;
                    }

                    #endregion

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
                       .Select<StockFiches>(sf => new { sf.Id, sf.InputOutputCode, sf.FicheNo, sf.Date_, sf.Description_, sf.FicheType, sf.NetAmount, sf.WarehouseID, sf.Time_, sf.SpecialCode, sf.ProductionOrderID, sf.ExchangeRate, sf.DataOpenStatusUserId, sf.DataOpenStatus, sf.CurrencyID, sf.BranchID })
                       .Join<Branches>
                        (
                            b => new { BranchCode = b.Code , BranchID = b.Id},
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                            nameof(StockFiches.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.StockFiches);

                var stockFiches = queryFactory.Get<SelectStockFichesDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StockFicheLines)
                       .Select<StockFicheLines>(sfl => new
                       { sfl.UnitSetID, sfl.UnitPrice, sfl.StockFicheID, sfl.Quantity, sfl.ProductID, sfl.LineNr, sfl.LineDescription, sfl.LineAmount, sfl.Id, sfl.FicheType, sfl.DataOpenStatusUserId, sfl.DataOpenStatus})
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
                       .Select<StockFiches>(sf => new { sf.FicheNo, sf.InputOutputCode, sf.Date_, sf.Description_, sf.FicheType, sf.NetAmount , sf.Id, sf.WarehouseID, sf.Time_, sf.SpecialCode, sf.ProductionOrderID, sf.ExchangeRate, sf.DataOpenStatusUserId, sf.DataOpenStatus, sf.CurrencyID, sf.BranchID })
                       .Join<Branches>
                        (
                            b => new { BranchCode = b.Code },
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            w => new { WarehouseCode = w.Code },
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
                       .Select<StockFiches>(sf => new { sf.Id, sf.FicheNo, sf.InputOutputCode, sf.Date_, sf.Description_, sf.FicheType, sf.NetAmount, sf.WarehouseID, sf.Time_, sf.SpecialCode, sf.ProductionOrderID, sf.ExchangeRate, sf.DataOpenStatusUserId, sf.DataOpenStatus, sf.CurrencyID, sf.BranchID, sf.CreationTime, sf.DeletionTime })
                       .Join<Branches>
                        (
                            b => new { BranchCode = b.Code, BranchID = b.Id },
                            nameof(StockFiches.BranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                       .Join<Warehouses>
                        (
                            w => new { WarehouseCode = w.Code, WarehouseID = w.Id },
                            nameof(StockFiches.WarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.StockFiches);

                var entity = queryFactory.Get<SelectStockFichesDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StockFicheLines)
                       .Select<StockFicheLines>(sfl => new
                       { sfl.UnitSetID, sfl.UnitPrice, sfl.StockFicheID, sfl.Quantity, sfl.ProductID, sfl.LineNr, sfl.LineDescription, sfl.LineAmount, sfl.Id, sfl.FicheType, sfl.DataOpenStatusUserId, sfl.DataOpenStatus })
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

                if(input.FicheType != 25)
                {
                    switch (input.FicheType)
                    {
                        case 12: input.InputOutputCode = 1; break;
                        case 11: input.InputOutputCode = 1; break;
                        case 13: input.InputOutputCode = 0; break;
                        case 50: input.InputOutputCode = 0; break;
                        case 51: input.InputOutputCode = 1; break;
                    }
                }


                var query = queryFactory.Query().From(Tables.StockFiches).Update(new UpdateStockFichesDto
                {
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    InputOutputCode = input.InputOutputCode,
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
                            FicheType = (int)item.FicheType,
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
                                FicheType = (int)item.FicheType,
                                LineAmount = item.LineAmount,
                                LineDescription = item.LineDescription,
                                UnitPrice = item.UnitPrice
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var stockFiche = queryFactory.Update<SelectStockFichesDto>(query, "Id", true);

                #region Stock Movement Service

                switch (input.FicheType)
                {
                    case 12: StockMovementsService.UpdateTotalWastages(entity,input);  break;
                    case 11: StockMovementsService.UpdateTotalConsumptions(entity, input);  break;
                    case 13: StockMovementsService.UpdateTotalProductions(entity, input);  break;
                    case 50: StockMovementsService.UpdateTotalGoods(entity, input);  break;
                    case 51: StockMovementsService.UpdateTotalGoodIssues(entity, input);  break;
                    case 25: StockMovementsService.UpdateTotalWarehouseShippings(entity, input); break;
                }

                #endregion

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.StockFiches, LogType.Update, entity.Id);

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
                    InputOutputCode = entity.InputOutputCode,
                    ExchangeRate = entity.ExchangeRate,
                    FicheType = (int)entity.FicheType,
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
