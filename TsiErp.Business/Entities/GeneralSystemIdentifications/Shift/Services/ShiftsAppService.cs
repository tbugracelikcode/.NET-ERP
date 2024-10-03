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
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.Shift.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Shifts.Page;

namespace TsiErp.Business.Entities.Shift.Services
{
    [ServiceRegistration(typeof(IShiftsAppService), DependencyInjectionType.Scoped)]
    public class ShiftsAppService : ApplicationService<ShiftsResource>, IShiftsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ShiftsAppService(IStringLocalizer<ShiftsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateShiftsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectShiftsDto>> CreateAsync(CreateShiftsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Shifts).Select("Code").Where(new { Code = input.Code },  "");
            var list = queryFactory.ControlList<Shifts>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Shifts).Insert(new CreateShiftsDto
            {
                NetWorkTime = input.NetWorkTime,
                Overtime = input.Overtime,
                ShiftOrder = input.ShiftOrder,
                TotalWorkTime = input.TotalWorkTime,
                TotalBreakTime = input.TotalBreakTime,
                Code = input.Code,
                CreationTime = now,
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
            });

            foreach (var item in input.SelectShiftLinesDto)
            {
                var queryLine = queryFactory.Query().From(Tables.ShiftLines).Insert(new CreateShiftLinesDto
                {
                    Coefficient = item.Coefficient,
                    EndHour = item.EndHour,
                    StartHour = item.StartHour,
                    Type = (int)item.Type,
                    ShiftID = addedEntityId,
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    LineNr = item.LineNr,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var shift = queryFactory.Insert<SelectShiftsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ShiftsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Shifts, LogType.Insert, shift.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShiftsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectShiftsDto>(shift);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ShiftID", new List<string>
            {
                Tables.CalendarLines,
                Tables.ContractProductionTrackings,
                Tables.ProductionTrackings
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;

                var query = queryFactory.Query().From(Tables.Shifts).Select("*").Where(new { Id = id }, "");

                var shifts = queryFactory.Get<SelectShiftsDto>(query);

                if (shifts.Id != Guid.Empty && shifts != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Shifts).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ShiftLines).Delete(LoginedUserService.UserId).Where(new { ShiftID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var shift = queryFactory.Update<SelectShiftsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Shifts, LogType.Delete, id);

                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShiftsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = notTemplate.ContextMenuName_,
                                        IsViewed = false,
                                         
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
                    return new SuccessDataResult<SelectShiftsDto>(shift);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ShiftLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                    var shiftLines = queryFactory.Update<SelectShiftLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShiftLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectShiftLinesDto>(shiftLines);
                }
            }
        }

        public async Task<IDataResult<SelectShiftsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
          new
          {
              Id = id
          }, "");

            var shifts = queryFactory.Get<SelectShiftsDto>(query);

            var queryLines = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(
          new
          {
              ShiftID = id
          },  "");

            var shiftLine = queryFactory.GetList<SelectShiftLinesDto>(queryLines).ToList();

            shifts.SelectShiftLinesDto = shiftLine;

            LogsAppService.InsertLogToDatabase(shifts, shifts, LoginedUserService.UserId, Tables.Shifts, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShiftsDto>(shifts);
        }

        public async Task<IDataResult<IList<ListShiftsDto>>> GetListAsync(ListShiftsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.Shifts)
                   .Select<Shifts>(s => new { s.Code, s.Name, s.TotalWorkTime, s.TotalBreakTime, s.NetWorkTime, s.Overtime, s.ShiftOrder, s.Id })
                    .Where(null, Tables.Shifts);

            var shifts = queryFactory.GetList<ListShiftsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShiftsDto>>(shifts);
        }

        [ValidationAspect(typeof(UpdateShiftsValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectShiftsDto>> UpdateAsync(UpdateShiftsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
          new
          {
              Id = input.Id
          }, "");

            var entity = queryFactory.Get<SelectShiftsDto>(entityQuery);

            var queryLines = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(
          new
          {
              ShiftID = input.Id
          }, "");

            var shiftLine = queryFactory.GetList<SelectShiftLinesDto>(queryLines).ToList();

            entity.SelectShiftLinesDto = shiftLine;

            #region Update Control
            var listQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
          new
          {
              Code = input.Code
          }, "");

            var list = queryFactory.GetList<ListShiftsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Shifts).Update(new UpdateShiftsDto
            {
                NetWorkTime = input.NetWorkTime,
                Overtime = input.Overtime,
                ShiftOrder = input.ShiftOrder,
                TotalWorkTime = input.TotalWorkTime,
                TotalBreakTime = input.TotalBreakTime,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectShiftLinesDto)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ShiftLines).Insert(new CreateShiftLinesDto
                    {
                        Coefficient = item.Coefficient,
                        EndHour = item.EndHour,
                        StartHour = item.StartHour,
                        Type = (int)item.Type,
                        ShiftID = input.Id,
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectShiftLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ShiftLines).Update(new UpdateShiftLinesDto
                        {
                            Coefficient = item.Coefficient,
                            EndHour = item.EndHour,
                            StartHour = item.StartHour,
                            Type = (int)item.Type,
                            ShiftID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var shift = queryFactory.Update<SelectShiftsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Shifts, LogType.Update, shift.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShiftsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
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
            return new SuccessDataResult<SelectShiftsDto>(shift);
        }

        public async Task<IDataResult<SelectShiftsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<Shifts>(entityQuery);

            var query = queryFactory.Query().From(Tables.Shifts).Update(new UpdateShiftsDto
            {
                NetWorkTime = entity.NetWorkTime,
                Overtime = entity.Overtime,
                ShiftOrder = entity.ShiftOrder,
                TotalWorkTime = entity.TotalWorkTime,
                TotalBreakTime = entity.TotalBreakTime,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var shiftsDto = queryFactory.Update<SelectShiftsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectShiftsDto>(shiftsDto);

        }
    }
}
