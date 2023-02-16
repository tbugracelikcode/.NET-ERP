using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ContractProductionTracking.BusinessRules;
using TsiErp.Business.Entities.ContractProductionTracking.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    [ServiceRegistration(typeof(IContractProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ContractProductionTrackingsAppService : IContractProductionTrackingsAppService
    {
        ContractProductionTrackingManager _manager { get; set; } = new ContractProductionTrackingManager();

        [ValidationAspect(typeof(CreateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> CreateAsync(CreateContractProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ContractProductionTrackingsRepository);

                var entity = ObjectMapper.Map<CreateContractProductionTrackingsDto, ContractProductionTrackings>(input);

                var addedEntity = await _uow.ContractProductionTrackingsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ContractProductionTrackingsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(t => t.Id == id, t => t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations);
                var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(entity);
                return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractProductionTrackingsRepository.GetListAsync(null, t => t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<ListContractProductionTrackingsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListContractProductionTrackingsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateAsync(UpdateContractProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ContractProductionTrackingsRepository, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateContractProductionTrackingsDto, ContractProductionTrackings>(input);

                await _uow.ContractProductionTrackingsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractProductionTrackingsRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<SelectContractProductionTrackingsDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectContractProductionTrackingsDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ContractProductionTrackingsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(updatedEntity);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
            }
        }
    }
}
