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
using TsiErp.Business.Entities.MaintenanceInstruction.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenanceInstructions.Page;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    [ServiceRegistration(typeof(IMaintenanceInstructionsAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceInstructionsAppService : ApplicationService<MaintenanceInstructionsResource>, IMaintenanceInstructionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public MaintenanceInstructionsAppService(IStringLocalizer<MaintenanceInstructionsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> CreateAsync(CreateMaintenanceInstructionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<MaintenanceInstructions>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Insert(new CreateMaintenanceInstructionsDto
                {
                    InstructionName = input.InstructionName,
                    Note_ = input.Note_,
                    PeriodID = input.PeriodID,
                    PeriodTime = input.PeriodTime,
                    PlannedMaintenanceTime = input.PlannedMaintenanceTime,
                    StationID = input.StationID,
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
                });

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Insert(new CreateMaintenanceInstructionLinesDto
                    {
                        Amount = item.Amount,
                        InstructionDescription = item.InstructionDescription,
                        InstructionID = addedEntityId,
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
                        ProductID = item.ProductID,
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var maintenanceInstruction = queryFactory.Insert<SelectMaintenanceInstructionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("*").Where(new { Id = id }, false, false, "");

                var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

                if (maintenanceInstructions.Id != Guid.Empty && maintenanceInstructions != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Delete(LoginedUserService.UserId).Where(new { InstructionID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var maintenanceInstruction = queryFactory.Update<SelectMaintenanceInstructionsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Delete, id);
                    return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var maintenanceInstructionLines = queryFactory.Update<SelectMaintenanceInstructionLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceInstructionLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectMaintenanceInstructionLinesDto>(maintenanceInstructionLines);
                }
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructions)
                       .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                       .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(MaintenanceInstructions.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<MaintenancePeriods>
                        (
                            mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                            nameof(MaintenanceInstructions.PeriodID),
                            nameof(MaintenancePeriods.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.MaintenanceInstructions);

                var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructionLines)
                       .Select<MaintenanceInstructionLines>(mil => new { mil.UnitSetID, mil.ProductID, mil.LineNr, mil.InstructionID, mil.InstructionDescription, mil.Id, mil.DataOpenStatusUserId, mil.DataOpenStatus, mil.Amount })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Code, ProductCode = p.Code },
                            nameof(MaintenanceInstructionLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(MaintenanceInstructionLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { InstructionID = id }, false, false, Tables.MaintenanceInstructionLines);

                var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

                maintenanceInstructions.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

                LogsAppService.InsertLogToDatabase(maintenanceInstructions, maintenanceInstructions, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Get, id);

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructions);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceInstructionsDto>>> GetListAsync(ListMaintenanceInstructionsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructions)
                       .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                       .Join<Stations>
                        (
                            s => new { StationCode = s.Code },
                            nameof(MaintenanceInstructions.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<MaintenancePeriods>
                        (
                            mp => new { PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                            nameof(MaintenanceInstructions.PeriodID),
                            nameof(MaintenancePeriods.Id),
                            JoinType.Left
                        )
                        .Where(null, false, false, Tables.MaintenanceInstructions);

                var maintenanceInstructions = queryFactory.GetList<ListMaintenanceInstructionsDto>(query).ToList();
                return new SuccessDataResult<IList<ListMaintenanceInstructionsDto>>(maintenanceInstructions);
            }
        }

        [ValidationAspect(typeof(UpdateMaintenanceInstructionValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateAsync(UpdateMaintenanceInstructionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructions)
                       .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                        .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(MaintenanceInstructions.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<MaintenancePeriods>
                        (
                            mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                            nameof(MaintenanceInstructions.PeriodID),
                            nameof(MaintenancePeriods.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.MaintenanceInstructions);

                var entity = queryFactory.Get<SelectMaintenanceInstructionsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructionLines)
                       .Select<MaintenanceInstructionLines>(mil => new { mil.UnitSetID, mil.ProductID, mil.LineNr, mil.InstructionID, mil.InstructionDescription, mil.Id, mil.DataOpenStatusUserId, mil.DataOpenStatus, mil.Amount })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Code, ProductCode = p.Code },
                            nameof(MaintenanceInstructionLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(MaintenanceInstructionLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { InstructionID = input.Id }, false, false, Tables.MaintenanceInstructionLines);

                var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

                entity.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.MaintenanceInstructions)
                               .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                               .Join<Stations>
                                (
                                    s => new { StationID = s.Id, StationCode = s.Code },
                                    nameof(MaintenanceInstructions.StationID),
                                    nameof(Stations.Id),
                                    JoinType.Left
                                )
                                .Join<MaintenancePeriods>
                                (
                                    mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                                    nameof(MaintenanceInstructions.PeriodID),
                                    nameof(MaintenancePeriods.Id),
                                    JoinType.Left
                                )
                                .Where(new { Code = input.Code }, false, false, Tables.MaintenanceInstructions);

                var list = queryFactory.GetList<ListMaintenanceInstructionsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Update(new UpdateMaintenanceInstructionsDto
                {
                    InstructionName = input.InstructionName,
                    Note_ = input.Note_,
                    PeriodID = input.PeriodID,
                    PeriodTime = input.PeriodTime,
                    PlannedMaintenanceTime = input.PlannedMaintenanceTime,
                    StationID = input.StationID,
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
                }).Where(new { Id = input.Id }, false, false, "");

                foreach (var item in input.SelectMaintenanceInstructionLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Insert(new CreateMaintenanceInstructionLinesDto
                        {
                            Amount = item.Amount,
                            InstructionDescription = item.InstructionDescription,
                            DeletionTime = null,
                            InstructionID = input.Id,
                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID,
                            UnitSetID = item.UnitSetID,
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectMaintenanceInstructionLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.MaintenanceInstructionLines).Update(new UpdateMaintenanceInstructionLinesDto
                            {
                                InstructionID = input.Id,
                                Amount = item.Amount,
                                InstructionDescription = item.InstructionDescription,
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
                                ProductID = item.ProductID,
                                UnitSetID = item.UnitSetID,
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var maintenanceInstruction = queryFactory.Update<SelectMaintenanceInstructionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstruction);
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructions)
                       .Select<MaintenanceInstructions>(mi => new { mi.StationID, mi.PlannedMaintenanceTime, mi.PeriodTime, mi.PeriodID, mi.Note_, mi.InstructionName, mi.Id, mi.DataOpenStatusUserId, mi.DataOpenStatus, mi.Code })
                       .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(MaintenanceInstructions.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<MaintenancePeriods>
                        (
                            mp => new { PeriodID = mp.Id, PeriodName = mp.Name, PeriodTime = mp.PeriodTime },
                            nameof(MaintenanceInstructions.PeriodID),
                            nameof(MaintenancePeriods.Id),
                            JoinType.Left
                        )
                        .Where(new { StationID = stationID, PeriodID = periodID }, false, false, Tables.MaintenanceInstructions);

                var maintenanceInstructions = queryFactory.Get<SelectMaintenanceInstructionsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.MaintenanceInstructionLines)
                       .Select<MaintenanceInstructionLines>(mil => new { mil.UnitSetID, mil.ProductID, mil.LineNr, mil.InstructionID, mil.InstructionDescription, mil.Id, mil.DataOpenStatusUserId, mil.DataOpenStatus, mil.Amount })
                       .Join<Products>
                        (
                            p => new { ProductID = p.Code, ProductCode = p.Code },
                            nameof(MaintenanceInstructionLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                       .Join<UnitSets>
                        (
                            u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                            nameof(MaintenanceInstructionLines.UnitSetID),
                            nameof(UnitSets.Id),
                            JoinType.Left
                        )
                        .Where(new { InstructionID = maintenanceInstructions.Id }, false, false, Tables.MaintenanceInstructionLines);

                var maintenanceInstructionLine = queryFactory.GetList<SelectMaintenanceInstructionLinesDto>(queryLines).ToList();

                maintenanceInstructions.SelectMaintenanceInstructionLines = maintenanceInstructionLine;

                LogsAppService.InsertLogToDatabase(maintenanceInstructions, maintenanceInstructions, LoginedUserService.UserId, Tables.MaintenanceInstructions, LogType.Get, maintenanceInstructions.Id);

                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructions);
            }
        }

        public async Task<IDataResult<SelectMaintenanceInstructionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.MaintenanceInstructions).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<MaintenanceInstructions>(entityQuery);

                var query = queryFactory.Query().From(Tables.MaintenanceInstructions).Update(new UpdateMaintenanceInstructionsDto
                {
                    InstructionName = entity.InstructionName,
                    Note_ = entity.Note_,
                    PeriodID = entity.PeriodID,
                    PeriodTime = entity.PeriodTime,
                    PlannedMaintenanceTime = entity.PlannedMaintenanceTime,
                    StationID = entity.StationID,
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
                }).Where(new { Id = id }, false, false, "");

                var maintenanceInstructionsDto = queryFactory.Update<SelectMaintenanceInstructionsDto>(query, "Id", true);
                return new SuccessDataResult<SelectMaintenanceInstructionsDto>(maintenanceInstructionsDto);

            }
        }
    }
}
