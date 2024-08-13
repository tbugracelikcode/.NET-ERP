using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Calendar.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.Calendar;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Calendars.Page;

namespace TsiErp.Business.Entities.Calendar.Services
{
    [ServiceRegistration(typeof(ICalendarsAppService), DependencyInjectionType.Scoped)]
    public class CalendarsAppService : ApplicationService<CalendarsResource>, ICalendarsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CalendarsAppService(IStringLocalizer<CalendarsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        [ValidationAspect(typeof(CreateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Calendars).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<Calendars>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Calendars).Insert(new CreateCalendarsDto
            {
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = LoginedUserService.UserId,
                    WorkStatus = item.WorkStatus,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    MaintenanceType = item.MaintenanceType,
                    PlannedMaintenanceTime = item.PlannedMaintenanceTime,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    AvailableTime = item.AvailableTime,
                    Date_ = item.Date_,
                    PlannedHaltTimes = item.PlannedHaltTimes,
                    ShiftID = item.ShiftID.GetValueOrDefault(),
                    ShiftOverTime = item.ShiftOverTime,
                    ShiftTime = item.ShiftTime,
                    StationID = item.StationID.GetValueOrDefault()
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            #region Renkli Takvim Oluşturma

            int thisYear = _GetSQLDateAppService.GetDateFromSQL().Year;
            DateTime startDate = new DateTime(thisYear, 1, 1);
            DateTime endDate = new DateTime(thisYear, 12, 31);
            int numberofDays = 0;

            foreach (DateTime day in EachDay(startDate, endDate))
            {
                numberofDays++;

                if (day.DayOfWeek == DayOfWeek.Sunday)
                {
                    SelectCalendarDaysDto calendarDays = new SelectCalendarDaysDto
                    {
                        CalendarDayStateEnum = 4,
                        Date_ = day,
                    };
                    input.SelectCalendarDaysDto.Add(calendarDays);
                }
                else if (day.Month == 8 && day.Day == 30 || day.Month == 4 && day.Day == 23 || day.Month == 5 && day.Day == 1 || day.Month == 5 && day.Day == 19 || day.Month == 7 && day.Day == 15 || day.Month == 10 && day.Day == 29)
                {
                    if (!input.SelectCalendarDaysDto.Where(t => t.Date_.Month == day.Month && t.Date_.Day == day.Day).Any())
                    {
                        SelectCalendarDaysDto calendarDays = new SelectCalendarDaysDto
                        {
                            CalendarDayStateEnum = 3,
                            Date_ = day,
                        };
                        input.SelectCalendarDaysDto.Add(calendarDays);
                    }
                }
                else
                {
                    SelectCalendarDaysDto calendarDays = new SelectCalendarDaysDto
                    {
                        CalendarDayStateEnum = 1,
                        Date_ = day,
                    };
                    input.SelectCalendarDaysDto.Add(calendarDays);
                }


            }


            #endregion

            foreach (var item in input.SelectCalendarDaysDto)
            {
                var queryDay = queryFactory.Query().From(Tables.CalendarDays).Insert(new CreateCalendarDaysDto
                {
                    CalendarID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            await FicheNumbersAppService.UpdateFicheNumberAsync("CalendarChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Calendars, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalendarChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalendarsDto>(calendar);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.Calendars).Select("*").Where(new { Id = id }, "");

            var calendars = queryFactory.Get<SelectCalendarsDto>(query);

            if (calendars.Id != Guid.Empty && calendars != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.Calendars).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.CalendarLines).Delete(LoginedUserService.UserId).Where(new { BomID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var calendar = queryFactory.Update<SelectCalendarsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Calendars, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalendarChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains(","))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split(',');

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(user),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }
                        else
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(notTemplate.TargetUsersId),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }

                }

                #endregion

                await Task.CompletedTask;
                return new SuccessDataResult<SelectCalendarsDto>(calendar);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.CalendarLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var calendarLines = queryFactory.Update<SelectCalendarLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalendarLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectCalendarLinesDto>(calendarLines);
            }
        }

        public async Task<IDataResult<SelectCalendarsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Calendars).Select("*").Where(
           new
           {
               Id = id
           }, "");
            var calendar = queryFactory.Get<SelectCalendarsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CalendarLines)
                   .Select<CalendarLines>(null)
                   .Join<Stations>
                    (
                        s => new { StationName = s.Name, StationID = s.Id, StationCode = s.Code },
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
                    .Where(new { CalendarID = id }, Tables.CalendarLines);

            var calendarLine = queryFactory.GetList<SelectCalendarLinesDto>(queryLines).ToList();

            calendar.SelectCalendarLinesDto = calendarLine;

            var queryDays = queryFactory
                   .Query()
                   .From(Tables.CalendarDays)
                   .Select("*").Where(new { CalendarID = id }, Tables.CalendarDays);

            var calendarDay = queryFactory.GetList<SelectCalendarDaysDto>(queryDays).ToList();

            calendar.SelectCalendarDaysDto = calendarDay;

            LogsAppService.InsertLogToDatabase(calendar, calendar, LoginedUserService.UserId, Tables.Calendars, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalendarsDto>(calendar);

        }

        public async Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID)
        {
            var query = queryFactory.Query().From(Tables.CalendarDays).Select<CalendarDays>(s => new { s.CalendarID, s.Date_, s.CalendarDayStateEnum, s.ColorCode }).Where(new { CalendarID = calendarID }, "");
            var calendarDays = queryFactory.GetList<SelectCalendarDaysDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectCalendarDaysDto>>(calendarDays);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalendarsDto>>> GetListAsync(ListCalendarsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Calendars).Select<Calendars>(s => new { s.Code, s.Name, s.Year, s.IsPlanned, s._Description }).Where(null, "");
            var calendars = queryFactory.GetList<ListCalendarsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCalendarsDto>>(calendars);
        }

        [ValidationAspect(typeof(UpdateCalendarsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalendarsDto>> UpdateAsync(UpdateCalendarsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Calendars).Select("*").Where(
          new
          {
              Id = input.Id
          }, "");
            var entity = queryFactory.Get<SelectCalendarsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CalendarLines)
                   .Select<CalendarLines>(null)
                   .Join<Stations>
                    (
                        s => new { StationName = s.Name, StationID = s.Id, StationCode = s.Code },
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
                    .Where(new { CalendarID = input.Id }, Tables.CalendarLines);

            var calendarLine = queryFactory.GetList<SelectCalendarLinesDto>(queryLines).ToList();

            entity.SelectCalendarLinesDto = calendarLine;

            var queryDays = queryFactory
                   .Query()
                   .From(Tables.CalendarDays)
                   .Select("*").Where(new { CalendarID = input.Id },  Tables.CalendarDays);

            var calendarDay = queryFactory.GetList<SelectCalendarDaysDto>(queryDays).ToList();

            entity.SelectCalendarDaysDto = calendarDay;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Calendars).Select("*").Where(new { Code = input.Code }, Tables.Calendars);

            var list = queryFactory.GetList<ListCalendarsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
                _Description = input._Description,
                AvailableDays = input.AvailableDays,
                IsPlanned = input.IsPlanned,
                OfficialHolidayDays = input.OfficialHolidayDays,
                TotalDays = input.TotalDays,
                Year = input.Year,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectCalendarLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CalendarLines).Insert(new CreateCalendarLinesDto
                    {
                        CalendarID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        MaintenanceType = item.MaintenanceType,
                        PlannedMaintenanceTime = item.PlannedMaintenanceTime,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        WorkStatus = item.WorkStatus,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        AvailableTime = item.AvailableTime,
                        Date_ = item.Date_,
                        PlannedHaltTimes = item.PlannedHaltTimes,
                        ShiftID = item.ShiftID.GetValueOrDefault(),
                        ShiftOverTime = item.ShiftOverTime,
                        ShiftTime = item.ShiftTime,
                        StationID = item.StationID.GetValueOrDefault()
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CalendarLines).Select("*").Where(new { Id = item.Id }, "");

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
                            PlannedMaintenanceTime = line.PlannedMaintenanceTime,
                            MaintenanceType = line.MaintenanceType,
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            AvailableTime = item.AvailableTime,
                            Date_ = item.Date_,
                            PlannedHaltTimes = item.PlannedHaltTimes,
                            ShiftID = item.ShiftID.GetValueOrDefault(),
                            ShiftOverTime = item.ShiftOverTime,
                            WorkStatus = item.WorkStatus,
                            ShiftTime = item.ShiftTime,
                            StationID = item.StationID.GetValueOrDefault()
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectCalendarDaysDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryDay = queryFactory.Query().From(Tables.CalendarDays).Insert(new CreateCalendarDaysDto
                    {
                        CalendarID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    var dayGetQuery = queryFactory.Query().From(Tables.CalendarDays).Select("*").Where(new { Id = item.Id }, "");

                    var day = queryFactory.Get<SelectCalendarDaysDto>(dayGetQuery);

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
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            Date_ = item.Date_,
                            ColorCode = item.ColorCode,
                            CalendarDayStateEnum = item.CalendarDayStateEnum
                        }).Where(new { Id = day.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryDay.Sql + " where " + queryDay.WhereSentence;
                    }
                }
            }

            var calendar = queryFactory.Update<SelectCalendarsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Calendars, LogType.Update, input.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalendarChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalendarsDto>(calendar);
        }

        public async Task<IDataResult<SelectCalendarsDto>> UpdateWithoutDaysAsync(UpdateCalendarsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Calendars).Select("*").Where(
          new
          {
              Id = input.Id
          }, "");
            var entity = queryFactory.Get<SelectCalendarsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CalendarLines)
                   .Select<CalendarLines>(null)
                   .Join<Stations>
                    (
                        s => new { StationName = s.Name, StationID = s.Id, StationCode = s.Code },
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
                    .Where(new { CalendarID = input.Id }, Tables.CalendarLines);

            var calendarLine = queryFactory.GetList<SelectCalendarLinesDto>(queryLines).ToList();

            entity.SelectCalendarLinesDto = calendarLine;

            var queryDays = queryFactory
                   .Query()
                   .From(Tables.CalendarDays)
                   .Select("*").Where(new { CalendarID = input.Id }, Tables.CalendarDays);

            var calendarDay = queryFactory.GetList<SelectCalendarDaysDto>(queryDays).ToList();

            entity.SelectCalendarDaysDto = calendarDay;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.Calendars).Select("*").Where(new { Code = input.Code }, Tables.Calendars);

