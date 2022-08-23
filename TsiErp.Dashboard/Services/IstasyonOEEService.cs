using Microsoft.Data.SqlClient;
using TsiErp.Dashboard.Helpers;
using TsiErp.Dashboard.Helpers.HelperModels;
using TsiErp.Dashboard.Models;

namespace TsiErp.Dashboard.Services
{
    public class IstasyonOEEService
    {
        SqlConnection _connection;

        public IstasyonOEEService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<StationOEEAnalysis> GetStationOEEAnalysis(DateTime startDate, DateTime endDate)
        {
            startDate = new DateTime(2022, 06, 01);
            endDate = new DateTime(2022, 08, 22);

            List<StationOEEAnalysis> stationOEEAnalysis = new List<StationOEEAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var stationList = operationLines.Select(t => t.ISTASYONID).Distinct().ToList();

            if (stationList != null)
            {
                foreach (var stationID in stationList)
                {
                    var tempCalendarLines = calenderLines.Where(t => t.ISTASYONID == stationID).ToList();
                    var tempOperationLines = operationLines.Where(t => t.ISTASYONID == stationID).ToList();

                    decimal vardiyaCalismaSuresi = _VardiyaCalismaSuresiHesapla(tempCalendarLines);
                    decimal gerceklesenOperasyonSuresi = tempOperationLines.Sum(t => t.OPERASYONSURESI);
                    decimal hurdaSuresi = _HurdaSuresiHesapla(tempOperationLines);
                    decimal planlananOperasyonSuresi = _PlanlananOperasyonSuresiHesapla(tempOperationLines);

                    StationOEEAnalysis analysis = new StationOEEAnalysis
                    {
                        StationID = stationID,
                        Code = operationLines.Where(t => t.ISTASYONID == stationID).Select(t => t.MAKINEKODU).FirstOrDefault(),
                        ShiftTime = vardiyaCalismaSuresi,
                        PlannedOperationTime = planlananOperasyonSuresi,
                        OccuredOperationTime = gerceklesenOperasyonSuresi,
                        ScrapTime = hurdaSuresi,
                        Availability = vardiyaCalismaSuresi > 0 && gerceklesenOperasyonSuresi > 0 ? gerceklesenOperasyonSuresi / vardiyaCalismaSuresi : 0,
                        Performance = planlananOperasyonSuresi > 0 && gerceklesenOperasyonSuresi > 0 ? planlananOperasyonSuresi / gerceklesenOperasyonSuresi : 0,
                        Quality = gerceklesenOperasyonSuresi > 0 ? (gerceklesenOperasyonSuresi - hurdaSuresi) / gerceklesenOperasyonSuresi : 0,
                        OEE = (gerceklesenOperasyonSuresi / vardiyaCalismaSuresi) * (planlananOperasyonSuresi / gerceklesenOperasyonSuresi) * ((gerceklesenOperasyonSuresi - hurdaSuresi) / gerceklesenOperasyonSuresi)
                    };
                    stationOEEAnalysis.Add(analysis);
                }
            }
            return stationOEEAnalysis;
        }

        private decimal _VardiyaCalismaSuresiHesapla(List<Takvim> calendarList)
        {

            var list = calendarList.Where(t => t.CALISMADURUMU == "ÇALIŞMA VAR" && t.PLANLANAN == "Hayır").ToList();
            int toplamSure = 0;
            foreach (var item in list)
            {
                toplamSure += (int)item.TOPLAMCALISABILIRSURE;
            }

            return toplamSure;
        }

        private decimal _PlanlananOperasyonSuresiHesapla(List<OperasyonSatir> oprList)
        {
            decimal planlananOprSuresi = 0;
            foreach (var satir in oprList)
            {
                if (satir.URETILENADET > 0 && satir.ISEMRIID > 0)
                {
                    planlananOprSuresi += ((decimal)satir.PLANLANANOPRSURESI * (decimal)satir.URETILENADET);
                }
            }

            return planlananOprSuresi;
        }

        private decimal _HurdaSuresiHesapla(List<OperasyonSatir> oprList)
        {
            decimal hurdaSuresi = 0;
            foreach (var satir in oprList)
            {
                if (satir.HURDAADET > 0)
                {
                    hurdaSuresi += ((decimal)satir.BIRIMSURE * (decimal)satir.HURDAADET);
                }
            }
            return hurdaSuresi;
        }
    }
}
