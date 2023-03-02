using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.ContractUnsuitabilityItems.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ContractUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.ContractUnsuitabilityItem.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityItemsAppService : ApplicationService<ContractUnsuitabilityItemsResource>, IContractUnsuitabilityItemsAppService
    {
        public ContractUnsuitabilityItemsAppService(IStringLocalizer<ContractUnsuitabilityItemsResource> l) : base(l)
        {
        }

        ContractUnsuitabilityItemManager _manager { get; set; } = new ContractUnsuitabilityItemManager();


        [ValidationAspect(typeof(CreateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> CreateAsync(CreateContractUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ContractUnsuitabilityItemsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

                var addedEntity = await _uow.ContractUnsuitabilityItemsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ContractUnsuitabilityItems", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ContractUnsuitabilityItemsRepository.DeleteAsync(id);

                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ContractUnsuitabilityItems", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();


                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(entity);

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ContractUnsuitabilityItems", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractUnsuitabilityItemsDto>>> GetListAsync(ListContractUnsuitabilityItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractUnsuitabilityItemsRepository.GetListAsync(null);

                var mappedEntity = ObjectMapper.Map<List<ContractUnsuitabilityItems>, List<ListContractUnsuitabilityItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListContractUnsuitabilityItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateContractUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateAsync(UpdateContractUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ContractUnsuitabilityItemsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>(input);

                await _uow.ContractUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<ContractUnsuitabilityItems, UpdateContractUnsuitabilityItemsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ContractUnsuitabilityItems", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractUnsuitabilityItemsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ContractUnsuitabilityItemsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>(updatedEntity);

                return new SuccessDataResult<SelectContractUnsuitabilityItemsDto>(mappedEntity);
            }
        }
    }
}
