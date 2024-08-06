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
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.EntityContracts.Station;
using TsiErp.Localizations.Resources.Stations.Page;

namespace TsiErp.Business.Entities.Station.Services
{
    [ServiceRegistration(typeof(IStationsAppService), DependencyInjectionType.Scoped)]
    public class StationsAppService : ApplicationService<StationsResource>, IStationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StationsAppService(IStringLocalizer<StationsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> CreateAsync(CreateStationsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<Stations>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Stations).Insert(new CreateStationsDto
            {
                Amortization = input.Amortization,
                AreaCovered = input.AreaCovered,
                Brand = input.Brand,
                Capacity = input.Capacity,
                GroupID = input.GroupID.GetValueOrDefault(),
                IsContract = input.IsContract,
                IsFixtures = input.IsFixtures,
                KWA = input.KWA,
                MachineCost = input.MachineCost,
                Shift = input.Shift,
                Model = input.Model,
                PowerFactor = input.PowerFactor,
                ShiftWorkingTime = input.ShiftWorkingTime,
                UsageArea = input.UsageArea,
                X = input.X,
                Y = input.Y,
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
                Name = input.Name,
            });

            foreach (var item in input.SelectStationInventoriesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
                {
                    Amount = item.Amount,
                    Description_ = item.Description_,
                    StationID = addedEntityId,
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

            var station = queryFactory.Insert<SelectStationsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("StationsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Stations, LogType.Insert, station.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationsDto>(station);
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("StationID", new List<string>
            {
                Tables.CalendarLines,
                Tables.ContractProductionTrackings,
                Tables.ContractTrackingFicheLines,
                Tables.MaintenanceInstructions,
                Tables.OperationUnsuitabilityReports,
                Tables.PlannedMaintenances,
                Tables.ProductionTrackings,
                Tables.ProductsOperationLines,
                Tables.TemplateOperationLines,
                Tables.UnplannedMaintenances,
                Tables.WorkOrders
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {

                var query = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Id = id }, "");

                var stations = queryFactory.Get<SelectStationsDto>(query);

                if (stations.Id != Guid.Empty && stations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Stations).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { StationID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var station = queryFactory.Update<SelectStationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Stations, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectStationsDto>(station);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
                    var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationInventories, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
                }
            }
        }

        public async Task<IDataResult<SelectStationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Stations)
                   .Select<Stations>(null)
                   .Join<StationGroups>
                    (
                        sg => new { GroupID = sg.Id, StationGroup = sg.Name },
                        nameof(Stations.GroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.Stations);

            var stations = queryFactory.Get<SelectStationsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StationInventories)
                   .Select("*").Where(new { StationID = id },  Tables.StationInventories);

            var stationInventory = queryFactory.GetList<SelectStationInventoriesDto>(queryLines).ToList();

            stations.SelectStationInventoriesDto = stationInventory;

            LogsAppService.InsertLogToDatabase(stations, stations, LoginedUserService.UserId, Tables.Stations, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationsDto>(stations);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationsDto>>> GetListAsync(ListStationsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Stations)
                   .Select<Stations>(null)
                   .Join<StationGroups>
                    (
                        sg => new { StationGroup = sg.Name, GroupID = sg.Id },
                        nameof(Stations.GroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.Stations);

            var stations = queryFactory.GetList<ListStationsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationsDto>>(stations);
        }


        [ValidationAspect(typeof(UpdateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> UpdateAsync(UpdateStationsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Stations).Select("*")
                    .Join<StationGroups>
                    (
                        sg => new { GroupID = sg.Id, StationGroup = sg.Name, StationGroupCode = sg.Code },
                        nameof(Stations.GroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.Stations);

            var entity = queryFactory.Get<SelectStationsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.StationInventories)
                   .Select("*").Where(new { StationID = input.Id }, Tables.StationInventories);

            var stationInventory = queryFactory.GetList<SelectStationInventoriesDto>(queryLines).ToList();

            entity.SelectStationInventoriesDto = stationInventory;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Stations)
                           .Select<Stations>(s => new { s.Y, s.X, s.UsageArea, s.ShiftWorkingTime, s.Shift, s.PowerFactor, s.Name, s.Model, s.MachineCost, s.KWA, s.IsFixtures, s.IsContract, s.Id, s.GroupID, s.DataOpenStatusUserId, s.DataOpenStatus, s.Code, s.Capacity, s.Brand, s.AreaCovered, s.Amortization })
                           .Join<StationGroups>
                    (
                        sg => new { GroupID = sg.Id, StationGroup = sg.Name },
                        nameof(Stations.GroupID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.Stations);

            var list = queryFactory.GetList<ListStationsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.Stations).Update(new UpdateStationsDto
            {
                Amortization = input.Amortization,
                AreaCovered = input.AreaCovered,
                Brand = input.Brand,
                Capacity = input.Capacity,
                GroupID = input.GroupID.GetValueOrDefault(),
                IsContract = input.IsContract,
                IsFixtures = input.IsFixtures,
                KWA = input.KWA,
                MachineCost = input.MachineCost,
                Shift = input.Shift,
                Model = input.Model,
                PowerFactor = input.PowerFactor,
                ShiftWorkingTime = input.ShiftWorkingTime,
                UsageArea = input.UsageArea,
                X = input.X,
                Y = input.Y,
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
                Name = input.Name,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectStationInventoriesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
                    {
                        Amount = item.Amount,
                        Description_ = item.Description_,
                        StationID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectStationInventoriesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StationInventories).Update(new UpdateStationInventoriesDto
                        {
                            Description_ = line.Description_,
                            Amount = line.Amount,
                            StationID = input.Id,
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
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var station = queryFactory.Update<SelectStationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Stations, LogType.Update, station.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationsDto>(station);
        }

        public async Task<IDataResult<SelectStationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Stations>(entityQuery);

            var query = queryFactory.Query().From(Tables.Stations).Update(new UpdateStationsDto
            {
                Amortization = entity.Amortization,
                AreaCovered = entity.AreaCovered,
                Brand = entity.Brand,
                Capacity = entity.Capacity,
                GroupID = entity.GroupID,
                IsContract = entity.IsContract,
                IsFixtures = entity.IsFixtures,
                KWA = entity.KWA,
                MachineCost = entity.MachineCost,
                Shift = entity.Shift,
                Model = entity.Model,
                PowerFactor = entity.PowerFactor,
                ShiftWorkingTime = entity.ShiftWorkingTime,
                UsageArea = entity.UsageArea,
                X = entity.X,
                Y = entity.Y,
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
                Name = entity.Name,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var stationsDto = queryFactory.Update<SelectStationsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationsDto>(stationsDto);
        }
    }
}
