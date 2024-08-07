using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.PlanningManagement.StationOccupancy;
using TsiErp.Entities.Entities.PlanningManagement.StationOccupancy.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StationOccupancies.Page;

namespace TsiErp.Business.Entities.StationOccupancy.Services
{
    [ServiceRegistration(typeof(IStationOccupanciesAppService), DependencyInjectionType.Scoped)]
    public class StationOccupanciesAppService : ApplicationService<StationOccupanciesResource>, IStationOccupanciesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StationOccupanciesAppService(IStringLocalizer<StationOccupanciesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationOccupanciesDto>> CreateAsync(CreateStationOccupanciesDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StationOccupancies).Insert(new CreateStationOccupanciesDto
            {
                ProductID = input.ProductID,
                SalesOrderID = input.SalesOrderID,
                StationID = input.StationID,
                TimeItWillWork = input.TimeItWillWork,
                TimeItWorked = input.TimeItWorked,
                WorkOrderID = input.WorkOrderID,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var StationOccupancies = queryFactory.Insert<SelectStationOccupanciesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StationOccupancies, LogType.Insert, addedEntityId);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var StationOccupancies = queryFactory.Update<SelectStationOccupanciesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationOccupancies, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }


        public async Task<IDataResult<SelectStationOccupanciesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).Select<StationOccupancies>(null)
                .Join<Stations>
                        (
                            pr => new { StationCode = pr.Code, StationID = pr.Id },
                            nameof(StationOccupancies.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<SalesOrders>
                        (
                            pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderID = pr.Id },
                            nameof(StationOccupancies.SalesOrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                            nameof(StationOccupancies.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<WorkOrders>
                        (
                            pr => new { WorkOrderFicheNo = pr.WorkOrderNo, WorkOrderID = pr.Id },
                            nameof(StationOccupancies.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                .Where(new { Id = id }, Tables.StationOccupancies);
            var StationOccupancy = queryFactory.Get<SelectStationOccupanciesDto>(query);


            LogsAppService.InsertLogToDatabase(StationOccupancy, StationOccupancy, LoginedUserService.UserId, Tables.StationOccupancies, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancy);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationOccupanciesDto>>> GetListAsync(ListStationOccupanciesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).Select<StationOccupancies>(null)
                .Join<Stations>
                        (
                            pr => new { StationCode = pr.Code, StationID = pr.Id },
                            nameof(StationOccupancies.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<SalesOrders>
                        (
                            pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderID = pr.Id },
                            nameof(StationOccupancies.SalesOrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                            nameof(StationOccupancies.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                         .Join<WorkOrders>
                        (
                            pr => new { WorkOrderFicheNo = pr.WorkOrderNo, WorkOrderID = pr.Id },
                            nameof(StationOccupancies.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        ).Where(null, Tables.StationOccupancies);
            var stationOccupancies = queryFactory.GetList<ListStationOccupanciesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationOccupanciesDto>>(stationOccupancies);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationOccupanciesDto>> UpdateAsync(UpdateStationOccupanciesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationOccupancies).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<StationOccupancies>(entityQuery);

            var query = queryFactory.Query().From(Tables.StationOccupancies).Update(new UpdateStationOccupanciesDto
            {
                Id = input.Id,
                WorkOrderID = input.WorkOrderID,
                TimeItWorked = input.TimeItWorked,
                TimeItWillWork = input.TimeItWillWork,
                ProductID = input.ProductID,
                SalesOrderID = input.SalesOrderID,
                StationID = input.StationID,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var StationOccupancies = queryFactory.Update<SelectStationOccupanciesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, StationOccupancies, LoginedUserService.UserId, Tables.StationOccupancies, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }

        public async Task<IDataResult<SelectStationOccupanciesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationOccupancies).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<StationOccupancies>(entityQuery);

            var query = queryFactory.Query().From(Tables.StationOccupancies).Update(new UpdateStationOccupanciesDto
            {
                StationID = entity.StationID,
                SalesOrderID = entity.SalesOrderID,
                ProductID = entity.ProductID,
                TimeItWillWork = entity.TimeItWillWork,
                TimeItWorked = entity.TimeItWorked,
                WorkOrderID = entity.WorkOrderID,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var StationOccupancies = queryFactory.Update<SelectStationOccupanciesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }
    }
}
