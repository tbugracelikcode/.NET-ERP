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
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.QualityControl.FirstProductApproval.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.FirstProductApprovals.Page;

namespace TsiErp.Business.Entities.FirstProductApproval.Services
{
    [ServiceRegistration(typeof(IFirstProductApprovalsAppService), DependencyInjectionType.Scoped)]
    public class FirstProductApprovalsAppService : ApplicationService<FirstProductApprovalsResource>, IFirstProductApprovalsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }


        public FirstProductApprovalsAppService(IStringLocalizer<FirstProductApprovalsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateFirstProductApprovalsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFirstProductApprovalsDto>> CreateAsync(CreateFirstProductApprovalsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Select("*").Where(new { Code = input.Code }, false, false, "");
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
                WorkOrderID = input.WorkOrderID,
                EmployeeID = input.EmployeeID,
                OperationQualityPlanID = input.OperationQualityPlanID,
                ProductID = input.ProductID,
                Code = input.Code,
                Description_ = input.Description_,
                CreationTime = DateTime.Now,
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
                IsApproval = input.IsApproval
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
                    CreationTime = DateTime.Now,
                    CreatorId = item.CreatorId != Guid.Empty ? item.CreatorId : LoginedUserService.UserId,
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

            var FirstProductApproval = queryFactory.Insert<SelectFirstProductApprovalsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("FirstProductApprovalChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApproval);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Select("*").Where(new { Id = id }, true, true, "");

            var FirstProductApprovals = queryFactory.Get<SelectFirstProductApprovalsDto>(query);

            if (FirstProductApprovals.Id != Guid.Empty && FirstProductApprovals != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.FirstProductApprovalLines).Delete(LoginedUserService.UserId).Where(new { FirstProductApprovalID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var billOfMaterial = queryFactory.Update<SelectFirstProductApprovalsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Delete, id);
                return new SuccessDataResult<SelectFirstProductApprovalsDto>(billOfMaterial);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.FirstProductApprovalLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var billOfMaterialLines = queryFactory.Update<SelectFirstProductApprovalLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.FirstProductApprovalLines, LogType.Delete, id);
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
                    .Where(new { Id = id }, false, false, Tables.FirstProductApprovals);

            var firstProductApprovals = queryFactory.Get<SelectFirstProductApprovalsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.FirstProductApprovalLines)
                   .Select("*")
                    .Where(new { FirstProductApprovalID = id }, false, false, Tables.FirstProductApprovalLines);

            var FirstProductApprovalLine = queryFactory.GetList<SelectFirstProductApprovalLinesDto>(queryLines).ToList();

            firstProductApprovals.SelectFirstProductApprovalLines = FirstProductApprovalLine;

            LogsAppService.InsertLogToDatabase(firstProductApprovals, firstProductApprovals, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Get, id);

            return new SuccessDataResult<SelectFirstProductApprovalsDto>(firstProductApprovals);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFirstProductApprovalsDto>>> GetListAsync(ListFirstProductApprovalsParameterDto input)
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
                    .Where(null, false, false, Tables.FirstProductApprovals);

            var firstProductApprovals = queryFactory.GetList<ListFirstProductApprovalsDto>(query).ToList();
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
                    .Where(new { Id = input.Id }, false, false, Tables.FirstProductApprovals);

            var entity = queryFactory.Get<SelectFirstProductApprovalsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.FirstProductApprovalLines)
                   .Select("*")
                    .Where(new { FirstProductApprovalID = input.Id }, false, false, Tables.FirstProductApprovalLines);

            var FirstProductApprovalLine = queryFactory.GetList<SelectFirstProductApprovalLinesDto>(queryLines).ToList();

            entity.SelectFirstProductApprovalLines = FirstProductApprovalLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.FirstProductApprovals)
                   .Select("*")
                    .Where(new { Code = input.Code }, false, false, Tables.FirstProductApprovals);

            var list = queryFactory.GetList<ListFirstProductApprovalsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Update(new UpdateFirstProductApprovalsDto
            {
                ControlDate = input.ControlDate,
                EmployeeID = input.EmployeeID,
                OperationQualityPlanID = input.OperationQualityPlanID,
                ProductID = input.ProductID,
                WorkOrderID = input.WorkOrderID,
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
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
                AdjustmentUserID = input.AdjustmentUserID.GetValueOrDefault(),
                IsApproval = entity.IsApproval
            }).Where(new { Id = input.Id }, false, false, "");

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
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.FirstProductApprovalLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var FirstProductApproval = queryFactory.Update<SelectFirstProductApprovalsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.FirstProductApprovals, LogType.Update, FirstProductApproval.Id);

            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApproval);

        }

        public async Task<IDataResult<SelectFirstProductApprovalsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.FirstProductApprovals).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<FirstProductApprovals>(entityQuery);

            var query = queryFactory.Query().From(Tables.FirstProductApprovals).Update(new UpdateFirstProductApprovalsDto
            {
                WorkOrderID = entity.WorkOrderID,
                ControlDate = entity.ControlDate,
                EmployeeID = entity.EmployeeID,
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
                IsApproval = entity.IsApproval
            }).Where(new { Id = id }, false, false, "");

            var FirstProductApprovalsDto = queryFactory.Update<SelectFirstProductApprovalsDto>(query, "Id", true);
            return new SuccessDataResult<SelectFirstProductApprovalsDto>(FirstProductApprovalsDto);


        }
    }
}
