using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ContractTrackingFiche.Services;
using TsiErp.Business.Entities.ContractUnsuitabilityReport.Services;
using TsiErp.Business.Entities.CurrentAccountCard.Services;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services;
using TsiErp.Business.Entities.LeanProduction.OEEDetail.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Business.Entities.PurchaseOrder.Services;
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Business.Entities.StockFiche.Services;
using TsiErp.Business.Models.AdminDashboard;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Localizations.Resources.Dashboards.Page;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using Microsoft.SqlServer.Management.Smo;
using System.Globalization;
using Humanizer;
using System;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Business.Entities.HaltReason.Services;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;

namespace TsiErp.Business.Entities.Dashboard.AdminDashboard
{
    public class AdminDashboardAppService : ApplicationService<DashboardsResource>, IAdminDashboardAppService
    {
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IGeneralOEEsAppService _GeneralOEEsAppService;
        private readonly IOEEDetailsAppService _OEEDetailsAppService;
        private readonly IOperationUnsuitabilityReportsAppService _OperationUnsuitabilityReportsAppService;
        private readonly IStationsAppService _StationsAppService;
        private readonly IEmployeesAppService _EmployeesAppService;
        private readonly IContractTrackingFichesAppService _ContractTrackingFichesAppService;
        private readonly IContractUnsuitabilityReportsAppService _ContractUnsuitabilityReportsAppService;
        private readonly ICurrentAccountCardsAppService _CurrentAccountCardsAppService;
        private readonly IStockFichesAppService _StockFichesAppService;
        private readonly IPurchaseUnsuitabilityReportsAppService _PurchaseUnsuitabilityReportsAppService;
        private readonly IPurchaseOrdersAppService _PurchaseOrdersAppService;
        private readonly IProductionTrackingsAppService _ProductionTrackingsAppService;
        private readonly IProductGroupsAppService _ProductGroupsAppService;
        private readonly IProductsAppService _ProductsAppService;
        private readonly IProductionOrdersAppService _ProductionOrdersAppService;
        private readonly IUnsuitabilityItemsAppService _UnsuitabilityItemsAppService;
        private readonly IHaltReasonsAppService _HaltReasonsAppService;
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        public AdminDashboardAppService(IStringLocalizer<DashboardsResource> L, IGetSQLDateAppService getSQLDateAppService, IGeneralOEEsAppService generalOEEsAppService, IOEEDetailsAppService oEEDetailsAppService, IOperationUnsuitabilityReportsAppService operationUnsuitabilityReportsAppService, IStationsAppService stationsAppService, IEmployeesAppService employeesAppService, IContractTrackingFichesAppService contractTrackingFichesAppService, IContractUnsuitabilityReportsAppService contractUnsuitabilityReportsAppService, ICurrentAccountCardsAppService currentAccountCardsAppService, IStockFichesAppService stockFichesAppService, IPurchaseUnsuitabilityReportsAppService purchaseUnsuitabilityReportsAppService, IPurchaseOrdersAppService purchaseOrdersAppService, IProductionTrackingsAppService productionTrackingsAppService, IProductGroupsAppService productGroupsAppService, IProductsAppService productsAppService, IProductionOrdersAppService productionOrdersAppService, IUnsuitabilityItemsAppService unsuitabilityItemsAppService, IHaltReasonsAppService haltReasonsAppService) : base(L)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _GeneralOEEsAppService = generalOEEsAppService;
            _OEEDetailsAppService = oEEDetailsAppService;
            _OperationUnsuitabilityReportsAppService = operationUnsuitabilityReportsAppService;
            _StationsAppService = stationsAppService;
            _EmployeesAppService = employeesAppService;
            _ContractTrackingFichesAppService = contractTrackingFichesAppService;
            _ContractUnsuitabilityReportsAppService = contractUnsuitabilityReportsAppService;
            _CurrentAccountCardsAppService = currentAccountCardsAppService;
            _StockFichesAppService = stockFichesAppService;
            _PurchaseUnsuitabilityReportsAppService = purchaseUnsuitabilityReportsAppService;
            _PurchaseOrdersAppService = purchaseOrdersAppService;
            _ProductionTrackingsAppService = productionTrackingsAppService;
            _ProductGroupsAppService = productGroupsAppService;
            _ProductsAppService = productsAppService;
            _ProductionOrdersAppService = productionOrdersAppService;
            _UnsuitabilityItemsAppService = unsuitabilityItemsAppService;
            _HaltReasonsAppService = haltReasonsAppService;
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

            generalOEEList = generalOEEList.OrderBy(t => t.Date_).ToList();

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
                    DIFFOEE = differenceOEE,
                    DIFFPER = differencePerformance,
                    DIFFQUA = differenceQuality,
                    MONTH = GetMonth(group.Select(t => t.Month_).FirstOrDefault()),
                    OEE = oee,
                    PERFORMANCE = perf,
                    QUALITY = quality,
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

            var groupedMachineOEEList = machineOEEList.GroupBy(t => new { Month = t.Month_, Year = t.Year_ });

            int count = 0;

