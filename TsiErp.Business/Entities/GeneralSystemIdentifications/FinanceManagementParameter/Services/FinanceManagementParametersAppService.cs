using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public FinanceManagementParametersAppService(IStringLocalizer<FinanceManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectFinanceManagementParametersDto>> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();

        }

        public async Task<IDataResult<SelectFinanceManagementParametersDto>> CreateAsync(CreateFinanceManagementParametersDto input)
        {
            var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Insert(new CreateFinanceManagementParametersDto
            {
                Id = GuidGenerator.CreateGuid()
            }).UseIsDelete(false); ;


            var FinanceManagementParameter = queryFactory.Insert<SelectFinanceManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.FinanceManagementParameters, LogType.Insert, FinanceManagementParameter.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectFinanceManagementParametersDto>(FinanceManagementParameter);

        }

        public async Task<IDataResult<SelectFinanceManagementParametersDto>> GetFinanceManagementParametersAsync()
        {
            SelectFinanceManagementParametersDto result = new SelectFinanceManagementParametersDto();

            var controlQuery = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").UseIsDelete(false);

            SelectFinanceManagementParametersDto FinanceManagementParameter = queryFactory.Get<SelectFinanceManagementParametersDto>(controlQuery);

            if (FinanceManagementParameter != null)
            {
                var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").Where(
                 new
                 {
                     Id = FinanceManagementParameter.Id
                 }, "").UseIsDelete(false);

                result = queryFactory.Get<SelectFinanceManagementParametersDto>(query);
            }

            //LogsAppService.InsertLogToDatabase(result, result, LoginedUserService.UserId, Tables.FinanceManagementParameters, LogType.Get, result.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectFinanceManagementParametersDto>(result);

        }


        public async Task<IDataResult<SelectFinanceManagementParametersDto>> UpdateAsync(UpdateFinanceManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").Where(new { Id = input.Id }, "").UseIsDelete(false);

            var entity = queryFactory.Get<FinanceManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Update(new UpdateFinanceManagementParametersDto
            {
                Id = input.Id
            }).Where(new { Id = input.Id },  "").UseIsDelete(false);


            var FinanceManagementParameters = queryFactory.Update<SelectFinanceManagementParametersDto>(query, "Id", true);

            //LogsAppService.InsertLogToDatabase(entity, FinanceManagementParameters, LoginedUserService.UserId, Tables.FinanceManagementParameters, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectFinanceManagementParametersDto>(FinanceManagementParameters);

        }

        public async Task<IDataResult<IList<ListFinanceManagementParametersDto>>> GetListAsync(ListFinanceManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.FinanceManagementParameters).Select("*").UseIsDelete(false);

            var FinanceManagementParameters = queryFactory.GetList<ListFinanceManagementParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListFinanceManagementParametersDto>>(FinanceManagementParameters);

        }

        #region Unused Implemented Methods

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
