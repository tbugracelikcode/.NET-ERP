using Microsoft.Extensions.Localization;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Services;
using TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services;
using TsiErp.Business.Entities.LeanProduction.OEEDetail.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.StationGroup.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Localizations.Resources.PurchaseManagementParameter.Page;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public class AdminDashboardAppService : IAdminDashboardAppService
    {
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IGeneralOEEsAppService _GeneralOEEsAppService;
        private readonly IOEEDetailsAppService _OEEDetailsAppService;
        private readonly IOperationUnsuitabilityReportsAppService _OperationUnsuitabilityReportsAppService;
        private readonly IStationsAppService _StationsAppService;
        private readonly IEmployeesAppService _EmployeesAppService;

        public AdminDashboardAppService(IGetSQLDateAppService getSQLDateAppService, IGeneralOEEsAppService generalOEEsAppService, IOEEDetailsAppService oEEDetailsAppService, IOperationUnsuitabilityReportsAppService operationUnsuitabilityReportsAppService, IStationsAppService stationsAppService, IEmployeesAppService employeesAppService)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _GeneralOEEsAppService = generalOEEsAppService;
            _OEEDetailsAppService = oEEDetailsAppService;
            _OperationUnsuitabilityReportsAppService = operationUnsuitabilityReportsAppService;
            _StationsAppService = stationsAppService;
            _EmployeesAppService = employeesAppService;
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

        public async Task<List<AdminMachineOEEGrid>> GetAdminMachineGrid(DateTime startDate, DateTime endDate)
        {
            List<AdminMachineOEEGrid> adminMachineGrid = new List<AdminMachineOEEGrid>();

            #region Değişkenler

            Guid stationID = Guid.Empty;
            string stationCode = string.Empty;
            Guid stationGroupID = Guid.Empty;
            string stationGroup = string.Empty;
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

            var groupedMachineOEEList = machineOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_, Station = t.StationID });

            int count = 0;

            foreach (var group in groupedMachineOEEList)
            {
                count++;

                availability = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.NetWorkingTime) / group.Sum(t => t.OccuredTime);
                perf = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.PlannedTime) / group.Sum(t => t.OccuredTime);

                decimal numberofScrap = (await _OperationUnsuitabilityReportsAppService.GetListbyStartEndDateScrapAsync(group.Select(t => t.Date_).FirstOrDefault(), group.Select(t => t.Date_).LastOrDefault())).Data.Where(t => t.isStationProductivityAnalysis).Sum(t => t.UnsuitableAmount);

                quality = group.Sum(t => t.ProducedQuantity) == 0 ? 0 : (group.Sum(t => t.ProducedQuantity) - numberofScrap) / group.Sum(t => t.ProducedQuantity);
                oee = availability * perf * quality;

                SelectStationsDto stationDataSource = (await _StationsAppService.GetAsync(group.Key.Station)).Data;

                if (stationDataSource != null && stationDataSource.Id != Guid.Empty)
                {
                    stationCode = stationDataSource.Code;
                    stationGroupID = stationDataSource.GroupID;
                    stationGroup = stationDataSource.StationGroup;
                }

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

                AdminMachineOEEGrid gridModel = new AdminMachineOEEGrid
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
                    STATIONID = group.Key.Station,
                    STATIONCODE = stationCode,
                    STATIONGROUP = stationGroup,
                    STATIONGROUPID = stationGroupID,

                };

                previousMonthAvailability = availability;
                previousMonthPerformance = perf;
                previousMonthQuality = quality;
                previousMonthOEE = oee;

                adminMachineGrid.Add(gridModel);
            }



            return await Task.FromResult(adminMachineGrid);
        }


        #endregion

        #endregion

        #region Employee OEE

        #region Chart

        public async Task<List<AdminEmployeeOEEChart>> GetAdminEmployeeChart(DateTime startDate, DateTime endDate)
        {
            List<AdminEmployeeOEEChart> adminEmployeeChart = new List<AdminEmployeeOEEChart>();

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


            List<ListOEEDetailsDto> employeeOEEList = (await _OEEDetailsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            employeeOEEList = employeeOEEList.OrderBy(t => t.Date_).ToList();

            var groupedEmployeeOEEList = employeeOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_ });

            int count = 0;

            foreach (var group in groupedEmployeeOEEList)
            {
                count++;

                availability = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.NetWorkingTime) / group.Sum(t => t.OccuredTime);
                perf = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.PlannedTime) / group.Sum(t => t.OccuredTime);

                decimal numberofScrap = (await _OperationUnsuitabilityReportsAppService.GetListbyStartEndDateScrapAsync(group.Select(t => t.Date_).FirstOrDefault(), group.Select(t => t.Date_).LastOrDefault())).Data.Where(t => t.isEmployeeProductivityAnalysis).Sum(t => t.UnsuitableAmount);

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

                AdminEmployeeOEEChart chartModel = new AdminEmployeeOEEChart
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

                adminEmployeeChart.Add(chartModel);
            }

            return await Task.FromResult(adminEmployeeChart);
        }

        #endregion

        #region Grid

        public async Task<List<AdminEmployeeOEEGrid>> GetAdminEmployeeGrid(DateTime startDate, DateTime endDate)
        {
            List<AdminEmployeeOEEGrid> adminEmployeeGrid = new List<AdminEmployeeOEEGrid>();

            #region Değişkenler

            Guid employeeID = Guid.Empty;
            string employee = string.Empty;
            Guid departmentID = Guid.Empty;
            string department = string.Empty;
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


            List<ListOEEDetailsDto> employeeOEEList = (await _OEEDetailsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            employeeOEEList = employeeOEEList.OrderBy(t => t.Date_).ToList();

            var groupedEmployeeOEEList = employeeOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_, Employee = t.EmployeeID });

            int count = 0;

            foreach (var group in groupedEmployeeOEEList)
            {
                count++;

                availability = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.NetWorkingTime) / group.Sum(t => t.OccuredTime);
                perf = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.PlannedTime) / group.Sum(t => t.OccuredTime);

                decimal numberofScrap = (await _OperationUnsuitabilityReportsAppService.GetListbyStartEndDateScrapAsync(group.Select(t => t.Date_).FirstOrDefault(), group.Select(t => t.Date_).LastOrDefault())).Data.Where(t => t.isEmployeeProductivityAnalysis).Sum(t => t.UnsuitableAmount);

                quality = group.Sum(t => t.ProducedQuantity) == 0 ? 0 : (group.Sum(t => t.ProducedQuantity) - numberofScrap) / group.Sum(t => t.ProducedQuantity);
                oee = availability * perf * quality;

                SelectEmployeesDto employeeDataSource = (await _EmployeesAppService.GetAsync(group.Key.Employee)).Data;

                if (employeeDataSource != null && employeeDataSource.Id != Guid.Empty)
                {
                    employee = employeeDataSource.Name + " " + employeeDataSource.Surname;
                    departmentID = employeeDataSource.DepartmentID.GetValueOrDefault();
                    department = employeeDataSource.Department;
                }

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

                AdminEmployeeOEEGrid gridModel = new AdminEmployeeOEEGrid
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
                    EMPLOYEEID = group.Key.Employee,
                    EMPLOYEE = employee,
                    DEPARTMENT = department,
                    DEPARTMENID = departmentID,

                };

                previousMonthAvailability = availability;
                previousMonthPerformance = perf;
                previousMonthQuality = quality;
                previousMonthOEE = oee;

                adminEmployeeGrid.Add(gridModel);
            }

            return await Task.FromResult(adminEmployeeGrid);
        }

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
