using System.Reflection;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.PurchasingUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IPurchasingUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class PurchasingUnsuitabilityItemsAppService : ApplicationService, IPurchasingUnsuitabilityItemsAppService
    {
        PurchasingUnsuitabilityItemManager _manager { get; set; } = new PurchasingUnsuitabilityItemManager();

        [ValidationAspect(typeof(CreatePurchasingUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> CreateAsync(CreatePurchasingUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchasingUnsuitabilityItemsRepository, input.Code);

                var entity = ObjectMapper.Map<CreatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

                var addedEntity = await _uow.PurchasingUnsuitabilityItemsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.PurchasingUnsuitabilityItemsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasingUnsuitabilityItemsRepository.GetAsync(t => t.Id == id, null);
                var mappedEntity = ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(entity);
                return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchasingUnsuitabilityItemsDto>>> GetListAsync(ListPurchasingUnsuitabilityItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchasingUnsuitabilityItemsRepository.GetListAsync(t => t.IsActive == input.IsActive, null);

                var mappedEntity = ObjectMapper.Map<List<PurchasingUnsuitabilityItems>, List<ListPurchasingUnsuitabilityItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPurchasingUnsuitabilityItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdatePurchasingUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> UpdateAsync(UpdatePurchasingUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasingUnsuitabilityItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PurchasingUnsuitabilityItemsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

                await _uow.PurchasingUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasingUnsuitabilityItemsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PurchasingUnsuitabilityItemsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(updatedEntity);

                return new SuccessDataResult<SelectPurchasingUnsuitabilityItemsDto>(mappedEntity);
            }
        }
    }
}
