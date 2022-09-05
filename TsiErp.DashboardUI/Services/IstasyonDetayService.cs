using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class IstasyonDetayService
    {

        SqlConnection _connection;
        public IstasyonDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Duruş Analizi
        public List<StationDetailedHaltAnalysis> GetStationDetailedHaltAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //makineID = 8;
            List<StationDetailedHaltAnalysis> stationDetailedHaltAnalysis = new List<StationDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryStation(makineID, startDate, endDate);

            foreach (var code in haltCodes)
            {
                int durusID = code.ID;

                StationDetailedHaltAnalysis analysis = new StationDetailedHaltAnalysis
                {
                    Code = code.KOD,
                    HaltID = durusID,
                    Time = haltLines.Where(t => t.DURUSID == durusID).Sum(t => t.DURUSSURE),
                    StationName = haltLines.Where(t=>t.DURUSID == durusID).Select(t=>t.MAKINEKODU).FirstOrDefault()
                };

                stationDetailedHaltAnalysis.Add(analysis);
            }
            return stationDetailedHaltAnalysis;
        }
        public List<StationDetailedHaltAnalysis> GetStationDetailedHaltAnalysisChart(int makineID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //makineID = 8;
            List<StationDetailedHaltAnalysis> stationDetailedHaltAnalysisChart = new List<StationDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryStation(makineID, startDate, endDate);

            foreach (var code in haltCodes)
            {
                int durusID = code.ID;

                StationDetailedHaltAnalysis analysis = new StationDetailedHaltAnalysis
                {
                    Code = code.KOD,
                    HaltID = durusID,
                    Time = haltLines.Where(t => t.DURUSID == durusID).Sum(t => t.DURUSSURE),
                    StationName = haltLines.Where(t => t.DURUSID == durusID).Select(t => t.MAKINEKODU).FirstOrDefault()
                };
                if(analysis.Time >0)
                {
                    stationDetailedHaltAnalysisChart.Add(analysis);
                }

                
            }
            return stationDetailedHaltAnalysisChart;
        }
        #endregion

        #region Stok Analizi
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
                        int planlananBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                        int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);

                        StationDetailedProductChart analysis = new StationDetailedProductChart
                        {
                            ProductID = productID,
                            ProductGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault(),
                            Performance = (decimal)(gerceklesenBirimSure > 0 ? ((double)planlananBirimSure / (double)gerceklesenBirimSure) : 0)
                        };

                        stationDetailedProductChart.Add(analysis);


                    }
                    break;

                case 2:
                    productList = operationLines.Select(t => t.STOKID).Distinct().ToList();
                    foreach (var productID in productList)
                    {
                        int planlananBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                        int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);

                        StationDetailedProductChart analysis = new StationDetailedProductChart
                        {
                            ProductID = productID,
                            ProductGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault(),
                            Performance = (decimal)(gerceklesenBirimSure > 0 ? ((double)planlananBirimSure / (double)gerceklesenBirimSure) : 0)
                        };

                        stationDetailedProductChart.Add(analysis);

                    }
                    break;

                default: break;
            }
            return stationDetailedProductChart;
        }
        public List<StationDetailedProductAnalysis> GetStationDetailedProductAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {

            List<StationDetailedProductAnalysis> stationDetailedProductAnalysis = new List<StationDetailedProductAnalysis>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID, startDate, endDate);

            var productList = operationLines.Select(t => t.STOKID).Distinct().ToList();
            foreach (var productID in productList)
            {
                int planlananBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.PLANLANANOPRSURESI);
                int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Average(t => t.BIRIMSURE);

                StationDetailedProductAnalysis analysis = new StationDetailedProductAnalysis
                {
                    ProductID = productID,
                    ProductCode = operationLines.Where(t => t.STOKID == productID).Select(t => t.STOKKODU).FirstOrDefault(),
                    ProductGroup = operationLines.Where(t => t.STOKID == productID).Select(t => t.URUNGRUBU).FirstOrDefault(),
                    TotalProduction = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.URETILENADET),
                    TotalScrap = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.HURDAADET),
                    PlannedUnitTime = planlananBirimSure,
                    OccuredUnitTime = gerceklesenBirimSure,
                    PlannedQuantity = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.PLNMIKTAR),
                    Performance = (decimal)(gerceklesenBirimSure > 0 ? ((double)planlananBirimSure / (double)gerceklesenBirimSure) : 0)
                };
                if (analysis.Performance > 0 && analysis.Performance < 2)
                {
                    stationDetailedProductAnalysis.Add(analysis);
                }

            }


            return stationDetailedProductAnalysis;
        }
        #endregion

        #region Personel Analizi
        public List<StationDetailedEmployeeAnalysis> GetStationDetailedEmployeeAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //makineID = 8;

            List<StationDetailedEmployeeAnalysis> stationDetailedEmployeeAnalysis = new List<StationDetailedEmployeeAnalysis>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID, startDate, endDate);
            var employeeList = operationLines.Select(t => t.CALISANID).Distinct().ToList();

            foreach (var employeeID in employeeList)
            {

                StationDetailedEmployeeAnalysis analysis = new StationDetailedEmployeeAnalysis
                {
                    EmployeeID = employeeID,
                    EmployeeName = operationLines.Where(t => t.CALISANID == employeeID).Select(t => t.CALISAN).FirstOrDefault(),
                    TotalProduction = (int)operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.URETILENADET),
                    TotalScrap = (int)operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.HURDAADET),
                    OperationTime = (int)operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.OPERASYONSURESI)
                };

                stationDetailedEmployeeAnalysis.Add(analysis);
            }
            return stationDetailedEmployeeAnalysis;
        }
        #endregion
    }
}
