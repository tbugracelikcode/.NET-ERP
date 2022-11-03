using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Calendar.BusinessRules;
using TsiErp.Business.Entities.Calendar.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarLine;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.CalendarLine.Dtos;

namespace TsiErp.Business.Entities.Calendar.Services
{
    [ServiceRegistration(typeof(ICalendarsAppService), DependencyInjectionType.Scoped)]
    public class CalendarsAppService : ApplicationService, ICalendarsAppService
    {
        private readonly ICalendarsRepository _repository;
        private readonly ICalendarLinesRepository _lineRepository;

        CalendarManager _manager { get; set; } = new CalendarManager();

        public CalendarsAppService(ICalendarsRepository repository, ICalendarLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateCalendarsDto, Calendars>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectCalendarLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.CalendarID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectCalendarsDto>(ObjectMapper.Map<Calendars, SelectCalendarsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectCalendarsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.CalendarLines);

            var mappedEntity = ObjectMapper.Map<Calendars, SelectCalendarsDto>(entity);

            mappedEntity.SelectCalendarLinesDto = ObjectMapper.Map<List<CalendarLines>, List<SelectCalendarLinesDto>>(entity.CalendarLines.ToList());

            return new SuccessDataResult<SelectCalendarsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalendarsDto>>> GetListAsync(ListCalendarsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.CalendarLines);

            var mappedEntity = ObjectMapper.Map<List<Calendars>, List<ListCalendarsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCalendarsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> UpdateAsync(UpdateCalendarsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateCalendarsDto, Calendars>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectCalendarLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(item);
                lineEntity.CalendarID = mappedEntity.Id;
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

            return new SuccessDataResult<SelectCalendarsDto>(ObjectMapper.Map<Calendars, SelectCalendarsDto>(mappedEntity));
        }
    }
}
