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
using TsiErp.Business.Entities.ProductionTracking.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionTrackings.Page;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    [ServiceRegistration(typeof(IProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ProductionTrackingsAppService : ApplicationService<ProductionTrackingsResource>, IProductionTrackingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ProductionTrackingsAppService(IStringLocalizer<ProductionTrackingsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> CreateAsync(CreateProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<ProductionTrackings>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Insert(new CreateProductionTrackingsDto
                {
                    AdjustmentTime = input.AdjustmentTime,
                    EmployeeID = input.EmployeeID,
                    HaltTime = input.HaltTime,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = GuidGenerator.CreateGuid(),
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

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Insert(new CreateProductionTrackingHaltLinesDto
                    {
                        HaltID = item.HaltID,
                        HaltTime = item.HaltTime,
                        ProductionTrackingID = addedEntityId,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }

                var productionTracking = queryFactory.Insert<SelectProductionTrackingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Id = id }, false, false, "");

                var productionTrackings = queryFactory.Get<SelectProductionTrackingsDto>(query);

                if (productionTrackings.Id != Guid.Empty && productionTrackings != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.ProductionTrackings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Delete(LoginedUserService.UserId).Where(new { ProductionTrackingID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var productionTrackingLines = queryFactory.Update<SelectProductionTrackingHaltLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionTrackingHaltLines, LogType.Delete, id);
                    return new SuccessDataResult<SelectProductionTrackingHaltLinesDto>(productionTrackingLines);
                }
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackings)
                       .Select("*")
                       .Join<WorkOrders>
                        (
                            wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.Code },
                            nameof(ProductionTrackings.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                         .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(ProductionTrackings.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<Shifts>
                        (
                            sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                            nameof(ProductionTrackings.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                            nameof(ProductionTrackings.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.ProductionTrackings);

                var productionTrackings = queryFactory.Get<SelectProductionTrackingsDto>(query);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackingHaltLines)
                       .Select<ProductionTrackingHaltLines>(hl => new { hl.ProductionTrackingID, hl.Id, hl.HaltTime, hl.HaltName, hl.HaltID, hl.HaltCode, hl.DataOpenStatusUserId, hl.DataOpenStatus })
                       .Join<HaltReasons>
                        (
                            hr => new { HaltID = hr.Id, HaltName = hr.Name, HaltCode = hr.Code },
                            nameof(ProductionTrackingHaltLines.HaltID),
                            nameof(HaltReasons.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductionTrackingID = id }, false, false, Tables.ProductionTrackingHaltLines);

                var productionTrackingLine = queryFactory.GetList<SelectProductionTrackingHaltLinesDto>(queryLines).ToList();

                productionTrackings.SelectProductionTrackingHaltLines = productionTrackingLine;

                LogsAppService.InsertLogToDatabase(productionTrackings, productionTrackings, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Get, id);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackings);
            }

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListAsync(ListProductionTrackingsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackings)
                       .Select("*")
                       .Join<WorkOrders>
                        (
                            wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.Code },
                            nameof(ProductionTrackings.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                         .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(ProductionTrackings.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<Shifts>
                        (
                            sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                            nameof(ProductionTrackings.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                            nameof(ProductionTrackings.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            e => new { CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(null, false, false, Tables.ProductionTrackings);

                var productionTrackings = queryFactory.GetList<ListProductionTrackingsDto>(query).ToList();
                return new SuccessDataResult<IList<ListProductionTrackingsDto>>(productionTrackings);
            }
        }

        [ValidationAspect(typeof(UpdateProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateAsync(UpdateProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackings)
                       .Select("*")
                       .Join<WorkOrders>
                        (
                            wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.Code },
                            nameof(ProductionTrackings.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                         .Join<Stations>
                        (
                            s => new { StationID = s.Id, StationCode = s.Code },
                            nameof(ProductionTrackings.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                        .Join<Shifts>
                        (
                            sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                            nameof(ProductionTrackings.ShiftID),
                            nameof(Shifts.Id),
                            JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                            nameof(ProductionTrackings.EmployeeID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            e => new { CurrentAccountCardID = e.Id, CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = input.Id }, false, false, Tables.ProductionTrackings);

                var entity = queryFactory.Get<SelectProductionTrackingsDto>(entityQuery);

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ProductionTrackingHaltLines)
                       .Select<ProductionTrackingHaltLines>(hl => new { hl.ProductionTrackingID, hl.Id, hl.HaltTime, hl.HaltName, hl.HaltID, hl.HaltCode, hl.DataOpenStatusUserId, hl.DataOpenStatus })
                       .Join<HaltReasons>
                        (
                            hr => new { HaltID = hr.Id, HaltName = hr.Name, HaltCode = hr.Code },
                            nameof(ProductionTrackingHaltLines.HaltID),
                            nameof(HaltReasons.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductionTrackingID = input.Id }, false, false, Tables.ProductionTrackingHaltLines);

                var productionTrackingLine = queryFactory.GetList<SelectProductionTrackingHaltLinesDto>(queryLines).ToList();

                entity.SelectProductionTrackingHaltLines = productionTrackingLine;

                #region Update Control
                var listQuery = queryFactory
                               .Query()
                               .From(Tables.ProductionTrackings)
                               .Select("*")
                               .Join<WorkOrders>
                                (
                                    wo => new { WorkOrderID = wo.Id, WorkOrderCode = wo.Code },
                                    nameof(ProductionTrackings.WorkOrderID),
                                    nameof(WorkOrders.Id),
                                    JoinType.Left
                                )
                                 .Join<Stations>
                                (
                                    s => new { StationID = s.Id, StationCode = s.Code },
                                    nameof(ProductionTrackings.StationID),
                                    nameof(Stations.Id),
                                    JoinType.Left
                                )
                                .Join<Shifts>
                                (
                                    sh => new { ShiftID = sh.Id, ShiftCode = sh.Code },
                                    nameof(ProductionTrackings.ShiftID),
                                    nameof(Shifts.Id),
                                    JoinType.Left
                                )
                                .Join<Employees>
                                (
                                    e => new { EmployeeID = e.Id, EmployeeName = e.Name },
                                    nameof(ProductionTrackings.EmployeeID),
                                    nameof(Employees.Id),
                                    JoinType.Left
                                )
                                  .Join<CurrentAccountCards>
                        (
                            e => new { CustomerCode = e.CustomerCode },
                            nameof(ProductionTrackings.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                                .Where(new { Code = input.Code }, false, false, Tables.ProductionTrackings);

                var list = queryFactory.GetList<ListProductionTrackingsDto>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
                {
                    AdjustmentTime = input.AdjustmentTime,
                    EmployeeID = input.EmployeeID,
                    HaltTime = input.HaltTime,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = input.WorkOrderID,
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

                foreach (var item in input.SelectProductionTrackingHaltLinesDto)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Insert(new CreateProductionTrackingHaltLinesDto
                        {
                            HaltID = item.HaltID,
                            HaltTime = item.HaltTime,
                            LastModifierId = Guid.Empty,
                            ProductionTrackingID = input.Id,
                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            Id = GuidGenerator.CreateGuid(),
                            IsDeleted = false,
                            LastModificationTime = null
                        });

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                    }
                    else
                    {
                        var lineGetQuery = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                        var line = queryFactory.Get<SelectProductionTrackingHaltLinesDto>(lineGetQuery);

                        if (line != null)
                        {
                            var queryLine = queryFactory.Query().From(Tables.ProductionTrackingHaltLines).Update(new UpdateProductionTrackingHaltLinesDto
                            {
                                HaltTime = item.HaltTime,
                                HaltID = item.HaltID,
                                ProductionTrackingID = input.Id,
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
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }

                var productionTracking = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ProductionTrackings, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTracking);
            }
        }

        public async Task<IDataResult<SelectProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductionTrackings).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<ProductionTrackings>(entityQuery);

                var query = queryFactory.Query().From(Tables.ProductionTrackings).Update(new UpdateProductionTrackingsDto
                {
                    AdjustmentTime = entity.AdjustmentTime,
                    EmployeeID = entity.EmployeeID,
                    HaltTime = entity.HaltTime,
                    IsFinished = entity.IsFinished,
                    OperationEndDate = entity.OperationEndDate,
                    OperationEndTime = entity.OperationEndTime,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    OperationStartDate = entity.OperationStartDate,
                    OperationStartTime = entity.OperationStartTime,
                    OperationTime = entity.OperationTime,
                    PlannedQuantity = entity.PlannedQuantity,
                    ProducedQuantity = entity.ProducedQuantity,
                    ShiftID = entity.ShiftID,
                    StationID = entity.StationID,
                    WorkOrderID = entity.WorkOrderID,
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
                }).Where(new { Id = id }, false, false, "");

                var productionTrackingsDto = queryFactory.Update<SelectProductionTrackingsDto>(query, "Id", true);
                return new SuccessDataResult<SelectProductionTrackingsDto>(productionTrackingsDto);

            }
        }
    }
}
