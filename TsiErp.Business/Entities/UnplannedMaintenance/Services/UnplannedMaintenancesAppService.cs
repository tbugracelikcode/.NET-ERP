using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.UnplannedMaintenance.BusinessRules;
using TsiErp.Business.Entities.UnplannedMaintenance.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine.Dtos;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    [ServiceRegistration(typeof(IUnplannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class UnplannedMaintenancesAppService : ApplicationService, IUnplannedMaintenancesAppService
    {
        private readonly IUnplannedMaintenancesRepository _repository;
        private readonly IUnplannedMaintenanceLinesRepository _lineRepository;


        UnplannedMaintenanceManager _manager { get; set; } = new UnplannedMaintenanceManager();
        public UnplannedMaintenancesAppService(IUnplannedMaintenancesRepository repository, IUnplannedMaintenanceLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> CreateAsync(CreateUnplannedMaintenancesDto input)
        {
            await _manager.CodeControl(_repository, input.RegistrationNo);

            var entity = ObjectMapper.Map<CreateUnplannedMaintenancesDto, UnplannedMaintenances>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.UnplannedMaintenanceID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.UnplannedMaintenanceID, lines.Id, true);
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

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.UnplannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(entity);

            mappedEntity.SelectUnplannedMaintenanceLines = ObjectMapper.Map<List<UnplannedMaintenanceLines>, List<SelectUnplannedMaintenanceLinesDto>>(entity.UnplannedMaintenanceLines.ToList());

            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnplannedMaintenancesDto>>> GetListAsync(ListUnplannedMaintenancesParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.UnplannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<UnplannedMaintenances>, List<ListUnplannedMaintenancesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListUnplannedMaintenancesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateAsync(UpdateUnplannedMaintenancesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.RegistrationNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateUnplannedMaintenancesDto, UnplannedMaintenances>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectUnplannedMaintenanceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>(item);
                lineEntity.UnplannedMaintenanceID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectUnplannedMaintenancesDto>(ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(mappedEntity));
        }
    }
}
