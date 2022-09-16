using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class IstasyonService : IIstasyonService
    {

        #region Genel Analiz
        public async Task< List<StationGeneralAnalysis>> GetStationGeneralAnalyies(DateTime startDate, DateTime endDate)
        {

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

                    #region Değişkenler

                    int unplannedHaltTime = tempHaltLines.Where(t => t.PLANLI == "PLANSIZ").Sum(t => t.DURUSSURE);
                    int stationHaltTime = tempHaltLines.Sum(t => t.DURUSSURE);
                    string machineCode = tempOperationLines.Where(t => t.ISTASYONID == item).Select(t => t.MAKINEKODU).FirstOrDefault();
                    decimal stationProductionTime = tempOperationLines.Sum(t => t.OPERASYONSURESI) + tempOperationLines.Sum(t => t.AYARSURESI);
                    int plannedHaltTime = tempHaltLines.Where(t => t.PLANLI == "PLANLI").Sum(t => t.DURUSSURE);

                    #endregion

                    StationGeneralAnalysis analysis = new StationGeneralAnalysis
                    {
                        AvailableTime = _KullanilabilirZaman(tempCalendarLines),
                        StationID = item,
                        Code = machineCode,
                        TheoreticalTime = _TeorikSure(tempCalendarLines),
                        StationOnTime = _MakineninAcikOlduguSure(tempOperationLines, tempHaltLines),
                        StationProductionTime = stationProductionTime,
                        StationHaltTime = stationHaltTime,
                        PlannedHaltTime = plannedHaltTime,
                        UnplannedHaltTime = unplannedHaltTime,
                        UnplannedPercentage = unplannedHaltTime > 0 && stationHaltTime > 0 ? ((double)unplannedHaltTime / (double)stationHaltTime) : 0

                    };
                    stationGeneralAnalyses.Add(analysis);
                }
            }

            return await Task.FromResult(stationGeneralAnalyses);
        }



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
        #endregion
    }
}
