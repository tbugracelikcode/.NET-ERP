using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.MaintenanceInstruction.BusinessRules;
using TsiErp.Business.Entities.MaintenanceInstruction.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    [ServiceRegistration(typeof(IMaintenanceInstructionsAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceInstructionsAppService : ApplicationService, IMaintenanceInstructionsAppService
    {
        MaintenanceInstructionManager _manager { get; set; } = new MaintenanceInstructionManager();
        
        [ValidationAspect(typeof(CreateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> CreateAsync(CreateMaintenanceInstructionsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.MaintenanceInstructionsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

                var addedEntity = await _uow.MaintenanceInstructionsRepository.InsertAsync(entity);

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.InstructionID = addedEntity.Id;
                    await _uow.MaintenanceInstructionLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.MaintenanceInstructionLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.MaintenanceInstructionsRepository, lines.InstructionID, lines.Id, true);
                    await _uow.MaintenanceInstructionLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.MaintenanceInstructionsRepository, id, Guid.Empty, false);
                    await _uow.MaintenanceInstructionsRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenanceInstructionsRepository.GetAsync(t => t.Id == id,
                t => t.MaintenanceInstructionLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(entity);

                mappedEntity.SelectMaintenanceInstructionLines = ObjectMapper.Map<List<MaintenanceInstructionLines>, List<SelectMaintenanceInstructionLinesDto>>(entity.MaintenanceInstructionLines.ToList());

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceInstructionsDto>>> GetListAsync(ListMaintenanceInstructionsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.MaintenanceInstructionsRepository.GetListAsync(null,
                t => t.MaintenanceInstructionLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<MaintenanceInstructions>, List<ListMaintenanceInstructionsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListMaintenanceInstructionsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateAsync(UpdateMaintenanceInstructionsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenanceInstructionsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.MaintenanceInstructionsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

                await _uow.MaintenanceInstructionsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                    lineEntity.InstructionID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.MaintenanceInstructionLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.MaintenanceInstructionLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenanceInstructionsRepository.GetAsync(t => t.StationID == stationID && t.PeriodID == periodID,
                t => t.MaintenanceInstructionLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(entity);

                mappedEntity.SelectMaintenanceInstructionLines = ObjectMapper.Map<List<MaintenanceInstructionLines>, List<SelectMaintenanceInstructionLinesDto>>(entity.MaintenanceInstructionLines.ToList());

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(mappedEntity);
            }
        }
    }
}
