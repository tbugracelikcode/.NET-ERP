using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ContractProductionTracking.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractProductionTrackings.Page;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    [ServiceRegistration(typeof(IContractProductionTrackingsAppService), DependencyInjectionType.Scoped)]
    public class ContractProductionTrackingsAppService : ApplicationService<ContractProductionTrackingsResource>, IContractProductionTrackingsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ContractProductionTrackingsAppService(IStringLocalizer<ContractProductionTrackingsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> CreateAsync(CreateContractProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Insert(new CreateContractProductionTrackingsDto
                {
                    CurrentAccountID = input.CurrentAccountID,
                    EmployeeID = input.EmployeeID,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = input.WorkOrderID,
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

                var contractProductionTrackings = queryFactory.Insert<SelectContractProductionTrackingsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Delete, id);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            }
        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(c => new { c.Id, c.StationID, c.StationCode, c.CurrentAccountCardID, c.EmployeeID,c.EmployeeName,c.OperationEndDate,c.OperationEndTime,c.OperationStartDate,c.OperationStartTime,c.OperationTime,c.PlannedQuantity,c.ProducedQuantity,c.ProductID,c.ShiftCode,c.ShiftID,c.WorkOrderID, c.DataOpenStatus, c.DataOpenStatusUserId })
                            .Join<WorkOrders>
                            (
                                w => new { WorkOrderCode = w.Code, WorkOrderID = w.Id },
                                nameof(ContractProductionTrackings.WorkOrderID),
                                nameof(WorkOrders.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                                nameof(ContractProductionTrackings.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<Stations>
                            (
                                s => new { StationCode = s.Code, StationID = s.Id },
                                nameof(ContractProductionTrackings.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                              .Join<Shifts>
                            (
                                sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                                nameof(ContractProductionTrackings.ShiftID),
                                nameof(Shifts.Id),
                                JoinType.Left
                            )
                             .Join<Employees>
                            (
                                e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                                nameof(ContractProductionTrackings.EmployeeID),
                                nameof(Employees.Id),
                                JoinType.Left
                            )
                             .Join<CurrentAccountCards>
                            (
                                c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                                nameof(ContractProductionTrackings.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.ContractProductionTrackings);

                var contractProductionTracking = queryFactory.Get<SelectContractProductionTrackingsDto>(query);

                LogsAppService.InsertLogToDatabase(contractProductionTracking, contractProductionTracking, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Get, id);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTracking);

            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractProductionTrackingsDto>>> GetListAsync(ListContractProductionTrackingsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(c => new { c.Id, c.StationID, c.StationCode, c.CurrentAccountCardID, c.EmployeeID, c.EmployeeName, c.OperationEndDate, c.OperationEndTime, c.OperationStartDate, c.OperationStartTime, c.OperationTime, c.PlannedQuantity, c.ProducedQuantity, c.ProductID, c.ShiftCode, c.ShiftID, c.WorkOrderID })
                           .Join<WorkOrders>
                           (
                               w => new { WorkOrderCode = w.Code, WorkOrderID = w.Id },
                               nameof(ContractProductionTrackings.WorkOrderID),
                               nameof(WorkOrders.Id),
                               JoinType.Left
                           )
                            .Join<Products>
                           (
                               p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                               nameof(ContractProductionTrackings.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
                            .Join<Stations>
                           (
                               s => new { StationCode = s.Code, StationID = s.Id },
                               nameof(ContractProductionTrackings.StationID),
                               nameof(Stations.Id),
                               JoinType.Left
                           )
                             .Join<Shifts>
                           (
                               sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                               nameof(ContractProductionTrackings.ShiftID),
                               nameof(Shifts.Id),
                               JoinType.Left
                           )
                            .Join<Employees>
                           (
                               e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                               nameof(ContractProductionTrackings.EmployeeID),
                               nameof(Employees.Id),
                               JoinType.Left
                           )
                            .Join<CurrentAccountCards>
                           (
                               c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                               nameof(ContractProductionTrackings.CurrentAccountCardID),
                               nameof(CurrentAccountCards.Id),
                               JoinType.Left
                           )
                           .Where(null, false, false, Tables.ContractProductionTrackings);


                var contractProductionTrackings = queryFactory.GetList<ListContractProductionTrackingsDto>(query).ToList();
                return new SuccessDataResult<IList<ListContractProductionTrackingsDto>>(contractProductionTrackings);
            }
        }

        [ValidationAspect(typeof(UpdateContractProductionTrackingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateAsync(UpdateContractProductionTrackingsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ContractProductionTrackings).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ContractProductionTrackings>(entityQuery);

                var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Update(new UpdateContractProductionTrackingsDto
                {
                    CurrentAccountID = input.CurrentAccountID,
                    EmployeeID = input.EmployeeID,
                    IsFinished = input.IsFinished,
                    OperationEndDate = input.OperationEndDate,
                    OperationEndTime = input.OperationEndTime,
                    OperationStartDate = input.OperationStartDate,
                    OperationStartTime = input.OperationStartTime,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID,
                    ShiftID = input.ShiftID,
                    StationID = input.StationID,
                    WorkOrderID = input.WorkOrderID,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, contractProductionTrackings, LoginedUserService.UserId, Tables.ContractProductionTrackings, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);
            }
        }

        public async Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query().From(Tables.ContractProductionTrackings).Select<ContractProductionTrackings>(c => new { c.Id, c.StationID, c.StationCode, c.CurrentAccountCardID, c.EmployeeID, c.EmployeeName, c.OperationEndDate, c.OperationEndTime, c.OperationStartDate, c.OperationStartTime, c.OperationTime, c.PlannedQuantity, c.ProducedQuantity, c.ProductID, c.ShiftCode, c.ShiftID, c.WorkOrderID })
                           .Join<WorkOrders>
                           (
                               w => new { WorkOrderCode = w.Code, WorkOrderID = w.Id },
                               nameof(ContractProductionTrackings.WorkOrderID),
                               nameof(WorkOrders.Id),
                               JoinType.Left
                           )
                            .Join<Products>
                           (
                               p => new { ProductCode = p.Code, ProductName = p.Name, ProductID = p.Id },
                               nameof(ContractProductionTrackings.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
                            .Join<Stations>
                           (
                               s => new { StationCode = s.Code, StationID = s.Id },
                               nameof(ContractProductionTrackings.StationID),
                               nameof(Stations.Id),
                               JoinType.Left
                           )
                             .Join<Shifts>
                           (
                               sh => new { ShiftCode = sh.Code, ShiftID = sh.Id },
                               nameof(ContractProductionTrackings.ShiftID),
                               nameof(Shifts.Id),
                               JoinType.Left
                           )
                            .Join<Employees>
                           (
                               e => new { EmployeeName = e.Name, EmployeeID = e.Id },
                               nameof(ContractProductionTrackings.EmployeeID),
                               nameof(Employees.Id),
                               JoinType.Left
                           )
                            .Join<CurrentAccountCards>
                           (
                               c => new { CurrentAccountCardCode = c.Code, CurrentAccountCardID = c.Id },
                               nameof(ContractProductionTrackings.CurrentAccountCardID),
                               nameof(CurrentAccountCards.Id),
                               JoinType.Left
                           )
                           .Where(new { ProductID = productId }, false, false, Tables.ContractProductionTrackings);


                var contractProductionTrackings = queryFactory.GetList<SelectContractProductionTrackingsDto>(query).ToList();
                return new SuccessDataResult<IList<SelectContractProductionTrackingsDto>>(contractProductionTrackings);
            }


        }

        public async Task<IDataResult<SelectContractProductionTrackingsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ContractProductionTrackings).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<ContractProductionTrackings>(entityQuery);

                var query = queryFactory.Query().From(Tables.ContractProductionTrackings).Update(new UpdateContractProductionTrackingsDto
                {
                    CurrentAccountID = entity.CurrentAccountCardID,
                    EmployeeID = entity.EmployeeID,
                    IsFinished = entity.IsFinished,
                    OperationEndDate = entity.OperationEndDate,
                    OperationEndTime = entity.OperationEndTime,
                    OperationStartDate = entity.OperationStartDate,
                    OperationStartTime = entity.OperationStartTime,
                    OperationTime = entity.OperationTime,
                    PlannedQuantity = entity.PlannedQuantity,
                    ProducedQuantity = entity.ProducedQuantity,
                    ProductID = entity.ProductID,
                    ShiftID = entity.ShiftID,
                    StationID = entity.StationID,
                    WorkOrderID = entity.WorkOrderID,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, false, false, "");

                var contractProductionTrackings = queryFactory.Update<SelectContractProductionTrackingsDto>(query, "Id", true);

                return new SuccessDataResult<SelectContractProductionTrackingsDto>(contractProductionTrackings);

            }
        }
    }
}
