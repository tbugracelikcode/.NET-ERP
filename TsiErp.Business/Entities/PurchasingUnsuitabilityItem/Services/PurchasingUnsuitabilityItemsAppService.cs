using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IPurchasingUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class PurchasingUnsuitabilityItemsAppService : ApplicationService, IPurchasingUnsuitabilityItemsAppService
    {
        private readonly IPurchasingUnsuitabilityItemsRepository _repository;

        public PurchasingUnsuitabilityItemsAppService(IPurchasingUnsuitabilityItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreatePurchasingUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> CreateAsync(CreatePurchasingUnsuitabilityItemsDto input)
        {
            var entity = ObjectMapper.Map<CreatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, null);
            var mappedEntity = ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(entity);
            return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchasingUnsuitabilityItemsDto>>> GetListAsync(ListPurchasingUnsuitabilityItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<PurchasingUnsuitabilityItems>, List<ListPurchasingUnsuitabilityItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPurchasingUnsuitabilityItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdatePurchasingUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> UpdateAsync(UpdatePurchasingUnsuitabilityItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(mappedEntity));
        }
    }
}
