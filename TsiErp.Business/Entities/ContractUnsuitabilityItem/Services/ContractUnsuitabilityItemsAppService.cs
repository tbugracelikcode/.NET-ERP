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
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.Business.Entities.ContractUnsuitabilityItem.Validations;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.ContractUnsuitabilityItem.BusinessRules;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityItemsAppService : ApplicationService, IContractUnsuitabilityItemsAppService
    {
        private readonly IContractUnsuitabilityItemsRepository _repository;

        ContractUnsuitabilityItemManager _manager { get; set; } = new ContractUnsuitabilityItemManager();

        public ContractUnsuitabilityItemsAppService(IContractUnsuitabilityItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> CreateAsync(CreateContractUnsuitabilityItemsDto input)
        {

            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(entity);
            return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractUnsuitabilityItemsDto>>> GetListAsync(ListContractUnsuitabilityItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<ContractUnsuitabilityItems>, List<ListContractUnsuitabilityItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListContractUnsuitabilityItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateAsync(UpdateContractUnsuitabilityItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(mappedEntity));
        }
    }
}
