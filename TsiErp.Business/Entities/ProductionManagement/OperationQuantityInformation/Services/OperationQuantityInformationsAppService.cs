using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Currency.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Currencies.Page;
using TsiErp.Localizations.Resources.OperationQuantityInformations.Page;
using TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;

namespace TsiErp.Business.Entities.ProductionManagement.OperationQuantityInformation.Services
{
    [ServiceRegistration(typeof(IOperationQuantityInformationsAppService), DependencyInjectionType.Scoped)]
    public class OperationQuantityInformationsAppService : ApplicationService<OperationQuantityInformationsResource>, IOperationQuantityInformationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public OperationQuantityInformationsAppService(IStringLocalizer<OperationQuantityInformationsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateCurrenciesValidator), Priority = 1)]
        public async Task<IDataResult<SelectOperationQuantityInformationsDto>> CreateAsync(CreateOperationQuantityInformationsDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OperationQuantityInformations).Insert(new CreateOperationQuantityInformationsDto
            {
                Id = addedEntityId,
                AttachmentTime = input.AttachmentTime,
                OperationQuantityInformationsType = input.OperationQuantityInformationsType,
                OperationTime = input.OperationTime,
                OperatorID = input.OperatorID,
                ProductionOrderID = input.ProductionOrderID,
                ProductionTrackingID = input.ProductionTrackingID,
                ProductsOperationID = input.ProductsOperationID,
                StationID = input.StationID,
                WorkOrderID = input.WorkOrderID,
                Date_ = input.Date_,
                Hour_ = input.Hour_,
            });


            var operationQuantityInformation = queryFactory.Insert<SelectOperationQuantityInformationsDto>(query, "Id", true);


            await Task.CompletedTask;

            return new SuccessDataResult<SelectOperationQuantityInformationsDto>(operationQuantityInformation);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.OperationQuantityInformations).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var operationQuantityInformations = queryFactory.Update<SelectOperationQuantityInformationsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationQuantityInformationsDto>(operationQuantityInformations);

        }


        public async Task<IDataResult<SelectOperationQuantityInformationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.OperationQuantityInformations)
                .Select<OperationQuantityInformations>(null)
                .Join<WorkOrders>
                        (
                            b => new { WorkOrderNo = b.WorkOrderNo, WorkOrderID = b.Id },
                            nameof(OperationQuantityInformations.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                         .Join<ProductionOrders>
                        (
                            b => new { ProductionOrderNo = b.FicheNo, ProductionOrderID = b.Id },
                            nameof(OperationQuantityInformations.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                         .Join<Employees>
                        (
                            b => new { OperatorName = b.Name + " " + b.Surname, OperatorID = b.Id },
                            nameof(OperationQuantityInformations.OperatorID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                        .Join<ProductionTrackings>
                        (
                            b => new { ProductionTrackingNo = b.Code, ProductionTrackingID = b.Id },
                            nameof(OperationQuantityInformations.ProductionTrackingID),
                            nameof(ProductionTrackings.Id),
                            JoinType.Left
                        )
                          .Join<Stations>
                        (
                            b => new { StationCode = b.Code, StationID = b.Id },
                            nameof(OperationQuantityInformations.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                          .Join<ProductsOperations>
                        (
                            b => new { ProductsOperationName = b.Name, ProductsOperationID = b.Id },
                            nameof(OperationQuantityInformations.ProductsOperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                .Where(new { Id = id }, Tables.OperationQuantityInformations);
            var operationQuantityInformation = queryFactory.Get<SelectOperationQuantityInformationsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationQuantityInformationsDto>(operationQuantityInformation);

        }

        public async Task<IDataResult<IList<ListOperationQuantityInformationsDto>>> GetListAsync(ListOperationQuantityInformationsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.OperationQuantityInformations)
                .Select<OperationQuantityInformations>(null)
                .Join<WorkOrders>
                        (
                            b => new { WorkOrderNo = b.WorkOrderNo, WorkOrderID = b.Id },
                            nameof(OperationQuantityInformations.WorkOrderID),
                            nameof(WorkOrders.Id),
                            JoinType.Left
                        )
                         .Join<ProductionOrders>
                        (
                            b => new { ProductionOrderNo = b.FicheNo, ProductionOrderID = b.Id },
                            nameof(OperationQuantityInformations.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )
                         .Join<Employees>
                        (
                            b => new { OperatorName = b.Name + " " + b.Surname, OperatorID = b.Id },
                            nameof(OperationQuantityInformations.OperatorID),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                        .Join<ProductionTrackings>
                        (
                            b => new { ProductionTrackingNo = b.Code, ProductionTrackingID = b.Id },
                            nameof(OperationQuantityInformations.ProductionTrackingID),
                            nameof(ProductionTrackings.Id),
                            JoinType.Left
                        )
                          .Join<Stations>
                        (
                            b => new { StationCode = b.Code, StationID = b.Id },
                            nameof(OperationQuantityInformations.StationID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                          .Join<ProductsOperations>
                        (
                            b => new { ProductsOperationName = b.Name, ProductsOperationID = b.Id },
                            nameof(OperationQuantityInformations.ProductsOperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                .Where(null, Tables.OperationQuantityInformations);


            var operationQuantityInformations = queryFactory.GetList<ListOperationQuantityInformationsDto>(query).ToList();


            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOperationQuantityInformationsDto>>(operationQuantityInformations);

        }
        #region Unused Methods

        public Task<IDataResult<SelectOperationQuantityInformationsDto>> UpdateAsync(UpdateOperationQuantityInformationsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationQuantityInformationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
