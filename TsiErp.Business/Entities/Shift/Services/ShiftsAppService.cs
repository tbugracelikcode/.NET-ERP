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

namespace TsiErp.Business.Entities.Shift.Services
{
    [ServiceRegistration(typeof(IShiftsAppService), DependencyInjectionType.Scoped)]
    public class ShiftsAppService : IShiftsAppService
    {
        private readonly IShiftsRepository _repository;
        private readonly IShiftLinesRepository _lineRepository;

        ShiftManager _manager { get; set; } = new ShiftManager();

        public ShiftsAppService(IShiftsRepository repository, IShiftLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> CreateAsync(CreateShiftsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateShiftsDto, Shifts>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectShiftLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectShiftLinesDto, ShiftLines>(item);
                lineEntity.ShiftID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            return new SuccessDataResult<SelectShiftsDto>(ObjectMapper.Map<Shifts, SelectShiftsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);

            //var lines = (await _lineRepository.GetListAsync(t => t.SalesPropositionID == id)).ToList();

            //foreach (var line in lines)
            //{
            //    await _lineRepository.DeleteAsync(line.Id);
            //}

            await _repository.DeleteAsync(id);

            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectShiftsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.ShiftLines
               /* t => t.CurrentAccountCards,*/);

            var mappedEntity = ObjectMapper.Map<Shifts, SelectShiftsDto>(entity);

            mappedEntity.SelectShiftLinesDto = ObjectMapper.Map<List<ShiftLines>, List<SelectShiftLinesDto>>(entity.ShiftLines.ToList());

            return new SuccessDataResult<SelectShiftsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShiftsDto>>> GetListAsync(ListShiftsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.ShiftLines
                //t => t.CurrentAccountCards,
                //t => t.Warehouses,
                //t => t.Branches,
                //t => t.Currencies,
                /*t => t.PaymentPlan*/);

            var mappedEntity = ObjectMapper.Map<List<Shifts>, List<ListShiftsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListShiftsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> UpdateAsync(UpdateShiftsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateShiftsDto, Shifts>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectShiftLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectShiftLinesDto, ShiftLines>(item);
                lineEntity.ShiftID = mappedEntity.Id;
                await _lineRepository.UpdateAsync(lineEntity);
            }

            return new SuccessDataResult<SelectShiftsDto>(ObjectMapper.Map<Shifts, SelectShiftsDto>(mappedEntity));
        }

    }
}
