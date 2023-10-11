using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services
{
    [ServiceRegistration(typeof(ISalesManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class SalesManagementParametersAppService : ApplicationService<SalesManagementParametersResource>, ISalesManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public SalesManagementParametersAppService(IStringLocalizer<SalesManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> CreateAsync(CreateSalesManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Insert(new CreateSalesManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid(),
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                PropositionFutureDateParameter = input.PropositionFutureDateParameter
            }).UseIsDelete(false); ;


            var SalesManagementParameter = queryFactory.Insert<SelectSalesManagementParametersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Insert, SalesManagementParameter.Id);

            return new SuccessDataResult<SelectSalesManagementParametersDto>(SalesManagementParameter);
        }

        public async Task<IDataResult<SelectSalesManagementParametersDto>> GetSalesManagementParametersAsync()
        {
            SelectSalesManagementParametersDto result = new SelectSalesManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").UseIsDelete(false);

            SelectSalesManagementParametersDto SalesManagementParameter = queryFactory.Get<SelectSalesManagementParametersDto>(controlQuery);

            if (SalesManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(
                 new
                 {
                     Id = SalesManagementParameter.Id
                 }, false, false, "").UseIsDelete(false);

                result = queryFactory.Get<SelectSalesManagementParametersDto>(query);
            }

            LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Get, result.Id);

            return new SuccessDataResult<SelectSalesManagementParametersDto>(result);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesManagementParametersDto>> UpdateAsync(UpdateSalesManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);

            var entity = queryFactory.Get<SalesManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Update(new UpdateSalesManagementParametersDto
            {
                OrderFutureDateParameter = input.OrderFutureDateParameter,
                PropositionFutureDateParameter = input.PropositionFutureDateParameter,
                Id = input.Id
            }).Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);


            var SalesManagementParameters = queryFactory.Update<SelectSalesManagementParametersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, SalesManagementParameters, LoginedUserService.UserId, Tables.SalesManagementParameters, LogType.Update, entity.Id);

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


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesManagementParametersDto>>> GetListAsync(ListSalesManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.SalesManagementParameters).Select("*").UseIsDelete(false);

            var SalesManagementParameters = queryFactory.GetList<ListSalesManagementParametersDto>(query).ToList();

            return new SuccessDataResult<IList<ListSalesManagementParametersDto>>(SalesManagementParameters);
        }

        #endregion
    }
}
