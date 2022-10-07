using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Product;
using TsiErp.Business.Entities.Product.Validations;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Product;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Product.BusinessRules;

namespace TsiErp.Business.Entities.Product.Services
{
    [ServiceRegistration(typeof(IProductsAppService), DependencyInjectionType.Scoped)]
    public class ProductsAppService : ApplicationService, IProductsAppService
    {
        private readonly IProductsRepository _repository;

        ProductManager _manager { get; set; } = new ProductManager();

        public ProductsAppService(IProductsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> CreateAsync(CreateProductsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateProductsDto, Products>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectProductsDto>(ObjectMapper.Map<Products, SelectProductsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectProductsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.ProductGroups, y => y.UnitSets, y => y.RouteLines, y => y.SalesPropositionLines);
            var mappedEntity = ObjectMapper.Map<Products, SelectProductsDto>(entity);
            return new SuccessDataResult<SelectProductsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsDto>>> GetListAsync(ListProductsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, x => x.ProductGroups, y => y.UnitSets, y => y.RouteLines, y => y.SalesPropositionLines);

            var mappedEntity = ObjectMapper.Map<List<Products>, List<ListProductsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> UpdateAsync(UpdateProductsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductsDto, Products>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectProductsDto>(ObjectMapper.Map<Products, SelectProductsDto>(mappedEntity));
        }
    }
}
