using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StationOccupancies.Page;


namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StationOccupancy
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
        public async Task<IDataResult<SelectStationOccupanciesDto>> CreateAsync(CreateStationOccupanciesDto input)
        {
            Guid addedEntityID = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StationOccupancies).Insert(new CreateStationOccupanciesDto
            {
                Id = addedEntityID,
                StationID = input.StationID,
                FreeDate = input.FreeDate,
            });

            foreach (var item in input.SelectStationOccupancyLines)
            {
                var queryLine = queryFactory.Query().From(Tables.StationOccupancyLines).Insert(new CreateStationOccupancyLinesDto
                {
                    Id = GuidGenerator.CreateGuid(),
                    LineNr = item.LineNr,
                    StationOccupancyID = addedEntityID,
                    ProductionOrderID = item.ProductionOrderID,
                    WorkOrderID = item.WorkOrderID,
                    ProductsOperationID = item.ProductsOperationID,
                    PlannedStartDate = item.PlannedStartDate,
                    PlannedEndDate = item.PlannedEndDate,
                    ShipmentPlanningID = item.ShipmentPlanningID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var StationOccupancies = queryFactory.Insert<SelectStationOccupanciesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.StationOccupancies).Select("*").Where(new { Id = id }, "").UseIsDelete(false);

            var stationOccupancies = queryFactory.Get<SelectStationOccupanciesDto>(query);

            if (stationOccupancies.Id != Guid.Empty && stationOccupancies != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.StationOccupancies).UseIsDelete(false).Delete(LoginedUserService.UserId).Where(new { Id = id }, "").UseIsDelete(false);

                var lineDeleteQuery = queryFactory.Query().From(Tables.StationOccupancyLines).UseIsDelete(false).Delete(LoginedUserService.UserId).Where(new { StationOccupancyID = id }, "").UseIsDelete(false);

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var stationOccupancy = queryFactory.Update<SelectStationOccupanciesDto>(deleteQuery, "Id", true);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStationOccupanciesDto>("");
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.StationOccupancyLines).UseIsDelete(false).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var stationOccupancyLines = queryFactory.Update<SelectStationOccupancyLinesDto>(queryLine, "Id", true);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectStationOccupancyLinesDto>(stationOccupancyLines);
            }
        }

        public async Task<IDataResult<SelectStationOccupanciesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).Select("*").Where(new
            {
                Id = id
            }, "").UseIsDelete(false);

            var stationOccupancies = queryFactory.Get<SelectStationOccupanciesDto>(query); ;

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StationOccupancyLines)
                   .Select("*").UseIsDelete(false)
                    .Where(new { StationOccupanyID = id }, "");

            var StationOccupancyLine = queryFactory.GetList<SelectStationOccupancyLinesDto>(queryLines).ToList();

            stationOccupancies.SelectStationOccupancyLines = StationOccupancyLine;

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(stationOccupancies);

        }

        public async Task<IDataResult<SelectStationOccupanciesDto>> GetbyStationAsync(Guid stationID)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).UseIsDelete(false).Select("*").Where(new
            {
                StationID = stationID
            }, "");

            var stationOccupancies = queryFactory.Get<SelectStationOccupanciesDto>(query); ;

            if (stationOccupancies != null && stationOccupancies.Id != Guid.Empty)
            {
                var queryLines = queryFactory
                   .Query()
                   .From(Tables.StationOccupancyLines)
                   .Select("*").UseIsDelete(false)
                    .Where(new { StationOccupanyID = stationOccupancies.Id }, "");

                var StationOccupancyLine = queryFactory.GetList<SelectStationOccupancyLinesDto>(queryLines).ToList();

                stationOccupancies.SelectStationOccupancyLines = StationOccupancyLine;
            }




            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(stationOccupancies);

        }

        public async Task<IDataResult<IList<ListStationOccupanciesDto>>> GetListAsync(ListStationOccupanciesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancies).UseIsDelete(false).Select("*").Where(null, ""); 

            var stationOccupancies = queryFactory.GetList<ListStationOccupanciesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationOccupanciesDto>>(stationOccupancies);

        }

        public async Task<IDataResult<SelectStationOccupanciesDto>> UpdateAsync(UpdateStationOccupanciesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.StationOccupancies)
                   .Select("*").UseIsDelete(false)
                    .Where(new { Id = input.Id }, "");

            var entity = queryFactory.Get<SelectStationOccupanciesDto>(entityQuery);

    


            var query = queryFactory.Query().From(Tables.StationOccupancies).Update(new UpdateStationOccupanciesDto
            {
                Id = input.Id,
                StationID = input.StationID,
                FreeDate = input.FreeDate,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            foreach (var item in input.SelectStationOccupancyLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.StationOccupancyLines).Insert(new CreateStationOccupancyLinesDto
                    {
                        Id = GuidGenerator.CreateGuid(),
                        StationOccupancyID = item.StationOccupancyID,
                        LineNr = item.LineNr,
                        ProductionOrderID = item.ProductionOrderID,
                        WorkOrderID = item.WorkOrderID,
                        ProductsOperationID = item.ProductsOperationID,
                        PlannedStartDate = item.PlannedStartDate,
                        PlannedEndDate = item.PlannedEndDate,
                        ShipmentPlanningID = item.ShipmentPlanningID,

                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
              
            }
            var StationOccupancies = queryFactory.Update<SelectStationOccupanciesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupanciesDto>(StationOccupancies);

        }
        public  Task<IDataResult<SelectStationOccupanciesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();

        }
    }
}
