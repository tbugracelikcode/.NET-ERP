﻿using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.ProductionManagement.OperationAdjustment.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment;
using TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.ProductionManagement.OperationAdjustment.Services
{
    [ServiceRegistration(typeof(IOperationAdjustmentsAppService), DependencyInjectionType.Scoped)]
    public class OperationAdjustmentsAppService : ApplicationService, IOperationAdjustmentsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public OperationAdjustmentsAppService(IGetSQLDateAppService getSQLDateAppService)
        {

            _GetSQLDateAppService = getSQLDateAppService;
        }



        [ValidationAspect(typeof(CreateOperationAdjustmentsValidator), Priority = 1)]
        public async Task<IDataResult<SelectOperationAdjustmentsDto>> CreateAsync(CreateOperationAdjustmentsDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OperationAdjustments).Insert(new CreateOperationAdjustmentsDto
            {
                Id = GuidGenerator.CreateGuid(),
                CreationTime =now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                AdjustmentStartDate = input.AdjustmentStartDate,
                AdjustmentUserId = input.AdjustmentUserId.GetValueOrDefault(),
                TotalAdjustmentTime = input.TotalAdjustmentTime,
                WorkOrderId = input.WorkOrderId.GetValueOrDefault(),
                ApprovedQuantity = input.ApprovedQuantity,
                OperatorId = input.OperatorId,
                ScrapQuantity = input.ScrapQuantity,
                TotalQualityControlApprovedTime = input.TotalQualityControlApprovedTime
            });

            var operationAdjustment = queryFactory.Insert<SelectOperationAdjustmentsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationAdjustments, LogType.Insert, operationAdjustment.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationAdjustmentsDto>(operationAdjustment);
        }

        public async Task<IDataResult<SelectOperationAdjustmentsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.OperationAdjustments)
                    .Select<OperationAdjustments>(null)
                        .Join<WorkOrders>
                        (
                           wo => new { WorkOrderId = wo.Id, WorkOrderNr = wo.WorkOrderNo },
                           nameof(OperationAdjustments.WorkOrderId),
                           nameof(WorkOrders.Id),
                           JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { AdjustmentUserId = e.Id, AdjustmentUserName = e.Name },
                            nameof(OperationAdjustments.AdjustmentUserId),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { OperatorId = e.Id, OperatorName = e.Name },
                            nameof(OperationAdjustments.AdjustmentUserId),
                            nameof(Employees.Id), 
                            "EmployeesOperator",
                            JoinType.Left
                        )
                        .Where(new { Id = id },  Tables.OperationAdjustments);

            var operationAdjustment = queryFactory.Get<SelectOperationAdjustmentsDto>(query);

            LogsAppService.InsertLogToDatabase(operationAdjustment, operationAdjustment, LoginedUserService.UserId, Tables.OperationAdjustments, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationAdjustmentsDto>(operationAdjustment);
        }

        public async Task<IDataResult<IList<ListOperationAdjustmentsDto>>> GetListAsync(ListOperationAdjustmentsParameterDto input)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.OperationAdjustments)
                    .Select<OperationAdjustments>(null)
                        .Join<WorkOrders>
                        (
                           wo => new { WorkOrderId = wo.Id, WorkOrderNr = wo.WorkOrderNo },
                           nameof(OperationAdjustments.WorkOrderId),
                           nameof(WorkOrders.Id),
                           JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { AdjustmentUserId = e.Id, AdjustmentUserName = e.Name },
                            nameof(OperationAdjustments.AdjustmentUserId),
                            nameof(Employees.Id),
                            JoinType.Left
                        )
                        .Join<Employees>
                        (
                            e => new { OperatorId = e.Id, OperatorName = e.Name },
                            nameof(OperationAdjustments.AdjustmentUserId),
                            nameof(Employees.Id),
                            "EmployeesOperator",
                            JoinType.Left
                        )
                        .Where(null, Tables.OperationAdjustments);

            var operationAdjustments = queryFactory.GetList<ListOperationAdjustmentsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOperationAdjustmentsDto>>(operationAdjustments);
        }


        public async Task<int> GetTotalAdjustmentTimeAsync(Guid workOrderId)
        {
            var query = queryFactory.Query().From(Tables.OperationAdjustments).Sum("TotalAdjustmentTime").Where("WorkOrderId",workOrderId,"");

            var operationAdjustment = queryFactory.Get<int>(query);

            await Task.CompletedTask;
            return operationAdjustment;
        }



        #region Unused Methods
        public Task<IDataResult<SelectOperationAdjustmentsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        [ValidationAspect(typeof(UpdateOperationAdjustmentsValidator), Priority = 1)]
        public Task<IDataResult<SelectOperationAdjustmentsDto>> UpdateAsync(UpdateOperationAdjustmentsDto input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