            var list = queryFactory.GetList<ListCalendarsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
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
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
                _Description = input._Description,
                AvailableDays = input.AvailableDays,
                IsPlanned = input.IsPlanned,
                OfficialHolidayDays = input.OfficialHolidayDays,
                TotalDays = input.TotalDays,
                Year = input.Year,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectCalendarLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CalendarLines).Insert(new CreateCalendarLinesDto
                    {
                        CalendarID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        MaintenanceType = item.MaintenanceType,
                        PlannedMaintenanceTime = item.PlannedMaintenanceTime,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        WorkStatus = item.WorkStatus,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.CalendarLines).Select("*").Where(new { Id = item.Id }, "");

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
                            PlannedMaintenanceTime = line.PlannedMaintenanceTime,
                            MaintenanceType = line.MaintenanceType,
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            AvailableTime = item.AvailableTime,
                            Date_ = item.Date_,
                            PlannedHaltTimes = item.PlannedHaltTimes,
                            ShiftID = item.ShiftID.Value,
                            ShiftOverTime = item.ShiftOverTime,
                            WorkStatus = item.WorkStatus,
                            ShiftTime = item.ShiftTime,
                            StationID = item.StationID.Value
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var calendar = queryFactory.Update<SelectCalendarsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Calendars, LogType.Update, input.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalendarsDto>(calendar);
        }

        public async Task<IDataResult<SelectCalendarsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Calendars).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Calendars>(entityQuery);

            var query = queryFactory.Query().From(Tables.Calendars).Update(new UpdateCalendarsDto
            {
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var calendarsDto = queryFactory.Update<SelectCalendarsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalendarsDto>(calendarsDto);


        }

        public bool UpdateDays(SelectCalendarDaysDto day)
        {

            var query = queryFactory.Query().From(Tables.CalendarDays).Update(new UpdateCalendarDaysDto
            {
                CalendarID = day.CalendarID,
                CreationTime = day.CreationTime,
                CreatorId = day.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = day.DeleterId.GetValueOrDefault(),
                DeletionTime = day.DeletionTime.GetValueOrDefault(),
                Id = day.Id,
                IsDeleted = day.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                Date_ = day.Date_,
                ColorCode = day.ColorCode,
                CalendarDayStateEnum = day.CalendarDayStateEnum

            }).Where(new { Id = day.Id }, "");

            var calendarsDto = queryFactory.Update<SelectCalendarDaysDto>(query, "Id", true);

            return true;
        }
    }
}
