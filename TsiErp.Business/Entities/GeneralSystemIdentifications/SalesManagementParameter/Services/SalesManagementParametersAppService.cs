﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services
{
    [ServiceRegistration(typeof(ISalesManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class SalesManagementParametersAppService : ApplicationService<SalesManagementParametersResource>, ISalesManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public SalesManagementParametersAppService(IStringLocalizer<SalesManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> CreateAsync(CreateSalesManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Insert(new CreateSalesManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                SalesOrderExchangeRateType = input.SalesOrderExchangeRateType,
                SalesPropositionExchangeRateType = input.SalesPropositionExchangeRateType,
                PropositionFutureDateParameter = input.PropositionFutureDateParameter,
                DefaultBranchID = input.DefaultBranchID,
                DefaultWarehouseID = input.DefaultWarehouseID,
                 SaleVAT = input.SaleVAT,
            }).UseIsDelete(false); ;


            var SalesManagementParameter = queryFactory.Insert<SelectSalesManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Insert, SalesManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesManagementParametersDto>(SalesManagementParameter);
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> GetSalesManagementParametersAsync()
        {
            SelectSalesManagementParametersDto result = new SelectSalesManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").UseIsDelete(false);

            SelectSalesManagementParametersDto SalesManagementParameter = queryFactory.Get<SelectSalesManagementParametersDto>(controlQuery);

            if (SalesManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select<SalesManagementParameters>(s => new { s.Id, s.PropositionFutureDateParameter, s.OrderFutureDateParameter, s.SalesOrderExchangeRateType, s.SalesPropositionExchangeRateType, s.SaleVAT })
                        .Join<Branches>
                        (
                            b => new { DefaultBranchName = b.Name, DefaultBranchCode = b.Code, DefaultBranchID = b.Id },
                            nameof(SalesManagementParameters.DefaultBranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                        .Join<Warehouses>
                        (
                            b => new { DefaultWarehouseName = b.Name, DefaultWarehouseCode = b.Code, DefaultWarehouseID = b.Id },
                            nameof(SalesManagementParameters.DefaultWarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        ).Where(new { Id = SalesManagementParameter.Id }, Tables.SalesManagementParameters).UseIsDelete(false);

                result = queryFactory.Get<SelectSalesManagementParametersDto>(query);
            }

            //LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Get, result.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesManagementParametersDto>(result);
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> UpdateAsync(UpdateSalesManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(new { Id = input.Id },  "").UseIsDelete(false);

            var entity = queryFactory.Get<SalesManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Update(new UpdateSalesManagementParametersDto
            {
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                SalesPropositionExchangeRateType = input.SalesPropositionExchangeRateType,
                SalesOrderExchangeRateType = input.SalesOrderExchangeRateType,
                PropositionFutureDateParameter = input.PropositionFutureDateParameter,
                Id = input.Id,
                DefaultWarehouseID = input.DefaultWarehouseID,
                DefaultBranchID = input.DefaultBranchID,
                 SaleVAT = input.SaleVAT,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);


            var SalesManagementParameters = queryFactory.Update<SelectSalesManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(entity, SalesManagementParameters, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectSalesManagementParametersDto>(SalesManagementParameters);
        }

        #region Unused Implemented Methods

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectSalesManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<IList<ListSalesManagementParametersDto>>> GetListAsync(ListSalesManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").UseIsDelete(false);

            var SalesManagementParameters = queryFactory.GetList<ListSalesManagementParametersDto>(query).ToList();

            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListSalesManagementParametersDto>>(SalesManagementParameters);
        }

        #endregion
    }
}
