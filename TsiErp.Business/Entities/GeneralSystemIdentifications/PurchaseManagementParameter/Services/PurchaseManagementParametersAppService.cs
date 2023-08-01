using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Services
{
    [ServiceRegistration(typeof(IPurchaseManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseManagementParametersAppService : ApplicationService<PurchaseManagementParametersResource>, IPurchaseManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public PurchaseManagementParametersAppService(IStringLocalizer<PurchaseManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var PurchaseManagementParameter = queryFactory.Get<SelectPurchaseManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(PurchaseManagementParameter, PurchaseManagementParameter, LoginedUserService.UserId, Tables.PurchaseManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectPurchaseManagementParametersDto>(PurchaseManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseManagementParametersDto>>> GetListAsync(ListPurchaseManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").Where(null, false, false, "");

                var PurchaseManagementParameters = queryFactory.GetList<ListPurchaseManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListPurchaseManagementParametersDto>>(PurchaseManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseManagementParametersDto>> UpdateAsync(UpdatePurchaseManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PurchaseManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<PurchaseManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.PurchaseManagementParameters).Update(new UpdatePurchaseManagementParametersDto
                {
                    FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var PurchaseManagementParameters = queryFactory.Update<SelectPurchaseManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, PurchaseManagementParameters, LoginedUserService.UserId, Tables.PurchaseManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectPurchaseManagementParametersDto>(PurchaseManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectPurchaseManagementParametersDto>> CreateAsync(CreatePurchaseManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchaseManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
