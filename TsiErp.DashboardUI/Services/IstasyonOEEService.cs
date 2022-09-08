using Microsoft.Data.SqlClient;
using System.Globalization;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class IstasyonOEEService
    {
        SqlConnection _connection;

        public IstasyonOEEService()
        {
            _connection = DBHelper.GetSqlConnection();
        }


        public List<AdminMachineChart> GetAdminMachineChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminMachineChart> adminMachineChart = new List<AdminMachineChart>();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var haltLines = DBHelper.GetHaltQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);

            //var list = (from a in calenderLines
            //            join
            //            b in operationLines.DefaultIfEmpty() on a.ISTASYONID equals b.ISTASYONID
            //            select new AdminCommonOEEMachineAnalysisChart
            //            {
            //                ISTASYONID = (int)a.ISTASYONID,
            //                TARIH = b.TARIH,
            //                BAKIMDURUMU = a.BAKIMDURUMU,
            //                ATILSURE = b.ATILSURE,
            //                CALISMADURUMU = a.CALISMADURUMU,
            //                AYARSURESI = b.AYARSURESI,
            //                BAKIMSURESI = a.BAKIMSURESI,
            //                GECEPLNDURUSSURESI = a.GECEPLNDURUSSURESI,
            //                GECEVARDIYASI = a.GECEVARDIYASI,
            //                GUNDUZFAZLAMESAI = a.GUNDUZFAZLAMESAI,
            //                GUNDUZFAZLAMESAISURESI = a.GUNDUZFAZLAMESAISURESI,
            //                GUNDUZMESAISURESI = a.GUNDUZMESAISURESI,
            //                HURDAADET = b.HURDAADET,
            //                GUNDUZPLNDURUSSURESI = a.GUNDUZPLNDURUSSURESI,
            //                GECEFAZLAMESAI = a.GECEFAZLAMESAI,
            //                BIRIMSURE = b.BIRIMSURE,
            //                GECEFAZLAMESAISURESI = a.GECEFAZLAMESAISURESI,
            //                PERFORMANS = b.PERFORMANS,
            //                GECEMESAISURESI = a.GECEMESAISURESI,
            //                GECETOPLAMCALISMAZAMANI = a.GECETOPLAMCALISMAZAMANI,
            //                GUNDUZTOPLAMCALISMAZAMANI = a.GUNDUZTOPLAMCALISMAZAMANI,
            //                GUNDUZVARDIYASI = a.GUNDUZVARDIYASI,
            //                GUNDUZYARIMGUN = a.GUNDUZYARIMGUN,
            //                ID = (int)a.ID,
            //                MAKINEKODU = b.MAKINEKODU,
            //                OPERASYONID = b.OPERASYONID,
            //                OPRID = b.OPRID,
            //                PLANLIBAKIMVARDIYASI = a.PLANLIBAKIMVARDIYASI,
            //                ROTAID = b.ROTAID,
            //                TAKVIMID = a.TAKVIMID,
            //                URETILENADET = b.URETILENADET,
            //                VARDIYACALISMASURESI = b.VARDIYACALISMASURESI,
            //                GECEYARIMGUN = a.GECEYARIMGUN,
            //                GRCMIKTAR = b.GRCMIKTAR,
            //                ISLEMESURESI = b.ISLEMESURESI,
            //                OPERASYONSURESI = b.OPERASYONSURESI,
            //                KALITE = b.KALITE,
            //                KULLANILABILIRLIK = b.KULLANILABILIRLIK,
            //                OEE = b.OEE,
            //                PLANLANAN = a.PLANLANAN,
            //                PLANLANANOPRSURESI = b.PLANLANANOPRSURESI,
            //                PLNMIKTAR = b.PLNMIKTAR,
            //                TOPLAMCALISABILIRSURE = a.TOPLAMCALISABILIRSURE
            //            }).ToList();
            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminMachineChart
                {
                    AY = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),
                    ISTASYONLAR = t.Select(t => t.MAKINEKODU).Distinct().ToList(),
                    KULLANILABILIRLIK = (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0 ? ((decimal)t.Sum(t=>t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c =>c.CALISMADURUMU=="ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) : 0,
                    PERFORMANS = t.Sum(t => t.BIRIMSURE) > 0 ? (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) : 0,
                    KALITE = ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))),
                    OEE = ((calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE)) > 0) && ( t.Sum(t => t.BIRIMSURE) > 0) && ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE))>0) ? ((decimal)t.Sum(t => t.OPERASYONSURESI) / (decimal)(calenderLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır" && c.TARIH.Value.Month == t.Key.AY && c.TARIH.Value.Year == t.Key.YIL).Sum(c => c.TOPLAMCALISABILIRSURE))) * (t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE)) * (((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)) - (t.Sum(t => t.BIRIMSURE) * (unsuitabilityLines.Where(b => b.TARIH.Month == t.Key.AY && b.ISTVERIMLILIIKANALIZI == true).Sum(t => t.OLCUKONTROLFORMBEYAN)))) / ((t.Sum(t => t.URETILENADET) * t.Sum(t => t.BIRIMSURE)))) : 0
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

            return adminMachineChart;

        }


        public List<StationOEEAnalysis> GetStationOEEAnalysis(DateTime startDate, DateTime endDate)
        {

            List<StationOEEAnalysis> stationOEEAnalysis = new List<StationOEEAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);
            var stationList = operationLines.Select(t => t.ISTASYONID).Distinct().ToList();
            //var haltLines = DBHelper.GetHaltQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);

            if (stationList != null)
            {
                foreach (var stationID in stationList)
                {
                    var tempCalendarLines = calenderLines.Where(t => t.ISTASYONID == stationID).ToList();
                    var tempOperationLines = operationLines.Where(t => t.ISTASYONID == stationID).ToList();
                    var tempUnsuitabilityLines = unsuitabilityLines.Where(t=>t.ISTASYONID == stationID && t.ISTVERIMLILIIKANALIZI == true).ToList();
                    //decimal vardiyaCalismaSuresi = _VardiyaCalismaSuresiHesapla(tempCalendarLines);
                    //decimal gerceklesenOperasyonSuresi = tempOperationLines.Sum(t => t.OPERASYONSURESI);
                    //decimal hurdaSuresi = _HurdaSuresiHesapla(tempOperationLines);
                    //decimal planlananOperasyonSuresi = _PlanlananOperasyonSuresiHesapla(tempOperationLines);
                    
                    decimal kull = (decimal)(tempOperationLines.Sum(t => t.OPERASYONSURESI) / (decimal)(tempCalendarLines.Where(c => c.CALISMADURUMU == "ÇALIŞMA VAR" && c.VERITOPLAMA == true && c.PLANLANAN == "Hayır").Sum(c => c.TOPLAMCALISABILIRSURE)));
                    decimal perf = tempOperationLines.Sum(t => t.PLANLANANOPRSURESI) / tempOperationLines.Sum(t => t.BIRIMSURE);
                    decimal kalite = ((((tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)) - (tempUnsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN) * tempOperationLines.Sum(t => t.BIRIMSURE)))) / (tempOperationLines.Sum(t => t.URETILENADET) * tempOperationLines.Sum(t => t.BIRIMSURE)));

                    StationOEEAnalysis analysis = new StationOEEAnalysis
                    {
                        StationID = stationID,
                        Code = operationLines.Where(t => t.ISTASYONID == stationID).Select(t => t.MAKINEKODU).FirstOrDefault(),
                        Availability = kull,
                        Performance = perf,
                        Quality = kalite,
                        OEE = kull * perf * kalite,
                        //ShiftTime = vardiyaCalismaSuresi,
                        //PlannedOperationTime = planlananOperasyonSuresi,
                        //OccuredOperationTime = gerceklesenOperasyonSuresi,
                        //ScrapTime = hurdaSuresi,
                        //HaltTime = haltLines.Where(t => t.ISTASYONID == stationID).Sum(t => t.DURUSSURE),
                        //Availability = vardiyaCalismaSuresi > 0 && gerceklesenOperasyonSuresi > 0 ? gerceklesenOperasyonSuresi / vardiyaCalismaSuresi : 0,
                        //Performance = planlananOperasyonSuresi > 0 && gerceklesenOperasyonSuresi > 0 ? planlananOperasyonSuresi / gerceklesenOperasyonSuresi : 0,
                        //Quality = gerceklesenOperasyonSuresi > 0 ? (gerceklesenOperasyonSuresi - hurdaSuresi) / gerceklesenOperasyonSuresi : 0,
                        //OEE = (gerceklesenOperasyonSuresi / vardiyaCalismaSuresi) * (planlananOperasyonSuresi / gerceklesenOperasyonSuresi) * ((gerceklesenOperasyonSuresi - hurdaSuresi) / gerceklesenOperasyonSuresi),
                        Department = tempOperationLines.Select(t => t.DEPARTMAN).FirstOrDefault()
                    };
                    stationOEEAnalysis.Add(analysis);
                }
            }
            return stationOEEAnalysis;
        }

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
