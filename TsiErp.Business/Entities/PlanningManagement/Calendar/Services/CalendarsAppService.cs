using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Calendar.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.PlanningManagement.Calendar;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Localizations.Resources.Calendars.Page;

namespace TsiErp.Business.Entities.Calendar.Services
{
    [ServiceRegistration(typeof(ICalendarsAppService), DependencyInjectionType.Scoped)]
    public class CalendarsAppService : ApplicationService<CalendarsResource>, ICalendarsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public CalendarsAppService(IStringLocalizer<CalendarsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Calendars).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<Calendars>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.Calendars).Insert(new CreateCalendarsDto
                {
                    Code = input.Code,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name,
                    _Description = input._Description,
                    AvailableDays = input.AvailableDays,
                    IsPlanned = input.IsPlanned,
                    OfficialHolidayDays = input.OfficialHolidayDays,
                    SelectCalendarDaysDto = input.SelectCalendarDaysDto,
                    TotalDays = input.TotalDays,
                    Year = input.Year,
                });

                foreach (var item in input.SelectCalendarLinesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.CalendarLines).Insert(new CreateCalendarLinesDto
                    {
                        CalendarID = addedEntityId,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        AvailableTime = item.AvailableTime,
                        Date_ = item.Date_,
                        PlannedHaltTimes = item.PlannedHaltTimes,
                        ShiftID = item.ShiftID.Value,
                        ShiftOverTime = item.ShiftOverTime,
                        ShiftTime = item.ShiftTime,
                        StationID = item.StationID.Value
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                foreach (var item in input.SelectCalendarDaysDto)
                {
                    var queryDay = queryFactory.Query().From(Tables.CalendarDays).Insert(new CreateCalendarDaysDto
                    {
                        CalendarID = addedEntityId,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        Date_ = item.Date_,
                        CalendarDayStateEnum = item.CalendarDayStateEnum,
                        ColorCode = item.ColorCode,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryDay.Sql;
                }

                var calendar = queryFactory.Insert<SelectCalendarsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Calendars, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectCalendarsDto>(calendar);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Calendars).Select("*").Where(new { Id = id }, false, false, "");

                var calendars = queryFactory.Get<SelectCalendarsDto>(query);

                if (calendars.Id != Guid.Empty && calendars != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Calendars).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.CalendarLines).Delete(LoginedUserService.UserId).Where(new { BomID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var calendar = queryFactory.Update<SelectCalendarsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Calendars, LogType.Delete, id);
                    return new SuccessDataResult<SelectCalendarsDto>(calendar);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.CalendarLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var calendarLines = queryFactory.Update<SelectCalendarLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalendarLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectCalendarLinesDto>(calendarLines);
                }
            }
        }

        public async Task<IDataResult<SelectCalendarsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Calendars).Select("*").Where(
               new
               {
                   Id = id
               }, false, false, "");
                var calendar = queryFactory.Get<SelectCalendarsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.CalendarLines)
                       .Select<CalendarLines>(cl => new { cl.ShiftID, cl.CalendarID, cl.StationID, cl.Id, cl.DataOpenStatus, cl.DataOpenStatusUserId, cl.ShiftTime, cl.ShiftOverTime, cl.PlannedHaltTimes, cl.Date_, cl.AvailableTime })
                       .Join<Stations>
                        (
                            s => new { StationName = s.Name, StationID = s.Id },
                            nameof(CalendarLines.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                       .Join<Shifts>
                        (
                            sh => new { ShiftOrder = sh.ShiftOrder, ShiftName = sh.Name, AvailableTime = sh.NetWorkTime, PlannedHaltTimes = sh.TotalBreakTime, ShiftOverTime = sh.Overtime, ShiftID = sh.Id },
                            nameof(CalendarLines.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        )
                        .Where(new { CalendarID = id }, false, false, Tables.CalendarLines);

                var calendarLine = queryFactory.GetList<SelectCalendarLinesDto>(queryLines).ToList();

                calendar.SelectCalendarLinesDto = calendarLine;

                var queryDays = queryFactory
                       .Query()
                       .From(Tables.CalendarDays)
                       .Select("*").Where(new { CalendarID = id }, false, false, Tables.CalendarDays);

                var calendarDay = queryFactory.GetList<SelectCalendarDaysDto>(queryDays).ToList();

                calendar.SelectCalendarDaysDto = calendarDay;

                LogsAppService.InsertLogToDatabase(calendar, calendar, LoginedUserService.UserId, Tables.Calendars, LogType.Get, id);

                return new SuccessDataResult<SelectCalendarsDto>(calendar);
            }
        }

        public async Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.CalendarDays).Select("*").Where(new { CalendarID = calendarID }, false, false, "");
                var calendarDays = queryFactory.GetList<SelectCalendarDaysDto>(query).ToList();
                return new SuccessDataResult<IList<SelectCalendarDaysDto>>(calendarDays);


            }
        }

        public async Task<IDataResult<IList<ListCalendarLinesDto>>> GetLineListAsync(Guid calendarID)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.CalendarLines).Select<CalendarLines>(cl => new { cl.ShiftID, cl.CalendarID, cl.StationID, cl.Id, cl.DataOpenStatus, cl.DataOpenStatusUserId, cl.ShiftTime, cl.ShiftOverTime, cl.PlannedHaltTimes, cl.Date_, cl.AvailableTime })
                       .Join<Stations>
                        (
                            s => new { StationName = s.Name },
                            nameof(CalendarLines.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                       .Join<Shifts>
                        (
                            sh => new { ShiftOrder = sh.ShiftOrder, ShiftName = sh.Name, AvailableTime = sh.NetWorkTime, PlannedHaltTimes = sh.TotalBreakTime, ShiftOverTime = sh.Overtime },
                            nameof(CalendarLines.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        ).Where(new { CalendarID = calendarID }, false, false, "");
                var calendarLines = queryFactory.GetList<ListCalendarLinesDto>(query).ToList();

                return new SuccessDataResult<IList<ListCalendarLinesDto>>(calendarLines);


            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalendarsDto>>> GetListAsync(ListCalendarsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Calendars).Select("*").Where(null, false, false, "");
                var calendars = queryFactory.GetList<ListCalendarsDto>(query).ToList();
                return new SuccessDataResult<IList<ListCalendarsDto>>(calendars);


            }
        }

        [ValidationAspect(typeof(UpdateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> UpdateAsync(UpdateCalendarsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Calendars).Select("*").Where(
              new
              {
                  Id = input.Id
              }, false, false, "");
                var entity = queryFactory.Get<SelectCalendarsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.CalendarLines)
                       .Select<CalendarLines>(cl => new { cl.ShiftID, cl.CalendarID, cl.StationID, cl.Id, cl.DataOpenStatus, cl.DataOpenStatusUserId, cl.ShiftTime, cl.ShiftOverTime, cl.PlannedHaltTimes, cl.Date_, cl.AvailableTime })
                       .Join<Stations>
                        (
                            s => new { StationName = s.Name,StationID = s.Id },
                            nameof(CalendarLines.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                       .Join<Shifts>
                        (
                            sh => new { ShiftOrder = sh.ShiftOrder, ShiftName = sh.Name, AvailableTime = sh.NetWorkTime, PlannedHaltTimes = sh.TotalBreakTime, ShiftOverTime = sh.Overtime, ShiftID = sh.Id },
                            nameof(CalendarLines.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        )
                        .Where(new { CalendarID = input.Id }, false, false, Tables.CalendarLines);

                var calendarLine = queryFactory.GetList<SelectCalendarLinesDto>(queryLines).ToList();

                entity.SelectCalendarLinesDto = calendarLine;

                var queryDays = queryFactory
                       .Query()
                       .From(Tables.CalendarDays)
                       .Select("*").Where(new { CalendarID = input.Id }, false, false, Tables.CalendarDays);

                var calendarDay = queryFactory.GetList<SelectCalendarDaysDto>(queryDays).ToList();

                entity.SelectCalendarDaysDto = calendarDay;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.Calendars).Select("*").Where(new { Code = input.Code }, false, false, Tables.Calendars);

                var list = queryFactory.GetList<ListCalendarsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.Calendars).Update(new UpdateCalendarsDto
                {
                    Code = input.Code,
                    CreationTime = entity.CreationTime,
                    CreatorId = entity.CreatorId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = input.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                    Name = input.Name,
                    _Description = input._Description,
                    AvailableDays = input.AvailableDays,
                    IsPlanned = input.IsPlanned,
                    OfficialHolidayDays = input.OfficialHolidayDays,
                    TotalDays = input.TotalDays,
                    Year = input.Year,
                }).Where(new { Id = input.Id }, false, false, "");

                foreach (var item in input.SelectCalendarLinesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CalendarLines).Insert(new CreateCalendarLinesDto
                        {
                            CalendarID = input.Id,
                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            AvailableTime = item.AvailableTime,
                            Date_ = item.Date_,
                            PlannedHaltTimes = item.PlannedHaltTimes,
                            ShiftID = item.ShiftID.Value,
                            ShiftOverTime = item.ShiftOverTime,
                            ShiftTime = item.ShiftTime,
                            StationID = item.StationID.Value
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.CalendarLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectCalendarLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.CalendarLines).Update(new UpdateCalendarLinesDto
                            {
                                CalendarID = input.Id,
                                CreationTime = line.CreationTime,
                                CreatorId = line.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = line.DeleterId.GetValueOrDefault(),
                                DeletionTime = line.DeletionTime.GetValueOrDefault(),
                                Id = item.Id,
                                IsDeleted = item.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                AvailableTime = item.AvailableTime,
                                Date_ = item.Date_,
                                PlannedHaltTimes = item.PlannedHaltTimes,
                                ShiftID = item.ShiftID.Value,
                                ShiftOverTime = item.ShiftOverTime,
                                ShiftTime = item.ShiftTime,
                                StationID = item.StationID.Value
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                foreach (var item in input.SelectCalendarDaysDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryDay = queryFactory.Query().From(Tables.CalendarLines).Insert(new CreateCalendarDaysDto
                        {
                            CalendarID = input.Id,
                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            Date_ = item.Date_,
                            CalendarDayStateEnum = item.CalendarDayStateEnum,
                            ColorCode = item.ColorCode,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryDay.Sql;
                    }
                    else
                    {
                        var dayGetQuery = queryFactory.Query().From(Tables.CalendarDays).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var day = queryFactory.Get<SelectCalendarLinesDto>(dayGetQuery);

                        if (day != null)
                        {
                            var queryDay = queryFactory.Query().From(Tables.CalendarDays).Update(new UpdateCalendarDaysDto
                            {
                                CalendarID = input.Id,
                                CreationTime = day.CreationTime,
                                CreatorId = day.CreatorId,
                                DataOpenStatus = false,
                                DataOpenStatusUserId = Guid.Empty,
                                DeleterId = day.DeleterId.GetValueOrDefault(),
                                DeletionTime = day.DeletionTime.GetValueOrDefault(),
                                Id = item.Id,
                                IsDeleted = item.IsDeleted,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                Date_ = item.Date_,
                                ColorCode = item.ColorCode,
                                CalendarDayStateEnum = item.CalendarDayStateEnum
                            }).Where(new { Id = day.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryDay.Sql + " where " + queryDay.WhereSentence;
                        }
                    }
                }

                var calendar = queryFactory.Update<SelectCalendarsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Calendars, LogType.Update, calendar.Id);

                return new SuccessDataResult<SelectCalendarsDto>(calendar);
            }
        }

        public async Task<IDataResult<SelectCalendarsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Calendars).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<Calendars>(entityQuery);

                var query = queryFactory.Query().From(Tables.Calendars).Update(new UpdateCalendarsDto
                {
                    Code = entity.Code,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Name = entity.Name,
                    _Description = entity._Description,
                    AvailableDays = entity.AvailableDays,
                    IsPlanned = entity.IsPlanned,
                    OfficialHolidayDays = entity.OfficialHolidayDays,
                    TotalDays = entity.TotalDays,
                    Year = entity.Year,
                }).Where(new { Id = id }, false, false, "");

                var calendarsDto = queryFactory.Update<SelectCalendarsDto>(query, "Id", true);
                return new SuccessDataResult<SelectCalendarsDto>(calendarsDto);

            }
        }
    }
}
