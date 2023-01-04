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
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using Tsi.Core.Utilities.Guids;
using Tsi.Application.Contract.Services.EntityFrameworkCore;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService, IProductionTrackingsAppService
    {
        private readonly IProductionTrackingsRepository _repository;
        private readonly IProductionTrackingHaltLinesRepository _lineRepository;

        ProductionTrackingManager _manager { get; set; } = new ProductionTrackingManager();

        public ProductionTrackingsAppService(IProductionTrackingsRepository repository, IProductionTrackingHaltLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateProductionTrackingsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            var entity = ObjectMapper.Map<CreateProductionTrackingsDto, ProductionTrackings>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectProductionTrackingHaltLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.ProductionTrackingID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();

            return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.Id);
                await _lineRepository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
            else
            {
                await _manager.DeleteControl(_repository, id);
                await _repository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.ProductionTrackingHaltLines);

            var mappedEntity = ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(entity);

            mappedEntity.SelectProductionTrackingHaltLines = ObjectMapper.Map<List<ProductionTrackingHaltLines>, List<SelectProductionTrackingHaltLinesDto>>(entity.ProductionTrackingHaltLines.ToList());

            return new SuccessDataResult<SelectProductionTrackingsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.ProductionTrackingHaltLines);

            var mappedEntity = ObjectMapper.Map<List<ProductionTrackings>, List<ListProductionTrackingsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductionTrackingsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductionTrackingsDto, ProductionTrackings>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectProductionTrackingHaltLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                lineEntity.ProductionTrackingID = mappedEntity.Id;
                if (lineEntity.Id == Guid.Empty)
                {
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    await _lineRepository.InsertAsync(lineEntity);
                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                }
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(mappedEntity));
        }

    }
}
