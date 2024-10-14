using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralOEE.Services
{
    [ServiceRegistration(typeof(IGeneralOEEsAppService), DependencyInjectionType.Scoped)]
    public class GeneralOEEsAppService : ApplicationService<PurchaseManagementParametersResource>, IGeneralOEEsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public GeneralOEEsAppService(IStringLocalizer<PurchaseManagementParametersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectGeneralOEEsDto>> CreateAsync(CreateGeneralOEEsDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.GeneralOEEs).Insert(new CreateGeneralOEEsDto
            {
                Id = GuidGenerator.CreateGuid(),
                Date_ = input.Date_,
                Month_ = input.Month_,
                Availability = input.Availability,
                Productivity = input.Productivity,
                ScrapRate = input.ScrapRate,
                Year_ = input.Year_,
            });


            var GeneralOEEs = queryFactory.Insert<SelectGeneralOEEsDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectGeneralOEEsDto>(GeneralOEEs);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.GeneralOEEs).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var GeneralOEEs = queryFactory.Update<SelectGeneralOEEsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.GeneralOEEs, LogType.Delete, id);



            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralOEEsDto>(GeneralOEEs);

        }

        public async Task<IDataResult<SelectGeneralOEEsDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.GeneralOEEs).Select("*").Where(
            new
            {
                Id = id
            }, "").UseIsDelete(false);
            var GeneralOEE = queryFactory.Get<SelectGeneralOEEsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralOEEsDto>(GeneralOEE);

        }

        public async Task<IDataResult<SelectGeneralOEEsDto>> GetbyDateAsync(DateTime date)
        {

            var query = queryFactory.Query().From(Tables.GeneralOEEs).Select("*").Where(
            new
            {
                Date_ = date
            }, "").UseIsDelete(false);
            var GeneralOEE = queryFactory.Get<SelectGeneralOEEsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralOEEsDto>(GeneralOEE);

        }

        public async Task<IDataResult<IList<ListGeneralOEEsDto>>> GetListAsync(ListGeneralOEEsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.GeneralOEEs).Select<GeneralOEEs>(null).Where(null, "").UseIsDelete(false);
            var GeneralOEEs = queryFactory.GetList<ListGeneralOEEsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListGeneralOEEsDto>>(GeneralOEEs);

        }
        public async Task<IDataResult<SelectGeneralOEEsDto>> UpdateAsync(UpdateGeneralOEEsDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.GeneralOEEs).Update(new UpdateGeneralOEEsDto
            {
                Id = input.Id,
                Year_ = input.Year_,
                ScrapRate = input.ScrapRate,
                Productivity = input.Productivity,
                Availability = input.Availability,
                Date_ = input.Date_,
                Month_ = input.Month_,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var GeneralOEEs = queryFactory.Update<SelectGeneralOEEsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectGeneralOEEsDto>(GeneralOEEs);

        }

        public Task<IDataResult<SelectGeneralOEEsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
