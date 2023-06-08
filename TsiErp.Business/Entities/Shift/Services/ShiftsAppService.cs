using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Shift.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Shifts.Page;

namespace TsiErp.Business.Entities.Shift.Services
{
    [ServiceRegistration(typeof(IShiftsAppService), DependencyInjectionType.Scoped)]
    public class ShiftsAppService : ApplicationService<ShiftsResource>, IShiftsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ShiftsAppService(IStringLocalizer<ShiftsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> CreateAsync(CreateShiftsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<Shifts>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.Shifts).Insert(new CreateShiftsDto
                {
                    NetWorkTime = input.NetWorkTime,
                    Overtime = input.Overtime,
                    ShiftOrder = input.ShiftOrder,
                    TotalWorkTime = input.TotalWorkTime,
                    TotalBreakTime = input.TotalBreakTime,
                    Code = input.Code,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsActive = true,
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
                        LineNr = item.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var shift = queryFactory.Insert<SelectShiftsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Shifts, LogType.Insert, shift.Id);

                return new SuccessDataResult<SelectShiftsDto>(shift);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Shifts).Select("*").Where(new { Id = id }, true, true, "");

                var shifts = queryFactory.Get<SelectShiftsDto>(query);

                if (shifts.Id != Guid.Empty && shifts != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.Shifts).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ShiftLines).Delete(LoginedUserService.UserId).Where(new { ShiftID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var shift = queryFactory.Update<SelectShiftsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Shifts, LogType.Delete, id);
                    return new SuccessDataResult<SelectShiftsDto>(shift);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ShiftLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var shiftLines = queryFactory.Update<SelectShiftLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShiftLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectShiftLinesDto>(shiftLines);
                }
            }
        }

        public async Task<IDataResult<SelectShiftsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
              new
              {
                  Id = id
              }, true, true, "");

                var shifts = queryFactory.Get<SelectShiftsDto>(query);

                var queryLines = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(
              new
              {
                  ShiftID = id
              }, false, false, "");

                var shiftLine = queryFactory.GetList<SelectShiftLinesDto>(queryLines).ToList();

                shifts.SelectShiftLinesDto = shiftLine;

                LogsAppService.InsertLogToDatabase(shifts, shifts, LoginedUserService.UserId, Tables.Shifts, LogType.Get, id);

                return new SuccessDataResult<SelectShiftsDto>(shifts);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShiftsDto>>> GetListAsync(ListShiftsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.Shifts)
                       .Select("*")
                        .Where(null, true, true, Tables.Shifts);

                var shifts = queryFactory.GetList<ListShiftsDto>(query).ToList();
                return new SuccessDataResult<IList<ListShiftsDto>>(shifts);
            }
        }

        [ValidationAspect(typeof(UpdateShiftsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShiftsDto>> UpdateAsync(UpdateShiftsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
              new
              {
                  Id = input.Id
              }, true, true, "");

                var entity = queryFactory.Get<SelectShiftsDto>(entityQuery);

                var queryLines = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(
              new
              {
                  ShiftID = input.Id
              }, false, false, "");

                var shiftLine = queryFactory.GetList<SelectShiftLinesDto>(queryLines).ToList();

                entity.SelectShiftLinesDto = shiftLine;

                #region Update Control
                var listQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(
              new
              {
                  Code = input.Code
              }, false, false, "");

                var list = queryFactory.GetList<ListShiftsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

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
                    IsActive = input.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                    Name = input.Name,
                }).Where(new { Id = input.Id }, true, true, "");

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
                            LineNr = item.LineNr,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.ShiftLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                                IsDeleted = false,
                                LastModificationTime = DateTime.Now,
                                LastModifierId = LoginedUserService.UserId,
                                LineNr = item.LineNr,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var shift = queryFactory.Update<SelectShiftsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.Shifts, LogType.Update, shift.Id);

                return new SuccessDataResult<SelectShiftsDto>(shift);
            }
        }

        public async Task<IDataResult<SelectShiftsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Shifts).Select("*").Where(new { Id = id }, true, true, "");

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
                    DeletionTime = entity.DeletionTime.Value,
                    Id = entity.Id,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Name = entity.Name,
                }).Where(new { Id = id }, true, true, "");

                var shiftsDto = queryFactory.Update<SelectShiftsDto>(query, "Id", true);
                return new SuccessDataResult<SelectShiftsDto>(shiftsDto);

            }
        }
    }
}
