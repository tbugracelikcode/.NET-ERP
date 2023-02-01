using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.MaintenanceInstruction.BusinessRules;
using TsiErp.Business.Entities.MaintenanceInstruction.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
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
        private readonly IMaintenanceInstructionsRepository _repository;
        private readonly IMaintenanceInstructionLinesRepository _lineRepository;


        MaintenanceInstructionManager _manager { get; set; } = new MaintenanceInstructionManager();
        public MaintenanceInstructionsAppService(IMaintenanceInstructionsRepository repository, IMaintenanceInstructionLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> CreateAsync(CreateMaintenanceInstructionsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectMaintenanceInstructionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.InstructionID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.InstructionID, lines.Id, true);
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

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.MaintenanceInstructionLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(entity);

            mappedEntity.SelectMaintenanceInstructionLines = ObjectMapper.Map<List<MaintenanceInstructionLines>, List<SelectMaintenanceInstructionLinesDto>>(entity.MaintenanceInstructionLines.ToList());

            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceInstructionsDto>>> GetListAsync(ListMaintenanceInstructionsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.MaintenanceInstructionLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<MaintenanceInstructions>, List<ListMaintenanceInstructionsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListMaintenanceInstructionsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateAsync(UpdateMaintenanceInstructionsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectMaintenanceInstructionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                lineEntity.InstructionID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectMaintenanceInstructionsDto>(ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(mappedEntity));
        }
    }
}
