using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.PlannedMaintenance.BusinessRules;
using TsiErp.Business.Entities.PlannedMaintenance.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.PlannedMaintenanceLine.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    [ServiceRegistration(typeof(IPlannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class PlannedMaintenancesAppService : ApplicationService, IPlannedMaintenancesAppService
    {
        private readonly IPlannedMaintenancesRepository _repository;
        private readonly IPlannedMaintenanceLinesRepository _lineRepository;


        PlannedMaintenanceManager _manager { get; set; } = new PlannedMaintenanceManager();
        public PlannedMaintenancesAppService(IPlannedMaintenancesRepository repository, IPlannedMaintenanceLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> CreateAsync(CreatePlannedMaintenancesDto input)
        {
            await _manager.CodeControl(_repository, input.RegistrationNo);

            var entity = ObjectMapper.Map<CreatePlannedMaintenancesDto, PlannedMaintenances>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPlannedMaintenanceLinesDto, PlannedMaintenanceLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.PlannedMaintenanceID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectPlannedMaintenancesDto>(ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.PlannedMaintenanceID, lines.Id, true);
                await _lineRepository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
            else
            {
                await _manager.DeleteControl(_repository, id, Guid.Empty, false);
                await _repository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.PlannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(entity);

            mappedEntity.SelectPlannedMaintenanceLines = ObjectMapper.Map<List<PlannedMaintenanceLines>, List<SelectPlannedMaintenanceLinesDto>>(entity.PlannedMaintenanceLines.ToList());

            return new SuccessDataResult<SelectPlannedMaintenancesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPlannedMaintenancesDto>>> GetListAsync(ListPlannedMaintenancesParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.PlannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<PlannedMaintenances>, List<ListPlannedMaintenancesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPlannedMaintenancesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateAsync(UpdatePlannedMaintenancesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.RegistrationNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdatePlannedMaintenancesDto, PlannedMaintenances>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectPlannedMaintenanceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPlannedMaintenanceLinesDto, PlannedMaintenanceLines>(item);
                lineEntity.PlannedMaintenanceID = mappedEntity.Id;

                if (lineEntity.Id == Guid.Empty)
                {
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    await _lineRepository.InsertAsync(lineEntity);
                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                }
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();

            return new SuccessDataResult<SelectPlannedMaintenancesDto>(ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(mappedEntity));
        }
    }
}
