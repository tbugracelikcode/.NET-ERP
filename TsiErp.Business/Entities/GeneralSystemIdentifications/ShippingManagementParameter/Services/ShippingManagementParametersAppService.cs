using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public ShippingManagementParametersAppService(IStringLocalizer<ShippingManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectShippingManagementParametersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(
             new
             {
                 Id = id
             }, "");

            var ShippingManagementParameter = queryFactory.Get<SelectShippingManagementParametersDto>(query);

            //LogsAppService.InsertLogToDatabase(ShippingManagementParameter, ShippingManagementParameter, LoginedUserService.UserId, Tables.ShippingManagementParameters, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingManagementParametersDto>(ShippingManagementParameter);

        }


        public async Task<IDataResult<IList<ListShippingManagementParametersDto>>> GetListAsync(ListShippingManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(null, "");

            var ShippingManagementParameters = queryFactory.GetList<ListShippingManagementParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShippingManagementParametersDto>>(ShippingManagementParameters);
        }

        public async Task<IDataResult<SelectShippingManagementParametersDto>> UpdateAsync(UpdateShippingManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShippingManagementParameters).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ShippingManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.ShippingManagementParameters).Update(new UpdateShippingManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                Id = input.Id
            }).Where(new { Id = input.Id }, "");

            var ShippingManagementParameters = queryFactory.Update<SelectShippingManagementParametersDto>(query, "Id", true);


            //LogsAppService.InsertLogToDatabase(entity, ShippingManagementParameters, LoginedUserService.UserId, Tables.ShippingManagementParameters, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectShippingManagementParametersDto>(ShippingManagementParameters);
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
