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

            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                {
                    decimal kullanilabilirlik = (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0

                    ? ((decimal)t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)))

                    : 0;

                    decimal oee = ((calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0) && (t.Sum(t => t.BIRIMSURE) > 0) && ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) > 0) ? ((decimal)t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) * (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) * (((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)))) : 0;

                    return new AdminMachineChart
                    {
                        AY = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),

                        ISTASYONLAR = t.Select(t => t.MAKINEKODU).Distinct().ToList(),

                        KULLANILABILIRLIK = kullanilabilirlik,

                        PERFORMANS = t.Sum(t => t.BIRIMSURE) > 0 ? (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) : 0,

                        KALITE = ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))),

                        OEE = oee

                    };
                }).ToList();

                adminMachineChart = gList;
            }

            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminMachineChart
                {
                    AY = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),

                    ISTASYONLAR = t.Select(x => x.MAKINEKODU).Distinct().ToList(),

                    KULLANILABILIRLIK = (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) == 0 ? 0 : ((decimal)t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))),

                    PERFORMANS = t.Sum(t => t.BIRIMSURE) == 0 ? 0 : (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)),

                    KALITE = ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Date == t.Key.HAFTA && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))),

                    OEE = (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))== 0 || (t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) == 0 ? 0 : ((decimal)t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) * (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) * (((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Date == t.Key.HAFTA && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))))

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
                    var tempUnsuitabilityLines = unsuitabilityLines.Where(t=>t.ISTASYONID == stationID && t.ISTVERIMLILIIKANALIZI == true).ToList();

                    #region Değişkenler

                    decimal availability = (decimal)(tempCalendarLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)) == 0 ? 0 :   (decimal)(tempOperationLines.Sum(t => t.OPERASYONSURESI) / (decimal)(tempCalendarLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)));

                    decimal perf = tempOperationLines.Sum(t => t.BIRIMSURE) == 0 ? 0 :  tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE);

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
