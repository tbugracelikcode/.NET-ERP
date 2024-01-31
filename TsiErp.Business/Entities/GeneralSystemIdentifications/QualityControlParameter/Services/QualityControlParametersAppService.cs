using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.QualityControlParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.QualityControlParameter.Services
{
    [ServiceRegistration(typeof(IQualityControlParametersAppService), DependencyInjectionType.Scoped)]
    public class QualityControlParametersAppService : ApplicationService<QualityControlParametersResource>, IQualityControlParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public QualityControlParametersAppService(IStringLocalizer<QualityControlParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectQualityControlParametersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.QualityControlParameters).Select("*").Where(
             new
             {
                 Id = id
             }, false, false, "");

            var QualityControlParameter = queryFactory.Get<SelectQualityControlParametersDto>(query);

            LogsAppService.InsertLogToDatabase(QualityControlParameter, QualityControlParameter, LoginedUserService.UserId, Tables.QualityControlParameters, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectQualityControlParametersDto>(QualityControlParameter);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListQualityControlParametersDto>>> GetListAsync(ListQualityControlParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.QualityControlParameters).Select("*").Where(null, false, false, "");

            var QualityControlParameters = queryFactory.GetList<ListQualityControlParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListQualityControlParametersDto>>(QualityControlParameters);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectQualityControlParametersDto>> UpdateAsync(UpdateQualityControlParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.QualityControlParameters).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<QualityControlParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.QualityControlParameters).Update(new UpdateQualityControlParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                Id = input.Id
            }).Where(new { Id = input.Id }, false, false, "");

            var QualityControlParameters = queryFactory.Update<SelectQualityControlParametersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, QualityControlParameters, LoginedUserService.UserId, Tables.QualityControlParameters, LogType.Update, entity.Id);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectQualityControlParametersDto>(QualityControlParameters);
        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectQualityControlParametersDto>> CreateAsync(CreateQualityControlParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectQualityControlParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
