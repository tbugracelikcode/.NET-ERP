using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Services
{
    [ServiceRegistration(typeof(IProductionManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class ProductionManagementParametersAppService : ApplicationService<ProductionManagementParametersResource>, IProductionManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ProductionManagementParametersAppService(IStringLocalizer<ProductionManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectProductionManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var ProductionManagementParameter = queryFactory.Get<SelectProductionManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(ProductionManagementParameter, ProductionManagementParameter, LoginedUserService.UserId, Tables.ProductionManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectProductionManagementParametersDto>(ProductionManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionManagementParametersDto>>> GetListAsync(ListProductionManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").Where(null, false, false, "");

                var ProductionManagementParameters = queryFactory.GetList<ListProductionManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListProductionManagementParametersDto>>(ProductionManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionManagementParametersDto>> UpdateAsync(UpdateProductionManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductionManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ProductionManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.ProductionManagementParameters).Update(new UpdateProductionManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var ProductionManagementParameters = queryFactory.Update<SelectProductionManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, ProductionManagementParameters, LoginedUserService.UserId, Tables.ProductionManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectProductionManagementParametersDto>(ProductionManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectProductionManagementParametersDto>> CreateAsync(CreateProductionManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectProductionManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
