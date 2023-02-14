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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

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
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(entity);
                return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractProductionTrackingsRepository.GetListAsync();

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
    }
}
