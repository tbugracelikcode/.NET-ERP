using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
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

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    [ServiceRegistration(typeof(IContractProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ContractProductionTrackingsAppService : IContractProductionTrackingsAppService
    {
        private readonly IContractProductionTrackingsRepository _repository;

        ContractProductionTrackingManager _manager { get; set; } = new ContractProductionTrackingManager();

        public ContractProductionTrackingsAppService(IContractProductionTrackingsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> CreateAsync(CreateContractProductionTrackingsDto input)
        {
            await _manager.CodeControl(_repository);

            var entity = ObjectMapper.Map<CreateContractProductionTrackingsDto, ContractProductionTrackings>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t=>t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations );
            var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(entity);
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<ListContractProductionTrackingsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListContractProductionTrackingsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateAsync(UpdateContractProductionTrackingsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateContractProductionTrackingsDto, ContractProductionTrackings>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();
            return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(mappedEntity));
        }

        public async Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId)
        {
            var list = await _repository.GetListAsync(t => t.ProductID == productId, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<SelectContractProductionTrackingsDto>>(list.ToList());

            return new SuccessDataResult<IList<SelectContractProductionTrackingsDto>>(mappedEntity);
        }
    }
}
