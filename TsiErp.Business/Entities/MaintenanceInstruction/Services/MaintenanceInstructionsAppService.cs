using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.MaintenanceInstructions.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MaintenanceInstruction.BusinessRules;
using TsiErp.Business.Entities.MaintenanceInstruction.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    [ServiceRegistration(typeof(IMaintenanceInstructionsAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceInstructionsAppService : ApplicationService<MaintenanceInstructionsResource>, IMaintenanceInstructionsAppService
    {
        public MaintenanceInstructionsAppService(IStringLocalizer<MaintenanceInstructionsResource> l) : base(l)
        {
        }

        MaintenanceInstructionManager _manager { get; set; } = new MaintenanceInstructionManager();
        
        [ValidationAspect(typeof(CreateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> CreateAsync(CreateMaintenanceInstructionsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.MaintenanceInstructionsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

                var addedEntity = await _uow.MaintenanceInstructionsRepository.InsertAsync(entity);

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                    lineEntity.InstructionID = addedEntity.Id;
                    await _uow.MaintenanceInstructionLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "MaintenanceInstructions", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    await _manager.DeleteControl(_uow.MaintenanceInstructionsRepository, id, Guid.Empty, false);

                    var list = (await _uow.MaintenanceInstructionLinesRepository.GetListAsync(t => t.InstructionID == id));
                    foreach (var line in list)
                    {
                        await _uow.MaintenanceInstructionLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.MaintenanceInstructionsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "MaintenanceInstructions", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
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

                foreach (var item in mappedEntity.SelectMaintenanceInstructionLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.UnitSetCode = (await _uow.UnitSetsRepository.GetAsync(t => t.Id == item.UnitSetID)).Code;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "MaintenanceInstructions", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

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

                await _manager.UpdateControl(_uow.MaintenanceInstructionsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateMaintenanceInstructionsDto, MaintenanceInstructions>(input);

                await _uow.MaintenanceInstructionsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>(item);
                    lineEntity.InstructionID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.MaintenanceInstructionLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.MaintenanceInstructionLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<MaintenanceInstructions, UpdateMaintenanceInstructionsDto>(entity);
                before.SelectMaintenanceInstructionLines = ObjectMapper.Map<List<MaintenanceInstructionLines>, List<SelectMaintenanceInstructionLinesDto>>(entity.MaintenanceInstructionLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "MaintenanceInstructions", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

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

                SelectMaintenanceInstructionsDto mappedEntity = new SelectMaintenanceInstructionsDto();

                if(entity != null)
                {
                    mappedEntity = ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(entity);

                    mappedEntity.SelectMaintenanceInstructionLines = ObjectMapper.Map<List<MaintenanceInstructionLines>, List<SelectMaintenanceInstructionLinesDto>>(entity.MaintenanceInstructionLines.ToList());
                }

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MaintenanceInstructionsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.MaintenanceInstructionsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<MaintenanceInstructions, SelectMaintenanceInstructionsDto>(updatedEntity);

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(mappedEntity);
            }
        }
    }
}
