using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.CustomerComplaintItem.BusinessRules;
using TsiErp.Business.Entities.CustomerComplaintItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    [ServiceRegistration(typeof(ICustomerComplaintItemsAppService), DependencyInjectionType.Scoped)]
    public class CustomerComplaintItemsAppService : ApplicationService, ICustomerComplaintItemsAppService
    {
        private readonly ICustomerComplaintItemsRepository _repository;

        CustomerComplaintItemManager _manager { get; set; } = new CustomerComplaintItemManager();

        public CustomerComplaintItemsAppService(ICustomerComplaintItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateCustomerComplaintItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> CreateAsync(CreateCustomerComplaintItemsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateCustomerComplaintItemsDto, CustomerComplaintItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectCustomerComplaintItemsDto>(ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, null);
            var mappedEntity = ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(entity);
            return new SuccessDataResult<SelectCustomerComplaintItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCustomerComplaintItemsDto>>> GetListAsync(ListCustomerComplaintItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<CustomerComplaintItems>, List<ListCustomerComplaintItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCustomerComplaintItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateCustomerComplaintItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> UpdateAsync(UpdateCustomerComplaintItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateCustomerComplaintItemsDto, CustomerComplaintItems>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectCustomerComplaintItemsDto>(ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(mappedEntity));
        }
    }
}
