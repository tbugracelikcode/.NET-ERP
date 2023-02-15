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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService, IWarehousesAppService
    {
        WarehouseManager _manager { get; set; } = new WarehouseManager();


        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.WarehousesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateWarehousesDto, Warehouses>(input);

                var addedEntity = await _uow.WarehousesRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.WarehousesRepository, id);
                await _uow.WarehousesRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(t => t.Id == id, t => t.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<Warehouses, SelectWarehousesDto>(entity);
                return new SuccessDataResult<SelectWarehousesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.WarehousesRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<Warehouses>, List<ListWarehousesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListWarehousesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.WarehousesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateWarehousesDto, Warehouses>(input);

                await _uow.WarehousesRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectWarehousesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.WarehousesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Warehouses, SelectWarehousesDto>(updatedEntity);

                return new SuccessDataResult<SelectWarehousesDto>(mappedEntity);
            }
        }
    }
}
