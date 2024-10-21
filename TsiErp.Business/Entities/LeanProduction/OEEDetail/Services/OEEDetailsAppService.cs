using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services;
using TsiErp.Business.Entities.LeanProduction.OEEDetail.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
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
        private readonly IGeneralOEEsAppService _GeneralOEEsAppService;

        public OEEDetailsAppService(IStringLocalizer<PurchaseManagementParametersResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IGeneralOEEsAppService generalOEEsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _GeneralOEEsAppService = generalOEEsAppService;
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
                NetWorkingTime = input.NetWorkingTime,
                ProducedQuantity = input.ProducedQuantity,
                ScrapQuantity = input.ScrapQuantity,
                StationID = input.StationID,
                WorkOrderID = input.WorkOrderID,
                Year_ = input.Year_,
            });


            var OEEDetails = queryFactory.Insert<SelectOEEDetailsDto>(query, "Id", true);

            #region General OEE Insert - Update

            List<ListOEEDetailsDto> detailList = (await GetListbyDateAsync(now.Date)).Data.ToList();

            if (detailList != null && detailList.Count > 0)
            {
                #region Hurda

                decimal scrap = detailList.Sum(t => t.ProducedQuantity) == 0 ? 0 : (detailList.Sum(t => t.ProducedQuantity) - detailList.Sum(t => t.ScrapQuantity)) / detailList.Sum(t => t.ProducedQuantity);

                #endregion

                #region Kullanılabilirlik

                decimal availability = detailList.Sum(t => t.OccuredTime) == 0 ? 0 : (detailList.Sum(t => t.NetWorkingTime) / detailList.Sum(t => t.OccuredTime));

                #endregion

                #region Verimlilik

                // A / B ------> B = (A*Harcanan Süre) / Planlanan Süre ====> Planlanan Süre / Harcanan Süre

                decimal productivity = detailList.Sum(t => t.OccuredTime) == 0 ? 0 : (detailList.Sum(t => t.PlannedTime) / detailList.Sum(t => t.OccuredTime));

                #endregion

                SelectGeneralOEEsDto generalOEE = (await _GeneralOEEsAppService.GetbyDateAsync(input.Date_)).Data;

                if (generalOEE != null && generalOEE.Id != Guid.Empty) // Update General OEE
                {
                    generalOEE.ScrapRate = scrap;
                    generalOEE.Availability = availability;
                    generalOEE.Productivity = productivity;

                    UpdateGeneralOEEsDto generalOEEUpdatedInput = ObjectMapper.Map<SelectGeneralOEEsDto, UpdateGeneralOEEsDto>(generalOEE);

                    await _GeneralOEEsAppService.UpdateAsync(generalOEEUpdatedInput);
                }
                else // Insert General OEE
                {
                    CreateGeneralOEEsDto createGeneralOEE = new CreateGeneralOEEsDto
                    {
                        Availability = availability,
                        Date_ = input.Date_,
                        Month_ = input.Month_,
                        Productivity = productivity,
                        ScrapRate = scrap,
                        Year_ = input.Year_,
                    };

                    await _GeneralOEEsAppService.CreateAsync(createGeneralOEE);
                }
            }

            #endregion

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
            }, "").UseIsDelete(false);
            var OEEDetail = queryFactory.Get<SelectOEEDetailsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetail);

        }

        public async Task<IDataResult<SelectOEEDetailsDto>> GetbyDateStationEmployeeWorkOrderAsync(DateTime date, Guid stationID, Guid employeeID, Guid workOrderID)
        {

            var query = queryFactory.Query().From(Tables.OEEDetails).Select("*").Where(
            new
            {
                Date_ = date,
                StationID = stationID,
                EmployeeID = employeeID,
                WorkOrderID = workOrderID
            }, "").UseIsDelete(false);
            var OEEDetail = queryFactory.Get<SelectOEEDetailsDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetail);

        }

        public async Task<IDataResult<IList<ListOEEDetailsDto>>> GetListAsync(ListOEEDetailsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.OEEDetails).Select<OEEDetails>(null).Where(null, "").UseIsDelete(false);
            var OEEDetails = queryFactory.GetList<ListOEEDetailsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOEEDetailsDto>>(OEEDetails);

        }

        public async Task<IDataResult<IList<ListOEEDetailsDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate)
        {
            string resultQuery = "SELECT * FROM " + Tables.OEEDetails;

            string where = string.Empty;

            where = " (Date_>='" + startDate + "' and '" + endDate + "'>=Date_) ";


            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;
            var stockFicheLine = queryFactory.GetList<ListOEEDetailsDto>(query).ToList();
            await Task.CompletedTask;

            return new SuccessDataResult<IList<ListOEEDetailsDto>>(stockFicheLine);

        }


        public async Task<IDataResult<IList<ListOEEDetailsDto>>> GetListbyDateAsync(DateTime date)
        {
            var query = queryFactory.Query().From(Tables.OEEDetails).Select<OEEDetails>(null).Where(new { Date_ = date }, "").UseIsDelete(false);
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
                NetWorkingTime = input.NetWorkingTime,
                PlannedQuantity = input.PlannedQuantity,
                OccuredTime = input.OccuredTime,
                Date_ = input.Date_,
                EmployeeID = input.EmployeeID,
                Month_ = input.Month_,
            }).Where(new { Id = input.Id }, "").UseIsDelete(false);

            var OEEDetails = queryFactory.Update<SelectOEEDetailsDto>(query, "Id", true);

            #region General OEE Insert - Update

            List<ListOEEDetailsDto> detailList = (await GetListbyDateAsync(now.Date)).Data.ToList();

            if (detailList != null && detailList.Count > 0)
            {
                #region Hurda

                decimal scrap = detailList.Sum(t => t.ProducedQuantity) == 0 ? 0 : (detailList.Sum(t => t.ProducedQuantity) - detailList.Sum(t => t.ScrapQuantity)) / detailList.Sum(t => t.ProducedQuantity);

                #endregion

                #region Kullanılabilirlik

                decimal availability = detailList.Sum(t => t.OccuredTime) == 0 ? 0 : (detailList.Sum(t => t.NetWorkingTime) / detailList.Sum(t => t.OccuredTime));

                #endregion

                #region Verimlilik

                // A / B ------> B = (A*Harcanan Süre) / Planlanan Süre ====> Planlanan Süre / Harcanan Süre

                decimal productivity = detailList.Sum(t => t.OccuredTime) == 0 ? 0 : (detailList.Sum(t => t.PlannedTime) / detailList.Sum(t => t.OccuredTime));

                #endregion

                SelectGeneralOEEsDto generalOEE = (await _GeneralOEEsAppService.GetbyDateAsync(input.Date_)).Data;

                if (generalOEE != null && generalOEE.Id != Guid.Empty) // Update General OEE
                {
                    generalOEE.ScrapRate = scrap;
                    generalOEE.Availability = availability;
                    generalOEE.Productivity = productivity;

                    UpdateGeneralOEEsDto generalOEEUpdatedInput = ObjectMapper.Map<SelectGeneralOEEsDto, UpdateGeneralOEEsDto>(generalOEE);

                    await _GeneralOEEsAppService.UpdateAsync(generalOEEUpdatedInput);
                }
                else // Insert General OEE
                {
                    CreateGeneralOEEsDto createGeneralOEE = new CreateGeneralOEEsDto
                    {
                        Availability = availability,
                        Date_ = input.Date_,
                        Month_ = input.Month_,
                        Productivity = productivity,
                        ScrapRate = scrap,
                        Year_ = input.Year_,
                    };

                    await _GeneralOEEsAppService.CreateAsync(createGeneralOEE);
                }
            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOEEDetailsDto>(OEEDetails);

        }

        public Task<IDataResult<SelectOEEDetailsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
