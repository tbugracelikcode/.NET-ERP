using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Shift.BusinessRules;
using TsiErp.Business.Entities.Shift.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.ShiftLine;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Shift.Services
{
    [ServiceRegistration(typeof(IShiftsAppService), DependencyInjectionType.Scoped)]
    public class ShiftsAppService : ApplicationService<BranchesResource>, IShiftsAppService
    {
        public ShiftsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        ShiftManager _manager { get; set; } = new ShiftManager();

        [ValidationAspect(typeof(CreateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> CreateAsync(CreateShiftsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ShiftsRepository, input.Code, input.ShiftOrder);

                var entity = ObjectMapper.Map<CreateShiftsDto, Shifts>(input);

                var addedEntity = await _uow.ShiftsRepository.InsertAsync(entity);

                foreach (var item in input.SelectShiftLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectShiftLinesDto, ShiftLines>(item);
                    lineEntity.ShiftID = addedEntity.Id;
                    await _uow.ShiftLinesRepository.InsertAsync(lineEntity);
                }

                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Shifts", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectShiftsDto>(ObjectMapper.Map<Shifts, SelectShiftsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.ShiftLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.ShiftsRepository, lines.Id);
                    await _uow.ShiftLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.ShiftsRepository, id);

                    var list = (await _uow.ShiftLinesRepository.GetListAsync(t => t.ShiftID == id));
                    foreach (var line in list)
                    {
                        await _uow.ShiftLinesRepository.DeleteAsync(line.Id);


                    }
                    await _uow.ShiftsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Shifts", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectShiftsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShiftsRepository.GetAsync(t => t.Id == id,
                t => t.ShiftLines);

                var mappedEntity = ObjectMapper.Map<Shifts, SelectShiftsDto>(entity);

                mappedEntity.SelectShiftLinesDto = ObjectMapper.Map<List<ShiftLines>, List<SelectShiftLinesDto>>(entity.ShiftLines.ToList());

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Shifts", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectShiftsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShiftsDto>>> GetListAsync(ListShiftsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ShiftsRepository.GetListAsync(null,
                t => t.ShiftLines);

                var mappedEntity = ObjectMapper.Map<List<Shifts>, List<ListShiftsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListShiftsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> UpdateAsync(UpdateShiftsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShiftsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ShiftsRepository, input.Code, input.Id, entity, input.ShiftOrder);

                var mappedEntity = ObjectMapper.Map<UpdateShiftsDto, Shifts>(input);

                await _uow.ShiftsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectShiftLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectShiftLinesDto, ShiftLines>(item);
                    lineEntity.ShiftID = mappedEntity.Id;
                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.ShiftLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ShiftLinesRepository.UpdateAsync(lineEntity);
                    }
                }
                var before = ObjectMapper.Map<Shifts, UpdateShiftsDto>(entity);
                before.SelectShiftLinesDto = ObjectMapper.Map<List<ShiftLines>, List<SelectShiftLinesDto>>(entity.ShiftLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Shifts", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectShiftsDto>(ObjectMapper.Map<Shifts, SelectShiftsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectShiftsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShiftsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ShiftsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Shifts, SelectShiftsDto>(updatedEntity);

                return new SuccessDataResult<SelectShiftsDto>(mappedEntity);
            }
        }
    }
}
