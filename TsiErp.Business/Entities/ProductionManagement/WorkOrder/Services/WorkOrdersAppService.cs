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
using TsiErp.Business.Entities.WorkOrder.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.WorkOrders.Page;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    [ServiceRegistration(typeof(IWorkOrdersAppService), DependencyInjectionType.Scoped)]
    public class WorkOrdersAppService : ApplicationService<WorkOrdersResource>, IWorkOrdersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public WorkOrdersAppService(IStringLocalizer<WorkOrdersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> CreateAsync(CreateWorkOrdersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("WorkOrderNo").Where(new { WorkOrderNo = input.WorkOrderNo },  "");

            var list = queryFactory.ControlList<WorkOrders>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.WorkOrders).Insert(new CreateWorkOrdersDto
            {
                AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                IsCancel = input.IsCancel,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                LineNr = input.LineNr,
                LinkedWorkOrderID = input.LinkedWorkOrderID.GetValueOrDefault(),
                OccuredFinishDate = input.OccuredFinishDate,
                SplitQuantity = 0,
                OccuredStartDate = input.OccuredStartDate,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderNo = input.WorkOrderNo,
                WorkOrderState = input.WorkOrderState,
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
                OrderID = input.OrderID
            });

            var workOrders = queryFactory.Insert<SelectWorkOrdersDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("WorkOrdersChildMenu", input.WorkOrderNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.WorkOrders, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WorkOrdersChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("LinkedWorkOrderID", new List<string>
            {
                Tables.WorkOrders
            });

            DeleteControl.ControlList.Add("WorkOrderId", new List<string>
            {
                Tables.OperationAdjustments
            });

            DeleteControl.ControlList.Add("WorkOrderID", new List<string>
            {
                Tables.ContractProductionTrackings,
                Tables.ContractTrackingFicheLines,
                Tables.ContractUnsuitabilityReports,
                Tables.FirstProductApprovals,
                Tables.OperationUnsuitabilityReports,
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
                var query = queryFactory.Query().From(Tables.WorkOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.WorkOrders, LogType.Delete, id);

                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WorkOrdersChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = string.Empty,
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
                                RecordNumber = string.Empty,
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
                return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);
            }
        }


        public async Task<IDataResult<SelectWorkOrdersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.WorkOrders).Select<WorkOrders>(null)
                        .Join<ProductionOrders>
                        (
                            po => new { ProductionOrderID = po.Id, ProductionOrderFicheNo = po.FicheNo },
                            nameof(WorkOrders.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionID = sp.Id, PropositionFicheNo = sp.FicheNo },
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
                            pro => new { ProductsOperationID = pro.Id, ProductsOperationCode = pro.Code, ProductsOperationName = pro.Name },
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
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
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
                        ).Join<SalesOrders>
                        (
                            so => new { OrderFicheNo = so.FicheNo, OrderID = so.Id },
                            nameof(WorkOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.WorkOrders);

            var workOrder = queryFactory.Get<SelectWorkOrdersDto>(query);

            LogsAppService.InsertLogToDatabase(workOrder, workOrder, LoginedUserService.UserId, Tables.WorkOrders, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrder);

        }

        public async Task<IDataResult<SelectWorkOrdersDto>> GetbyLinkedWorkOrderAsync(Guid linkedWorkOrderID)
        {
            var query = queryFactory
                    .Query().From(Tables.WorkOrders).Select<WorkOrders>(null)
                        .Join<ProductionOrders>
                        (
                            po => new { ProductionOrderID = po.Id, ProductionOrderFicheNo = po.FicheNo },
                            nameof(WorkOrders.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionID = sp.Id, PropositionFicheNo = sp.FicheNo },
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
                            pro => new { ProductsOperationID = pro.Id, ProductsOperationCode = pro.Code, ProductsOperationName = pro.Name },
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
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
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
                        ).Join<SalesOrders>
                        (
                            so => new { OrderFicheNo = so.FicheNo, OrderID = so.Id },
                            nameof(WorkOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                        .Where(new { LinkedWorkOrderID = linkedWorkOrderID }, Tables.WorkOrders);

            var workOrder = queryFactory.Get<SelectWorkOrdersDto>(query);

            LogsAppService.InsertLogToDatabase(workOrder, workOrder, LoginedUserService.UserId, Tables.WorkOrders, LogType.Get, workOrder.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrder);

        }



        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWorkOrdersDto>>> GetListAsync(ListWorkOrdersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.WorkOrders).Select<WorkOrders>(s => new { s.WorkOrderNo, s.WorkOrderState, s.AdjustmentAndControlTime, s.OperationTime, s.OccuredFinishDate, s.OccuredStartDate, s.PlannedQuantity, s.ProducedQuantity, s.Id })
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
                            pro => new { ProductsOperationCode = pro.Code, ProductsOperationName = pro.Name },
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
                        ).Where(null,Tables.WorkOrders);

            var workOrders = queryFactory.GetList<ListWorkOrdersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListWorkOrdersDto>>(workOrders);

        }

        public async Task<IDataResult<IList<SelectWorkOrdersDto>>> GetSelectListbyProductionOrderAsync(Guid productionOrderID)
        {
            var query = queryFactory
                    .Query().From(Tables.WorkOrders).Select<WorkOrders>(null)
                        .Join<ProductionOrders>
                        (
                            po => new { ProductionOrderID = po.Id, ProductionOrderFicheNo = po.FicheNo },
                            nameof(WorkOrders.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                         .Join<SalesPropositions>
                        (
                            sp => new { PropositionID = sp.Id, PropositionFicheNo = sp.FicheNo },
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
                            pro => new { ProductsOperationID = pro.Id, ProductsOperationCode = pro.Code, ProductsOperationName = pro.Name },
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
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
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
                        ).Join<SalesOrders>
                        (
                            so => new { OrderFicheNo = so.FicheNo, OrderID = so.Id },
                            nameof(WorkOrders.OrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )
                        .Where(new { ProductionOrderID = productionOrderID },  Tables.WorkOrders);

            var workOrders = queryFactory.GetList<SelectWorkOrdersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectWorkOrdersDto>>(workOrders);

        }


        [ValidationAspect(typeof(UpdateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateAsync(UpdateWorkOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<WorkOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { WorkOrderNo = input.WorkOrderNo }, "");
            var list = queryFactory.GetList<WorkOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.WorkOrderNo != input.WorkOrderNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
            {
                AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                IsCancel = input.IsCancel,
                LineNr = input.LineNr,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                LinkedWorkOrderID = input.LinkedWorkOrderID.GetValueOrDefault(),
                OccuredFinishDate = input.OccuredFinishDate,
                SplitQuantity = input.SplitQuantity,
                OccuredStartDate = input.OccuredStartDate,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderNo = input.WorkOrderNo,
                WorkOrderState = input.WorkOrderState,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                OrderID = input.OrderID
            }).Where(new { Id = input.Id }, "");

            var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, workOrders, LoginedUserService.UserId, Tables.WorkOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["WorkOrdersChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

        }

        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateChangeStationAsync(UpdateWorkOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<WorkOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { WorkOrderNo = input.WorkOrderNo }, "");
            var list = queryFactory.GetList<WorkOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.WorkOrderNo != input.WorkOrderNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
            {
                AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                IsCancel = input.IsCancel,
                LineNr = input.LineNr,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                LinkedWorkOrderID = input.LinkedWorkOrderID.GetValueOrDefault(),
                OccuredFinishDate = input.OccuredFinishDate,
                SplitQuantity = input.SplitQuantity,
                OccuredStartDate = input.OccuredStartDate,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderNo = input.WorkOrderNo,
                WorkOrderState = input.WorkOrderState,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                OrderID = input.OrderID
            }).Where(new { Id = input.Id }, "");

            var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, workOrders, LoginedUserService.UserId, Tables.WorkOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["WorkOrdersChildMenu"], L["WorkOrderContextChangeStation"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["WorkOrderContextChangeStation"],
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = string.Empty,
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
                            ContextMenuName_ = L["WorkOrderContextChangeStation"],
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

        }


        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateWorkOrderSplitAsync(UpdateWorkOrdersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<WorkOrders>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { WorkOrderNo = input.WorkOrderNo }, "");
            var list = queryFactory.GetList<WorkOrders>(listQuery).ToList();

            if (list.Count > 0 && entity.WorkOrderNo != input.WorkOrderNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
            {
                AdjustmentAndControlTime = input.AdjustmentAndControlTime,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                IsCancel = input.IsCancel,
                LineNr = input.LineNr,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                LinkedWorkOrderID = input.LinkedWorkOrderID.GetValueOrDefault(),
                OccuredFinishDate = input.OccuredFinishDate,
                SplitQuantity = input.SplitQuantity,
                OccuredStartDate = input.OccuredStartDate,
                OperationTime = input.OperationTime,
                PlannedQuantity = input.PlannedQuantity,
                ProducedQuantity = input.ProducedQuantity,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                PropositionID = input.PropositionID.GetValueOrDefault(),
                RouteID = input.RouteID.GetValueOrDefault(),
                StationGroupID = input.StationGroupID.GetValueOrDefault(),
                StationID = input.StationID.GetValueOrDefault(),
                WorkOrderNo = input.WorkOrderNo,
                WorkOrderState = input.WorkOrderState,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                OrderID = input.OrderID
            }).Where(new { Id = input.Id }, "");

            var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, workOrders, LoginedUserService.UserId, Tables.WorkOrders, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["WorkOrdersChildMenu"],  L["WorkOrderContextSplitWorkOrder"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["WorkOrderContextSplitWorkOrder"],
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = string.Empty,
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
                            ContextMenuName_ = L["WorkOrderContextSplitWorkOrder"],
                            IsViewed = false,
                            Message_ = notTemplate.Message_,
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

        }

        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.WorkOrders).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<WorkOrders>(entityQuery);

            var query = queryFactory.Query().From(Tables.WorkOrders).Update(new UpdateWorkOrdersDto
            {
                AdjustmentAndControlTime = entity.AdjustmentAndControlTime,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                IsCancel = entity.IsCancel,
                LineNr = entity.LineNr,
                LinkedWorkOrderID = entity.LinkedWorkOrderID,
                IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                SplitQuantity = entity.SplitQuantity,
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
                WorkOrderState = (int)entity.WorkOrderState,
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
                OrderID = entity.OrderID
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var workOrders = queryFactory.Update<SelectWorkOrdersDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectWorkOrdersDto>(workOrders);

        }
    }
}
