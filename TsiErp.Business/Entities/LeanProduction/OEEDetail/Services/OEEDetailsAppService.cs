using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.LeanProduction.OEEDetail.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.OEEDetail.Services
{
    [ServiceRegistration(typeof(IOEEDetailsAppService), DependencyInjectionType.Scoped)]
    public class OEEDetailsAppService : ApplicationService<PurchaseManagementParametersResource>, IOEEDetailsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public OEEDetailsAppService(IStringLocalizer<PurchaseManagementParametersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectOEEDetailsDto>> CreateAsync(CreateOEEDetailsDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OEEDetails).Insert(new CreateOEEDetailsDto
            {
                Id = GuidGenerator.CreateGuid(),
                Date_ = input.Date_,
                EmployeeID = input.EmployeeID,
                Month_ = input.Month_,
                OccuredTime = input.OccuredTime,
                PlannedQuantity = input.PlannedQuantity,
                PlannedTime = input.PlannedTime,
                ProducedQuantity = input.ProducedQuantity,
                ScrapQuantity = input.ScrapQuantity,
                StationID = input.StationID,
                WorkOrderID = input.WorkOrderID,
                Year_ = input.Year_,
            });


            var OEEDetails = queryFactory.Insert<SelectOEEDetailsDto>(query, "Id", true);

            await Task.CompletedTask;

            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetails);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.OEEDetails).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var OEEDetails = queryFactory.Update<SelectOEEDetailsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OEEDetails, LogType.Delete, id);



            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetails);

        }

        public async Task<IDataResult<SelectOEEDetailsDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.OEEDetails).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var OEEDetail = queryFactory.Get<SelectOEEDetailsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetail);

        }

        public async Task<IDataResult<IList<ListOEEDetailsDto>>> GetListAsync(ListOEEDetailsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.OEEDetails).Select<OEEDetails>(null).Where(null, "");
            var OEEDetails = queryFactory.GetList<ListOEEDetailsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOEEDetailsDto>>(OEEDetails);

        }
        public async Task<IDataResult<SelectOEEDetailsDto>> UpdateAsync(UpdateOEEDetailsDto input)
        {
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OEEDetails).Update(new UpdateOEEDetailsDto
            {
                Id = input.Id,
                Year_ = input.Year_,
                WorkOrderID = input.WorkOrderID,
                StationID = input.StationID,
                ScrapQuantity = input.ScrapQuantity,
                ProducedQuantity = input.ProducedQuantity,
                PlannedTime = input.PlannedTime,
                PlannedQuantity = input.PlannedQuantity,
                OccuredTime = input.OccuredTime,
                Date_ = input.Date_,
                EmployeeID = input.EmployeeID,
                Month_ = input.Month_,
            }).Where(new { Id = input.Id }, "");

            var OEEDetails = queryFactory.Update<SelectOEEDetailsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetails);

        }

        public Task<IDataResult<SelectOEEDetailsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
