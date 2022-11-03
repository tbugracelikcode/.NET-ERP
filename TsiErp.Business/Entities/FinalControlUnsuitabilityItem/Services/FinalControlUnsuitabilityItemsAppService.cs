using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.FinalControlUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityItemsAppService : ApplicationService, IFinalControlUnsuitabilityItemsAppService
    {
        private readonly IFinalControlUnsuitabilityItemsRepository _repository;

        FinalControlUnsuitabilityItemManager _manager { get; set; } = new FinalControlUnsuitabilityItemManager();

        public FinalControlUnsuitabilityItemsAppService(IFinalControlUnsuitabilityItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateFinalControlUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> CreateAsync(CreateFinalControlUnsuitabilityItemsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, null);
            var mappedEntity = ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(entity);
            return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFinalControlUnsuitabilityItemsDto>>> GetListAsync(ListFinalControlUnsuitabilityItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<FinalControlUnsuitabilityItems>, List<ListFinalControlUnsuitabilityItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListFinalControlUnsuitabilityItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateFinalControlUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(mappedEntity));
        }
    }
}
