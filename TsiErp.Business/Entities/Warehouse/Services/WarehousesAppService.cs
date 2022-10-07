using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Business.Entities.Warehouse.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.Warehouse.BusinessRules;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService, IWarehousesAppService
    {
        private readonly IWarehousesRepository _repository;

        WarehouseManager _manager { get; set; } = new WarehouseManager();

        public WarehousesAppService(IWarehousesRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateWarehousesDto, Warehouses>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t=>t.SalesPropositions, y=>y.SalesPropositionLines);
            var mappedEntity = ObjectMapper.Map<Warehouses, SelectWarehousesDto>(entity);
            return new SuccessDataResult<SelectWarehousesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.SalesPropositions, y => y.SalesPropositionLines);

            var mappedEntity = ObjectMapper.Map<List<Warehouses>, List<ListWarehousesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListWarehousesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateWarehousesDto, Warehouses>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(mappedEntity));
        }
    }
}
