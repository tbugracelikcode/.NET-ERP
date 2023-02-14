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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    [ServiceRegistration(typeof(IUnplannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class UnplannedMaintenancesAppService : ApplicationService, IUnplannedMaintenancesAppService
    {
        UnplannedMaintenanceManager _manager { get; set; } = new UnplannedMaintenanceManager();

        [ValidationAspect(typeof(CreateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> CreateAsync(CreateUnplannedMaintenancesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.UnplannedMaintenancesRepository, input.RegistrationNo);

                var entity = ObjectMapper.Map<CreateUnplannedMaintenancesDto, UnplannedMaintenances>(input);

                var addedEntity = await _uow.UnplannedMaintenancesRepository.InsertAsync(entity);

                foreach (var item in input.SelectUnplannedMaintenanceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.UnplannedMaintenanceID = addedEntity.Id;
                    await _uow.UnplannedMaintenanceLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectUnplannedMaintenancesDto>(ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.UnplannedMaintenanceLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.UnplannedMaintenancesRepository, lines.UnplannedMaintenanceID, lines.Id, true);
                    await _uow.UnplannedMaintenanceLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.UnplannedMaintenancesRepository, id, Guid.Empty, false);
                    await _uow.UnplannedMaintenancesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UnplannedMaintenancesRepository.GetAsync(t => t.Id == id,
                t => t.UnplannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(entity);

                mappedEntity.SelectUnplannedMaintenanceLines = ObjectMapper.Map<List<UnplannedMaintenanceLines>, List<SelectUnplannedMaintenanceLinesDto>>(entity.UnplannedMaintenanceLines.ToList());

                return new SuccessDataResult<SelectUnplannedMaintenancesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnplannedMaintenancesDto>>> GetListAsync(ListUnplannedMaintenancesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.UnplannedMaintenancesRepository.GetListAsync(null,
                t => t.UnplannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<UnplannedMaintenances>, List<ListUnplannedMaintenancesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListUnplannedMaintenancesDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateUnplannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnplannedMaintenancesDto>> UpdateAsync(UpdateUnplannedMaintenancesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UnplannedMaintenancesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.UnplannedMaintenancesRepository, input.RegistrationNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateUnplannedMaintenancesDto, UnplannedMaintenances>(input);

                await _uow.UnplannedMaintenancesRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectUnplannedMaintenanceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>(item);
                    lineEntity.UnplannedMaintenanceID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.UnplannedMaintenanceLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.UnplannedMaintenanceLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUnplannedMaintenancesDto>(ObjectMapper.Map<UnplannedMaintenances, SelectUnplannedMaintenancesDto>(mappedEntity));
            }
        }
    }
}
