using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.ContractProductionTrackings.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ContractProductionTracking.BusinessRules;
using TsiErp.Business.Entities.ContractProductionTracking.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;
using Microsoft.Extensions.Localization;
using TsiErp.Business.BusinessCoreServices;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    [ServiceRegistration(typeof(IContractProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ContractProductionTrackingsAppService : ApplicationService<ContractProductionTrackingsResource>, IContractProductionTrackingsAppService
    {
        ContractProductionTrackingManager _manager { get; set; } = new ContractProductionTrackingManager();

        public ContractProductionTrackingsAppService(IStringLocalizer<ContractProductionTrackingsResource> l) : base(l)
        {

        }

        [ValidationAspect(typeof(CreateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> CreateAsync(CreateContractProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ContractProductionTrackingsRepository, L);

                var entity = ObjectMapper.Map<CreateContractProductionTrackingsDto, ContractProductionTrackings>(input);

                var addedEntity = await _uow.ContractProductionTrackingsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ContractProductionTrackings", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ContractProductionTrackingsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ContractProductionTrackings", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(t => t.Id == id, t => t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations);
                var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ContractProductionTrackings", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractProductionTrackingsRepository.GetListAsync(null, t => t.Products, t => t.CurrentAccountCards, t => t.Employees, t => t.Shifts, t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<ListContractProductionTrackingsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListContractProductionTrackingsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateAsync(UpdateContractProductionTrackingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ContractProductionTrackingsRepository, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdateContractProductionTrackingsDto, ContractProductionTrackings>(input);

                await _uow.ContractProductionTrackingsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<ContractProductionTrackings, UpdateContractProductionTrackingsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ContractProductionTrackings", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectContractProductionTrackingsDto>(ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ContractProductionTrackingsRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ContractProductionTrackings>, List<SelectContractProductionTrackingsDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectContractProductionTrackingsDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ContractProductionTrackingsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ContractProductionTrackingsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ContractProductionTrackings, SelectContractProductionTrackingsDto>(updatedEntity);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(mappedEntity);
            }
        }
    }
}
