using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StationOccupancies.Page;
using TsiErp.Localizations.Resources.StationOccupancyHistories.Page;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Services
{
    public class StationOccupancyHistoriesAppService : ApplicationService<StationOccupancyHistoriesResource>, IStationOccupancyHistoriesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StationOccupancyHistoriesAppService(IStringLocalizer<StationOccupancyHistoriesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectStationOccupancyHistoriesDto>> CreateAsync(CreateStationOccupancyHistoriesDto input)
        {

            Guid addedEntityID = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StationOccupancyHistories).Insert(new CreateStationOccupancyHistoriesDto
            {
                Id = addedEntityID,
                StationID = input.StationID,
                FreeDate = input.FreeDate,
                ShipmentPlanningID = input.ShipmentPlanningID,
            });
            var stationOccupancyHistories = queryFactory.Insert<SelectStationOccupancyHistoriesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupancyHistoriesDto>(stationOccupancyHistories);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancyHistories).UseIsDelete(false).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var stationOccupancyHistories = queryFactory.Update<SelectStationOccupancyHistoriesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupancyHistoriesDto>(stationOccupancyHistories);
        }

        public async Task<IDataResult<SelectStationOccupancyHistoriesDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancyHistories).Select("*").Where(new
            {
                Id = id
            }, "").UseIsDelete(false);

            var stationOccupancyHistory = queryFactory.Get<SelectStationOccupancyHistoriesDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationOccupancyHistoriesDto>(stationOccupancyHistory);
        }

        public async Task<IDataResult<IList<ListStationOccupancyHistoriesDto>>> GetListAsync(ListStationOccupancyHistoriesParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StationOccupancyHistories).Select("*").UseIsDelete(false).Where(null, "");

            var stationOccupancyHistory = queryFactory.GetList<ListStationOccupancyHistoriesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationOccupancyHistoriesDto>>(stationOccupancyHistory);

        }

        #region Unused Methods

        public Task<IDataResult<SelectStationOccupancyHistoriesDto>> UpdateAsync(UpdateStationOccupancyHistoriesDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectStationOccupancyHistoriesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
