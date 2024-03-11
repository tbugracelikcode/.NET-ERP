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
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Services;
using TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ShipmentPlannings.Page;

namespace TsiErp.Business.Entities.ShipmentPlanning.Services
{
    [ServiceRegistration(typeof(IShipmentPlanningsAppService), DependencyInjectionType.Scoped)]
    public class ShipmentPlanningsAppService : ApplicationService<ShipmentPlanningsResource>, IShipmentPlanningsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ShipmentPlanningsAppService(IStringLocalizer<ShipmentPlanningsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        [ValidationAspect(typeof(CreateShipmentPlanningsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShipmentPlanningsDto>> CreateAsync(CreateShipmentPlanningsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<ShipmentPlannings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Insert(new CreateShipmentPlanningsDto
            {
                PlannedAmount = input.PlannedAmount,
                ShipmentPlanningDate = input.ShipmentPlanningDate,
                TotalAmount = input.TotalAmount,
                TotalGrossKG = input.TotalGrossKG,
                TotalNetKG = input.TotalNetKG,
                Description_ = input.Description_,
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            foreach (var item in input.SelectShipmentPlanningLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Insert(new CreateShipmentPlanningLinesDto
                {
                    LineNr = item.LineNr,
                    ProductID = item.ProductID,
                    GrossWeightKG = item.GrossWeightKG,
                    LineDescription_ = item.LineDescription_,
                    NetWeightKG = item.NetWeightKG,
                    PlannedQuantity = item.PlannedQuantity,
                    ProductionOrderID = item.ProductionOrderID,
                    RequestedLoadingDate = _GetSQLDateAppService.GetDateFromSQL(),
                    SentQuantity = item.SentQuantity,
                    ShipmentQuantity = item.ShipmentQuantity,
                    UnitWeightKG = item.UnitWeightKG,
                    SalesOrderID = item.SalesOrderID,
                    ShipmentPlanningID = addedEntityId,
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
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var ShipmentPlanning = queryFactory.Insert<SelectShipmentPlanningsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ShipmentPlanningChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ShipmentPlanningID", new List<string>
            {
                //Tables.PurchaseRequests
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, false, false, "");

                var ShipmentPlannings = queryFactory.Get<SelectShipmentPlanningsDto>(query);

                if (ShipmentPlannings.Id != Guid.Empty && ShipmentPlannings != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ShipmentPlanningLines).Delete(LoginedUserService.UserId).Where(new { BomID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var ShipmentPlanning = queryFactory.Update<SelectShipmentPlanningsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var ShipmentPlanningLines = queryFactory.Update<SelectShipmentPlanningLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShipmentPlanningLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectShipmentPlanningLinesDto>(ShipmentPlanningLines);
                }
            }
        }

        public async Task<IDataResult<SelectShipmentPlanningsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, false, false, "");
            var ShipmentPlanning = queryFactory.Get<SelectShipmentPlanningsDto>(query);


            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(null)
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { ShipmentPlanningID = id }, false, false, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.GetList<SelectShipmentPlanningLinesDto>(queryLines).ToList();

            ShipmentPlanning.SelectShipmentPlanningLines = ShipmentPlanningLine;

            LogsAppService.InsertLogToDatabase(ShipmentPlanning, ShipmentPlanning, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IDataResult<SelectShipmentPlanningLinesDto>> GetLinebyProductionOrderAsync(Guid productionOrderID)
        {
           
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(null)
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { ProductionOrderID = productionOrderID }, false, false, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.Get<SelectShipmentPlanningLinesDto>(queryLines);


            LogsAppService.InsertLogToDatabase(ShipmentPlanningLine, ShipmentPlanningLine, LoginedUserService.UserId, Tables.ShipmentPlanningLines, LogType.Get, ShipmentPlanningLine.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningLinesDto>(ShipmentPlanningLine);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShipmentPlanningsDto>>> GetListAsync(ListShipmentPlanningsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(null, false, false, "");
            var ShipmentPlannings = queryFactory.GetList<ListShipmentPlanningsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShipmentPlanningsDto>>(ShipmentPlannings);
        }

        [ValidationAspect(typeof(UpdateShipmentPlanningsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShipmentPlanningsDto>> UpdateAsync(UpdateShipmentPlanningsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<SelectShipmentPlanningsDto>(entityQuery);

            var queryLines = queryFactory
                 .Query()
                 .From(Tables.ShipmentPlanningLines)
                 .Select<ShipmentPlanningLines>(null)
                  .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                  .Where(new { ShipmentPlanningID = input.Id }, false, false, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.GetList<SelectShipmentPlanningLinesDto>(queryLines).ToList();

            entity.SelectShipmentPlanningLines = ShipmentPlanningLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.ShipmentPlannings).Select("*").Where(new { Code = input.Code }, false, false, Tables.ShipmentPlannings);

            var list = queryFactory.GetList<ListShipmentPlanningsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Update(new UpdateShipmentPlanningsDto
            {
                Description_ = input.Description_,
                PlannedAmount = input.PlannedAmount,
                ShipmentPlanningDate = input.ShipmentPlanningDate,
                TotalAmount = input.TotalAmount,
                TotalGrossKG = input.TotalGrossKG,
                TotalNetKG = input.TotalNetKG,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectShipmentPlanningLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Insert(new CreateShipmentPlanningLinesDto
                    {
                        LineNr = item.LineNr,
                        ProductID = item.ProductID,
                        SalesOrderID = item.SalesOrderID,
                        PlannedQuantity = item.PlannedQuantity,
                        ProductionOrderID = item.ProductionOrderID,
                        GrossWeightKG = item.GrossWeightKG,
                        LineDescription_ = item.LineDescription_,
                        NetWeightKG = item.NetWeightKG,
                        RequestedLoadingDate = item.RequestedLoadingDate,
                        SentQuantity = item.SentQuantity,
                        ShipmentQuantity = item.ShipmentQuantity,
                        UnitWeightKG = item.UnitWeightKG,
                        ShipmentPlanningID = input.Id,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ShipmentPlanningLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectShipmentPlanningLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Update(new UpdateShipmentPlanningLinesDto
                        {
                            LineNr = item.LineNr,
                            UnitWeightKG = item.SentQuantity,
                            SentQuantity = item.SentQuantity,
                            ShipmentQuantity = item.ShipmentQuantity,
                            RequestedLoadingDate = _GetSQLDateAppService.GetDateFromSQL(),
                            NetWeightKG = item.NetWeightKG,
                            LineDescription_ = item.LineDescription_,
                            GrossWeightKG = item.GrossWeightKG,
                            ProductionOrderID = item.ProductionOrderID,
                            PlannedQuantity = item.PlannedQuantity,
                            ProductID = item.ProductID,
                            SalesOrderID = item.SalesOrderID,
                            ShipmentPlanningID = input.Id,
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

            var ShipmentPlanning = queryFactory.Update<SelectShipmentPlanningsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Update, input.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IDataResult<SelectShipmentPlanningsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<ShipmentPlannings>(entityQuery);

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Update(new UpdateShipmentPlanningsDto
            {
                Description_ = entity.Description_,
                PlannedAmount = entity.PlannedAmount,
                ShipmentPlanningDate = entity.ShipmentPlanningDate,
                TotalAmount = entity.TotalAmount,
                TotalGrossKG = entity.TotalGrossKG,
                TotalNetKG = entity.TotalNetKG,
                Code = entity.Code,
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

            var ShipmentPlanningsDto = queryFactory.Update<SelectShipmentPlanningsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanningsDto);


        }
    }
}
