using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class GenelOEEDetayService
    {

        SqlConnection _connection;
        public GenelOEEDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Duruş Analizi

        #region Grid
        public List<StationDetailedHaltAnalysis> GetStationDetailedHaltAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {
            List<StationDetailedHaltAnalysis> stationDetailedHaltAnalysis = new List<StationDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryStation(makineID, startDate, endDate);
            int totaltime = haltLines.Sum(t => t.DURUSSURE);

            foreach (var code in haltCodes)
            {
                #region Değişkenler

                int haltID = code.ID;
                string stationName = haltLines.Where(t => t.DURUSID == haltID).Select(t => t.MAKINEKODU).FirstOrDefault();
                int time = haltLines.Where(t => t.DURUSID == haltID).Sum(t => t.DURUSSURE);

                #endregion

                StationDetailedHaltAnalysis analysis = new StationDetailedHaltAnalysis
                {
                    Code = code.KOD,
                    HaltID = haltID,
                    Time = time,
                    StationName = stationName ,
                    Total = totaltime
                };

                stationDetailedHaltAnalysis.Add(analysis);
            }
            return stationDetailedHaltAnalysis;
        }

        #endregion

        #region Chart

        public List<StationDetailedHaltAnalysis> GetStationDetailedHaltAnalysisChart(int makineID, DateTime startDate, DateTime endDate)
        {
            List<StationDetailedHaltAnalysis> stationDetailedHaltAnalysisChart = new List<StationDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryStation(makineID, startDate, endDate);
            int totaltime = haltLines.Sum(t => t.DURUSSURE);

            foreach (var code in haltCodes)
            {
                #region Değişkenler

                int haltID = code.ID;
                int time = haltLines.Where(t => t.DURUSID == haltID).Sum(t => t.DURUSSURE);
                string stationName = haltLines.Where(t => t.DURUSID == haltID).Select(t => t.MAKINEKODU).FirstOrDefault();

                #endregion

                StationDetailedHaltAnalysis analysis = new StationDetailedHaltAnalysis
                {
                    Code = code.KOD,
                    HaltID = haltID,
                    Time = time,
                    StationName = stationName ,
                    Total = totaltime,
                    Percent = (double)time / (double)totaltime

                };
                if (analysis.Time > 0)
                {
                    stationDetailedHaltAnalysisChart.Add(analysis);
                }


            }
            return stationDetailedHaltAnalysisChart;
        }
        #endregion

        #endregion

        #region Stok Analizi

        #region Chart

        public List<StationDetailedProductChart> GetStationDetailedProductChart(int makineID, DateTime startDate, DateTime endDate, int products)
        {

            List<StationDetailedProductChart> stationDetailedProductChart = new List<StationDetailedProductChart>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID, startDate, endDate);

            switch (products)
            {
                case 1:
                    var productList = operationLines.Where(t => t.STOKTURU == 12).Select(t => t.STOKID).Distinct().ToList();

                    foreach (var productID in productList)
                    {
                        #region Değişkenler

                        int planlananBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                        int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);
                        string productGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault();

                        #endregion

                        StationDetailedProductChart analysis = new StationDetailedProductChart
                        {
                            ProductID = productID,
                            ProductGroup = productGroup,
                            Performance = (decimal)(gerceklesenBirimSure > 0 ? ((double)planlananBirimSure / (double)gerceklesenBirimSure) : 0)
                        };

                        stationDetailedProductChart.Add(analysis);


                    }
                    break;

                case 2:
                    productList = operationLines.Select(t => t.STOKID).Distinct().ToList();

                    foreach (var productID in productList)
                    {
                        #region Değişkenler

                        int planlananBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                        int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);
                        string productGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault();

                        #endregion

                        StationDetailedProductChart analysis = new StationDetailedProductChart
                        {
                            ProductID = productID,
                            ProductGroup = productGroup,
                            Performance = (decimal)(gerceklesenBirimSure > 0 ? ((double)planlananBirimSure / (double)gerceklesenBirimSure) : 0)
                        };

                        stationDetailedProductChart.Add(analysis);

                    }
                    break;

                default: break;
            }
            return stationDetailedProductChart;
        }

        #endregion

        #region Grid
        public List<StationDetailedProductAnalysis> GetStationDetailedProductAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {

            List<StationDetailedProductAnalysis> stationDetailedProductAnalysis = new List<StationDetailedProductAnalysis>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID, startDate, endDate);
            var productList = operationLines.Select(t => t.STOKID).Distinct().ToList();

            foreach (var productID in productList)
            {
                int plannedUnitTime = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                int occuredUnitTime = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);
                string productCode = operationLines.Where(t => t.STOKID == productID).Select(t => t.STOKKODU).FirstOrDefault();
                string productGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault();
                int totalProduction = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.URETILENADET);
                int totalScrap = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.HURDAADET);
                int plannedQuantity = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.PLNMIKTAR);

                StationDetailedProductAnalysis analysis = new StationDetailedProductAnalysis
                {
                    ProductID = productID,
                    ProductCode = productCode,
                    ProductGroup = productGroup,
                    TotalProduction = totalProduction,
                    TotalScrap = totalScrap,
                    PlannedUnitTime = plannedUnitTime,
                    OccuredUnitTime = occuredUnitTime,
                    PlannedQuantity = plannedQuantity,
                    Performance = (decimal)(occuredUnitTime > 0 ? ((double)plannedUnitTime / (double)occuredUnitTime) : 0)
                };
                if (analysis.Performance > 0 && analysis.Performance < 2)
                {
                    stationDetailedProductAnalysis.Add(analysis);
                }

            }


            return stationDetailedProductAnalysis;
        }

        #endregion

        #endregion

        #region Personel Analizi

        #region Chart-Grid
        public List<StationDetailedEmployeeAnalysis> GetStationDetailedEmployeeAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {

            List<StationDetailedEmployeeAnalysis> stationDetailedEmployeeAnalysis = new List<StationDetailedEmployeeAnalysis>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID, startDate, endDate);
            var employeeList = operationLines.Select(t => t.CALISANID).Distinct().ToList();
            var unsuitibility = DBHelper.GetUnsuitabilityEmployeeQuery(makineID, startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate).Where(t => t.ISTASYONID == makineID).ToList();

            foreach (var employeeID in employeeList)
            {
                var tempUnsuitibility = unsuitibility.Where(t => t.CALISANID == employeeID).ToList();
                var tempOperationLines = operationLines.Where(t => t.CALISANID == employeeID).ToList();

                #region Değişkenler

                decimal availability = (calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)) > 0 ? (decimal)(tempOperationLines.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE))) : 0;
                decimal perf = tempOperationLines.Sum(t => t.BIRIMSURE) > 0 ? tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE) : 0;
                decimal quality = (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) > 0 ? (decimal)((((tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) - (tempUnsuitibility.Sum(t => t.OLCUKONTROLFORMBEYAN) * tempOperationLines.Sum(t => t.BIRIMSURE)))) / (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE))) : 0;
                string employeeName = operationLines.Where(t => t.CALISANID == employeeID).Select(t => t.CALISAN).FirstOrDefault();

                #endregion

                StationDetailedEmployeeAnalysis analysis = new StationDetailedEmployeeAnalysis
                {
                    EmployeeID = employeeID,
                    EmployeeName = employeeName,
                    Availability = availability,
                    Performance = perf,
                    Quality = quality,
                    OEE = availability * perf * quality
                };

                stationDetailedEmployeeAnalysis.Add(analysis);
            }
            return stationDetailedEmployeeAnalysis;
        }

        #endregion

        #endregion
    }
}
