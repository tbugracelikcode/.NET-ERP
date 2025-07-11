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
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.StockManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services
{
    [ServiceRegistration(typeof(IStockManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class StockManagementParametersAppService : ApplicationService<StockManagementParametersResource>, IStockManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public StockManagementParametersAppService(IStringLocalizer<StockManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> CreateAsync(CreateStockManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.StockManagementParameters).Insert(new CreateStockManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                FutureDateParameter = input.FutureDateParameter,
                AutoCostParameter = input.AutoCostParameter,
                 CostCalculationMethod = input.CostCalculationMethod,
                  DefaultBranchID = input.DefaultBranchID,
                   DefaultWarehouseID = input.DefaultWarehouseID,
            }).UseIsDelete(false); ;


            var stockManagementParameter = queryFactory.Insert<SelectStockManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Insert, stockManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockManagementParametersDto>(stockManagementParameter);
        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> GetStockManagementParametersAsync()
        {
            SelectStockManagementParametersDto result = new SelectStockManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").UseIsDelete(false);

            SelectStockManagementParametersDto StockManagementParameter = queryFactory.Get<SelectStockManagementParametersDto>(controlQuery);

            if (StockManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.StockManagementParameters).Select<StockManagementParameters>(s => new { s.Id, s.FutureDateParameter, s.CostCalculationMethod, s.AutoCostParameter})
                        .Join<Branches>
                        (
                            b => new { DefaultBranchName = b.Name, DefaultBranchCode = b.Code, DefaultBranchID = b.Id },
                            nameof(StockManagementParameters.DefaultBranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                        .Join<Warehouses>
                        (
                            b => new { DefaultWarehouseName = b.Name, DefaultWarehouseCode = b.Code, DefaultWarehouseID = b.Id },
                            nameof(StockManagementParameters.DefaultWarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        ).Where(new { Id = StockManagementParameter.Id }, Tables.StockManagementParameters).UseIsDelete(false);

                result = queryFactory.Get<SelectStockManagementParametersDto>(query);
            }

            //LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Get, result.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockManagementParametersDto>(result);
        }

        public async Task<IDataResult<SelectStockManagementParametersDto>> UpdateAsync(UpdateStockManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").Where(new { Id = input.Id }, "").UseIsDelete(false);

            var entity = queryFactory.Get<StockManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.StockManagementParameters).Update(new UpdateStockManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                AutoCostParameter = input.AutoCostParameter,
                CostCalculationMethod = input.CostCalculationMethod,
                Id = input.Id,
                 DefaultWarehouseID = input.DefaultWarehouseID,
                  DefaultBranchID   = input.DefaultBranchID,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);


            var StockManagementParameters = queryFactory.Update<SelectStockManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(entity, StockManagementParameters, LoginedUserService.UserId, Tables.StockManagementParameters, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectStockManagementParametersDto>(StockManagementParameters);
        }

        #region Unused Implemented Methods

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectStockManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<IList<ListStockManagementParametersDto>>> GetListAsync(ListStockManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.StockManagementParameters).Select("*").UseIsDelete(false);

            var StockManagementParameters = queryFactory.GetList<ListStockManagementParametersDto>(query).ToList();
            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListStockManagementParametersDto>>(StockManagementParameters);
        }

        #endregion
    }
}
