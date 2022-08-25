using Microsoft.Data.SqlClient;
using TsiErp.Dashboard.Helpers;
using TsiErp.Dashboard.Helpers.HelperModels;
using TsiErp.Dashboard.Models;

namespace TsiErp.Dashboard.Services
{
    public class IstasyonDetayService
    {
        SqlConnection _connection;
        public IstasyonDetayService()
        {            
            _connection = DBHelper.GetSqlConnection();
        }

        #region Duruş Analizi
        public List<StationDetailedHaltAnalysis> GetStationDetailedHaltAnalysis(int makineID,DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //makineID = 8;
            List<StationDetailedHaltAnalysis> stationDetailedHaltAnalysis = new List<StationDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryStation(makineID,startDate,endDate);

            foreach (var code in haltCodes)
            {
                int durusID = code.ID;

                StationDetailedHaltAnalysis analysis = new StationDetailedHaltAnalysis
                {
                    Code = code.KOD,
                    HaltID = durusID,
                    Time = haltLines.Where(t=>t.DURUSID == durusID).Sum(t=>t.DURUSSURE)
                };

                stationDetailedHaltAnalysis.Add(analysis);
            }
            return stationDetailedHaltAnalysis;
        }
        #endregion

        #region Stok Analizi
        public List<StationDetailedProductAnalysis> GetStationDetailedProductAnalysis(int makineID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //makineID = 8;

            List<StationDetailedProductAnalysis> stationDetailedProductAnalysis = new List<StationDetailedProductAnalysis>();

            var operationLines = DBHelper.GetOperationLinesStationQuery(makineID,startDate, endDate);
            var productList = operationLines.Select(t => t.STOKID).Distinct().ToList();

            foreach (var productID in productList)
            {
                int planlananBirimSure = operationLines.Where(t => t.STOKID == productID).Sum(t => t.PLANLANANOPRSURESI);
                int gerceklesenBirimSure = (int)operationLines.Where(t => t.STOKID == productID).Sum(t => t.BIRIMSURE);

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

                stationDetailedProductAnalysis.Add(analysis);
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
                    EmployeeID= employeeID,
                    EmployeeName = operationLines.Where(t=>t.CALISANID == employeeID).Select(t=>t.CALISAN).FirstOrDefault(),
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
