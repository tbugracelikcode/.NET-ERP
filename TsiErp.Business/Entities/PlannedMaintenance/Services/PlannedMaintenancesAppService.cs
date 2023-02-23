using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PlannedMaintenance.BusinessRules;
using TsiErp.Business.Entities.PlannedMaintenance.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.PlannedMaintenanceLine.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    [ServiceRegistration(typeof(IPlannedMaintenancesAppService), DependencyInjectionType.Scoped)]
    public class PlannedMaintenancesAppService : ApplicationService, IPlannedMaintenancesAppService
    {
        PlannedMaintenanceManager _manager { get; set; } = new PlannedMaintenanceManager();

        [ValidationAspect(typeof(CreatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> CreateAsync(CreatePlannedMaintenancesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PlannedMaintenancesRepository, input.RegistrationNo);

                var entity = ObjectMapper.Map<CreatePlannedMaintenancesDto, PlannedMaintenances>(input);

                var addedEntity = await _uow.PlannedMaintenancesRepository.InsertAsync(entity);

                foreach (var item in input.SelectPlannedMaintenanceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPlannedMaintenanceLinesDto, PlannedMaintenanceLines>(item);
                    lineEntity.PlannedMaintenanceID = addedEntity.Id;
                    await _uow.PlannedMaintenanceLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PlannedMaintenances", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPlannedMaintenancesDto>(ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.PlannedMaintenanceLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.PlannedMaintenancesRepository, lines.PlannedMaintenanceID, lines.Id, true);
                    await _uow.PlannedMaintenanceLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.PlannedMaintenancesRepository, id, Guid.Empty, false);

                    var list = (await _uow.PlannedMaintenanceLinesRepository.GetListAsync(t => t.PlannedMaintenanceID == id));
                    foreach (var line in list)
                    {
                        await _uow.PlannedMaintenanceLinesRepository.DeleteAsync(line.Id);
                    }


                    await _uow.PlannedMaintenanceLinesRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PlannedMaintenances", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PlannedMaintenancesRepository.GetAsync(t => t.Id == id,
                t => t.PlannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(entity);

                mappedEntity.SelectPlannedMaintenanceLines = ObjectMapper.Map<List<PlannedMaintenanceLines>, List<SelectPlannedMaintenanceLinesDto>>(entity.PlannedMaintenanceLines.ToList());

                foreach (var item in mappedEntity.SelectPlannedMaintenanceLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                    item.UnitSetCode = (await _uow.UnitSetsRepository.GetAsync(t => t.Id == item.UnitSetID)).Code;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PlannedMaintenances", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPlannedMaintenancesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPlannedMaintenancesDto>>> GetListAsync(ListPlannedMaintenancesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PlannedMaintenancesRepository.GetListAsync(null,
                t => t.PlannedMaintenanceLines,
                t => t.MaintenancePeriods,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<PlannedMaintenances>, List<ListPlannedMaintenancesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPlannedMaintenancesDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdatePlannedMaintenanceValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateAsync(UpdatePlannedMaintenancesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PlannedMaintenancesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PlannedMaintenancesRepository, input.RegistrationNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePlannedMaintenancesDto, PlannedMaintenances>(input);

                await _uow.PlannedMaintenancesRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectPlannedMaintenanceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPlannedMaintenanceLinesDto, PlannedMaintenanceLines>(item);
                    lineEntity.PlannedMaintenanceID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.PlannedMaintenanceLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.PlannedMaintenanceLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<PlannedMaintenances, UpdatePlannedMaintenancesDto>(entity);
                before.SelectPlannedMaintenanceLines = ObjectMapper.Map<List<PlannedMaintenanceLines>, List<SelectPlannedMaintenanceLinesDto>>(entity.PlannedMaintenanceLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PlannedMaintenances", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPlannedMaintenancesDto>(ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPlannedMaintenancesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PlannedMaintenancesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PlannedMaintenancesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PlannedMaintenances, SelectPlannedMaintenancesDto>(updatedEntity);

                return new SuccessDataResult<SelectPlannedMaintenancesDto>(mappedEntity);
            }
        }
    }
}
