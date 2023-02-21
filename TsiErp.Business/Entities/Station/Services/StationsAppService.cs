using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Station.BusinessRules;
using TsiErp.Business.Entities.StationInventory.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using TsiErp.EntityContracts.Station;

namespace TsiErp.Business.Entities.Station.Services
{
    [ServiceRegistration(typeof(IStationsAppService), DependencyInjectionType.Scoped)]
    public class StationsAppService : ApplicationService, IStationsAppService
    {
        private readonly IStationInventoriesAppService _inventioriesRepository;

        StationManager _manager { get; set; } = new StationManager();

        public StationsAppService(IStationInventoriesAppService inventioriesRepository)
        {
            _inventioriesRepository = inventioriesRepository;
        }

        [ValidationAspect(typeof(CreateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> CreateAsync(CreateStationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.StationsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateStationsDto, Stations>(input);

                var addedEntity = await _uow.StationsRepository.InsertAsync(entity);

                foreach (var item in input.SelectStationInventoriesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var inventories = ObjectMapper.Map<SelectStationInventoriesDto, CreateStationInventoriesDto>(item);
                        inventories.StationID = addedEntity.Id;
                        await _inventioriesRepository.CreateAsync(inventories);
                    }
                    else
                    {
                        var inventories = ObjectMapper.Map<SelectStationInventoriesDto, UpdateStationInventoriesDto>(item);
                        inventories.StationID = addedEntity.Id;
                        await _inventioriesRepository.UpdateAsync(inventories);
                    }

                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectStationsDto>(ObjectMapper.Map<Stations, SelectStationsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.StationsRepository, id);
                await _uow.StationsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectStationsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationsRepository.GetAsync(t => t.Id == id, t => t.StationGroups, t => t.TemplateOperationLines, t => t.StationInventories);
                var mappedEntity = ObjectMapper.Map<Stations, SelectStationsDto>(entity);

                mappedEntity.SelectStationInventoriesDto = ObjectMapper.Map<List<StationInventories>, List<SelectStationInventoriesDto>>(entity.StationInventories.Where(t => t.StationID == id).ToList());
                return new SuccessDataResult<SelectStationsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationsDto>>> GetListAsync(ListStationsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.StationsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.StationGroups, t => t.TemplateOperationLines);

                var mappedEntity = ObjectMapper.Map<List<Stations>, List<ListStationsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListStationsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> UpdateAsync(UpdateStationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.StationsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateStationsDto, Stations>(input);

                await _uow.StationsRepository.UpdateAsync(mappedEntity);



                foreach (var item in input.SelectStationInventoriesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var inventories = ObjectMapper.Map<SelectStationInventoriesDto, CreateStationInventoriesDto>(item);
                        inventories.StationID = mappedEntity.Id;
                        await _inventioriesRepository.CreateAsync(inventories);
                    }
                    else
                    {
                        var inventories = ObjectMapper.Map<SelectStationInventoriesDto, UpdateStationInventoriesDto>(item);
                        inventories.StationID = mappedEntity.Id;
                        await _inventioriesRepository.UpdateAsync(inventories);
                    }

                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectStationsDto>(ObjectMapper.Map<Stations, SelectStationsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectStationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.StationsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Stations, SelectStationsDto>(updatedEntity);

                return new SuccessDataResult<SelectStationsDto>(mappedEntity);
            }
        }
    }
}
