using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ContractQualityPlan.Services;
using TsiErp.Business.Entities.ContractTrackingFiche.Services;
using TsiErp.Business.Entities.ContractUnsuitabilityReport.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductsOperation.Services;
using TsiErp.Business.Entities.QualityControl.ContractQualityPlan.Services;
using TsiErp.Business.Entities.Route.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.ContractUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityReportsAppService : ApplicationService<ContractUnsuitabilityReportsResource>, IContractUnsuitabilityReportsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IWorkOrdersAppService _WorkOrdersAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IContractQualityPlansAppService _ContractQualityPlansAppService;
        private readonly IRoutesAppService _RoutesAppService;
        private readonly IContractTrackingFichesAppService _ContractTrackingFichesAppService;
        private readonly IProductsOperationsAppService _ProductsOperationsAppService;

        public ContractUnsuitabilityReportsAppService(IStringLocalizer<ContractUnsuitabilityReportsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IWorkOrdersAppService workOrdersAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IContractQualityPlansAppService contractQualityPlansAppService, IRoutesAppService routesAppService, IContractTrackingFichesAppService contractTrackingFichesAppService, IProductsOperationsAppService productsOperationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _WorkOrdersAppService = workOrdersAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _ContractQualityPlansAppService = contractQualityPlansAppService;
            _RoutesAppService = routesAppService;
            _ContractTrackingFichesAppService = contractTrackingFichesAppService;
            _ProductsOperationsAppService = productsOperationsAppService;
        }



        [ValidationAspect(typeof(CreateContractUnsuitabilityReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> CreateAsync(CreateContractUnsuitabilityReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");

            var list = queryFactory.ControlList<ContractUnsuitabilityReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();
            bool isUnsWorkOrderCreated = false;


            if (input.IsUnsuitabilityWorkOrder && input.Action_ != L["ComboboxToBeUsedAs"].Value)
            {
                SelectContractTrackingFichesDto contractTrackingFiche = (await _ContractTrackingFichesAppService.GetAsync(input.ContractTrackingFicheID.Value)).Data;

                if (contractTrackingFiche != null && contractTrackingFiche.Id != Guid.Empty)
                {
                    SelectContractQualityPlansDto contractQualityPlan = (await _ContractQualityPlansAppService.GetAsync(contractTrackingFiche.ContractQualityPlanID.Value)).Data;

                    if (contractQualityPlan != null && contractQualityPlan.Id != Guid.Empty && contractQualityPlan.SelectContractQualityPlanOperations != null && contractQualityPlan.SelectContractQualityPlanOperations.Count > 0)
                    {
                        foreach (var line in contractQualityPlan.SelectContractQualityPlanOperations)
                        {
                            SelectRoutesDto productsRoute = (await _RoutesAppService.GetbyProductIDAsync(contractQualityPlan.ProductID.Value)).Data;

                            if (productsRoute == null)
                            {
                                productsRoute.Id = Guid.Empty;
                            }

                            var productsOperation = (await _ProductsOperationsAppService.GetAsync(line.OperationID.Value)).Data;

                            string workOrderNo = string.Empty;

                            SelectWorkOrdersDto lastSuitibleWorkOrder = (await _WorkOrdersAppService.GetbyProductionOrderOperationRouteAsync(input.ProductionOrderID.GetValueOrDefault(), line.OperationID.GetValueOrDefault(), productsRoute.Id)).Data;

                            if (!lastSuitibleWorkOrder.WorkOrderNo.Contains("."))
                            {
                                workOrderNo = lastSuitibleWorkOrder.WorkOrderNo + ".1";
                            }
                            else
                            {
                                string[] workOrderNoArr = lastSuitibleWorkOrder.WorkOrderNo.Split(".");

                                int versionNo = Convert.ToInt32(workOrderNoArr[1]);

                                workOrderNo = workOrderNoArr[0] + "." + (versionNo + 1).ToString();
                            }

                            CreateWorkOrdersDto createdWorkOrder = new CreateWorkOrdersDto
                            {
                                AdjustmentAndControlTime = 0,
                                CurrentAccountCardID = contractQualityPlan.CurrrentAccountCardID,
                                IsCancel = false,
                                IsOperationUnsuitabilityWorkOrder = false,
                                IsContractUnsuitabilityWorkOrder = true,
                                LineNr = 1,
                                LinkedWorkOrderID = Guid.Empty,
                                OccuredStartDate = null,
                                OccuredFinishDate = null,
                                OperationTime = 0,
                                OrderID = Guid.Empty,
                                PlannedQuantity = input.UnsuitableAmount,
                                ProducedQuantity = 0,
                                ProductID = contractQualityPlan.ProductID,
                                ProductionOrderID = input.ProductionOrderID,
                                ProductsOperationID = line.OperationID,
                                RouteID = productsRoute.Id,
                                PropositionID = Guid.Empty,
                                SplitQuantity = 0,
                                StationGroupID = productsOperation.WorkCenterID,
                                StationID = Guid.Empty,
                                WorkOrderState = 1,
                                WorkOrderNo = workOrderNo,

                            };


                            await _WorkOrdersAppService.CreateAsync(createdWorkOrder);

                            isUnsWorkOrderCreated = true;
                        }
                    }
                }

            }

            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Insert(new CreateContractUnsuitabilityReportsDto
            {
                FicheNo = input.FicheNo,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault(),
                isUnsuitabilityWorkOrderCreated = isUnsWorkOrderCreated,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                Date_ = input.Date_,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Action_ = input.Action_,
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                UnsuitableAmount = input.UnsuitableAmount,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ContractTrackingFicheID = input.ContractTrackingFicheID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                WorkOrderID = input.WorkOrderID.GetValueOrDefault()
            });

            var ContractUnsuitabilityReport = queryFactory.Insert<SelectContractUnsuitabilityReportsDto>(query, "Id", true);


            await FicheNumbersAppService.UpdateFicheNumberAsync("ContUnsRecordsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContUnsRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Delete, id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContUnsRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = entity.FicheNo,
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
                            RecordNumber = entity.FicheNo,
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
            return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select<ContractUnsuitabilityReports>(null)
                .Join<WorkOrders>
                (
                   d => new { WorkOrderFicheNr = d.WorkOrderNo, WorkOrderID = d.Id }, nameof(ContractUnsuitabilityReports.WorkOrderID), nameof(WorkOrders.Id), JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNr = d.FicheNo, ProductionOrderID = d.Id }, nameof(ContractUnsuitabilityReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name, UnsuitabilityItemsID = d.Id }, nameof(ContractUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Join<ContractTrackingFiches>
                (
                   d => new { ContractTrackingFicheNr = d.FicheNr, ContractTrackingFicheID = d.Id }, nameof(ContractUnsuitabilityReports.ContractTrackingFicheID), nameof(ContractTrackingFiches.Id), JoinType.Left
                )
                 .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name, CurrentAccountCardID = d.Id },
                   nameof(ContractUnsuitabilityReports.CurrentAccountCardID),
                   nameof(CurrentAccountCards.Id),
                   JoinType.Left
                )
                .Where(new { Id = id }, Tables.ContractUnsuitabilityReports);

            var ContractUnsuitabilityReport = queryFactory.Get<SelectContractUnsuitabilityReportsDto>(query);

            LogsAppService.InsertLogToDatabase(ContractUnsuitabilityReport, ContractUnsuitabilityReport, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

        }

        public async Task<IDataResult<IList<ListContractUnsuitabilityReportsDto>>> GetListAsync(ListContractUnsuitabilityReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select<ContractUnsuitabilityReports>(null)
               .Join<WorkOrders>
                (
                   d => new { WorkOrderFicheNr = d.WorkOrderNo },
                   nameof(ContractUnsuitabilityReports.WorkOrderID),
                   nameof(WorkOrders.Id),
                   JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNr = d.FicheNo },
                   nameof(ContractUnsuitabilityReports.ProductionOrderID),
                   nameof(ProductionOrders.Id),
                   JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name, UnsuitabilityItemsId = d.Id },
                   nameof(ContractUnsuitabilityReports.UnsuitabilityItemsID),
                   nameof(UnsuitabilityItems.Id),
                   JoinType.Left
                )
                .Join<ContractTrackingFiches>
                (
                   d => new { ContractTrackingFicheNr = d.FicheNr }, nameof(ContractUnsuitabilityReports.ContractTrackingFicheID), nameof(ContractTrackingFiches.Id), JoinType.Left
                )
                 .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(ContractUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                )
                .Where(null, Tables.ContractUnsuitabilityReports);

            var contractUnsuitabilityReports = queryFactory.GetList<ListContractUnsuitabilityReportsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContractUnsuitabilityReportsDto>>(contractUnsuitabilityReports);


        }

        /// <summary>
        /// ADMİN DASHBOARD GETLIST
        /// </summary>

        public async Task<IDataResult<IList<ListContractUnsuitabilityReportsDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate)
        {
            string resultQuery = "SELECT * FROM " + Tables.ContractUnsuitabilityReports;

            string where = string.Empty;

            where = " (Date_>='" + startDate + "' and '" + endDate + "'>=Date_) ";


            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;
            var stockFicheLine = queryFactory.GetList<ListContractUnsuitabilityReportsDto>(query).ToList();
            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListContractUnsuitabilityReportsDto>>(stockFicheLine);

        }

        [ValidationAspect(typeof(UpdateContractUnsuitabilityReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> UpdateAsync(UpdateContractUnsuitabilityReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ContractUnsuitabilityReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, "");
            var list = queryFactory.GetList<ContractUnsuitabilityReports>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            bool isUnsWorkOrderCreated = false;

            if (input.IsUnsuitabilityWorkOrder && input.Action_ != L["ComboboxToBeUsedAs"].Value)
            {

                if (!input.isUnsuitabilityWorkOrderCreated)
                {
                    SelectContractTrackingFichesDto contractTrackingFiche = (await _ContractTrackingFichesAppService.GetAsync(input.ContractTrackingFicheID.Value)).Data;

                    if (contractTrackingFiche != null && contractTrackingFiche.Id != Guid.Empty)
                    {
                        SelectContractQualityPlansDto contractQualityPlan = (await _ContractQualityPlansAppService.GetAsync(contractTrackingFiche.ContractQualityPlanID.Value)).Data;

                        if (contractQualityPlan != null && contractQualityPlan.Id != Guid.Empty && contractQualityPlan.SelectContractQualityPlanOperations != null && contractQualityPlan.SelectContractQualityPlanOperations.Count > 0)
                        {
                            foreach (var line in contractQualityPlan.SelectContractQualityPlanOperations)
                            {
                                SelectRoutesDto productsRoute = (await _RoutesAppService.GetbyProductIDAsync(contractQualityPlan.ProductID.Value)).Data;

                                if (productsRoute == null)
                                {
                                    productsRoute.Id = Guid.Empty;
                                }

                                var productsOperation = (await _ProductsOperationsAppService.GetAsync(line.OperationID.Value)).Data;

                                string workOrderNo = string.Empty;

                                SelectWorkOrdersDto lastSuitibleWorkOrder = (await _WorkOrdersAppService.GetbyProductionOrderOperationRouteAsync(input.ProductionOrderID.GetValueOrDefault(), line.OperationID.GetValueOrDefault(), productsRoute.Id)).Data;

                                if (!lastSuitibleWorkOrder.WorkOrderNo.Contains("."))
                                {
                                    workOrderNo = lastSuitibleWorkOrder.WorkOrderNo + ".1";
                                }
                                else
                                {
                                    string[] workOrderNoArr = lastSuitibleWorkOrder.WorkOrderNo.Split(".");

                                    int versionNo = Convert.ToInt32(workOrderNoArr[1]);

                                    workOrderNo = workOrderNoArr[0] + "." + (versionNo + 1).ToString();
                                }

                                CreateWorkOrdersDto createdWorkOrder = new CreateWorkOrdersDto
                                {
                                    AdjustmentAndControlTime = 0,
                                    CurrentAccountCardID = contractQualityPlan.CurrrentAccountCardID,
                                    IsCancel = false,
                                    IsOperationUnsuitabilityWorkOrder = false,
                                    IsContractUnsuitabilityWorkOrder = true,
                                    LineNr = 1,
                                    LinkedWorkOrderID = Guid.Empty,
                                    OccuredStartDate = null,
                                    OccuredFinishDate = null,
                                    OperationTime = 0,
                                    OrderID = Guid.Empty,
                                    PlannedQuantity = input.UnsuitableAmount,
                                    ProducedQuantity = 0,
                                    ProductID = contractQualityPlan.ProductID,
                                    ProductionOrderID = input.ProductionOrderID,
                                    ProductsOperationID = line.OperationID,
                                    RouteID = productsRoute.Id,
                                    PropositionID = Guid.Empty,
                                    SplitQuantity = 0,
                                    StationGroupID = productsOperation.WorkCenterID,
                                    StationID = Guid.Empty,
                                    WorkOrderState = 1,
                                    WorkOrderNo = workOrderNo,

                                };


                                await _WorkOrdersAppService.CreateAsync(createdWorkOrder);

                                isUnsWorkOrderCreated = true;
                            }
                        }
                    }
                }

            }

            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Update(new UpdateContractUnsuitabilityReportsDto
            {
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault(),
                isUnsuitabilityWorkOrderCreated = isUnsWorkOrderCreated,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Action_ = input.Action_,
                FicheNo = input.FicheNo,
                Date_ = input.Date_,
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                ContractTrackingFicheID = input.ContractTrackingFicheID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                UnsuitableAmount = input.UnsuitableAmount,
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                WorkOrderID = input.WorkOrderID.GetValueOrDefault()
            }).Where(new { Id = input.Id }, "");

            var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);



            LogsAppService.InsertLogToDatabase(entity, ContractUnsuitabilityReport, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContUnsRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<ContractUnsuitabilityReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Update(new UpdateContractUnsuitabilityReportsDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                isUnsuitabilityWorkOrderCreated = entity.isUnsuitabilityWorkOrderCreated,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                UnsuitabilityItemsID = entity.UnsuitabilityItemsID,
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                Action_ = entity.Action_,
                UnsuitableAmount = entity.UnsuitableAmount,
                IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                FicheNo = entity.FicheNo,
                Description_ = entity.Description_,
                Date_ = entity.Date_,
                ProductionOrderID = entity.ProductionOrderID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                ContractTrackingFicheID = entity.ContractTrackingFicheID,
                WorkOrderID = entity.WorkOrderID
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

        }
    }
}
