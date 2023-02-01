using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenancePeriod;
using TsiErp.Business.Entities.MaintenancePeriod.Validations;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Business.Entities.MaintenancePeriod.BusinessRules;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    [ServiceRegistration(typeof(IMaintenancePeriodsAppService), DependencyInjectionType.Scoped)]
    public class MaintenancePeriodsAppService : ApplicationService, IMaintenancePeriodsAppService
    {
        private readonly IMaintenancePeriodsRepository _repository;

        MaintenancePeriodManager _manager { get; set; } = new MaintenancePeriodManager();

        public MaintenancePeriodsAppService(IMaintenancePeriodsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> CreateAsync(CreateMaintenancePeriodsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateMaintenancePeriodsDto, MaintenancePeriods>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectMaintenancePeriodsDto>(ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectMaintenancePeriodsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(entity);
            return new SuccessDataResult<SelectMaintenancePeriodsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenancePeriodsDto>>> GetListAsync(ListMaintenancePeriodsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive);

            var mappedEntity = ObjectMapper.Map<List<MaintenancePeriods>, List<ListMaintenancePeriodsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListMaintenancePeriodsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateMaintenancePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenancePeriodsDto>> UpdateAsync(UpdateMaintenancePeriodsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateMaintenancePeriodsDto, MaintenancePeriods>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectMaintenancePeriodsDto>(ObjectMapper.Map<MaintenancePeriods, SelectMaintenancePeriodsDto>(mappedEntity));
        }
    }
}
