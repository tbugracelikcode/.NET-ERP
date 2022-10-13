using Microsoft.Data.SqlClient;
using Syncfusion.DocIO.DLS;
using System.Globalization;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;
using System.Linq;

namespace TsiErp.DashboardUI.Services
{
    public class IstasyonOEEService : IIstasyonOEEService
    {

        #region Chart
        public async Task<List<AdminMachineChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminMachineChart> adminMachineChart = new List<AdminMachineChart>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);

            #region Değişkenler
            decimal previousMonthAvailability = 0;
            decimal previousMonthPerformance = 0;
            decimal previousMonthQuality = 0;
            decimal previousMonthOEE = 0;
            decimal oee = 0;
            decimal kullanilabilirlik = 0;
            decimal performans = 0;
            decimal kalite = 0;
            int planlananbirimsure = 0;
            decimal gerceklesenbirimsure = 0;
            decimal uretilenadet = 0;
            int hurda = 0;
            int? toplamcalisilabilirsure = 0;
            decimal operasyonsuresi = 0;


            #endregion


            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                {

                    #region Değişkenler

                    #region Kullanılabilirlik

                    previousMonthAvailability = kullanilabilirlik;

                    toplamcalisilabilirsure = calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE);

                    operasyonsuresi = t.Sum(t => t.OPERASYONSURESI);


                     kullanilabilirlik = (decimal)toplamcalisilabilirsure > 0  ? (operasyonsuresi / (decimal)toplamcalisilabilirsure)  : 0;

                    #endregion

                    #region Performans

                    previousMonthPerformance = performans;

                    gerceklesenbirimsure = t.Sum(t => t.BIRIMSURE);

                    planlananbirimsure = t.Sum(t => t.PLANLANANOPRSURESI);

                    performans = gerceklesenbirimsure > 0 ? (planlananbirimsure/ gerceklesenbirimsure) : 0;
                    #endregion

                    #region Kalite 

                    previousMonthQuality = kalite;

                    uretilenadet = t.Sum(t => t.URETILENADET);

                    hurda = unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                    kalite = ((uretilenadet * gerceklesenbirimsure) - (gerceklesenbirimsure* hurda)) / (uretilenadet *gerceklesenbirimsure);
                    #endregion

                    #region OEE

                    previousMonthOEE = oee;

                     oee = kalite*performans*kullanilabilirlik;
                    #endregion

                    decimal differenceOEE = oee - previousMonthOEE;

                    decimal differenceQuality = kalite - previousMonthQuality;

                    decimal differencePerformance = performans - previousMonthPerformance;

                    decimal differenceAvailability = kullanilabilirlik - previousMonthAvailability;

                    #endregion

                    return new AdminMachineChart
                    {
                        AY = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),

                        ISTASYONLAR = t.Select(t => t.MAKINEKODU).Distinct().ToList(),

                        KULLANILABILIRLIK = kullanilabilirlik,

                        PERFORMANS = performans,

                        KALITE = kalite,

                        OEE = oee,

                        DIFFOEE = differenceOEE,

                        DIFFAVA = differenceAvailability,

                        DIFFPER = differencePerformance,

                        DIFFQUA = differenceQuality,

                        OCCUREDUNITTIME = gerceklesenbirimsure,

                        PLANNEDUNITTIME = planlananbirimsure,

                        PRODUCTION = uretilenadet,

                        SCRAP = hurda,

                        TOTALSHIFTTIME = toplamcalisilabilirsure,

