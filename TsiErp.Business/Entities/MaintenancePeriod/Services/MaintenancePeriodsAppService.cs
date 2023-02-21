using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.MaintenancePeriod.BusinessRules;
using TsiErp.Business.Entities.MaintenancePeriod.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    [ServiceRegistration(typeof(IMaintenancePeriodsAppService), DependencyInjectionType.Scoped)]
    public class MaintenancePeriodsAppService : ApplicationService, IMaintenancePeriodsAppService
    {
        MaintenancePeriodManager _manager { get; set; } = new MaintenancePeriodManager();


        [ValidationAspect(typeof(CreateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> CreateAsync(CreateMaintenancePeriodsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.MaintenancePeriodsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateMaintenancePeriodsDto, MaintenancePeriods>(input);

                var addedEntity = await _uow.MaintenancePeriodsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.MaintenancePeriodsRepository, id);
                await _uow.MaintenancePeriodsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectMaintenancePeriodsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenancePeriodsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(entity);
                return new SuccessDataResult<SelectMaintenancePeriodsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenancePeriodsDto>>> GetListAsync(ListMaintenancePeriodsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.MaintenancePeriodsRepository.GetListAsync(t => t.IsActive == input.IsActive);

                var mappedEntity = ObjectMapper.Map<List<MaintenancePeriods>, List<ListMaintenancePeriodsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListMaintenancePeriodsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateAsync(UpdateMaintenancePeriodsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenancePeriodsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.MaintenancePeriodsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateMaintenancePeriodsDto, MaintenancePeriods>(input);

                await _uow.MaintenancePeriodsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenancePeriodsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.MaintenancePeriodsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(updatedEntity);

                return new SuccessDataResult<SelectMaintenancePeriodsDto>(mappedEntity);
            }
        }
    }
}
