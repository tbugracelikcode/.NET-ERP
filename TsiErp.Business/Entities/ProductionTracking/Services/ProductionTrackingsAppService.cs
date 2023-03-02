using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.ProductionTrackings.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductionTracking.BusinessRules;
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService<ProductionTrackingsResource>, IProductionTrackingsAppService
    {
        public ProductionTrackingsAppService(IStringLocalizer<ProductionTrackingsResource> l) : base(l)
        {
        }

        ProductionTrackingManager _manager { get; set; } = new ProductionTrackingManager();


        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductionTrackingsRepository, input.Code, L);

                var entity = ObjectMapper.Map<CreateProductionTrackingsDto, ProductionTrackings>(input);

                var addedEntity = await _uow.ProductionTrackingsRepository.InsertAsync(entity);

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                    lineEntity.ProductionTrackingID = addedEntity.Id;
                    await _uow.ProductionTrackingHaltLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ProductionTrackings", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

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
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    await _manager.DeleteControl(_uow.ProductionTrackingsRepository, id);

                    var list = (await _uow.ProductionTrackingHaltLinesRepository.GetListAsync(t => t.ProductionTrackingID == id));
                    foreach (var line in list)
                    {
                        await _uow.ProductionTrackingHaltLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.ProductionTrackingsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ProductionTrackings", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
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

                foreach (var item in mappedEntity.SelectProductionTrackingHaltLines)
                {
                    item.HaltCode = (await _uow.HaltReasonsRepository.GetAsync(t => t.Id == item.HaltID)).Code;
                    item.HaltName = (await _uow.HaltReasonsRepository.GetAsync(t => t.Id == item.HaltID)).Name;
                }
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ProductionTrackings", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);

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

                await _manager.UpdateControl(_uow.ProductionTrackingsRepository, input.Code, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdateProductionTrackingsDto, ProductionTrackings>(input);

                await _uow.ProductionTrackingsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>(item);
                    lineEntity.ProductionTrackingID = mappedEntity.Id;
                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.ProductionTrackingHaltLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ProductionTrackingHaltLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<ProductionTrackings, UpdateProductionTrackingsDto>(entity);
                before.SelectProductionTrackingHaltLinesDto = ObjectMapper.Map<List<ProductionTrackingHaltLines>, List<SelectProductionTrackingHaltLinesDto>>(entity.ProductionTrackingHaltLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ProductionTrackings", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

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
