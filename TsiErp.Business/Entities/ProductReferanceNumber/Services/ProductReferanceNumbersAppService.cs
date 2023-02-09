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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber;
using TsiErp.Business.Entities.ProductReferanceNumber.Validations;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Business.Entities.ProductReferanceNumber.BusinessRules;

namespace TsiErp.Business.Entities.ProductReferanceNumber.Services
{
    [ServiceRegistration(typeof(IProductReferanceNumbersAppService), DependencyInjectionType.Scoped)]
    public class ProductReferanceNumbersAppService : ApplicationService, IProductReferanceNumbersAppService
    {
        private readonly IProductReferanceNumbersRepository _repository;

        ProductReferanceNumberManager _manager { get; set; } = new ProductReferanceNumberManager();

        public ProductReferanceNumbersAppService(IProductReferanceNumbersRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> CreateAsync(CreateProductReferanceNumbersDto input)
        {
            await _manager.CodeControl(_repository, input.ReferanceNo);

            var entity = ObjectMapper.Map<CreateProductReferanceNumbersDto, ProductReferanceNumbers>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductReferanceNumbersDto>(ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectProductReferanceNumbersDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Products, t => t.CurrentAccountCards);
            var mappedEntity = ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(entity);
            return new SuccessDataResult<SelectProductReferanceNumbersDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductReferanceNumbersDto>>> GetListAsync(ListProductReferanceNumbersParameterDto input)
        {
            IList<ProductReferanceNumbers> list;

            if (input.ProductId == null)
            {
                list = await _repository.GetListAsync(null, t => t.Products);
            }
            else
            {
                list = await _repository.GetListAsync(t => t.ProductID == input.ProductId, t => t.Products);
            }

            var mappedEntity = ObjectMapper.Map<List<ProductReferanceNumbers>, List<ListProductReferanceNumbersDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductReferanceNumbersDto>>(mappedEntity);
        }

        public async Task<IDataResult<IList<SelectProductReferanceNumbersDto>>> GetSelectListAsync(Guid productId)
        {
            var list = await _repository.GetListAsync(t => t.ProductID == productId, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<ProductReferanceNumbers>, List<SelectProductReferanceNumbersDto>>(list.ToList());

            return new SuccessDataResult<IList<SelectProductReferanceNumbersDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateProductReferanceNumbersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductReferanceNumbersDto>> UpdateAsync(UpdateProductReferanceNumbersDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.ReferanceNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductReferanceNumbersDto, ProductReferanceNumbers>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductReferanceNumbersDto>(ObjectMapper.Map<ProductReferanceNumbers, SelectProductReferanceNumbersDto>(mappedEntity));
        }
    }
}
