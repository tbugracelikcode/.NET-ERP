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

        public StationsAppService(IStringLocalizer<StationsResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> CreateAsync(CreateStationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<Stations>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
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
                    GroupID = input.GroupID,
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

                foreach (var item in input.SelectStationInventoriesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
                    {
                        Amount = item.Amount,
                        Description_ = item.Description_,
                        StationID = addedEntityId,
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

                var station = queryFactory.Insert<SelectStationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Stations, LogType.Insert, station.Id);

                return new SuccessDataResult<SelectStationsDto>(station);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Id = id }, true, true, "");

                var stations = queryFactory.Get<SelectStationsDto>(query);

                if (stations.Id != Guid.Empty && stations != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Stations).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { StationID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var station = queryFactory.Update<SelectStationsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Stations, LogType.Delete, id);
                    return new SuccessDataResult<SelectStationsDto>(station);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationInventories, LogType.Delete, id);
                    return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
                }
            }
        }

        public async Task<IDataResult<SelectStationsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.Stations)
                       .Select<Stations>(s => new { s.Y, s.X, s.UsageArea, s.ShiftWorkingTime, s.Shift, s.PowerFactor, s.Name, s.Model, s.MachineCost, s.KWA, s.IsFixtures, s.IsContract, s.IsActive, s.Id, s.GroupID, s.DataOpenStatusUserId, s.DataOpenStatus, s.Code, s.Capacity, s.Brand, s.AreaCovered, s.Amortization })
                       .Join<StationGroups>
                        (
                            sg => new { GroupID = sg.Id, StationGroup = sg.Name },
                            nameof(Stations.GroupID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, true, true, Tables.Stations);

                var stations = queryFactory.Get<SelectStationsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StationInventories)
                       .Select("*").Where(new { StationID = id }, false, false, Tables.StationInventories);

                var stationInventory = queryFactory.GetList<SelectStationInventoriesDto>(queryLines).ToList();

                stations.SelectStationInventoriesDto = stationInventory;

                LogsAppService.InsertLogToDatabase(stations, stations, LoginedUserService.UserId, Tables.Stations, LogType.Get, id);

                return new SuccessDataResult<SelectStationsDto>(stations);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationsDto>>> GetListAsync(ListStationsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.Stations)
                       .Select<Stations>(s => new { s.Y, s.X, s.UsageArea, s.ShiftWorkingTime, s.Shift, s.PowerFactor, s.Name, s.Model, s.MachineCost, s.KWA, s.IsFixtures, s.IsContract, s.IsActive, s.Id, s.GroupID, s.DataOpenStatusUserId, s.DataOpenStatus, s.Code, s.Capacity, s.Brand, s.AreaCovered, s.Amortization })
                       .Join<StationGroups>
                        (
                            sg => new { StationGroup = sg.Name },
                            nameof(Stations.GroupID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(null, true, true, Tables.Stations);

                var stations = queryFactory.GetList<ListStationsDto>(query).ToList();
                return new SuccessDataResult<IList<ListStationsDto>>(stations);
            }

        }


        [ValidationAspect(typeof(UpdateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> UpdateAsync(UpdateStationsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.Stations)
                       .Select<Stations>(s => new { s.Y, s.X, s.UsageArea, s.ShiftWorkingTime, s.Shift, s.PowerFactor, s.Name, s.Model, s.MachineCost, s.KWA, s.IsFixtures, s.IsContract, s.IsActive, s.Id, s.GroupID, s.DataOpenStatusUserId, s.DataOpenStatus, s.Code, s.Capacity, s.Brand, s.AreaCovered, s.Amortization })
                       .Join<StationGroups>
                        (
                            sg => new { GroupID = sg.Id, StationGroup = sg.Name },
                            nameof(Stations.GroupID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, true, true, Tables.Stations);

                var entity = queryFactory.Get<SelectStationsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.StationInventories)
                       .Select("*").Where(new { StationID = input.Id }, false, false, Tables.StationInventories);

                var stationInventory = queryFactory.GetList<SelectStationInventoriesDto>(queryLines).ToList();

                entity.SelectStationInventoriesDto = stationInventory;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.Stations)
                               .Select<Stations>(s => new { s.Y, s.X, s.UsageArea, s.ShiftWorkingTime, s.Shift, s.PowerFactor, s.Name, s.Model, s.MachineCost, s.KWA, s.IsFixtures, s.IsContract, s.IsActive, s.Id, s.GroupID, s.DataOpenStatusUserId, s.DataOpenStatus, s.Code, s.Capacity, s.Brand, s.AreaCovered, s.Amortization })
                               .Join<StationGroups>
                        (
                            sg => new { GroupID = sg.Id, StationGroup = sg.Name },
                            nameof(Stations.GroupID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                                .Where(new { Code = input.Code }, false, false, Tables.Stations);

                var list = queryFactory.GetList<ListStationsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.Stations).Update(new UpdateStationsDto
                {
                    Amortization = input.Amortization,
                    AreaCovered = input.AreaCovered,
                    Brand = input.Brand,
                    Capacity = input.Capacity,
                    GroupID = input.GroupID,
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
                    IsActive = input.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                    Name = input.Name,
                }).Where(new { Id = input.Id }, true, true, "");

                foreach (var item in input.SelectStationInventoriesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
                        {
                            Amount = item.Amount,
                            Description_ = item.Description_,
                            StationID = input.Id,
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
                        var lineGetQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                ProductID = item.ProductID,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var station = queryFactory.Update<SelectStationsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Stations, LogType.Update, station.Id);

                return new SuccessDataResult<SelectStationsDto>(station);
            }
        }

        public async Task<IDataResult<SelectStationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Stations).Select("*").Where(new { Id = id }, true, true, "");

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
                    IsActive = entity.IsActive,
                    Name = entity.Name,
                }).Where(new { Id = id }, true, true, "");

                var stationsDto = queryFactory.Update<SelectStationsDto>(query, "Id", true);
                return new SuccessDataResult<SelectStationsDto>(stationsDto);

            }
        }
    }
}
