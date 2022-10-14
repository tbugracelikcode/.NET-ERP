using Microsoft.Data.SqlClient;
using System.Globalization;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class GenelOEEService : IGenelOEEService
    {
        

        #region Chart

        public async Task< List<AdminMachineChart>> GetAdminMachineChart(DateTime startDate, DateTime endDate)
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
            decimal availability = 0;
            decimal perf = 0;
            decimal quality = 0;
            int planlananbirimsure = 0;
            decimal gerceklesenbirimsure = 0;
            decimal uretilenadet = 0;
            int hurda = 0;
            int? toplamcalisilabilirsure = 0;
            decimal operasyonsuresi = 0;

            #endregion

            var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
            {

                #region Değişkenler

                #region Kullanılabilirlik

                previousMonthAvailability = availability;

                operasyonsuresi = t.Sum(t => t.OPERASYONSURESI);

                toplamcalisilabilirsure = calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE);

                availability = (decimal)toplamcalisilabilirsure > 0 ? (operasyonsuresi / (decimal)toplamcalisilabilirsure) : 0;

                #endregion

                #region Performans

                previousMonthPerformance = perf;

                gerceklesenbirimsure = t.Sum(t => t.BIRIMSURE);

                planlananbirimsure = t.Sum(t => t.PLANLANANOPRSURESI);

                perf = gerceklesenbirimsure > 0 ? (planlananbirimsure / gerceklesenbirimsure) : 0;
                #endregion

                #region Kalite 

                previousMonthQuality = quality;

                uretilenadet = t.Sum(t => t.URETILENADET);

                hurda = unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY).Sum(t => t.OLCUKONTROLFORMBEYAN);

                quality = ((uretilenadet * gerceklesenbirimsure) - (gerceklesenbirimsure * hurda)) / (uretilenadet * gerceklesenbirimsure);
                #endregion

                #region OEE

                previousMonthOEE = oee;

                oee = perf * quality * availability;
                #endregion

                decimal differenceOEE = oee - previousMonthOEE;

                decimal differenceQuality = quality - previousMonthQuality;

                decimal differencePerformance = perf - previousMonthPerformance;

                decimal differenceAvailability = availability - previousMonthAvailability;

                #endregion

                return new AdminMachineChart
                {
                    AY = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),

                    ISTASYONLAR = t.Select(t => t.MAKINEKODU).Distinct().ToList(),

                    KULLANILABILIRLIK = availability,

                    PERFORMANS = perf,

                    KALITE = quality,

                    OEE = oee,

                    DIFFAVA = differenceAvailability,

                    DIFFOEE = differenceOEE,

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

            return await Task.FromResult(adminMachineChart);

        }

        #endregion

        #region Grid

        public async Task< List<StationOEEAnalysis>> GetStationOEEAnalysis(DateTime startDate, DateTime endDate)
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
                    var tempUnsuitabilityLines = unsuitabilityLines.Where(t => t.ISTASYONID == stationID).ToList();

                    #region Değişkenler

                    string machineCode = operationLines.Where(t => t.ISTASYONID == stationID).Select(t => t.MAKINEKODU).FirstOrDefault();
                    string department = tempOperationLines.Select(t => t.DEPARTMAN).FirstOrDefault();

                    #region Kullanılabilirlik


                    decimal availability = (decimal)(tempOperationLines.Sum(t => t.OPERASYONSURESI) / (decimal)(tempCalendarLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)));

                    #endregion

                    #region Performans


                    decimal perf = tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE);

                    #endregion

                    #region Kalite


                    decimal quality = ((((tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) - (tempUnsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN) * tempOperationLines.Sum(t => t.BIRIMSURE)))) / (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)));
                    #endregion

                    #region OEE


                    decimal oee = availability * perf * quality;
                    #endregion


                    #endregion

                    StationOEEAnalysis analysis = new StationOEEAnalysis
                    {
                        StationID = stationID,
                        Code = machineCode,
                        Availability = availability,
                        Performance = perf,
                        Quality = quality,
                        OEE = oee,
                        Department = department,
                      
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
