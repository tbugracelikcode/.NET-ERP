using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CustomerComplaintItem.BusinessRules;
using TsiErp.Business.Entities.CustomerComplaintItem.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    [ServiceRegistration(typeof(ICustomerComplaintItemsAppService), DependencyInjectionType.Scoped)]
    public class CustomerComplaintItemsAppService : ApplicationService, ICustomerComplaintItemsAppService
    {
        CustomerComplaintItemManager _manager { get; set; } = new CustomerComplaintItemManager();

        [ValidationAspect(typeof(CreateCustomerComplaintItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> CreateAsync(CreateCustomerComplaintItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CustomerComplaintItemsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateCustomerComplaintItemsDto, CustomerComplaintItems>(input);

                var addedEntity = await _uow.CustomerComplaintItemsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "CustomerComplaintItems", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCustomerComplaintItemsDto>(ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.CustomerComplaintItemsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "CustomerComplaintItems", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CustomerComplaintItemsRepository.GetAsync(t => t.Id == id, null);
                var mappedEntity = ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "CustomerComplaintItems", LogType.Get, id);

                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectCustomerComplaintItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCustomerComplaintItemsDto>>> GetListAsync(ListCustomerComplaintItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.CustomerComplaintItemsRepository.GetListAsync(t => t.IsActive == input.IsActive, null);

                var mappedEntity = ObjectMapper.Map<List<CustomerComplaintItems>, List<ListCustomerComplaintItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListCustomerComplaintItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateCustomerComplaintItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> UpdateAsync(UpdateCustomerComplaintItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CustomerComplaintItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.CustomerComplaintItemsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateCustomerComplaintItemsDto, CustomerComplaintItems>(input);

                await _uow.CustomerComplaintItemsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<CustomerComplaintItems, UpdateCustomerComplaintItemsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "CustomerComplaintItems", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCustomerComplaintItemsDto>(ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectCustomerComplaintItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CustomerComplaintItemsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.CustomerComplaintItemsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<CustomerComplaintItems, SelectCustomerComplaintItemsDto>(updatedEntity);

                return new SuccessDataResult<SelectCustomerComplaintItemsDto>(mappedEntity);
            }
        }
    }
}
