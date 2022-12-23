using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ProductionTracking.BusinessRules;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTracking;
using AutoMapper.Internal.Mappers;
using TsiErp.Business.Extensions.ObjectMapping;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : IProductionTrackingsAppService
    {
        private readonly IProductionTrackingsRepository _repository;

        ProductionTrackingManager _manager { get; set; } = new ProductionTrackingManager();

        public ProductionTrackingsAppService(IProductionTrackingsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {

            var entity = ObjectMapper.Map<CreateProductionTrackingsDto, ProductionTrackings>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(entity);
            return new SuccessDataResult<SelectProductionTrackingsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            var list = await _repository.GetListAsync();

            var mappedEntity = ObjectMapper.Map<List<ProductionTrackings>, List<ListProductionTrackingsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductionTrackingsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductionTrackingsDto, ProductionTrackings>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();
            return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(mappedEntity));
        }
    }
}
