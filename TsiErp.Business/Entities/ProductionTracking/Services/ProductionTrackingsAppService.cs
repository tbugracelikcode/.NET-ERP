using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ProductionTracking.BusinessRules;
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService, IProductionTrackingsAppService
    {
        ProductionTrackingManager _manager { get; set; } = new ProductionTrackingManager();


        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateProductionTrackingsDto, ProductionTrackings>(input);

                var addedEntity = await _uow.ProductionTrackingsRepository.InsertAsync(entity);

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.ProductionTrackingID = addedEntity.Id;
                    await _uow.ProductionTrackingHaltLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.ProductionTrackingHaltLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.ProductionTrackingsRepository, lines.Id);
                    await _uow.ProductionTrackingHaltLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.ProductionTrackingsRepository, id);
                    await _uow.ProductionTrackingsRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionTrackingsRepository.GetAsync(t => t.Id == id,
                t => t.ProductionTrackingHaltLines);

                var mappedEntity = ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(entity);

                mappedEntity.SelectProductionTrackingHaltLines = ObjectMapper.Map<List<ProductionTrackingHaltLines>, List<SelectProductionTrackingHaltLinesDto>>(entity.ProductionTrackingHaltLines.ToList());

                return new SuccessDataResult<SelectProductionTrackingsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductionTrackingsRepository.GetListAsync(null,
                t => t.ProductionTrackingHaltLines);

                var mappedEntity = ObjectMapper.Map<List<ProductionTrackings>, List<ListProductionTrackingsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductionTrackingsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionTrackingsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductionTrackingsRepository, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateProductionTrackingsDto, ProductionTrackings>(input);

                await _uow.ProductionTrackingsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                    lineEntity.ProductionTrackingID = mappedEntity.Id;
                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.ProductionTrackingHaltLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ProductionTrackingHaltLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectProductionTrackingsDto>(ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionTrackingsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductionTrackingsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ProductionTrackings, SelectProductionTrackingsDto>(updatedEntity);

                return new SuccessDataResult<SelectProductionTrackingsDto>(mappedEntity);
            }
        }
    }
}
