using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;
using System.Globalization;

namespace TsiErp.DashboardUI.Services
{
    public class PersonelService : IPersonelService
    {

        #region Chart

        public async Task< List<AdminEmployeeChart>> GetEmployeeChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminEmployeeChart> adminEmployeeChart = new List<AdminEmployeeChart>();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var haltLines = DBHelper.GetHaltQuery(startDate, endDate);

            #region Değişkenler
            decimal previousMonthAvailability = 0;
            decimal previousMonthPerformance = 0;
            decimal previousMonthQuality = 0;
            decimal previousMonthOEE = 0;
            decimal oee = 0;
            decimal kullanilabilirlik = 0;
            decimal performans = 0;
            decimal kalite = 0;

            #endregion


            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
                {

                    #region Değişkenler

                    #region Kullanılabilirlik

                    previousMonthAvailability = kullanilabilirlik;

                    kullanilabilirlik = (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0 ? (decimal)(t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) : 0;

                    #endregion

                    #region Performans

                    previousMonthPerformance = performans;

                    performans = t.Sum(t => t.BIRIMSURE) > 0 ? (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) : 0;
                    #endregion

                    #region Kalite 

                    previousMonthQuality = kalite;

                    kalite = ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.PERVERIMLILIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)));
                    #endregion

                    #region OEE

                    previousMonthOEE = oee;

                    oee = ((calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0) && (t.Sum(t => t.BIRIMSURE) > 0) && ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) > 0) ? ((decimal)t.Sum(t => t.OPERASYONSURESI)  / (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) * (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) * (((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.PERVERIMLILIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)))) : 0;
                    #endregion

                    decimal differenceOEE = oee - previousMonthOEE;

                    decimal differenceQuality = kalite - previousMonthQuality;

                    decimal differencePerformance = performans - previousMonthPerformance;

                    decimal differenceAvailability = kullanilabilirlik - previousMonthAvailability;

                    #endregion

                    return new AdminEmployeeChart
                    {
                        AY = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),

                        PERSONELLER = t.Select(t => t.CALISAN).Distinct().ToList(),

                        KULLANILABILIRLIK = kullanilabilirlik,

                        PERFORMANS = performans,

                        KALITE = kalite,

                        OEE = oee,

                        DIFFOEE = differenceOEE,

                        DIFFAVA = differenceAvailability,

                        DIFFPER = differencePerformance,

                        DIFFQUA = differenceQuality
                    };
                   

                }).ToList();
                adminEmployeeChart = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => 
                {

                    #region Değişkenler

                    #region Performans

                    previousMonthPerformance = performans;

                    performans = t.Sum(t => t.BIRIMSURE) == 0 ? 0 : (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE));

                    #endregion

                    #region Kalite 

                    previousMonthQuality = kalite;

                    kalite = ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))) == 0 ? 0 : ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Date == t.Key.HAFTA && b.PERVERIMLILIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)));
                    #endregion

                    #region Kullanılabilirlik

                    previousMonthAvailability = kullanilabilirlik;

                    kullanilabilirlik = (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) == 0 ? 0 : (decimal)(t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)));

                    #endregion

                    #region OEE

                    previousMonthOEE = oee;

                    oee = (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) == 0 || t.Sum(t => t.BIRIMSURE) == 0 || ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))) == 0 ? 0 :
                    (decimal)(t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => (c.CALISMADURUMU != "ÇALIŞMA YOK" && c.CALISMADURUMU != "TATİL") && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Date == t.Key.HAFTA && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) * (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) * (((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Date == t.Key.HAFTA && b.PERVERIMLILIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))));

                    #endregion

                    decimal differenceOEE = oee - previousMonthOEE;

                    decimal differenceQuality = kalite - previousMonthQuality;

                    decimal differencePerformance = performans - previousMonthPerformance;

                    decimal differenceAvailability = kullanilabilirlik - previousMonthAvailability;

                    #endregion

                    return new AdminEmployeeChart
                    {
                        AY = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),

                        PERSONELLER = t.Select(x => x.MAKINEKODU).Distinct().ToList(),

                        KULLANILABILIRLIK = kullanilabilirlik,

                        PERFORMANS = performans,

                        KALITE = kalite,

                        OEE = oee,

                        DIFFOEE = differenceOEE,

                        DIFFAVA = differenceAvailability,

                        DIFFPER = differencePerformance,

                        DIFFQUA = differenceQuality
                    };
                   

                }).ToList();
                adminEmployeeChart = gList;
            }

            return await Task.FromResult(adminEmployeeChart);
        }

        #endregion

        #region Grid

        public async Task<List<EmployeeGeneralAnalysis>> GetEmployeeGeneralAnalysis(DateTime startDate, DateTime endDate)
        {
            List<EmployeeGeneralAnalysis> employeeGeneralAnalysis = new List<EmployeeGeneralAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var haltLines = DBHelper.GetHaltQuery(startDate, endDate);
            var employeeList = operationLines.Select(t => t.CALISANID).Distinct().ToList();
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            decimal vardiyaSure = (decimal)(calenderLines.Where(t => t.ISTASYONID == 8 && t.CALISMADURUMU != "ÇALIŞMA YOK" && t.CALISMADURUMU != "TATİL" &&  t.PLANLANAN == "Hayır").Sum(t => t.GUNDUZTOPLAMCALISMAZAMANI)- calenderLines.Where(t => t.ISTASYONID == 8 && t.CALISMADURUMU != "ÇALIŞMA YOK" && t.CALISMADURUMU != "TATİL" && t.PLANLANAN == "Hayır").Sum(t => t.GUNDUZPLNDURUSSURESI));

            if (employeeList != null)
            {
                foreach (var employeeID in employeeList)
                {
                    var tempOperationLines = operationLines.Where(t => t.CALISANID == employeeID).ToList();
                    var tempUnsuitabilityLines = unsuitabilityLines.Where(t => t.CALISANID == employeeID && t.PERVERIMLILIKANALIZI == true).ToList();
                    var tempHaltLines = haltLines.Where(t => t.CALISANID == employeeID && t.PKD == false).ToList();

                    #region Değişkenler

                    decimal availability = (decimal)((tempOperationLines.Sum(t => t.OPERASYONSURESI) + tempHaltLines.Sum(t=>t.DURUSSURE) )/ vardiyaSure);
                    decimal perf = tempOperationLines.Sum(t => t.BIRIMSURE) == 0 ? 0 : tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE) ;
                    decimal quality = tempOperationLines.Sum(t => t.BIRIMSURE) == 0 ? 0 : ((((tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) - (tempUnsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN) * tempOperationLines.Sum(t => t.BIRIMSURE)))) / (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)));
                    string employeeName = operationLines.Where(t => t.CALISANID == employeeID).Select(t => t.CALISAN).FirstOrDefault();

                    #endregion

                    EmployeeGeneralAnalysis analysis = new EmployeeGeneralAnalysis
                    {
                        EmployeeID = employeeID,
                        EmployeeName = employeeName,
                        Performance = perf,
                        Quality = quality,
                        Availability = availability,
                        OEE = perf* quality * availability

                    };
                    employeeGeneralAnalysis.Add(analysis);
                }
            }

            employeeGeneralAnalysis = employeeGeneralAnalysis.OrderByDescending(t => t.OEE).ToList();
            return await Task.FromResult(employeeGeneralAnalysis);
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