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
using TsiErp.Business.Entities.StationInventory.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StationInventrories.Page;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    [ServiceRegistration(typeof(IStationInventoriesAppService), DependencyInjectionType.Scoped)]
    public class StationInventoriesAppService : ApplicationService<StationInventroriesResource>, IStationInventoriesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public StationInventoriesAppService(IStringLocalizer<StationInventroriesResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> CreateAsync(CreateStationInventoriesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { StationID = input.StationID, ProductID = input.ProductID }, false, false, "");

                var list = queryFactory.ControlList<StationInventories>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
                {
                    ProductID = input.ProductID,
                    StationID = input.StationID,
                    Amount = input.Amount,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Description_ = input.Description_,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                var stationInventories = queryFactory.Insert<SelectStationInventoriesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StationInventories, LogType.Insert, stationInventories.Id);

                return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationInventories, LogType.Delete, id);

                return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
            }

        }


        public async Task<IDataResult<SelectStationInventoriesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.StationInventories).Select<StationInventories>(si => new { si.ProductID, si.StationID, si.Id, si.DataOpenStatus, si.DataOpenStatusUserId, si.Amount, si.Description_ })
                            .Join<Stations>
                            (
                                s => new { StationID = s.Id },
                                nameof(StationInventories.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.StationInventories);

                var stationInventory = queryFactory.Get<SelectStationInventoriesDto>(query);

                LogsAppService.InsertLogToDatabase(stationInventory, stationInventory, LoginedUserService.UserId, Tables.StationInventories, LogType.Get, id);

                return new SuccessDataResult<SelectStationInventoriesDto>(stationInventory);

            }

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationInventoriesDto>>> GetListAsync(ListStationInventoriesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.StationInventories).Select<StationInventories>(si => new { si.ProductID, si.StationID, si.Id, si.DataOpenStatus, si.DataOpenStatusUserId, si.Amount, si.Description_ })
                            .Join<Stations>
                            (
                                s => new { StationID = s.Id },
                                nameof(StationInventories.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            ).Where(null, false, false, Tables.StationInventories);

                var stationInventories = queryFactory.GetList<ListStationInventoriesDto>(query).ToList();

                return new SuccessDataResult<IList<ListStationInventoriesDto>>(stationInventories);
            }

        }


        [ValidationAspect(typeof(UpdateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateAsync(UpdateStationInventoriesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<StationInventories>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { StationID = input.StationID, ProductID = input.ProductID }, false, false, "");
                var list = queryFactory.GetList<StationInventories>(listQuery).ToList();

                if (list.Count > 0 && entity.Id != input.Id)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.StationInventories).Update(new UpdateStationInventoriesDto
                {
                    Description_ = input.Description_,
                    Amount = input.Amount,
                    ProductID = input.ProductID,
                    StationID = input.StationID,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, stationInventories, LoginedUserService.UserId, Tables.StationInventories, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
            }

        }

        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<StationInventories>(entityQuery);

                var query = queryFactory.Query().From(Tables.StationInventories).Update(new UpdateStationInventoriesDto
                {
                    StationID = entity.StationID,
                    ProductID = entity.ProductID,
                    Amount = entity.Amount,
                    Description_ = entity.Description_,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, false, false, "");

                var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

                return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);

            }
        }
    }
}
