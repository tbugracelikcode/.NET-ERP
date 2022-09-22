using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.OperationUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityItemsAppService : ApplicationService, IOperationUnsuitabilityItemsAppService
    {
        private readonly IOperationUnsuitabilityItemsRepository _repository;

        public OperationUnsuitabilityItemsAppService(IOperationUnsuitabilityItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateOperationUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> CreateAsync(CreateOperationUnsuitabilityItemsDto input)
        {
            var entity = ObjectMapper.Map<CreateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,null);
            var mappedEntity = ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(entity);
            return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationUnsuitabilityItemsDto>>> GetListAsync(ListOperationUnsuitabilityItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<OperationUnsuitabilityItems>, List<ListOperationUnsuitabilityItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListOperationUnsuitabilityItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateOperationUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> UpdateAsync(UpdateOperationUnsuitabilityItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(mappedEntity));
        }
    }
}
