using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Services
{
    [ServiceRegistration(typeof(IProductionManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class ProductionManagementParametersAppService : ApplicationService<ProductionManagementParametersResource>, IProductionManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ProductionManagementParametersAppService(IStringLocalizer<ProductionManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectProductionManagementParametersDto>> CreateAsync(CreateProductionManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Insert(new CreateProductionManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                FutureDateParameter = input.FutureDateParameter,
                Density_ = input.Density_,
                DefaultBranchID = input.DefaultBranchID,
                DefaultWarehouseID = input.DefaultWarehouseID,
            }).UseIsDelete(false);


            var ProductionManagementParameter = queryFactory.Insert<SelectProductionManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionManagementParameters, LogType.Insert, ProductionManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionManagementParametersDto>(ProductionManagementParameter);
        }

      

        public async Task<IDataResult<IList<ListProductionManagementParametersDto>>> GetListAsync(ListProductionManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").UseIsDelete(false);

            var ProductionManagementParameters = queryFactory.GetList<ListProductionManagementParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionManagementParametersDto>>(ProductionManagementParameters);
        }

        public async Task<IDataResult<SelectProductionManagementParametersDto>> GetProductionManagementParametersAsync()
        {
            SelectProductionManagementParametersDto result = new SelectProductionManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").UseIsDelete(false);

            SelectProductionManagementParametersDto ProductionManagementParameter = queryFactory.Get<SelectProductionManagementParametersDto>(controlQuery);

            if (ProductionManagementParameter != null && ProductionManagementParameter.Id != Guid.Empty)
            {
                var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Select<ProductionManagementParameters>(s=> new {s.Id, s.Density_})
                        .Join<Branches>
                        (
                            b => new { DefaultBranchName = b.Name, DefaultBranchCode = b.Code, DefaultBranchID = b.Id },
                            nameof(ProductionManagementParameters.DefaultBranchID),
                            nameof(Branches.Id),
                            JoinType.Left
                        )
                        .Join<Warehouses>
                        (
                            b => new { DefaultWarehouseName = b.Name, DefaultWarehouseCode = b.Code, DefaultWarehouseID = b.Id },
                            nameof(ProductionManagementParameters.DefaultWarehouseID),
                            nameof(Warehouses.Id),
                            JoinType.Left
                        ).Where(new { Id = ProductionManagementParameter.Id }, Tables.ProductionManagementParameters).UseIsDelete(false);

                result = queryFactory.Get<SelectProductionManagementParametersDto>(query);
            }


            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionManagementParametersDto>(result);

        }

        public async Task<IDataResult<SelectProductionManagementParametersDto>> UpdateAsync(UpdateProductionManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").Where(new { Id = input.Id },  "").UseIsDelete(false);
            var entity = queryFactory.Get<ProductionManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Update(new UpdateProductionManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                Density_ = input.Density_,
                Id = input.Id,
                DefaultWarehouseID = input.DefaultWarehouseID,
                DefaultBranchID   = input.DefaultBranchID,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var ProductionManagementParameters = queryFactory.Update<SelectProductionManagementParametersDto>(query, "Id", true);


            //LogsAppService.InsertLogToDatabase(entity, ProductionManagementParameters, LoginedUserService.UserId, Tables.ProductionManagementParameters, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionManagementParametersDto>(ProductionManagementParameters);
        }

        #region Unused Implemented Methods

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectProductionManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectProductionManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        #endregion
    }
}
