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
using TsiErp.Business.Entities.QualityControl.FirstProductApproval.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.FirstProductApprovals.Page;

namespace TsiErp.Business.Entities.FirstProductApproval.Services
{
    [ServiceRegistration(typeof(IFirstProductApprovalsAppService), DependencyInjectionType.Scoped)]
    public class FirstProductApprovalsAppService : ApplicationService<FirstProductApprovalsResource>, IFirstProductApprovalsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;


        public FirstProductApprovalsAppService(IStringLocalizer<FirstProductApprovalsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateFirstProductApprovalsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFirstProductApprovalsDto>> CreateAsync(CreateFirstProductApprovalsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Select("Code").Where(new { Code = input.Code },  "");
            var list = queryFactory.ControlList<FirstProductApprovals>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Insert(new CreateFirstProductApprovalsDto
            {
                ControlDate = input.ControlDate,
                WorkOrderID = input.WorkOrderID.GetValueOrDefault(),
                IsFinalControl = input.IsFinalControl,
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
                OperationQualityPlanID = input.OperationQualityPlanID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Code = input.Code,
                Description_ = input.Description_,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = input.CreatorId != Guid.Empty ? input.CreatorId : LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                AdjustmentUserID = input.AdjustmentUserID.GetValueOrDefault(),
                IsApproval = input.IsApproval,
                ApprovedQuantity = input.ApprovedQuantity,
                ScrapQuantity = input.ScrapQuantity
            });

            foreach (var item in input.SelectFirstProductApprovalLines)
            {
                var queryLine = queryFactory.Query().From(Tables.FirstProductApprovalLines).Insert(new CreateFirstProductApprovalLinesDto
                {
                    BottomTolerance = item.BottomTolerance,
                    Description_ = item.Description_,
                    IdealMeasure = item.IdealMeasure,
                    IsCriticalMeasurement = item.IsCriticalMeasurement,
                    MeasurementValue = item.MeasurementValue,
                    UpperTolerance = item.UpperTolerance,
                    LineNr = item.LineNr,
                    FirstProductApprovalID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                    CreatorId = item.CreatorId != Guid.Empty ? item.CreatorId : LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var FirstProductApproval = queryFactory.Insert<SelectFirstProductApprovalsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("FirstProductApprovalChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["FirstProductApprovalChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApproval);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Select("*").Where(new { Id = id },  "");

            var FirstProductApprovals = queryFactory.Get<SelectFirstProductApprovalsDto>(query);

            if (FirstProductApprovals.Id != Guid.Empty && FirstProductApprovals != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.FirstProductApprovalLines).Delete(LoginedUserService.UserId).Where(new { FirstProductApprovalID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var billOfMaterial = queryFactory.Update<SelectFirstProductApprovalsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["FirstProductApprovalChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectFirstProductApprovalsDto>(billOfMaterial);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.FirstProductApprovalLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
                var billOfMaterialLines = queryFactory.Update<SelectFirstProductApprovalLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.FirstProductApprovalLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectFirstProductApprovalLinesDto>(billOfMaterialLines);
            }

        }

        public async Task<IDataResult<SelectFirstProductApprovalsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.FirstProductApprovals)
                   .Select<FirstProductApprovals>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(FirstProductApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { EmployeeID = p.Id, EmployeeName = p.Name },
                        nameof(FirstProductApprovals.EmployeeID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { AdjustmentUserID = p.Id, AdjustmentUser = p.Name },
                        nameof(FirstProductApprovals.AdjustmentUserID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        "AdjustmentUser",
                        JoinType.Left
                    )
                    .Join<OperationalQualityPlans>
                    (
                        p => new { OperationQualityPlanID = p.Id, OperationQualityPlanDocumentNumber = p.DocumentNumber },
                        nameof(FirstProductApprovals.OperationQualityPlanID),
                        nameof(OperationalQualityPlans.Id),
                        JoinType.Left
                    )
                    .Join<WorkOrders>
                    (
                        p => new { WorkOrderNo = p.WorkOrderNo },
                        nameof(FirstProductApprovals.WorkOrderID),
                        nameof(OperationalQualityPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.FirstProductApprovals);

            var firstProductApprovals = queryFactory.Get<SelectFirstProductApprovalsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.FirstProductApprovalLines)
                   .Select("*")
                    .Where(new { FirstProductApprovalID = id }, Tables.FirstProductApprovalLines);

            var FirstProductApprovalLine = queryFactory.GetList<SelectFirstProductApprovalLinesDto>(queryLines).ToList();

            firstProductApprovals.SelectFirstProductApprovalLines = FirstProductApprovalLine;

            LogsAppService.InsertLogToDatabase(firstProductApprovals, firstProductApprovals, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectFirstProductApprovalsDto>(firstProductApprovals);

        }

        [CacheAspectWithRemove(duration: 60, pattern: "Get")]
        public async Task<IDataResult<IList<ListFirstProductApprovalsDto>>> GetListAsync(ListFirstProductApprovalsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.FirstProductApprovals)
                   .Select<FirstProductApprovals>(s => new { s.Code, s.CreationTime, s.IsApproval, s.IsFinalControl, s.Id })
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(FirstProductApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { EmployeeID = p.Id, EmployeeName = p.Name },
                        nameof(FirstProductApprovals.EmployeeID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { AdjustmentUserID = p.Id, AdjustmentUser = p.Name },
                        nameof(FirstProductApprovals.AdjustmentUserID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        "AdjustmentUser",
                        JoinType.Left
                    )
                    .Join<OperationalQualityPlans>
                    (
                        p => new { OperationQualityPlanID = p.Id, OperationQualityPlanDocumentNumber = p.DocumentNumber },
                        nameof(FirstProductApprovals.OperationQualityPlanID),
                        nameof(OperationalQualityPlans.Id),
                        JoinType.Left
                    )
                    .Join<WorkOrders>
                    (
                        p => new { WorkOrderNo = p.WorkOrderNo },
                        nameof(FirstProductApprovals.WorkOrderID),
                        nameof(OperationalQualityPlans.Id),
                        JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderFicheNo = p.FicheNo, ProductionOrderID=p.Id },
                        nameof(FirstProductApprovals.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        "PO",
                        JoinType.Left
                    )
                    .Where(null, Tables.FirstProductApprovals);

            var firstProductApprovals = queryFactory.GetList<ListFirstProductApprovalsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListFirstProductApprovalsDto>>(firstProductApprovals);

        }

        [ValidationAspect(typeof(UpdateFirstProductApprovalsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFirstProductApprovalsDto>> UpdateAsync(UpdateFirstProductApprovalsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.FirstProductApprovals)
                   .Select<FirstProductApprovals>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(FirstProductApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { EmployeeID = p.Id, EmployeeName = p.Name },
                        nameof(FirstProductApprovals.EmployeeID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        JoinType.Left
                    )
                    .Join<TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees>
                    (
                        p => new { AdjustmentUserID = p.Id, AdjustmentUser = p.Name },
                        nameof(FirstProductApprovals.AdjustmentUserID),
                        nameof(TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Employees.Id),
                        "AdjustmentUser",
                        JoinType.Left
                    )
                    .Join<OperationalQualityPlans>
                    (
                        p => new { OperationQualityPlanID = p.Id, OperationQualityPlanDocumentNumber = p.DocumentNumber },
                        nameof(FirstProductApprovals.OperationQualityPlanID),
                        nameof(OperationalQualityPlans.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.FirstProductApprovals);

            var entity = queryFactory.Get<SelectFirstProductApprovalsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.FirstProductApprovalLines)
                   .Select("*")
                    .Where(new { FirstProductApprovalID = input.Id }, Tables.FirstProductApprovalLines);

            var FirstProductApprovalLine = queryFactory.GetList<SelectFirstProductApprovalLinesDto>(queryLines).ToList();

            entity.SelectFirstProductApprovalLines = FirstProductApprovalLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.FirstProductApprovals)
                   .Select("*")
                    .Where(new { Code = input.Code }, Tables.FirstProductApprovals);

            var list = queryFactory.GetList<ListFirstProductApprovalsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Update(new UpdateFirstProductApprovalsDto
            {
                ControlDate = input.ControlDate,
                EmployeeID = input.EmployeeID.GetValueOrDefault(),
                OperationQualityPlanID = input.OperationQualityPlanID.GetValueOrDefault(),
                IsFinalControl = input.IsFinalControl,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                WorkOrderID = input.WorkOrderID.GetValueOrDefault(),
                Code = input.Code,
                Description_ = input.Description_,
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
                AdjustmentUserID = input.AdjustmentUserID.GetValueOrDefault(),
                IsApproval = input.IsApproval,
                ApprovedQuantity = input.ApprovedQuantity,
                ScrapQuantity = input.ScrapQuantity
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectFirstProductApprovalLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.FirstProductApprovalLines).Insert(new CreateFirstProductApprovalLinesDto
                    {
                        BottomTolerance = item.BottomTolerance,
                        Description_ = item.Description_,
                        IdealMeasure = item.IdealMeasure,
                        IsCriticalMeasurement = item.IsCriticalMeasurement,
                        MeasurementValue = item.MeasurementValue,
                        UpperTolerance = item.UpperTolerance,
                        LineNr = item.LineNr,
                        FirstProductApprovalID = input.Id,
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
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.FirstProductApprovalLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectFirstProductApprovalLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.FirstProductApprovalLines).Update(new UpdateFirstProductApprovalLinesDto
                        {
                            BottomTolerance = item.BottomTolerance,
                            Description_ = item.Description_,
                            IdealMeasure = item.IdealMeasure,
                            IsCriticalMeasurement = item.IsCriticalMeasurement,
                            MeasurementValue = item.MeasurementValue,
                            UpperTolerance = item.UpperTolerance,
                            LineNr = item.LineNr,
                            FirstProductApprovalID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var FirstProductApproval = queryFactory.Update<SelectFirstProductApprovalsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Update, FirstProductApproval.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["FirstProductApprovalChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApproval);

        }

        public async Task<IDataResult<SelectFirstProductApprovalsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<FirstProductApprovals>(entityQuery);

            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Update(new UpdateFirstProductApprovalsDto
            {
                WorkOrderID = entity.WorkOrderID,
                ControlDate = entity.ControlDate,
                EmployeeID = entity.EmployeeID,
                IsFinalControl = entity.IsFinalControl,
                ProductionOrderID = entity.ProductionOrderID,
                OperationQualityPlanID = entity.OperationQualityPlanID,
                ProductID = entity.ProductID,
                Code = entity.Code,
                Description_ = entity.Description_,
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
                AdjustmentUserID = entity.AdjustmentUserID,
                IsApproval = entity.IsApproval,
                ApprovedQuantity = entity.ApprovedQuantity,
                ScrapQuantity = entity.ScrapQuantity
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var FirstProductApprovalsDto = queryFactory.Update<SelectFirstProductApprovalsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApprovalsDto);


        }




    }
}
