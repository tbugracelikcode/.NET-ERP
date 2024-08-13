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
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.StationInventory.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StationInventrories.Page;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    [ServiceRegistration(typeof(IStationInventoriesAppService), DependencyInjectionType.Scoped)]
    public class StationInventoriesAppService : ApplicationService<StationInventroriesResource>, IStationInventoriesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StationInventoriesAppService(IStringLocalizer<StationInventroriesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> CreateAsync(CreateStationInventoriesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.StationInventories).Select("StationID").Where(new { StationID = input.StationID, ProductID = input.ProductID }, "");

            var list = queryFactory.ControlList<StationInventories>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.StationInventories).Insert(new CreateStationInventoriesDto
            {
                ProductID = input.ProductID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                Amount = input.Amount,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var stationInventories = queryFactory.Insert<SelectStationInventoriesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StationInventories, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.StationInventories).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.StationInventories, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
        }


        public async Task<IDataResult<SelectStationInventoriesDto>> GetAsync(Guid id)
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
                        .Where(new { Id = id }, Tables.StationInventories);

            var stationInventory = queryFactory.Get<SelectStationInventoriesDto>(query);

            LogsAppService.InsertLogToDatabase(stationInventory, stationInventory, LoginedUserService.UserId, Tables.StationInventories, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationInventoriesDto>(stationInventory);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationInventoriesDto>>> GetListAsync(ListStationInventoriesParameterDto input)
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
                        ).Where(null, Tables.StationInventories);

            var stationInventories = queryFactory.GetList<ListStationInventoriesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListStationInventoriesDto>>(stationInventories);

        }


        [ValidationAspect(typeof(UpdateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateAsync(UpdateStationInventoriesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<StationInventories>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.StationInventories).Select("*").Where(new { StationID = input.StationID, ProductID = input.ProductID }, "");
            var list = queryFactory.GetList<StationInventories>(listQuery).ToList();

            if (list.Count > 0 && entity.Id != input.Id)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.StationInventories).Update(new UpdateStationInventoriesDto
            {
                Description_ = input.Description_,
                Amount = input.Amount,
                ProductID = input.ProductID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                Id = input.Id,
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

            var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, stationInventories, LoginedUserService.UserId, Tables.StationInventories, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);

        }

        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.StationInventories).Select("Id").Where(new { Id = id }, "");
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var stationInventories = queryFactory.Update<SelectStationInventoriesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStationInventoriesDto>(stationInventories);
        }
    }
}
