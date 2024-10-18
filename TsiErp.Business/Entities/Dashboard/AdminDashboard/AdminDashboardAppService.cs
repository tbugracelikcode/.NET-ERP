using Microsoft.Extensions.Localization;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services;
using TsiErp.Business.Entities.LeanProduction.OEEDetail.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public class AdminDashboardAppService : IAdminDashboardAppService
    {
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IGeneralOEEsAppService _GeneralOEEsAppService;
        private readonly IOEEDetailsAppService _OEEDetailsAppService;
        private readonly IOperationUnsuitabilityReportsAppService _OperationUnsuitabilityReportsAppService;

        public AdminDashboardAppService(IGetSQLDateAppService getSQLDateAppService, IGeneralOEEsAppService generalOEEsAppService, IOEEDetailsAppService oEEDetailsAppService, IOperationUnsuitabilityReportsAppService operationUnsuitabilityReportsAppService)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _GeneralOEEsAppService = generalOEEsAppService;
            _OEEDetailsAppService = oEEDetailsAppService;
            _OperationUnsuitabilityReportsAppService = operationUnsuitabilityReportsAppService;
        }


        #region Overal OEE

        #region Chart - Grid

        public async Task<List<AdminOveralOEEChart>> GetAdminOveralChart(DateTime startDate, DateTime endDate)
        {
            List<AdminOveralOEEChart> adminMachineChart = new List<AdminOveralOEEChart>();

            #region Değişkenler

            decimal previousMonthAvailability = 0;
            decimal previousMonthPerformance = 0;
            decimal previousMonthQuality = 0;
            decimal previousMonthOEE = 0;
            decimal oee = 0;
            decimal availability = 0;
            decimal perf = 0;
            decimal quality = 0;
            decimal differenceOEE = 0;
            decimal differenceQuality = 0;
            decimal differencePerformance = 0;
            decimal differenceAvailability = 0;

            #endregion

            List<ListGeneralOEEsDto> generalOEEList = (await _GeneralOEEsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            generalOEEList = generalOEEList.OrderBy(t=>t.Date_).ToList();

            var groupedGeneralOEEList = generalOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_ });

            int count = 0;

            foreach (var group in groupedGeneralOEEList)
            {
                count++;

                availability = group.Average(t => t.Availability);
                perf = group.Average(t => t.Productivity);
                quality = group.Average(t => t.ScrapRate);
                oee = availability * perf * quality;

                if (count == 1)
                {
                    differenceOEE = 0;
                    differenceQuality = 0;
                    differencePerformance = 0;
                    differenceAvailability = 0;
                }
                else
                {
                    differenceOEE = oee - previousMonthOEE;
                    differenceQuality = quality - previousMonthQuality;
                    differencePerformance = perf - previousMonthPerformance;
                    differenceAvailability = availability - previousMonthAvailability;
                }

                AdminOveralOEEChart chartModel = new AdminOveralOEEChart
                {
                    AVAILABILITY = availability,
                    DIFFAVA = differenceAvailability,
                    DIFFOEE = differenceOEE ,
                    DIFFPER = differencePerformance,
                    DIFFQUA = differenceQuality ,
                    MONTH = GetMonth(group.Select(t => t.Month_).FirstOrDefault()),
                    OEE = oee,
                    PERFORMANCE = perf ,
                    QUALITY = quality ,
                    YEAR = group.Key.Year,

                };

                previousMonthAvailability = group.Average(t => t.Availability);
                previousMonthPerformance = group.Average(t => t.Productivity);
                previousMonthQuality = group.Average(t => t.ScrapRate);
                previousMonthOEE = previousMonthAvailability * previousMonthPerformance * previousMonthQuality;

                adminMachineChart.Add(chartModel);
            }



            return await Task.FromResult(adminMachineChart);
        }

        #endregion


        #endregion

        #region Machine OEE

        #region Chart

        public async Task<List<AdminMachineOEEChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate)
        {
            List<AdminMachineOEEChart> adminMachineChart = new List<AdminMachineOEEChart>();

            #region Değişkenler

            decimal previousMonthAvailability = 0;
            decimal previousMonthPerformance = 0;
            decimal previousMonthQuality = 0;
            decimal previousMonthOEE = 0;
            decimal oee = 0;
            decimal availability = 0;
            decimal perf = 0;
            decimal quality = 0;
            decimal differenceOEE = 0;
            decimal differenceQuality = 0;
            decimal differencePerformance = 0;
            decimal differenceAvailability = 0;

            #endregion


            List<ListOEEDetailsDto> machineOEEList = (await _OEEDetailsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            machineOEEList = machineOEEList.OrderBy(t => t.Date_).ToList();

            var groupedMachineOEEList = machineOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_});

            int count = 0;

            foreach (var group in groupedMachineOEEList)
            {
                count++;

                availability = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t=>t.NetWorkingTime) / group.Sum(t => t.OccuredTime);
                perf = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.PlannedTime) / group.Sum(t => t.OccuredTime);

                decimal numberofScrap = (await _OperationUnsuitabilityReportsAppService.GetListbyStartEndDateScrapAsync(group.Select(t => t.Date_).FirstOrDefault(), group.Select(t => t.Date_).LastOrDefault())).Data.Where(t => t.isStationProductivityAnalysis).Sum(t => t.UnsuitableAmount);

                quality = group.Sum(t => t.ProducedQuantity) == 0 ? 0 : (group.Sum(t => t.ProducedQuantity) - numberofScrap) / group.Sum(t => t.ProducedQuantity);
                oee = availability * perf * quality;

                if (count == 1)
                {
                    differenceOEE = 0;
                    differenceQuality = 0;
                    differencePerformance = 0;
                    differenceAvailability = 0;
                }
                else
                {
                    differenceOEE = oee - previousMonthOEE;
                    differenceQuality = quality - previousMonthQuality;
                    differencePerformance = perf - previousMonthPerformance;
                    differenceAvailability = availability - previousMonthAvailability;
                }

                AdminMachineOEEChart chartModel = new AdminMachineOEEChart
                {
                    AVAILABILITY = availability,
                    DIFFAVA = differenceAvailability,
                    DIFFOEE = differenceOEE,
                    DIFFPER = differencePerformance,
                    DIFFQUA = differenceQuality,
                    MONTH = GetMonth(group.Select(t => t.Month_).FirstOrDefault()),
                    OEE = oee,
                    PERFORMANCE = perf,
                    QUALITY = quality,
                    YEAR = group.Key.Year,

                };

                previousMonthAvailability = availability;
                previousMonthPerformance = perf;
                previousMonthQuality = quality;
                previousMonthOEE = oee;

                adminMachineChart.Add(chartModel);
            }



            return await Task.FromResult(adminMachineChart);
        }

        #endregion

        #region Grid



        #endregion

        #endregion

        private string GetMonth(int ay)
        {
            string aystr = string.Empty;
            switch (ay)
            {
                case 1: aystr = "1Month"; break;
                case 2: aystr = "2Month"; break;
                case 3: aystr = "3Month"; break;
                case 4: aystr = "4Month"; break;
                case 5: aystr = "5Month"; break;
                case 6: aystr = "6Month"; break;
                case 7: aystr = "7Month"; break;
                case 8: aystr = "8Month"; break;
                case 9: aystr = "9Month"; break;
                case 10: aystr = "10Month"; break;
                case 11: aystr = "11Month"; break;
                case 12: aystr = "12Month"; break;
                default: break;

            }
            return aystr;
        }
    }
}
