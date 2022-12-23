using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ProductionTrackingHaltLine.BusinessRules;
using TsiErp.Business.Entities.ProductionTrackingHaltLine.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Business.Entities.ProductionTrackingHaltLine.Services
{
    [ServiceRegistration(typeof(IProductionTrackingHaltLinesAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingHaltLinesAppService : IProductionTrackingHaltLinesAppService
    {
        private readonly IProductionTrackingHaltLinesRepository _repository;

        ProductionTrackingHaltLineManager _manager { get; set; } = new ProductionTrackingHaltLineManager();

        public ProductionTrackingHaltLinesAppService(IProductionTrackingHaltLinesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductionTrackingHaltLinesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingHaltLinesDto>> CreateAsync(CreateProductionTrackingHaltLinesDto input)
        {

            var entity = ObjectMapper.Map<CreateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductionTrackingHaltLinesDto>(ObjectMapper.Map<ProductionTrackingHaltLines, SelectProductionTrackingHaltLinesDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectProductionTrackingHaltLinesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<ProductionTrackingHaltLines, SelectProductionTrackingHaltLinesDto>(entity);
            return new SuccessDataResult<SelectProductionTrackingHaltLinesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingHaltLinesDto>>> GetListAsync(ListProductionTrackingHaltLinesParameterDto input)
        {
            var list = await _repository.GetListAsync();

            var mappedEntity = ObjectMapper.Map<List<ProductionTrackingHaltLines>, List<ListProductionTrackingHaltLinesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductionTrackingHaltLinesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateProductionTrackingHaltLinesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingHaltLinesDto>> UpdateAsync(UpdateProductionTrackingHaltLinesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();
            return new SuccessDataResult<SelectProductionTrackingHaltLinesDto>(ObjectMapper.Map<ProductionTrackingHaltLines, SelectProductionTrackingHaltLinesDto>(mappedEntity));
        }
    }
}
