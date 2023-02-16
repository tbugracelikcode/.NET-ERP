using System.Reflection;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.OperationUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.OperationUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityItemsAppService : ApplicationService, IOperationUnsuitabilityItemsAppService
    {
        OperationUnsuitabilityItemManager _manager { get; set; } = new OperationUnsuitabilityItemManager();


        [ValidationAspect(typeof(CreateOperationUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> CreateAsync(CreateOperationUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.OperationUnsuitabilityItemsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>(input);

                var addedEntity = await _uow.OperationUnsuitabilityItemsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.OperationUnsuitabilityItemsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityItemsRepository.GetAsync(t => t.Id == id, null);
                var mappedEntity = ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(entity);
                return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationUnsuitabilityItemsDto>>> GetListAsync(ListOperationUnsuitabilityItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.OperationUnsuitabilityItemsRepository.GetListAsync(t => t.IsActive == input.IsActive, null);

                var mappedEntity = ObjectMapper.Map<List<OperationUnsuitabilityItems>, List<ListOperationUnsuitabilityItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListOperationUnsuitabilityItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateOperationUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> UpdateAsync(UpdateOperationUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.OperationUnsuitabilityItemsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>(input);

                await _uow.OperationUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityItemsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.OperationUnsuitabilityItemsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>(updatedEntity);

                return new SuccessDataResult<SelectOperationUnsuitabilityItemsDto>(mappedEntity);
            }
        }
    }
}
