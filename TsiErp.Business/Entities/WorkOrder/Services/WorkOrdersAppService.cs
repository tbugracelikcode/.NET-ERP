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
using TsiErp.Business.Entities.WorkOrder.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.WorkOrders.Page;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    [ServiceRegistration(typeof(IWorkOrdersAppService), DependencyInjectionType.Scoped)]
    public class WorkOrdersAppService : ApplicationService<WorkOrdersResource>, IWorkOrdersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public WorkOrdersAppService(IStringLocalizer<WorkOrdersResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> CreateAsync(CreateWorkOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<WorkOrders>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.WorkOrders).Insert(new CreateWorkOrdersDto
                {
                    AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    IsCancel = input.IsCancel,
                    LineNr = input.LineNr,
                    LinkedWorkOrderID = input.LinkedWorkOrderID,
                    OccuredFinishDate = input.OccuredFinishDate,
                    OccuredStartDate = input.OccuredStartDate,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID,
                    ProductionOrderID = input.ProductionOrderID,
                    ProductsOperationID = input.ProductsOperationID,
                    PropositionID = input.PropositionID,
                    RouteID = input.RouteID,
                    StationGroupID = input.StationGroupID,
                    StationID = input.StationID,
                    WorkOrderNo = input.WorkOrderNo,
                    WorkOrderState = input.WorkOrderState,
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

                var workOrders = queryFactory.Insert<SelectWorkOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.WorkOrders, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.WorkOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.WorkOrders, LogType.Delete, id);

                return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);
            }

        }


        public async Task<IDataResult<SelectWorkOrdersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.WorkOrders).Select<WorkOrders>(wo => new {wo.WorkOrderState,wo.WorkOrderNo,wo.StationID,wo.StationGroupID,wo.RouteID,wo.PropositionID,wo.ProductsOperationID,wo.ProductionOrderID,wo.ProductID,wo.ProducedQuantity,wo.PlannedQuantity,wo.OperationTime,wo.OccuredStartDate,wo.OccuredFinishDate,wo.LinkedWorkOrderID,wo.LineNr,wo.IsCancel,wo.Id,wo.DataOpenStatusUserId,wo.DataOpenStatus,wo.CurrentAccountCardID,wo.Code,wo.AdjustmentAndControlTime})
                            .Join<ProductionOrders>
                            (
                                po => new { ProductionOrderID = po.Id, ProductionOrderFicheNo  = po.FicheNo},
                                nameof(WorkOrders.ProductionOrderID),
                                nameof(ProductionOrders.Id),
                                JoinType.Left
                            )
                             .Join<SalesPropositions>
                            (
                                sp => new { PropositionID  = sp.Id, PropositionFicheNo = sp.FicheNo},
                                nameof(WorkOrders.PropositionID),
                                nameof(SalesPropositions.Id),
                                JoinType.Left
                            )
                            .Join<Routes>
                            (
                                r => new { RouteID = r.Id, RouteCode = r.Code },
                                nameof(WorkOrders.RouteID),
                                nameof(Routes.Id),
                                JoinType.Left
                            )
                             .Join<ProductsOperations>
                            (
                                pro => new { ProductsOperationID = pro.Id, ProductsOperationCode = pro.Code},
                                nameof(WorkOrders.ProductsOperationID),
                                nameof(ProductsOperations.Id),
                                JoinType.Left
                            )
                            .Join<Stations>
                            (
                                s => new { StationID = s.Id, StationCode = s.Code, StationName = s.Name },
                                nameof(WorkOrders.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                             .Join<StationGroups>
                            (
                                sg => new { StationGroupID = sg.Id, StationGroupCode = sg.Code },
                                nameof(WorkOrders.StationGroupID),
                                nameof(StationGroups.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name},
                                nameof(WorkOrders.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                               .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                                nameof(WorkOrders.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.WorkOrders);

                var workOrder = queryFactory.Get<SelectWorkOrdersDto>(query);

                LogsAppService.InsertLogToDatabase(workOrder, workOrder, LoginedUserService.UserId, Tables.WorkOrders, LogType.Get, id);

                return new SuccessDataResult<SelectWorkOrdersDto>(workOrder);

            }

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWorkOrdersDto>>> GetListAsync(ListWorkOrdersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.WorkOrders).Select<WorkOrders>(wo => new { wo.WorkOrderState, wo.WorkOrderNo, wo.StationID, wo.StationGroupID, wo.RouteID, wo.PropositionID, wo.ProductsOperationID, wo.ProductionOrderID, wo.ProductID, wo.ProducedQuantity, wo.PlannedQuantity, wo.OperationTime, wo.OccuredStartDate, wo.OccuredFinishDate, wo.LinkedWorkOrderID, wo.LineNr, wo.IsCancel, wo.Id, wo.DataOpenStatusUserId, wo.DataOpenStatus, wo.CurrentAccountCardID, wo.Code, wo.AdjustmentAndControlTime })
                            .Join<ProductionOrders>
                            (
                                po => new { ProductionOrderFicheNo = po.FicheNo },
                                nameof(WorkOrders.ProductionOrderID),
                                nameof(ProductionOrders.Id),
                                JoinType.Left
                            )
                             .Join<SalesPropositions>
                            (
                                sp => new { PropositionFicheNo = sp.FicheNo },
                                nameof(WorkOrders.PropositionID),
                                nameof(SalesPropositions.Id),
                                JoinType.Left
                            )
                            .Join<Routes>
                            (
                                r => new { RouteCode = r.Code },
                                nameof(WorkOrders.RouteID),
                                nameof(Routes.Id),
                                JoinType.Left
                            )
                             .Join<ProductsOperations>
                            (
                                pro => new { ProductsOperationCode = pro.Code },
                                nameof(WorkOrders.ProductsOperationID),
                                nameof(ProductsOperations.Id),
                                JoinType.Left
                            )
                            .Join<Stations>
                            (
                                s => new { StationCode = s.Code, StationName = s.Name },
                                nameof(WorkOrders.StationID),
                                nameof(Stations.Id),
                                JoinType.Left
                            )
                             .Join<StationGroups>
                            (
                                sg => new { StationGroupCode = sg.Code },
                                nameof(WorkOrders.StationGroupID),
                                nameof(StationGroups.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { ProductCode = p.Code, ProductName = p.Name },
                                nameof(WorkOrders.ProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                               .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                                nameof(WorkOrders.CurrentAccountCardID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            ).Where(null, false, false, Tables.WorkOrders);

                var workOrders = queryFactory.GetList<ListWorkOrdersDto>(query).ToList();

                return new SuccessDataResult<IList<ListWorkOrdersDto>>(workOrders);
            }

        }


        [ValidationAspect(typeof(UpdateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateAsync(UpdateWorkOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<WorkOrders>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<WorkOrders>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
                {
                    AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    IsCancel = input.IsCancel,
                    LineNr = input.LineNr,
                    LinkedWorkOrderID = input.LinkedWorkOrderID,
                    OccuredFinishDate = input.OccuredFinishDate,
                    OccuredStartDate = input.OccuredStartDate,
                    OperationTime = input.OperationTime,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductID = input.ProductID,
                    ProductionOrderID = input.ProductionOrderID,
                    ProductsOperationID = input.ProductsOperationID,
                    PropositionID = input.PropositionID,
                    RouteID = input.RouteID,
                    StationGroupID = input.StationGroupID,
                    StationID = input.StationID,
                    WorkOrderNo = input.WorkOrderNo,
                    WorkOrderState = input.WorkOrderState,
                    Code = input.Code,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, workOrders, LoginedUserService.UserId, Tables.WorkOrders, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);
            }

        }

        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<WorkOrders>(entityQuery);

                var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
                {
                    AdjustmentAndControlTime = entity.AdjustmentAndControlTime,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    IsCancel = entity.IsCancel,
                    LineNr = entity.LineNr,
                    LinkedWorkOrderID = entity.LinkedWorkOrderID,
                    OccuredFinishDate = entity.OccuredFinishDate,
                    OccuredStartDate = entity.OccuredStartDate,
                    OperationTime = entity.OperationTime,
                    PlannedQuantity = entity.PlannedQuantity,
                    ProducedQuantity = entity.ProducedQuantity,
                    ProductID = entity.ProductID,
                    ProductionOrderID = entity.ProductionOrderID,
                    ProductsOperationID = entity.ProductsOperationID,
                    PropositionID = entity.PropositionID,
                    RouteID = entity.RouteID,
                    StationGroupID = entity.StationGroupID,
                    StationID = entity.StationID,
                    WorkOrderNo = entity.WorkOrderNo,
                    WorkOrderState = entity.WorkOrderState,
                    Code = entity.Code,
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

                var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

                return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

            }
        }
    }
}
