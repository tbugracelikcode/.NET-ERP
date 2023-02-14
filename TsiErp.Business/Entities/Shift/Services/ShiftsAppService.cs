using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Shift.Services;
using TsiErp.Business.Entities.Shift.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShiftLine;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using TsiErp.Entities.Entities.ShiftLine;
using Tsi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using TsiErp.Business.Entities.Shift.BusinessRules;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.Shift;
using Tsi.Core.Utilities.Guids;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.Shift.Services
{
    [ServiceRegistration(typeof(IShiftsAppService), DependencyInjectionType.Scoped)]
    public class ShiftsAppService : ApplicationService, IShiftsAppService
    {
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
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.ShiftID = addedEntity.Id;
                    await _uow.ShiftLinesRepository.InsertAsync(lineEntity);
                }

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
                    await _uow.ShiftsRepository.DeleteAsync(id);
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
                t => t.ShiftLines,
                t => t.CalendarLines);

                var mappedEntity = ObjectMapper.Map<Shifts, SelectShiftsDto>(entity);

                mappedEntity.SelectShiftLinesDto = ObjectMapper.Map<List<ShiftLines>, List<SelectShiftLinesDto>>(entity.ShiftLines.ToList());

                return new SuccessDataResult<SelectShiftsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShiftsDto>>> GetListAsync(ListShiftsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ShiftsRepository.GetListAsync(null,
                t => t.ShiftLines,
                t => t.CalendarLines);

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
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.ShiftLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ShiftLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectShiftsDto>(ObjectMapper.Map<Shifts, SelectShiftsDto>(mappedEntity));
            }
        }
    }
}
