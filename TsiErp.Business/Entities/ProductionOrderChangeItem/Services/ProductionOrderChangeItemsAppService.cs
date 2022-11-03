using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.ProductionOrderChangeItem.BusinessRules;
using TsiErp.Business.Entities.ProductionOrderChangeItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.Services
{
    [ServiceRegistration(typeof(IProductionOrderChangeItemsAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrderChangeItemsAppService : ApplicationService, IProductionOrderChangeItemsAppService
    {
        private readonly IProductionOrderChangeItemsRepository _repository;

        ProductionOrderChangeItemManager _manager { get; set; } = new ProductionOrderChangeItemManager();

        public ProductionOrderChangeItemsAppService(IProductionOrderChangeItemsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductionOrderChangeItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrderChangeItemsDto>> CreateAsync(CreateProductionOrderChangeItemsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateProductionOrderChangeItemsDto, ProductionOrderChangeItems>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductionOrderChangeItemsDto>(ObjectMapper.Map<ProductionOrderChangeItems, SelectProductionOrderChangeItemsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectProductionOrderChangeItemsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, null);
            var mappedEntity = ObjectMapper.Map<ProductionOrderChangeItems, SelectProductionOrderChangeItemsDto>(entity);
            return new SuccessDataResult<SelectProductionOrderChangeItemsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionOrderChangeItemsDto>>> GetListAsync(ListProductionOrderChangeItemsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<ProductionOrderChangeItems>, List<ListProductionOrderChangeItemsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductionOrderChangeItemsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateProductionOrderChangeItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrderChangeItemsDto>> UpdateAsync(UpdateProductionOrderChangeItemsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductionOrderChangeItemsDto, ProductionOrderChangeItems>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductionOrderChangeItemsDto>(ObjectMapper.Map<ProductionOrderChangeItems, SelectProductionOrderChangeItemsDto>(mappedEntity));
        }
    }
}
