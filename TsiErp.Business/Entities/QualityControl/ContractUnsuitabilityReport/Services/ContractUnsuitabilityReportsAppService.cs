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
using TsiErp.Business.Entities.ContractUnsuitabilityReport.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractUnsuitabilityReports.Page;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;

namespace TsiErp.Business.Entities.ContractUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityReportsAppService : ApplicationService<ContractUnsuitabilityReportsResource>, IContractUnsuitabilityReportsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ContractUnsuitabilityReportsAppService(IStringLocalizer<ContractUnsuitabilityReportsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateContractUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> CreateAsync(CreateContractUnsuitabilityReportsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");

                var list = queryFactory.ControlList<ContractUnsuitabilityReports>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Insert(new CreateContractUnsuitabilityReportsDto
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
                    UnsuitableAmount = input.UnsuitableAmount,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    ContractTrackingFicheID = input.ContractTrackingFicheID,
                    ProductionOrderID = input.ProductionOrderID,
                    WorkOrderID = input.WorkOrderID
                });


                var ContractUnsuitabilityReport = queryFactory.Insert<SelectContractUnsuitabilityReportsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Delete, id);

                return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);
            }
        }

        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*")
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
                       d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name, CurrentAccountCardID = d.Id }, nameof(ContractUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                    )
                    .Where(new { Id = id }, false, false, Tables.ContractUnsuitabilityReports);

                var ContractUnsuitabilityReport = queryFactory.Get<SelectContractUnsuitabilityReportsDto>(query);

                LogsAppService.InsertLogToDatabase(ContractUnsuitabilityReport, ContractUnsuitabilityReport, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Get, id);

                return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractUnsuitabilityReportsDto>>> GetListAsync(ListContractUnsuitabilityReportsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*")
                   .Join<WorkOrders>
                    (
                       d => new { WorkOrderFicheNr = d.WorkOrderNo }, nameof(ContractUnsuitabilityReports.WorkOrderID), nameof(WorkOrders.Id), JoinType.Left
                    )
                    .Join<ProductionOrders>
                    (
                       d => new { ProductionOrderFicheNr = d.FicheNo }, nameof(ContractUnsuitabilityReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                    )
                    .Join<UnsuitabilityItems>
                    (
                       d => new { UnsuitabilityItemsName = d.Name }, nameof(ContractUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                    )
                    .Join<ContractTrackingFiches>
                    (
                       d => new { ContractTrackingFicheNr = d.FicheNr }, nameof(ContractUnsuitabilityReports.ContractTrackingFicheID), nameof(ContractTrackingFiches.Id), JoinType.Left
                    )
                     .Join<CurrentAccountCards>
                    (
                       d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(ContractUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                    )
                    .Where(null, false, false, Tables.ContractUnsuitabilityReports);

                var contractUnsuitabilityReports = queryFactory.GetList<ListContractUnsuitabilityReportsDto>(query).ToList();

                return new SuccessDataResult<IList<ListContractUnsuitabilityReportsDto>>(contractUnsuitabilityReports);

            }
        }

        [ValidationAspect(typeof(UpdateContractUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> UpdateAsync(UpdateContractUnsuitabilityReportsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ContractUnsuitabilityReports>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.GetList<ContractUnsuitabilityReports>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNo != input.FicheNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Update(new UpdateContractUnsuitabilityReportsDto
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
                    ContractTrackingFicheID = input.ContractTrackingFicheID,
                    CurrentAccountCardID = input.CurrentAccountCardID,
                    UnsuitableAmount = input.UnsuitableAmount,
                    ProductionOrderID = input.ProductionOrderID,
                    WorkOrderID = input.WorkOrderID
                }).Where(new { Id = input.Id }, false, false, "");

                var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, ContractUnsuitabilityReport, LoginedUserService.UserId, Tables.ContractUnsuitabilityReports, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);
            }
        }

        public async Task<IDataResult<SelectContractUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<ContractUnsuitabilityReports>(entityQuery);

                var query = queryFactory.Query().From(Tables.ContractUnsuitabilityReports).Update(new UpdateContractUnsuitabilityReportsDto
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
                    IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                    FicheNo = entity.FicheNo,
                    Description_ = entity.Description_,
                    Date_ = entity.Date_,
                    ProductionOrderID = entity.ProductionOrderID,
                    CurrentAccountCardID = entity.CurrentAccountCardID,
                    ContractTrackingFicheID = entity.ContractTrackingFicheID,
                    WorkOrderID = entity.WorkOrderID
                }).Where(new { Id = id }, false, false, "");

                var ContractUnsuitabilityReport = queryFactory.Update<SelectContractUnsuitabilityReportsDto>(query, "Id", true);

                return new SuccessDataResult<SelectContractUnsuitabilityReportsDto>(ContractUnsuitabilityReport);

            }
        }
    }
}
