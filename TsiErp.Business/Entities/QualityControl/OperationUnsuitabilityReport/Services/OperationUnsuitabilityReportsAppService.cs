using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OperationUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityReportsAppService : ApplicationService<OperationUnsuitabilityReportsResource>, IOperationUnsuitabilityReportsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public OperationUnsuitabilityReportsAppService(IStringLocalizer<OperationUnsuitabilityReportsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> CreateAsync(CreateOperationUnsuitabilityReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");

            var list = queryFactory.ControlList<OperationUnsuitabilityReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Insert(new CreateOperationUnsuitabilityReportsDto
            {
                FicheNo = input.FicheNo,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID,
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
                ProductID = input.ProductID,
                UnsuitableAmount = input.UnsuitableAmount,
                EmployeeID = input.EmployeeID,
                OperationID = input.OperationID,
                ProductionOrderID = input.ProductionOrderID,
                StationGroupID = input.StationGroupID,
                StationID = input.StationID,
                WorkOrderID = input.WorkOrderID
            });


            var operationUnsuitabilityReport = queryFactory.Insert<SelectOperationUnsuitabilityReportsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("OprUnsRecordsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationUnsuitabilityReports, LogType.Insert, addedEntityId);

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(operationUnsuitabilityReport);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var operationUnsuitabilityReport = queryFactory.Update<SelectOperationUnsuitabilityReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationUnsuitabilityReports, LogType.Delete, id);

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(operationUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select<OperationUnsuitabilityReports>(null)
                .Join<WorkOrders>
                (
                   d => new { WorkOrderNo = d.WorkOrderNo }, nameof(OperationUnsuitabilityReports.WorkOrderID), nameof(WorkOrders.Id), JoinType.Left
                )
                .Join<Stations>
                (
                   d => new { StationCode = d.Code, StationName = d.Name }, nameof(OperationUnsuitabilityReports.StationID), nameof(Stations.Id), JoinType.Left
                )
                .Join<StationGroups>
                (
                   d => new { StationGroupCode = d.Code, StationGroupName = d.Name }, nameof(OperationUnsuitabilityReports.StationGroupID), nameof(StationGroups.Id), JoinType.Left
                )
                .Join<Employees>
                (
                   d => new { EmployeeName = d.Name + " " + d.Surname }, nameof(OperationUnsuitabilityReports.EmployeeID), nameof(Employees.Id), JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNo = d.FicheNo }, nameof(OperationUnsuitabilityReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(OperationUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<ProductsOperations>
                (
                   d => new { OperationCode = d.Code, OperationName = d.Name }, nameof(OperationUnsuitabilityReports.OperationID), nameof(ProductsOperations.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name, UnsuitabilityItemsID = d.Id }, nameof(OperationUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, false, false, Tables.OperationUnsuitabilityReports);

            var operationUnsuitabilityReport = queryFactory.Get<SelectOperationUnsuitabilityReportsDto>(query);

            LogsAppService.InsertLogToDatabase(operationUnsuitabilityReport, operationUnsuitabilityReport, LoginedUserService.UserId, Tables.OperationUnsuitabilityReports, LogType.Get, id);

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(operationUnsuitabilityReport);


        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationUnsuitabilityReportsDto>>> GetListAsync(ListOperationUnsuitabilityReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select<OperationUnsuitabilityReports>(null)
                .Join<WorkOrders>
                (
                   d => new { WorkOrderNo = d.WorkOrderNo }, nameof(OperationUnsuitabilityReports.WorkOrderID), nameof(WorkOrders.Id), JoinType.Left
                )
                .Join<Stations>
                (
                   d => new { StationCode = d.Code, StationName = d.Name }, nameof(OperationUnsuitabilityReports.StationID), nameof(Stations.Id), JoinType.Left
                )
                .Join<StationGroups>
                (
                   d => new { StationGroupCode = d.Code, StationGroupName = d.Name }, nameof(OperationUnsuitabilityReports.StationGroupID), nameof(StationGroups.Id), JoinType.Left
                )
                .Join<Employees>
                (
                   d => new { EmployeeName = d.Name + " " + d.Surname }, nameof(OperationUnsuitabilityReports.EmployeeID), nameof(Employees.Id), JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNo = d.FicheNo }, nameof(OperationUnsuitabilityReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(OperationUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<ProductsOperations>
                (
                   d => new { OperationCode = d.Code, OperationName = d.Name }, nameof(OperationUnsuitabilityReports.OperationID), nameof(ProductsOperations.Id), JoinType.Left
                )
                 .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name }, nameof(OperationUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, false, false, Tables.OperationUnsuitabilityReports);

            var operationUnsuitabilityReports = queryFactory.GetList<ListOperationUnsuitabilityReportsDto>(query).ToList();

            return new SuccessDataResult<IList<ListOperationUnsuitabilityReportsDto>>(operationUnsuitabilityReports);


        }

        [ValidationAspect(typeof(UpdateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateAsync(UpdateOperationUnsuitabilityReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<OperationUnsuitabilityReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
            var list = queryFactory.GetList<OperationUnsuitabilityReports>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Update(new UpdateOperationUnsuitabilityReportsDto
            {
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                UnsuitabilityItemsID = input.UnsuitabilityItemsID,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
                Action_ = input.Action_,
                FicheNo = input.FicheNo,
                Date_ = input.Date_,
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                ProductID = input.ProductID,
                UnsuitableAmount = input.UnsuitableAmount,
                EmployeeID = input.EmployeeID,
                OperationID = input.OperationID,
                ProductionOrderID = input.ProductionOrderID,
                StationGroupID = input.StationGroupID,
                StationID = input.StationID,
                WorkOrderID = input.WorkOrderID
            }).Where(new { Id = input.Id }, false, false, "");

            var operationUnsuitabilityReport = queryFactory.Update<SelectOperationUnsuitabilityReportsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, operationUnsuitabilityReport, LoginedUserService.UserId, Tables.OperationUnsuitabilityReports, LogType.Update, entity.Id);


            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(operationUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Select("*").Where(new { Id = id }, false, false, "");
            var entity = queryFactory.Get<OperationUnsuitabilityReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.OperationUnsuitabilityReports).Update(new UpdateOperationUnsuitabilityReportsDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                UnsuitabilityItemsID = entity.UnsuitabilityItemsID,
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                Action_ = entity.Action_,
                UnsuitableAmount = entity.UnsuitableAmount,
                ProductID = entity.ProductID,
                IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                FicheNo = entity.FicheNo,
                Description_ = entity.Description_,
                Date_ = entity.Date_,
                EmployeeID = entity.EmployeeID,
                OperationID = entity.OperationID,
                ProductionOrderID = entity.ProductionOrderID,
                StationGroupID = entity.StationGroupID,
                StationID = entity.StationID,
                WorkOrderID = entity.WorkOrderID
            }).Where(new { Id = id }, false, false, "");

            var operationUnsuitabilityReport = queryFactory.Update<SelectOperationUnsuitabilityReportsDto>(query, "Id", true);

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(operationUnsuitabilityReport);


        }
    }
}
