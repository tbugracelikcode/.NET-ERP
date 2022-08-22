using Microsoft.Data.SqlClient;
using TsiErp.Dashboard.Helpers;
using TsiErp.Dashboard.Helpers.HelperModels;
using TsiErp.Dashboard.Models;

namespace TsiErp.Dashboard.Services
{
    public class IstasyonService
    {
        SqlConnection _connection;

        public IstasyonService()
        {

            _connection = DBHelper.GetSqlConnection();
        }

        public List<StationGeneralAnalysis> GetStationGeneralAnalyses(DateTime startDate, DateTime endDate)
        {
            startDate = new DateTime(2022, 06, 01);
            endDate = new DateTime(2022, 08, 22);

            List<StationGeneralAnalysis> stationGeneralAnalyses = new List<StationGeneralAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var haltLines = DBHelper.GetHaltQuery(startDate, endDate);

            var stationList = operationLines.Select(t=>t.ISTASYONID).Distinct().ToList();


            if (stationList != null)
            {
                foreach (var item in stationList)
                {
                    var tempCalendarLines = calenderLines.Where(t => t.ISTASYONID == item).ToList();
                    var tempOperationLines = operationLines.Where(t => t.ISTASYONID == item).ToList();
                    var tempHaltLines = haltLines.Where(t => t.ISTASYONID == item).ToList();

                    int unplannedHaltTime = tempHaltLines.Where(t => t.PLANLI == "PLANSIZ").Sum(t => t.DURUSSURE);
                    int stationHaltTime = tempHaltLines.Sum(t => t.DURUSSURE);
                    StationGeneralAnalysis analysis = new StationGeneralAnalysis
                    {
                        AvailableTime = _KullanilabilirZaman(tempCalendarLines),
                        StationID = item,
                        Code = tempOperationLines.Where(t=>t.ISTASYONID == item).Select(t=>t.MAKINEKODU).FirstOrDefault(),
                        TheoreticalTime = _TeorikSure(tempCalendarLines),
                        StationOnTime = _MakineninAcikOlduguSure(tempOperationLines, tempHaltLines),
                        StationProductionTime = tempOperationLines.Sum(t => t.OPERASYONSURESI) + tempOperationLines.Sum(t=>t.AYARSURESI),
                        StationHaltTime = stationHaltTime,
                        PlannedHaltTime = tempHaltLines.Where(t=>t.PLANLI == "PLANLI").Sum(t=>t.DURUSSURE),
                        UnplannedHaltTime = unplannedHaltTime,
                        UnplannedPercentage = unplannedHaltTime > 0 && stationHaltTime > 0 ? ((double)unplannedHaltTime / (double)stationHaltTime) : 0
                    };
                    stationGeneralAnalyses.Add(analysis);
                }
            }

            return stationGeneralAnalyses;
        }


        //public IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        //{
        //    for (var day = start.Date; day.Date <= end.Date; day = day.AddDays(1))
        //        yield return day;
        //}

        private decimal _KullanilabilirZaman(List<Takvim> calenderLines)
        {
            decimal kullanilabilirZaman = 0;
            foreach (var line in calenderLines)
            {
                kullanilabilirZaman += (decimal)line.TOPLAMCALISABILIRSURE;
            }

            return kullanilabilirZaman;
        }

        private decimal _TeorikSure(List<Takvim> calenderLines)
        {
            decimal teorikSure = 0;

            foreach (var line in calenderLines)
            {
                teorikSure += (decimal)line.GUNDUZMESAISURESI;
            }

            return teorikSure;
        }

        private decimal _MakineninAcikOlduguSure(List<OperasyonSatir> tempOperationLines, List<Durus> tempHaltLines)
        {
            decimal acikSure = 0;

            acikSure += tempOperationLines.Sum(t => t.OPERASYONSURESI);
            acikSure += tempOperationLines.Sum(t => t.AYARSURESI);
            acikSure += tempHaltLines.Sum(t => t.DURUSSURE);

            return acikSure;
        }
    }
}
