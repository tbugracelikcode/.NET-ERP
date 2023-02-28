using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Calendar.BusinessRules;
using TsiErp.Business.Entities.Calendar.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarDay;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarLine;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.CalendarLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Calendar.Services
{
    [ServiceRegistration(typeof(ICalendarsAppService), DependencyInjectionType.Scoped)]
    public class CalendarsAppService : ApplicationService<BranchesResource>, ICalendarsAppService
    {
        private readonly ICalendarsRepository _repository;
        private readonly ICalendarLinesRepository _lineRepository;
        private readonly ICalendarDaysRepository _dayRepository;
        private readonly IShiftsRepository _shiftsRepository;
        private readonly IStationsRepository _stationsRepository;

        public CalendarsAppService(IStringLocalizer<BranchesResource> l, ICalendarsRepository repository, ICalendarLinesRepository lineRepository, ICalendarDaysRepository dayRepository, IShiftsRepository shiftsRepository, IStationsRepository stationsRepository) : base(l)
        {
            _repository = repository;
            _lineRepository = lineRepository;
            _dayRepository = dayRepository;
            _shiftsRepository = shiftsRepository;
            _stationsRepository = stationsRepository;
        }

        CalendarManager _manager { get; set; } = new CalendarManager();



        [ValidationAspect(typeof(CreateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            #region Planlı çalışma takvimi satırları

            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateCalendarsDto, Calendars>(input);
            var addedEntity = await _repository.InsertAsync(entity);

            var shiftList = (await _shiftsRepository.GetListAsync()).OrderBy(t => t.ShiftOrder).ToList();
            var stationList = await _stationsRepository.GetListAsync();

            var yearDays = DateRange(new DateTime(addedEntity.Year, 1, 1), new DateTime(addedEntity.Year + 1, 1, 1).AddDays(-1)).ToList();

            // Seçilen yıl için loop
            for (int i = 0; i < yearDays.Count; i++)
            {
                // Makine sayısı kadar loop
                for (int k = 0; k < stationList.Count; k++)
                {
                    // Aktif Vardiya sayısı kadar loop
                    for (int j = 0; j < shiftList.Count; j++)
                    {
                        // Eğer eklenecek gün resmi tatil olarak seçildiyse çalışma ve duruş sürelerini 0 yap
                        if (input.SelectCalendarDaysDto.Select(t => t.Date_).Contains(yearDays[i]))
                        {
                            var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(new SelectCalendarLinesDto());
                            lineEntity.Id = GuidGenerator.CreateGuid();
                            lineEntity.CalendarID = addedEntity.Id;
                            lineEntity.StationID = stationList[k].Id;
                            lineEntity.ShiftID = shiftList[j].Id;
                            lineEntity.AvailableTime = 0;
                            lineEntity.ShiftOverTime = 0;
                            lineEntity.PlannedHaltTimes = 0;
                            lineEntity.ShiftTime = 0;
                            await _lineRepository.InsertAsync(lineEntity);
                        }
                        // Resmi tatil değilse vardiyadan süreleri al
                        else
                        {
                            var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(new SelectCalendarLinesDto());
                            lineEntity.Id = GuidGenerator.CreateGuid();
                            lineEntity.CalendarID = addedEntity.Id;
                            lineEntity.StationID = stationList[k].Id;
                            lineEntity.ShiftID = shiftList[j].Id;
                            lineEntity.AvailableTime = shiftList[j].NetWorkTime;
                            lineEntity.ShiftOverTime = shiftList[j].Overtime;
                            lineEntity.PlannedHaltTimes = shiftList[j].TotalBreakTime;
                            lineEntity.ShiftTime = shiftList[j].TotalWorkTime;
                            await _lineRepository.InsertAsync(lineEntity);
                        }
                    }
                }

                // Tüm günler için çalışma var satırları oluşturuldu
                var dayEntity = ObjectMapper.Map<SelectCalendarDaysDto, CalendarDays>(new SelectCalendarDaysDto());
                dayEntity.Id = GuidGenerator.CreateGuid();
                dayEntity.CalendarID = addedEntity.Id;
                dayEntity.Date_ = yearDays[i];
                dayEntity.CalendarDayStateEnum = 1;
                dayEntity.ColorCode = "#ffaa00";
                await _dayRepository.InsertAsync(dayEntity);

            }

            //Resmi tatil günleri satırları oluşturuldu
            for (int l = 0; l < input.SelectCalendarDaysDto.Count; l++)
            {
                var officialHolidayEntity = ObjectMapper.Map<SelectCalendarDaysDto, CalendarDays>(new SelectCalendarDaysDto());
                officialHolidayEntity.Id = GuidGenerator.CreateGuid();
                officialHolidayEntity.CalendarID = addedEntity.Id;
                officialHolidayEntity.Date_ = input.SelectCalendarDaysDto[l].Date_;
                officialHolidayEntity.CalendarDayStateEnum = 3;
                officialHolidayEntity.ColorCode = "#f8a398";
                await _dayRepository.InsertAsync(officialHolidayEntity);
            }


            #endregion

            #region Plansız çalışma takvimi satırları
            //var entityUnplanned = ObjectMapper.Map<CreateCalendarsDto, Calendars>(input);
            //entityUnplanned.IsPlanned = false;
            //var addedEntityUnplanned = await _repository.InsertAsync(entityUnplanned);

            //// Seçilen yıl için loop
            //for (int i = 0; i < yearDays.Count; i++)
            //{
            //    // Makine sayısı kadar loop
            //    for (int k = 0; k < stationList.Count; k++)
            //    {
            //        // Aktif Vardiya sayısı kadar loop
            //        for (int j = 0; j < shiftList.Count; j++)
            //        {
            //            // Eğer eklenecek gün resmi tatil olarak seçildiyse çalışma ve duruş sürelerini 0 yap
            //            if (input.SelectCalendarDaysDto.Select(t => t.Date_).Contains(yearDays[i]))
            //            {
            //                var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(new SelectCalendarLinesDto());
            //                lineEntity.Id = GuidGenerator.CreateGuid();
            //                lineEntity.CalendarID = addedEntityUnplanned.Id;
            //                lineEntity.StationID = stationList[k].Id;
            //                lineEntity.ShiftID = shiftList[j].Id;
            //                lineEntity.AvailableTime = shiftList[j].NetWorkTime;
            //                lineEntity.ShiftOverTime = 0;
            //                lineEntity.PlannedHaltTimes = 0;
            //                lineEntity.ShiftTime = 0;
            //                await _lineRepository.InsertAsync(lineEntity);
            //            }
            //            // Resmi tatil değilse vardiyadan süreleri al
            //            else
            //            {
            //                var lineEntity = ObjectMapper.Map<SelectCalendarLinesDto, CalendarLines>(new SelectCalendarLinesDto());
            //                lineEntity.Id = GuidGenerator.CreateGuid();
            //                lineEntity.CalendarID = addedEntityUnplanned.Id;
            //                lineEntity.StationID = stationList[k].Id;
            //                lineEntity.ShiftID = shiftList[j].Id;
            //                lineEntity.AvailableTime = shiftList[j].NetWorkTime;
            //                lineEntity.ShiftOverTime = shiftList[j].Overtime;
            //                lineEntity.PlannedHaltTimes = shiftList[j].TotalBreakTime;
            //                lineEntity.ShiftTime = shiftList[j].TotalWorkTime;
            //                await _lineRepository.InsertAsync(lineEntity);
            //            }
            //        }
            //    }

            //    var dayEntity = ObjectMapper.Map<SelectCalendarDaysDto, CalendarDays>(new SelectCalendarDaysDto());
            //    dayEntity.Id = GuidGenerator.CreateGuid();
            //    dayEntity.CalendarID = addedEntityUnplanned.Id;

            //    // Day entitysi için 365 günlük çalışma var satırı oluştur ancak eklenen resmi tatil günlerini hariç tut
            //    if (input.SelectCalendarDaysDto.Select(t => t.Date_).Contains(yearDays[i]))
            //    {
            //        dayEntity.IsWorkDay = true;
            //        dayEntity.IsHalfDay = false;
            //        dayEntity.IsHoliday = false;
            //        dayEntity.IsOfficialHoliday = false;
            //        dayEntity.IsMaintenance = false;
            //        dayEntity.IsNotWorkDay = false;
            //        dayEntity.IsShipmentDay = false;
            //    }
            //    // Resmi tatil günlerini burada ekle
            //    else
            //    {
            //        dayEntity.IsOfficialHoliday = true;
            //        dayEntity.IsWorkDay = false;
            //        dayEntity.IsHalfDay = false;
            //        dayEntity.IsHoliday = false;
            //        dayEntity.IsMaintenance = false;
            //        dayEntity.IsNotWorkDay = false;
            //        dayEntity.IsShipmentDay = false;
            //    }
            //    await _dayRepository.InsertAsync(dayEntity);
            //}
            #endregion


            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            await _dayRepository.SaveChanges();
            return new SuccessDataResult<SelectCalendarsDto>(ObjectMapper.Map<Calendars, SelectCalendarsDto>(addedEntity));
        }

        public IEnumerable<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d));
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

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID)
        {
            var list = await _dayRepository.GetListAsync(t => t.CalendarID == calendarID);

            var mappedEntity = ObjectMapper.Map<List<CalendarDays>, List<SelectCalendarDaysDto>>(list.ToList());

            return new SuccessDataResult<IList<SelectCalendarDaysDto>>(mappedEntity.Select(t => new SelectCalendarDaysDto()
            {
                CalendarID = calendarID,
                CalendarDayStateEnum = t.CalendarDayStateEnum,
                ColorCode = t.ColorCode,
                Date_ = t.Date_,
                EndTime = t.Date_.AddHours(1),
                StartTime = t.Date_,
                Id = t.Id,
                Subject = SubjectConvert(t.CalendarDayStateEnum)
            }).ToList());
        }

        private string SubjectConvert(int subjectCode)
        {
            string subject = "";
            switch (subjectCode)
            {
                case 1:
                    subject = "Çalışma Var";
                    break;
                case 2:
                    subject = "Çalışma Yok";
                    break;
                case 3:
                    subject = "Resmi Tatil";
                    break;
                case 4:
                    subject = "Tatil";
                    break;
                case 5:
                    subject = "Yarım Gün";
                    break;
                case 6:
                    subject = "Yükleme Günü";
                    break;
                case 7:
                    subject = "Bakım";
                    break;
                default:
                    break;
            }
            return subject;
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
                    await _lineRepository.SaveChanges();

                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                    await _lineRepository.SaveChanges();

                }
            }


            await _repository.SaveChanges();

            return new SuccessDataResult<SelectCalendarsDto>(ObjectMapper.Map<Calendars, SelectCalendarsDto>(mappedEntity));
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalendarLinesDto>>> GetLineListAsync(Guid calendarID)
        {
            var entity = await _repository.GetAsync(x => x.Id == calendarID);

            var mappedEntity = ObjectMapper.Map<List<CalendarLines>, List<ListCalendarLinesDto>>(entity.CalendarLines.ToList());

            return new SuccessDataResult<IList<ListCalendarLinesDto>>(mappedEntity);
        }

        public Task<IDataResult<SelectCalendarsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
