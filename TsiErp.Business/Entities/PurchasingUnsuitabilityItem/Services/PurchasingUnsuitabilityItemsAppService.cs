using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.PurchasingUnsuitabilityItems.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchasingUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IPurchasingUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class PurchasingUnsuitabilityItemsAppService : ApplicationService<PurchasingUnsuitabilityItemsResource>, IPurchasingUnsuitabilityItemsAppService
    {
        public PurchasingUnsuitabilityItemsAppService(IStringLocalizer<PurchasingUnsuitabilityItemsResource> l) : base(l)
        {
        }

        PurchasingUnsuitabilityItemManager _manager { get; set; } = new PurchasingUnsuitabilityItemManager();

        [ValidationAspect(typeof(CreatePurchasingUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> CreateAsync(CreatePurchasingUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchasingUnsuitabilityItemsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

                var addedEntity = await _uow.PurchasingUnsuitabilityItemsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PurchasingUnsuitabilityItems", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PurchasingUnsuitabilityItems", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasingUnsuitabilityItemsRepository.GetAsync(t => t.Id == id, null);
                var mappedEntity = ObjectMapper.Map<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PurchasingUnsuitabilityItems", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
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

                await _manager.UpdateControl(_uow.PurchasingUnsuitabilityItemsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>(input);

                await _uow.PurchasingUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<PurchasingUnsuitabilityItems, UpdatePurchasingUnsuitabilityItemsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PurchasingUnsuitabilityItems", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
