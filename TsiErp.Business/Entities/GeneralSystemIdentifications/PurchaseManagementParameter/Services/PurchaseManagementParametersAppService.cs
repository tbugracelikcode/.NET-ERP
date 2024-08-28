using Microsoft.Extensions.Localization;
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
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Services
{
    [ServiceRegistration(typeof(IPurchaseManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseManagementParametersAppService : ApplicationService<PurchaseManagementParametersResource>, IPurchaseManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PurchaseManagementParametersAppService(IStringLocalizer<PurchaseManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> CreateAsync(CreatePurchaseManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Insert(new CreatePurchaseManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                RequestFutureDateParameter = input.RequestFutureDateParameter,
                BranchID = input.BranchID,
                PurchaseOrderExchangeRateType = input.PurchaseOrderExchangeRateType,
                PurchaseRequestExchangeRateType = input.PurchaseRequestExchangeRateType,
                WarehouseID = input.WarehouseID,
                DefaultBranchID = input.DefaultBranchID,
                DefaultWarehouseID = input.DefaultWarehouseID,
            }).UseIsDelete(false); ;


            var PurchaseManagementParameter = queryFactory.Insert<SelectPurchaseManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseManagementParameters, LogType.Insert, PurchaseManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseManagementParametersDto>(PurchaseManagementParameter);
        }

        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> GetPurchaseManagementParametersAsync()
        {
            SelectPurchaseManagementParametersDto result = new SelectPurchaseManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").UseIsDelete(false);

            SelectPurchaseManagementParametersDto PurchaseManagementParameter = queryFactory.Get<SelectPurchaseManagementParametersDto>(controlQuery);

            if (PurchaseManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select<PurchaseManagementParameters>(s => new { s.Id, s.RequestFutureDateParameter, s.OrderFutureDateParameter })
                        .Join<Branches>
                        (
                            b => new { DefaultBranchName = b.Name, DefaultBranchCode = b.Code, DefaultBranchID = b.Id },
                            nameof(PurchaseManagementParameters.DefaultBranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                        .Join<Branches>
                        (
                            b => new { BranchCode = b.Code, BranchID = b.Id },
                            nameof(PurchaseManagementParameters.BranchID),
                            nameof(Branches.Id),
                            "MRPBranch",
                            JoinType.Left
                        )
                        .Join<Warehouses>
                        (
                            b => new { DefaultWarehouseName = b.Name, DefaultWarehouseCode = b.Code, DefaultWarehouseID = b.Id },
                            nameof(PurchaseManagementParameters.DefaultWarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        )
                        .Join<Warehouses>
                        (
                            b => new { WarehouseCode = b.Code, WarehouseID = b.Id },
                            nameof(PurchaseManagementParameters.WarehouseID),
                            nameof(Warehouses.Id),
                            "MRPWarehouse",
                            JoinType.Left
                            ).Where(new { Id = PurchaseManagementParameter.Id }, Tables.PurchaseManagementParameters).UseIsDelete(false);

                result = queryFactory.Get<SelectPurchaseManagementParametersDto>(query);
            }

            //LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.PurchaseManagementParameters, LogType.Get, result.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseManagementParametersDto>(result);
        }

        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> UpdateAsync(UpdatePurchaseManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").Where(new { Id = input.Id }, "").UseIsDelete(false);

            var entity = queryFactory.Get<PurchaseManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Update(new UpdatePurchaseManagementParametersDto
            {
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                RequestFutureDateParameter = input.RequestFutureDateParameter,
                Id = input.Id,
                PurchaseRequestExchangeRateType = input.PurchaseRequestExchangeRateType,
                PurchaseOrderExchangeRateType = input.PurchaseOrderExchangeRateType,
                WarehouseID = input.WarehouseID,
                BranchID = input.BranchID,
                DefaultWarehouseID = input.DefaultWarehouseID,
                DefaultBranchID = input.DefaultBranchID,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);


            var PurchaseManagementParameters = queryFactory.Update<SelectPurchaseManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(entity, PurchaseManagementParameters, LoginedUserService.UserId, Tables.PurchaseManagementParameters, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseManagementParametersDto>(PurchaseManagementParameters);
        }

        #region Unused Implemented Methods

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchaseManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<IList<ListPurchaseManagementParametersDto>>> GetListAsync(ListPurchaseManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").UseIsDelete(false);

            var PurchaseManagementParameters = queryFactory.GetList<ListPurchaseManagementParametersDto>(query).ToList();

            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListPurchaseManagementParametersDto>>(PurchaseManagementParameters);
        }

        #endregion
    }
}