                        OPRTIME = operasyonsuresi


                    };
                }).ToList();

                adminMachineChart = gList;
            }

            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => 
                {

                    #region Değişkenler

                    #region Performans

                    previousMonthPerformance = performans;

                    planlananbirimsure = t.Sum(t => t.PLANLANANOPRSURESI);

                    gerceklesenbirimsure = t.Sum(t => t.BIRIMSURE);

                    performans = gerceklesenbirimsure == 0 ? 0 : (planlananbirimsure / gerceklesenbirimsure);

                    #endregion

                    #region Kalite 

                    previousMonthQuality = kalite;

                    uretilenadet = t.Sum(t => t.URETILENADET);

                    hurda = unsuitabilityLines.Where(b => b.TARIH.Date == t.Key.HAFTA && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                    kalite = ((uretilenadet * gerceklesenbirimsure) - (gerceklesenbirimsure * hurda)) / (uretilenadet * gerceklesenbirimsure);
                    #endregion

                    #region Kullanılabilirlik

                    previousMonthAvailability = kullanilabilirlik;

                    toplamcalisilabilirsure = (calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE));

                    operasyonsuresi = t.Sum(t => t.OPERASYONSURESI);

                    kullanilabilirlik = (decimal)toplamcalisilabilirsure == 0 ? 0 : (operasyonsuresi / (decimal)toplamcalisilabilirsure);

                    #endregion

                    #region OEE

                    previousMonthOEE = oee;

                    oee = performans*kullanilabilirlik*kalite;

                    #endregion

                    decimal differenceOEE = oee - previousMonthOEE;

                    decimal differenceQuality = kalite - previousMonthQuality;

                    decimal differencePerformance = performans - previousMonthPerformance;

                    decimal differenceAvailability = kullanilabilirlik - previousMonthAvailability;

                    #endregion

                    return new AdminMachineChart
                    {
                        AY = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),

                        ISTASYONLAR = t.Select(x => x.MAKINEKODU).Distinct().ToList(),

                        KULLANILABILIRLIK = kullanilabilirlik,

                        PERFORMANS = performans,

                        KALITE = kalite,

                        OEE = oee,

                        DIFFOEE = differenceOEE,

                        DIFFAVA = differenceAvailability,

                        DIFFPER = differencePerformance,

                        DIFFQUA = differenceQuality,

                        OCCUREDUNITTIME = gerceklesenbirimsure,

                        PLANNEDUNITTIME = planlananbirimsure,

                        PRODUCTION = uretilenadet,

                        SCRAP = hurda,

                        TOTALSHIFTTIME = toplamcalisilabilirsure,

                        OPRTIME = operasyonsuresi
                    };
                   

                }).ToList();

                adminMachineChart = gList;
            }

            return await Task.FromResult(adminMachineChart);

        }

        #endregion

        #region Grid

        public async Task<List<StationOEEAnalysis>> GetStationOEEAnalysis(DateTime startDate, DateTime endDate)
        {

            List<StationOEEAnalysis> stationOEEAnalysis = new List<StationOEEAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var stationList = operationLines.Select(t => t.ISTASYONID).Distinct().ToList();
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);

            if (stationList != null)
            {
                foreach (var stationID in stationList)
                {
                    var tempCalendarLines = calenderLines.Where(t => t.ISTASYONID == stationID).ToList();
                    var tempOperationLines = operationLines.Where(t => t.ISTASYONID == stationID).ToList();
                    var tempUnsuitabilityLines = unsuitabilityLines.Where(t => t.ISTASYONID == stationID && t.ISTVERIMLILIIKANALIZI == true).ToList();

                    #region Değişkenler

                    decimal availability = (decimal)(tempCalendarLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)) == 0 ? 0 : (decimal)(tempOperationLines.Sum(t => t.OPERASYONSURESI) / (decimal)(tempCalendarLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)));

                    decimal perf = tempOperationLines.Sum(t => t.BIRIMSURE) == 0 ? 0 : tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE);

                    decimal quality = (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) == 0 ? 0 : ((((tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) - (tempUnsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN) * tempOperationLines.Sum(t => t.BIRIMSURE)))) / (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)));

                    string machineCode = operationLines.Where(t => t.ISTASYONID == stationID).Select(t => t.MAKINEKODU).FirstOrDefault();
                    string department = tempOperationLines.Select(t => t.DEPARTMAN).FirstOrDefault();

                    #endregion

                    StationOEEAnalysis analysis = new StationOEEAnalysis
                    {
                        StationID = stationID,
                        Code = machineCode,
                        Availability = availability,
                        Performance = perf,
                        Quality = quality,
                        OEE = availability * perf * quality,
                        Department = department
                    };
                    stationOEEAnalysis.Add(analysis);
                }
            }

            stationOEEAnalysis = stationOEEAnalysis.OrderByDescending(t => t.OEE).ToList();
            return await Task.FromResult(stationOEEAnalysis);
        }

        #endregion

        private string GetMonth(int ay)
        {
            string aystr = string.Empty;
            switch (ay)
            {
                case 1: aystr = "Ocak"; break;
                case 2: aystr = "Şubat"; break;
                case 3: aystr = "Mart"; break;
                case 4: aystr = "Nisan"; break;
                case 5: aystr = "Mayıs"; break;
                case 6: aystr = "Haziran"; break;
                case 7: aystr = "Temmuz"; break;
                case 8: aystr = "Ağustos"; break;
                case 9: aystr = "Eylül"; break;
                case 10: aystr = "Ekim"; break;
                case 11: aystr = "Kasım"; break;
                case 12: aystr = "Aralık"; break;
                default: break;

            }
            return aystr;
        }
    }
}
