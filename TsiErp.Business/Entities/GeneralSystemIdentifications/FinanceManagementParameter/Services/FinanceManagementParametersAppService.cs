using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.FinanceManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Services
{
    [ServiceRegistration(typeof(IFinanceManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class FinanceManagementParametersAppService : ApplicationService<FinanceManagementParametersResource>, IFinanceManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public FinanceManagementParametersAppService(IStringLocalizer<FinanceManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectFinanceManagementParametersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").Where(
                 new
                 {
                     Id = id
                 }, false, false, "");

                var financeManagementParameter = queryFactory.Get<SelectFinanceManagementParametersDto>(query);

                LogsAppService.InsertLogToDatabase(financeManagementParameter, financeManagementParameter, LoginedUserService.UserId, Tables.FinanceManagementParameters, LogType.Get, id);

                return new SuccessDataResult<SelectFinanceManagementParametersDto>(financeManagementParameter);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFinanceManagementParametersDto>>> GetListAsync(ListFinanceManagementParametersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").Where(null, false, false, "");

                var financeManagementParameters = queryFactory.GetList<ListFinanceManagementParametersDto>(query).ToList();

                return new SuccessDataResult<IList<ListFinanceManagementParametersDto>>(financeManagementParameters);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinanceManagementParametersDto>> UpdateAsync(UpdateFinanceManagementParametersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<FinanceManagementParameters>(entityQuery);

                var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Update(new UpdateFinanceManagementParametersDto
                {
                     FutureDateParameter = input.FutureDateParameter,
                    Id = input.Id
                }).Where(new { Id = input.Id }, false, false, "");

                var financeManagementParameters = queryFactory.Update<SelectFinanceManagementParametersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, financeManagementParameters, LoginedUserService.UserId, Tables.FinanceManagementParameters, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectFinanceManagementParametersDto>(financeManagementParameters);
            }
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectFinanceManagementParametersDto>> CreateAsync(CreateFinanceManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinanceManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
