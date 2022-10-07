using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup;
using TsiErp.Business.Entities.ProductGroup.Validations;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.ProductGroup;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ProductGroup.BusinessRules;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    [ServiceRegistration(typeof(IProductGroupsAppService), DependencyInjectionType.Scoped)]
    public class ProductGroupsAppService : ApplicationService, IProductGroupsAppService
    {
        private readonly IProductGroupsRepository _repository;

        ProductGroupManager _manager { get; set; } = new ProductGroupManager();

        public ProductGroupsAppService(IProductGroupsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> CreateAsync(CreateProductGroupsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateProductGroupsDto, ProductGroups>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectProductGroupsDto>(ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectProductGroupsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Products);
            var mappedEntity = ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(entity);
            return new SuccessDataResult<SelectProductGroupsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductGroupsDto>>> GetListAsync(ListProductGroupsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, x => x.Products);

            var mappedEntity = ObjectMapper.Map<List<ProductGroups>, List<ListProductGroupsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductGroupsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> UpdateAsync(UpdateProductGroupsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductGroupsDto, ProductGroups>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectProductGroupsDto>(ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(mappedEntity));
        }
    }
}