            foreach (var group in groupedMachineOEEList)
            {
                count++;

                availability = group.Sum(t => t.OccuredTime) == 0 ? 0 : group.Sum(t => t.NetWorkingTime) / group.Sum(t => t.OccuredTime);
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

        #region Contract Unsuitability Analysis

        #region Chart

        public async Task<List<AdminContractUnsuitabilityChart>> GetAdminContractUnsuitabilityChart(DateTime startDate, DateTime endDate)
        {
            List<AdminContractUnsuitabilityChart> adminContractUnsuitabilityChart = new List<AdminContractUnsuitabilityChart>();

            #region Değişkenler

            decimal totaloccuredquantity = 0;
            decimal totalscrap = 0;
            decimal totalreject = 0;
            decimal totalusedtobeas = 0;
            decimal totalcorrection = 0;
            decimal unspercent = 0;

            #endregion


            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingList = (await _ContractTrackingFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityList = (await _ContractUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            contractTrackingList = contractTrackingList.OrderBy(t => t.Date_).ToList();

            var groupedContractTrackingList = contractTrackingList.GroupBy(t => new { Month = t.Date_.Value.Month, Year = t.Date_.Value.Year });


            foreach (var groupTracking in groupedContractTrackingList)
            {

                var groupUnsuitability = contractUnsuitabilityList.Where(t => t.Date_.Value.Month == groupTracking.Key.Month && t.Date_.Value.Year == groupTracking.Key.Year).ToList();

                totaloccuredquantity = groupTracking.Sum(t => t.Amount_);
                totalscrap = groupUnsuitability.Where(T => T.Action_ == L["ComboboxScrap"]).Sum(t => t.UnsuitableAmount);
                totalreject = groupUnsuitability.Where(T => T.Action_ == L["ComboboxReject"]).Sum(t => t.UnsuitableAmount);
                totalusedtobeas = groupUnsuitability.Where(T => T.Action_ == L["ComboboxToBeUsedAs"]).Sum(t => t.UnsuitableAmount);
                totalcorrection = groupUnsuitability.Where(T => T.Action_ == L["ComboboxCorrection"]).Sum(t => t.UnsuitableAmount);

                unspercent = totaloccuredquantity == 0 ? 0 : (totalreject + totalscrap + totalusedtobeas + totalcorrection) / totaloccuredquantity;



                AdminContractUnsuitabilityChart chartModel = new AdminContractUnsuitabilityChart
                {
                    MONTH = GetMonth(groupTracking.Select(t => t.Date_.Value.Month).FirstOrDefault()),
                    YEAR = groupTracking.Key.Year,
                    TOTALCORRECTIONQUANTITY = totalcorrection,
                    TOTALOCCUREDQUANTITY = totaloccuredquantity,
                    TOTALREJECTQUANTITY = totalreject,
                    TOTALSCRAPQUANTITY = totalscrap,
                    TOTALTOBEUSEDASQUANTITY = totalusedtobeas,
                    TOTALUNSUITABILITYQUANTITY = totalcorrection + totalreject + totalusedtobeas + totalscrap,
                    UNSUITABILITYPERCENT = unspercent,
                };

                adminContractUnsuitabilityChart.Add(chartModel);
            }

            return await Task.FromResult(adminContractUnsuitabilityChart);
        }

        #endregion

        #region Grid


        public async Task<List<AdminContractUnsuitabilityGrid>> GetAdminContractUnsuitabilityGrid(DateTime startDate, DateTime endDate)
        {
            List<AdminContractUnsuitabilityGrid> adminContractUnsuitabilityGrid = new List<AdminContractUnsuitabilityGrid>();

            #region Değişkenler

            decimal totaloccuredquantity = 0;
            decimal totalscrap = 0;
            decimal totalreject = 0;
            decimal totalusedtobeas = 0;
            decimal totalcorrection = 0;
            string contractName = string.Empty;

            #endregion


            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingList = (await _ContractTrackingFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityList = (await _ContractUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            contractUnsuitabilityList = contractUnsuitabilityList.OrderBy(t => t.Date_).ToList();

            var groupedContractUnsuitabilityList = contractUnsuitabilityList.GroupBy(t => new { Month = t.Date_.Value.Month, Year = t.Date_.Value.Year, ContractID = t.CurrentAccountCardID });


            foreach (var groupUnsuitability in groupedContractUnsuitabilityList)
            {
                contractName = string.Empty;

                totaloccuredquantity = contractTrackingList.Where(t => t.Date_.Value.Month == groupUnsuitability.Key.Month && t.Date_.Value.Year == groupUnsuitability.Key.Year && t.CurrentAccountID == groupUnsuitability.Key.ContractID).Sum(t => t.Amount_);

                totalscrap = groupUnsuitability.Where(T => T.Action_ == L["ComboboxScrap"]).Sum(t => t.UnsuitableAmount);
                totalreject = groupUnsuitability.Where(T => T.Action_ == L["ComboboxReject"]).Sum(t => t.UnsuitableAmount);
                totalusedtobeas = groupUnsuitability.Where(T => T.Action_ == L["ComboboxToBeUsedAs"]).Sum(t => t.UnsuitableAmount);
                totalcorrection = groupUnsuitability.Where(T => T.Action_ == L["ComboboxCorrection"]).Sum(t => t.UnsuitableAmount);

                SelectCurrentAccountCardsDto contract = (await _CurrentAccountCardsAppService.GetAsync(groupUnsuitability.Key.ContractID.GetValueOrDefault())).Data;

                if (contract != null && contract.Id != Guid.Empty)
                {
                    contractName = contract.Name;
                }


                AdminContractUnsuitabilityGrid gridModel = new AdminContractUnsuitabilityGrid
                {
                    MONTH = GetMonth(groupUnsuitability.Select(t => t.Date_.Value.Month).FirstOrDefault()),
                    YEAR = groupUnsuitability.Key.Year,
                    TOTALCORRECTIONQUANTITY = totalcorrection,
                    TOTALOCCUREDQUANTITY = totaloccuredquantity,
                    TOTALREJECTQUANTITY = totalreject,
                    TOTALSCRAPQUANTITY = totalscrap,
                    TOTALTOBEUSEDASQUANTITY = totalusedtobeas,
                    TOTALUNSUITABILITYQUANTITY = totalcorrection + totalreject + totalusedtobeas + totalscrap,
                    CONTRACTNAME = contractName
                };

                adminContractUnsuitabilityGrid.Add(gridModel);
            }

            return await Task.FromResult(adminContractUnsuitabilityGrid);
        }

        #endregion

        #endregion

        #region Product Group Analysis

        #region Chart

        public async Task<List<AdminProductGroupAnalysisChart>> GetAdminProductGroupChart(DateTime startDate, DateTime endDate, Guid productGroupID)
        {
            List<AdminProductGroupAnalysisChart> adminProductGroupChart = new List<AdminProductGroupAnalysisChart>();

            List<Guid> ProductionOrderPlannedList = new List<Guid>();

            #region Değişkenler

            decimal previousMonthScrapPercent = 0;
            decimal scrappercent = 0;
            decimal differenceScrapPercent = 0;
            decimal totalscrap = 0;
            decimal numberofProduced = 0;
            decimal totalplannedquantity = 0;

            #endregion

            #region Üretim Takip GetList

            List<ListProductionTrackingsDto> prodTrackingList = (await _ProductionTrackingsAppService.GetListDashboardProductGroupAsync(startDate, endDate, productGroupID)).Data.ToList();

            var distinctedProdTrackingList = prodTrackingList.Select(t => t.ProductionOrderID).Distinct().ToList();

            foreach (Guid prodOrderId in distinctedProdTrackingList)
            {
                if (!ProductionOrderPlannedList.Contains(prodOrderId))
                {
                    SelectProductionOrdersDto prodOrder = (await _ProductionOrdersAppService.GetWithoutCloseConnectionAsync(prodOrderId)).Data;

                    if (prodOrder != null && prodOrder.Id != Guid.Empty)
                    {

                        ProductionOrderPlannedList.Add(prodOrder.Id);

                        totalplannedquantity += prodOrder.PlannedQuantity;

                    }
                }
            }

            //queryFactory.CloseConnection();

            #endregion

            #region Fason Takip GetList

            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingUsedList = new List<SelectContractTrackingFicheAmountEntryLinesDto>();

            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingList = (await _ContractTrackingFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            #region Product Group ID Filtresi

            var contractTrackingDistinctedList = contractTrackingList.Select(t => t.ProductionOrderID).Distinct().ToList();

            List<Guid> selectedProductionOrderIDList = new List<Guid>();

            foreach (var productionOrderID in contractTrackingDistinctedList)
            {
                SelectProductionOrdersDto productionOrder = (await _ProductionOrdersAppService.GetWithoutCloseConnectionAsync(productionOrderID.GetValueOrDefault())).Data;

                SelectProductsDto product = (await _ProductsAppService.GetWithoutCloseConnectionAsync(productionOrder.FinishedProductID.GetValueOrDefault())).Data;

                if (product != null && product.Id != Guid.Empty)
                {
                    productionOrder.ProductGroupID = product.ProductGrpID;
                }

                if (productionOrder != null && productionOrder.Id != Guid.Empty && productionOrder.ProductGroupID == productGroupID)
                {
                    selectedProductionOrderIDList.Add(productionOrder.Id);

                    if (!ProductionOrderPlannedList.Contains(productionOrder.Id))
                    {
                        ProductionOrderPlannedList.Add(productionOrder.Id);

                        totalplannedquantity += productionOrder.PlannedQuantity;
                    }
                }
            }

            //queryFactory.CloseConnection();

            foreach (Guid prodOrderId in selectedProductionOrderIDList)
            {
                contractTrackingUsedList.AddRange(contractTrackingList.Where(t => t.ProductionOrderID == prodOrderId).ToList());
            }

            #endregion

            #endregion

            #region Fason Uygunsuzluk GetList

            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityUsedList = new List<ListContractUnsuitabilityReportsDto>();

            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityList = (await _ContractUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            #region Product Group ID Filtresi

            var contractUnsuitabilityDistinctedList = contractUnsuitabilityList.Select(t => t.ProductionOrderID).Distinct().ToList();

            List<Guid> selectedProductionOrderIDUnsList = new List<Guid>();

            foreach (var productionOrderID in contractUnsuitabilityDistinctedList)
            {
                SelectProductionOrdersDto productionOrder = (await _ProductionOrdersAppService.GetWithoutCloseConnectionAsync(productionOrderID.GetValueOrDefault())).Data;

                SelectProductsDto product = (await _ProductsAppService.GetWithoutCloseConnectionAsync(productionOrder.FinishedProductID.GetValueOrDefault())).Data;

                if (product != null && product.Id != Guid.Empty)
                {
                    productionOrder.ProductGroupID = product.ProductGrpID;
                }

                if (productionOrder != null && productionOrder.Id != Guid.Empty && productionOrder.ProductGroupID == productGroupID)
                {
                    selectedProductionOrderIDUnsList.Add(productionOrder.Id);
                }
            }

            //queryFactory.CloseConnection();

            foreach (Guid prodOrderId in selectedProductionOrderIDUnsList)
            {
                contractUnsuitabilityUsedList.AddRange(contractUnsuitabilityList.Where(t => t.ProductionOrderID == prodOrderId).ToList());
            }
            #endregion


            contractUnsuitabilityUsedList = contractUnsuitabilityUsedList.OrderBy(t => t.Date_).ToList();

            var groupedcontractUnsuitabilityUsedList = contractUnsuitabilityUsedList.GroupBy(t => new { Month = t.Date_.Value.Month, Year = t.Date_.Value.Year });
            #endregion

            int count = 0;

            foreach (var groupUnsuitability in groupedcontractUnsuitabilityUsedList)
            {

                var groupContractTracking = contractTrackingUsedList.Where(t => t.Date_.Value.Month == groupUnsuitability.Key.Month && t.Date_.Value.Year == groupUnsuitability.Key.Year).ToList();

                var groupProductionTracking = prodTrackingList.Where(t => t.OperationStartDate.Value.Month == groupUnsuitability.Key.Month && t.OperationStartDate.Value.Year == groupUnsuitability.Key.Year).ToList();

                totalscrap = groupUnsuitability.Where(T => T.Action_ == L["ComboboxScrap"]).Sum(t => t.UnsuitableAmount) + groupProductionTracking.Sum(t => t.FaultyQuantity);

                numberofProduced = groupProductionTracking.Sum(t => t.ProducedQuantity) + groupContractTracking.Sum(t => t.Amount_);

                scrappercent = numberofProduced == 0 ? 0 : (totalscrap) / numberofProduced;

                if (count == 1)
                {
                    differenceScrapPercent = 0;
                }
                else
                {
                    differenceScrapPercent = scrappercent - previousMonthScrapPercent;
                }

                AdminProductGroupAnalysisChart chartModel = new AdminProductGroupAnalysisChart
                {

                    MONTH = GetMonth(groupUnsuitability.Select(t => t.Date_.Value.Month).FirstOrDefault()),
                    SCRAPPERCENT = scrappercent,
                    YEAR = groupUnsuitability.Key.Year,
                    DIFFSCRAPPERCENT = differenceScrapPercent,
                    PRODUCEDQUANTITY = (int)numberofProduced,
                    SCRAPQUANTITY = (int)totalscrap,
                    PLANNEDQUANTITY = (int)totalplannedquantity
                };

                previousMonthScrapPercent = scrappercent;

                adminProductGroupChart.Add(chartModel);
            }

            return await Task.FromResult(adminProductGroupChart);

        }

        #endregion

        #region Bar Chart

        public async Task<List<AdminProductGroupAnalysisBarChart>> GetAdminProductGroupBarChart(DateTime startDate, DateTime endDate, Guid productGroupID)
        {
            List<AdminProductGroupAnalysisBarChart> adminProductGroupBarChart = new List<AdminProductGroupAnalysisBarChart>();

            List<Guid> UnsuitabilityItemIDList = new List<Guid>();

            #region Değişkenler

            decimal totalscrap = 0;
            decimal numberofProduced = 0;
            decimal ppm = 0;

            #endregion

            #region Üretim Takip GetList

            List<ListProductionTrackingsDto> prodTrackingList = (await _ProductionTrackingsAppService.GetListDashboardProductGroupAsync(startDate, endDate, productGroupID)).Data.ToList();

            numberofProduced += prodTrackingList.Sum(t => t.ProducedQuantity);

            #endregion

            #region Fason Takip GetList

            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingUsedList = new List<SelectContractTrackingFicheAmountEntryLinesDto>();

            List<SelectContractTrackingFicheAmountEntryLinesDto> contractTrackingList = (await _ContractTrackingFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();

            #region Product Group ID Filtresi


            var contractTrackingDistinctedList = contractTrackingList.Select(t => t.ProductionOrderID).Distinct().ToList();

            List<Guid> selectedProductionOrderIDList = new List<Guid>();

            foreach (var productionOrderID in contractTrackingDistinctedList)
            {
                SelectProductionOrdersDto productionOrder = (await _ProductionOrdersAppService.GetWithoutCloseConnectionAsync(productionOrderID.GetValueOrDefault())).Data;

                SelectProductsDto product = (await _ProductsAppService.GetWithoutCloseConnectionAsync(productionOrder.FinishedProductID.GetValueOrDefault())).Data;

                if (product != null && product.Id != Guid.Empty)
                {
                    productionOrder.ProductGroupID = product.ProductGrpID;
                }
            }

            //queryFactory.CloseConnection();

            foreach (Guid prodOrderId in selectedProductionOrderIDList)
            {
                contractTrackingUsedList.AddRange(contractTrackingList.Where(t => t.ProductionOrderID == prodOrderId).ToList());
            }

            numberofProduced += contractTrackingUsedList.Sum(t => t.Amount_);

            #endregion

            #endregion

            #region Fason Uygunsuzluk GetList

            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityUsedList = new List<ListContractUnsuitabilityReportsDto>();

            List<ListContractUnsuitabilityReportsDto> contractUnsuitabilityList = (await _ContractUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            #region Product Group ID Filtresi
            var contractUnsuitabilityDistinctedList = contractUnsuitabilityList.Select(t => t.ProductionOrderID).Distinct().ToList();

            List<Guid> selectedProductionOrderIDUnsList = new List<Guid>();

            foreach (var productionOrderID in contractUnsuitabilityDistinctedList)
            {
                SelectProductionOrdersDto productionOrder = (await _ProductionOrdersAppService.GetWithoutCloseConnectionAsync(productionOrderID.GetValueOrDefault())).Data;

                SelectProductsDto product = (await _ProductsAppService.GetWithoutCloseConnectionAsync(productionOrder.FinishedProductID.GetValueOrDefault())).Data;

                if (product != null && product.Id != Guid.Empty)
                {
                    productionOrder.ProductGroupID = product.ProductGrpID;
                }

                if (productionOrder != null && productionOrder.Id != Guid.Empty && productionOrder.ProductGroupID == productGroupID)
                {
                    selectedProductionOrderIDUnsList.Add(productionOrder.Id);
                }
            }

            //queryFactory.CloseConnection();

            foreach (Guid prodOrderId in selectedProductionOrderIDUnsList)
            {
                contractUnsuitabilityUsedList.AddRange(contractUnsuitabilityList.Where(t => t.ProductionOrderID == prodOrderId).ToList());
            }

            var contractUnsuitabilityUsedDistinctedListbyUnsItem = contractUnsuitabilityUsedList.Select(t => t.UnsuitabilityItemsID).Distinct().ToList();

            foreach (Guid unsItemId in contractUnsuitabilityUsedDistinctedListbyUnsItem)
            {
                if (!UnsuitabilityItemIDList.Contains(unsItemId) && unsItemId != Guid.Empty)
                {
                    UnsuitabilityItemIDList.Add(unsItemId);
                }
            }

            #endregion


            #endregion

            #region Operasyon Uygunsuzluk GetList

            List<ListOperationUnsuitabilityReportsDto> operationUnsuitabilityUsedList = new List<ListOperationUnsuitabilityReportsDto>();

            List<ListOperationUnsuitabilityReportsDto> operationUnsuitabilityList = (await _OperationUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            #region Product Group ID Filtresi

            var operationUnsuitabilityDistinctedList = operationUnsuitabilityList.Select(t => t.ProductID).Distinct().ToList();

            List<Guid> selectedProductUnsList = new List<Guid>();

            foreach (var productID in operationUnsuitabilityDistinctedList)
            {

                SelectProductsDto product = (await _ProductsAppService.GetWithoutCloseConnectionAsync(productID.GetValueOrDefault())).Data;

                if (product != null && product.Id != Guid.Empty && product.ProductGrpID == productGroupID)
                {
                    selectedProductUnsList.Add(product.Id);
                }
            }

            //queryFactory.CloseConnection();

            foreach (Guid productID in selectedProductUnsList)
            {
                operationUnsuitabilityUsedList.AddRange(operationUnsuitabilityList.Where(t => t.ProductID == productID).ToList());
            }

            var operationUnsuitabilityUsedDistinctedListbyUnsItem = operationUnsuitabilityUsedList.Select(t => t.UnsuitabilityItemsID).Distinct().ToList();

            foreach (Guid unsItemId in operationUnsuitabilityUsedDistinctedListbyUnsItem)
            {
                if (!UnsuitabilityItemIDList.Contains(unsItemId) && unsItemId != Guid.Empty)
                {
                    UnsuitabilityItemIDList.Add(unsItemId);
                }
            }

            #endregion 


            #endregion


            foreach (Guid unsuitabilityItemID in UnsuitabilityItemIDList)
            {
                totalscrap = 0;

                string unsName = (await _UnsuitabilityItemsAppService.GetAsync(unsuitabilityItemID)).Data.Name;

                var operationScrapList = operationUnsuitabilityUsedList.Where(t => t.UnsuitabilityItemsID == unsuitabilityItemID).ToList();

                if (operationScrapList.Count() > 0)
                {
                    totalscrap += operationScrapList.Where(t => t.Action_ == L["ComboboxScrap"]).Sum(t => t.UnsuitableAmount);

                }

                var contractScrapList = contractUnsuitabilityUsedList.Where(t => t.UnsuitabilityItemsID == unsuitabilityItemID).ToList();

                if (contractScrapList.Count() > 0)
                {
                    totalscrap += contractScrapList.Where(t => t.Action_ == L["ComboboxScrap"]).Sum(t => t.UnsuitableAmount);

                }

                ppm = numberofProduced == 0 ? 0 : (totalscrap / numberofProduced) * 1000000;

                AdminProductGroupAnalysisBarChart barchartModel = new AdminProductGroupAnalysisBarChart
                {
                    UNSUITABILITYITEMID = unsuitabilityItemID,
                    UNSUITABILITYITEMNAME = unsName,
                    PPM = (int)ppm
                };


                adminProductGroupBarChart.Add(barchartModel);
            }

            return await Task.FromResult(adminProductGroupBarChart);

        }

        #endregion

        #endregion

        #region Purchase Unsuitability Analysis

        public class StockFicheLineQuantities
        {
            public DateTime Date_ { get; set; }
            public decimal Quantities { get; set; }
            public Guid CurrentAccountID { get; set; }
        }

        #region Chart

        public async Task<List<AdminPurchaseUnsuitabilityChart>> GetAdminPurchaseUnsuitabilityChart(DateTime startDate, DateTime endDate)
        {
            List<AdminPurchaseUnsuitabilityChart> adminPurchaseUnsuitabilityChart = new List<AdminPurchaseUnsuitabilityChart>();

            List<StockFicheLineQuantities> stockFicheLineQuantitiesList = new List<StockFicheLineQuantities>();

            #region Değişkenler

            decimal totaloccuredquantity = 0;
            decimal totalcontactsupplier = 0;
            decimal totalreject = 0;
            decimal totalusedtobeas = 0;
            decimal totalcorrection = 0;
            decimal unspercent = 0;

            #endregion


            List<SelectStockFichesDto> stockFicheList = (await _StockFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.Where(t => t.FicheType == TsiErp.Entities.Enums.StockFicheTypeEnum.StokGirisFisi && t.PurchaseOrderID != Guid.Empty).ToList();

            foreach (var stockFiche in stockFicheList)
            {
                decimal lineQuantity = 0;

                if (stockFiche.SelectStockFicheLines != null && stockFiche.SelectStockFicheLines.Count > 0)
                {
                    lineQuantity = stockFiche.SelectStockFicheLines.Sum(t => t.Quantity);
                }

                StockFicheLineQuantities stockFicheLineQuantity = new StockFicheLineQuantities
                {
                    Quantities = lineQuantity,
                    Date_ = stockFiche.Date_,
                };

                stockFicheLineQuantitiesList.Add(stockFicheLineQuantity);
            }


            List<ListPurchaseUnsuitabilityReportsDto> purchaseUnsuitabilityList = (await _PurchaseUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            stockFicheList = stockFicheList.OrderBy(t => t.Date_).ToList();

            var groupedStockFicheList = stockFicheList.GroupBy(t => new { Month = t.Date_.Month, Year = t.Date_.Year });


            foreach (var groupStockFiche in groupedStockFicheList)
            {

                var groupUnsuitability = purchaseUnsuitabilityList.Where(t => t.Date_.Value.Month == groupStockFiche.Key.Month && t.Date_.Value.Year == groupStockFiche.Key.Year).ToList();

                totaloccuredquantity = stockFicheLineQuantitiesList.Where(t => t.Date_.Month == groupStockFiche.Key.Month && t.Date_.Year == groupStockFiche.Key.Year).Sum(t => t.Quantities);

                totalcontactsupplier = groupUnsuitability.Where(T => T.Action_ == L["ComboboxContactSupplier"]).Sum(t => t.UnsuitableAmount);
                totalreject = groupUnsuitability.Where(T => T.Action_ == L["ComboboxReject"]).Sum(t => t.UnsuitableAmount);
                totalusedtobeas = groupUnsuitability.Where(T => T.Action_ == L["ComboboxToBeUsedAs"]).Sum(t => t.UnsuitableAmount);
                totalcorrection = groupUnsuitability.Where(T => T.Action_ == L["ComboboxCorrection"]).Sum(t => t.UnsuitableAmount);

                unspercent = totaloccuredquantity == 0 ? 0 : (totalreject + totalcontactsupplier + totalusedtobeas + totalcorrection) / totaloccuredquantity;



                AdminPurchaseUnsuitabilityChart chartModel = new AdminPurchaseUnsuitabilityChart
                {
                    MONTH = GetMonth(groupStockFiche.Select(t => t.Date_.Month).FirstOrDefault()),
                    YEAR = groupStockFiche.Key.Year,
                    TOTALCORRECTIONQUANTITY = totalcorrection,
                    TOTALOCCUREDQUANTITY = totaloccuredquantity,
                    TOTALREJECTQUANTITY = totalreject,
                    TOTALSUPPLIERCONTACTQUANTITY = totalcontactsupplier,
                    TOTALTOBEUSEDASQUANTITY = totalusedtobeas,
                    TOTALUNSUITABILITYQUANTITY = totalcorrection + totalreject + totalusedtobeas + totalcontactsupplier,
                    UNSUITABILITYPERCENT = unspercent,
                };

                adminPurchaseUnsuitabilityChart.Add(chartModel);

            }


            return await Task.FromResult(adminPurchaseUnsuitabilityChart);

        }

        #endregion

        #region Grid

        public async Task<List<AdminPurchaseUnsuitabilityGrid>> GetAdminPurchaseUnsuitabilityGrid(DateTime startDate, DateTime endDate)
        {
            List<AdminPurchaseUnsuitabilityGrid> adminPurchaseUnsuitabilityGrid = new List<AdminPurchaseUnsuitabilityGrid>();

            List<StockFicheLineQuantities> stockFicheLineQuantitiesList = new List<StockFicheLineQuantities>();

            #region Değişkenler

            decimal totaloccuredquantity = 0;
            decimal totalcontactsupplier = 0;
            decimal totalreject = 0;
            decimal totalusedtobeas = 0;
            decimal totalcorrection = 0;
            string supplierName = string.Empty;

            #endregion


            List<SelectStockFichesDto> stockFicheList = (await _StockFichesAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.Where(t => t.FicheType == TsiErp.Entities.Enums.StockFicheTypeEnum.StokGirisFisi && t.PurchaseOrderID != Guid.Empty).ToList();

            foreach (var stockFiche in stockFicheList)
            {
                decimal lineQuantity = 0;

                if (stockFiche.SelectStockFicheLines != null && stockFiche.SelectStockFicheLines.Count > 0)
                {
                    lineQuantity = stockFiche.SelectStockFicheLines.Sum(t => t.Quantity);
                }

                Guid CurrentAccountId = Guid.Empty;

                SelectPurchaseOrdersDto purchaseOrder = (await _PurchaseOrdersAppService.GetAsync(stockFiche.PurchaseOrderID.GetValueOrDefault())).Data;

                if (purchaseOrder != null && purchaseOrder.Id != Guid.Empty)
                {
                    CurrentAccountId = purchaseOrder.CurrentAccountCardID.GetValueOrDefault();
                }

                StockFicheLineQuantities stockFicheLineQuantity = new StockFicheLineQuantities
                {
                    Quantities = lineQuantity,
                    Date_ = stockFiche.Date_,
                    CurrentAccountID = CurrentAccountId
                };

                stockFicheLineQuantitiesList.Add(stockFicheLineQuantity);
            }


            List<ListPurchaseUnsuitabilityReportsDto> purchaseUnsuitabilityList = (await _PurchaseUnsuitabilityReportsAppService.GetListbyStartEndDateAsync(startDate, endDate)).Data.ToList();


            purchaseUnsuitabilityList = purchaseUnsuitabilityList.OrderBy(t => t.Date_).ToList();

            var groupedPurchaseUnsuitabilityList = purchaseUnsuitabilityList.GroupBy(t => new { Month = t.Date_.Value.Month, Year = t.Date_.Value.Year, SupplierID = t.CurrentAccountCardID });


            foreach (var groupUnsuitability in groupedPurchaseUnsuitabilityList)
            {
                supplierName = string.Empty;

                totaloccuredquantity = stockFicheLineQuantitiesList.Where(t => t.Date_.Month == groupUnsuitability.Key.Month && t.Date_.Year == groupUnsuitability.Key.Year && t.CurrentAccountID == groupUnsuitability.Key.SupplierID).Sum(t => t.Quantities);

                totalcontactsupplier = groupUnsuitability.Where(T => T.Action_ == L["ComboboxContactSupplier"]).Sum(t => t.UnsuitableAmount);
                totalreject = groupUnsuitability.Where(T => T.Action_ == L["ComboboxReject"]).Sum(t => t.UnsuitableAmount);
                totalusedtobeas = groupUnsuitability.Where(T => T.Action_ == L["ComboboxToBeUsedAs"]).Sum(t => t.UnsuitableAmount);
                totalcorrection = groupUnsuitability.Where(T => T.Action_ == L["ComboboxCorrection"]).Sum(t => t.UnsuitableAmount);

                SelectCurrentAccountCardsDto supplier = (await _CurrentAccountCardsAppService.GetAsync(groupUnsuitability.Key.SupplierID.GetValueOrDefault())).Data;

                if (supplier != null && supplier.Id != Guid.Empty)
                {
                    supplierName = supplier.Name;
                }


                AdminPurchaseUnsuitabilityGrid gridModel = new AdminPurchaseUnsuitabilityGrid
                {
                    MONTH = GetMonth(groupUnsuitability.Select(t => t.Date_.Value.Month).FirstOrDefault()),
                    YEAR = groupUnsuitability.Key.Year,
                    TOTALCORRECTIONQUANTITY = totalcorrection,
                    TOTALOCCUREDQUANTITY = totaloccuredquantity,
                    TOTALREJECTQUANTITY = totalreject,
                    TOTALSUPPLIERCONTACTQUANTITY = totalcontactsupplier,
                    TOTALTOBEUSEDASQUANTITY = totalusedtobeas,
                    TOTALUNSUITABILITYQUANTITY = totalcorrection + totalreject + totalusedtobeas + totalcontactsupplier,
                    SUPPLIERNAME = supplierName
                };

                adminPurchaseUnsuitabilityGrid.Add(gridModel);
            }

            return await Task.FromResult(adminPurchaseUnsuitabilityGrid);
        }

        #endregion

        #endregion

        #region Production Halt Analysis

        #region Chart

        public async Task<List<AdminProductionHaltAnalysisChart>> GetProductionHaltChart(DateTime startDate, DateTime endDate, int comboBox)
        {
            List<AdminProductionHaltAnalysisChart> adminHaltChart = new List<AdminProductionHaltAnalysisChart>();


            #region Değişkenler

            decimal previousMonthHalt = 0;
            decimal previousDayHalt = 0;
            decimal previousWeekHalt = 0;
            decimal haltTime = 0;
            decimal differenceHalt = 0;
            string week = string.Empty;
            string daily_ = string.Empty;

            #endregion


            List<ListProductionTrackingsDto> prodTrackingList = (await _ProductionTrackingsAppService.GetListDashboardHaltAnalysisAsync(startDate, endDate)).Data.Where(t => t.ProductionTrackingTypes == TsiErp.Entities.Enums.ProductionTrackingTypesEnum.Durus).ToList();

            prodTrackingList = prodTrackingList.OrderBy(t => t.OperationStartDate).ToList();

            int count = 0;

            if (comboBox == 3)
            {
                var groupedprodtrackingList = prodTrackingList.GroupBy(t => new { Month = t.OperationStartDate.Value.Month, Year = t.OperationStartDate.Value.Year });

                foreach (var group in groupedprodtrackingList)
                {
                    count++;
                    haltTime = group.Sum(t => t.HaltTime);


                    if (count == 1)
                    {
                        differenceHalt = 0;
                    }
                    else
                    {
                        differenceHalt = haltTime - previousMonthHalt;
                    }

                    AdminProductionHaltAnalysisChart chartModel = new AdminProductionHaltAnalysisChart
                    {
                        DIFFHALT = differenceHalt,
                        HALTTIME = haltTime,
                        TIME = GetMonth(group.Select(t => t.OperationStartDate.Value.Month).FirstOrDefault()),

                    };

                    previousMonthHalt = haltTime;

                    adminHaltChart.Add(chartModel);
                }



            }
            else if (comboBox == 2)
            {
                DateTime weeklyStartDate = DateTime.Today.AddDays(-7);
                DateTime weeklyEndDate = DateTime.Today;

                week = weeklyStartDate.ToString("dd MMMM yyyy") + " - " + weeklyEndDate.ToString("dd MMMM yyyy");
                haltTime = prodTrackingList.Sum(t => t.HaltTime);

                differenceHalt = haltTime - previousWeekHalt;

                AdminProductionHaltAnalysisChart chartModel = new AdminProductionHaltAnalysisChart
                {
                    DIFFHALT = differenceHalt,
                    HALTTIME = haltTime,
                    TIME = week,
                };

                previousWeekHalt = haltTime;

                adminHaltChart.Add(chartModel);
            }
            else
            {
                DateTime dailyDate = DateTime.Today;

                daily_ = dailyDate.ToString("dd MMM yyyy");
                haltTime = prodTrackingList.Sum(t => t.HaltTime);

                differenceHalt = haltTime - previousDayHalt;

                AdminProductionHaltAnalysisChart chartModel = new AdminProductionHaltAnalysisChart
                {
                    DIFFHALT = differenceHalt,
                    HALTTIME = haltTime,
                    TIME = daily_,
                };

                previousDayHalt = haltTime;

                adminHaltChart.Add(chartModel);
            }

            return await Task.FromResult(adminHaltChart);
        }



        #endregion

        #region Pie Chart
        public async Task<List<AdminProductionHaltAnalysisPieChart>> GetProductionHaltPieChart(DateTime startDate, DateTime endDate, int comboBox)
        {

            List<AdminProductionHaltAnalysisPieChart> adminHaltPieChart = new List<AdminProductionHaltAnalysisPieChart>();

            List<SelectHaltReasonsDto> OperatorHaltReasonsList = new List<SelectHaltReasonsDto>();

            List<SelectHaltReasonsDto> MachineHaltReasonsList = new List<SelectHaltReasonsDto>();

            List<SelectHaltReasonsDto> ManagementHaltReasonsList = new List<SelectHaltReasonsDto>();

            #region Değişkenler

            string week = string.Empty;
            string daily_ = string.Empty;
            decimal operatorHaltTime = 0;
            decimal machineHaltTime = 0;
            decimal managementHaltTime = 0;

            #endregion


            List<ListProductionTrackingsDto> prodTrackingUsedList = new List<ListProductionTrackingsDto>();

            List<ListProductionTrackingsDto> prodTrackingList = (await _ProductionTrackingsAppService.GetListDashboardHaltAnalysisAsync(startDate, endDate)).Data.Where(t => t.ProductionTrackingTypes == TsiErp.Entities.Enums.ProductionTrackingTypesEnum.Durus).ToList();

            prodTrackingList = prodTrackingList.OrderBy(t => t.OperationStartDate).ToList();

            var prodTrackingUsedDistinctedList = prodTrackingList.Select(t => t.HaltReasonID).Distinct().ToList();


            foreach (Guid haltReasonId in prodTrackingUsedDistinctedList)
            {
                if (haltReasonId != Guid.Empty)
                {
                    SelectHaltReasonsDto halt = (await _HaltReasonsAppService.GetAsync(haltReasonId)).Data;

                    if (halt.IsOperator)
                    {
                        OperatorHaltReasonsList.Add(halt);

                    }
                    else if (halt.IsMachine)
                    {
                        MachineHaltReasonsList.Add(halt);
                    }
                    else if (halt.IsManagement)
                    {
                        ManagementHaltReasonsList.Add(halt);
                    }
                }
            }

            foreach (var tracking in prodTrackingList)
            {
                if (OperatorHaltReasonsList.Where(t => t.Id == tracking.HaltReasonID.GetValueOrDefault()).Count() > 0)
                {
                    operatorHaltTime += tracking.HaltTime;
                }
                else if (MachineHaltReasonsList.Where(t => t.Id == tracking.HaltReasonID.GetValueOrDefault()).Count() > 0)
                {
                    machineHaltTime += tracking.HaltTime;
                }
                else if (ManagementHaltReasonsList.Where(t => t.Id == tracking.HaltReasonID.GetValueOrDefault()).Count() > 0)
                {
                    managementHaltTime += tracking.HaltTime;
                }
            }

            decimal totalHaltTime = operatorHaltTime + machineHaltTime + managementHaltTime;


            AdminProductionHaltAnalysisPieChart piechartModelOperator = new AdminProductionHaltAnalysisPieChart
            {
                HALTTIME = operatorHaltTime,
                HALTTYPE = L["OperatorHaltType"],
                HALTPERCENT = totalHaltTime == 0 ? 0 : (operatorHaltTime/ totalHaltTime) *100,
            };

            AdminProductionHaltAnalysisPieChart piechartModelMachine = new AdminProductionHaltAnalysisPieChart
            {
                HALTTIME = machineHaltTime,
                HALTTYPE = L["MachineHaltType"],
                HALTPERCENT = totalHaltTime == 0 ? 0 : (machineHaltTime / totalHaltTime) * 100,
            };

            AdminProductionHaltAnalysisPieChart piechartModelManagement = new AdminProductionHaltAnalysisPieChart
            {
                HALTTIME = managementHaltTime,
                HALTTYPE = L["ManagementHaltType"],
                HALTPERCENT = totalHaltTime == 0 ? 0 : (managementHaltTime / totalHaltTime) * 100,
            };


            adminHaltPieChart.Add(piechartModelOperator);
            adminHaltPieChart.Add(piechartModelMachine);
            adminHaltPieChart.Add(piechartModelManagement);


            return await Task.FromResult(adminHaltPieChart);

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

        //private string GetWeek(DayOfWeek ay)
        //{
        //    string aystr = string.Empty;
        //    switch (ay)
        //    {
        //        case DayOfWeek.Monday: aystr = "Monday"; break;
        //        case DayOfWeek.Tuesday: aystr = "Tuesday"; break;
        //        case DayOfWeek.Wednesday: aystr = "Wednesday"; break;
        //        case DayOfWeek.Thursday: aystr = "Thursday"; break;
        //        case DayOfWeek.Friday: aystr = "Friday"; break;
        //        case DayOfWeek.Saturday: aystr = "Saturday"; break;
        //        case DayOfWeek.Sunday: aystr = "Sunday"; break;
        //        default: break;

        //    }
        //    return aystr;
        //}

    }
}