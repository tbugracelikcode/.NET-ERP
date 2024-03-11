using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.GeneralParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.GeneralParameter.Services
{
    [ServiceRegistration(typeof(IGeneralParametersAppService), DependencyInjectionType.Scoped)]
    public class GeneralParametersAppService : ApplicationService<GeneralParametersResource>, IGeneralParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public GeneralParametersAppService(IStringLocalizer<GeneralParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectGeneralParametersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.GeneralParameters).Select("*").Where(
             new
             {
                 Id = id
             }, false, false, "");

            var GeneralParameter = queryFactory.Get<SelectGeneralParametersDto>(query);

            LogsAppService.InsertLogToDatabase(GeneralParameter, GeneralParameter, LoginedUserService.UserId, Tables.GeneralParameters, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralParametersDto>(GeneralParameter);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGeneralParametersDto>>> GetListAsync(ListGeneralParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.GeneralParameters).Select("*").Where(null, false, false, "");

            var GeneralParameters = queryFactory.GetList<ListGeneralParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListGeneralParametersDto>>(GeneralParameters);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGeneralParametersDto>> UpdateAsync(UpdateGeneralParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.GeneralParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<GeneralParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.GeneralParameters).Update(new UpdateGeneralParametersDto
            {
                Id = input.Id
            }).Where(new { Id = input.Id }, false, false, "");

            var GeneralParameters = queryFactory.Update<SelectGeneralParametersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, GeneralParameters, LoginedUserService.UserId, Tables.GeneralParameters, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralParametersDto>(GeneralParameters);

        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectGeneralParametersDto>> CreateAsync(CreateGeneralParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectGeneralParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
