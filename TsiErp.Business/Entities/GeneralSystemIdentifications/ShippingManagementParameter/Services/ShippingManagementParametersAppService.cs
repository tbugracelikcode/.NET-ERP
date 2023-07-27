using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ShippingManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Services
{
    [ServiceRegistration(typeof(IShippingManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class ShippingManagementParametersAppService : ApplicationService<ShippingManagementParametersResource>, IShippingManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ShippingManagementParametersAppService(IStringLocalizer<ShippingManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectShippingManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var ShippingManagementParameter = queryFactory.Get<SelectShippingManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(ShippingManagementParameter, ShippingManagementParameter, LoginedUserService.UserId, Tables.ShippingManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectShippingManagementParametersDto>(ShippingManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShippingManagementParametersDto>>> GetListAsync(ListShippingManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(null, false, false, "");

                var ShippingManagementParameters = queryFactory.GetList<ListShippingManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListShippingManagementParametersDto>>(ShippingManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingManagementParametersDto>> UpdateAsync(UpdateShippingManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ShippingManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Update(new UpdateShippingManagementParametersDto
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

                var ShippingManagementParameters = queryFactory.Update<SelectShippingManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, ShippingManagementParameters, LoginedUserService.UserId, Tables.ShippingManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectShippingManagementParametersDto>(ShippingManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectShippingManagementParametersDto>> CreateAsync(CreateShippingManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectShippingManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
