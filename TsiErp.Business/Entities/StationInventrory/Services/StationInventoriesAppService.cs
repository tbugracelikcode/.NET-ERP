using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.StationInventrories.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.StationInventory.BusinessRules;
using TsiErp.Business.Entities.StationInventory.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    [ServiceRegistration(typeof(IStationInventoriesAppService), DependencyInjectionType.Scoped)]
    public class StationInventoriesAppService : ApplicationService<StationInventroriesResource>, IStationInventoriesAppService
    {
        public StationInventoriesAppService(IStringLocalizer<StationInventroriesResource> l) : base(l)
        {
        }

        StationInventoryManager _manager { get; set; } = new StationInventoryManager();

        [ValidationAspect(typeof(CreateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> CreateAsync(CreateStationInventoriesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.ProductControl(_uow.StationInventoriesRepository, input.ProductID.GetValueOrDefault(), input.StationID.GetValueOrDefault(),L);

                var entity = ObjectMapper.Map<CreateStationInventoriesDto, StationInventories>(input);

                var addedEntity = await _uow.StationInventoriesRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "StationInventories", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectStationInventoriesDto>(ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.StationInventoriesRepository, id);
                await _uow.StationInventoriesRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "StationInventories", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectStationInventoriesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationInventoriesRepository.GetAsync(t => t.Id == id, t => t.Stations);
                var mappedEntity = ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "StationInventories", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectStationInventoriesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationInventoriesDto>>> GetListAsync(ListStationInventoriesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.StationInventoriesRepository.GetListAsync(null, t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<StationInventories>, List<ListStationInventoriesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListStationInventoriesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateAsync(UpdateStationInventoriesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationInventoriesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateProductControl(_uow.StationInventoriesRepository, input.ProductID.GetValueOrDefault(), input.StationID.GetValueOrDefault(), input.Id,L);

                var mappedEntity = ObjectMapper.Map<UpdateStationInventoriesDto, StationInventories>(input);

                await _uow.StationInventoriesRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<StationInventories, UpdateStationInventoriesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "StationInventories", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectStationInventoriesDto>(ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationInventoriesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.StationInventoriesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(updatedEntity);

                return new SuccessDataResult<SelectStationInventoriesDto>(mappedEntity);
            }
        }
    }
}
