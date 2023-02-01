using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory;
using TsiErp.Business.Entities.StationInventory.Validations;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.StationInventory;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.StationInventory.BusinessRules;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    [ServiceRegistration(typeof(IStationInventoriesAppService), DependencyInjectionType.Scoped)]
    public class StationInventoriesAppService : ApplicationService, IStationInventoriesAppService
    {
        private readonly IStationInventoriesRepository _repository;

        StationInventoryManager _manager { get; set; } = new StationInventoryManager();

        public StationInventoriesAppService(IStationInventoriesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> CreateAsync(CreateStationInventoriesDto input)
        {

            await _manager.ProductControl(_repository, input.ProductID.GetValueOrDefault(), input.StationID.GetValueOrDefault());

            var entity = ObjectMapper.Map<CreateStationInventoriesDto, StationInventories>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectStationInventoriesDto>(ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectStationInventoriesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Stations);
            var mappedEntity = ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(entity);
            return new SuccessDataResult<SelectStationInventoriesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationInventoriesDto>>> GetListAsync(ListStationInventoriesParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<StationInventories>, List<ListStationInventoriesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListStationInventoriesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateStationInventoriesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationInventoriesDto>> UpdateAsync(UpdateStationInventoriesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateProductControl(_repository, input.ProductID.GetValueOrDefault(), input.StationID.GetValueOrDefault(), input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateStationInventoriesDto, StationInventories>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectStationInventoriesDto>(ObjectMapper.Map<StationInventories, SelectStationInventoriesDto>(mappedEntity));
        }
    }
}
