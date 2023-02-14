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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityItemsAppService : ApplicationService, IContractUnsuitabilityItemsAppService
    {
        ContractUnsuitabilityItemManager _manager { get; set; } = new ContractUnsuitabilityItemManager();


        [ValidationAspect(typeof(CreateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> CreateAsync(CreateContractUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ContractUnsuitabilityItemsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

                var addedEntity = await _uow.ContractUnsuitabilityItemsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ContractUnsuitabilityItemsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(entity);
                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractUnsuitabilityItemsDto>>> GetListAsync(ListContractUnsuitabilityItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractUnsuitabilityItemsRepository.GetListAsync(null);

                var mappedEntity = ObjectMapper.Map<List<ContractUnsuitabilityItems>, List<ListContractUnsuitabilityItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListContractUnsuitabilityItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateAsync(UpdateContractUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ContractUnsuitabilityItemsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

                await _uow.ContractUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ContractUnsuitabilityItemsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(updatedEntity);

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(mappedEntity);
            }
        }
    }
}
