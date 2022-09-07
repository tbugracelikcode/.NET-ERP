using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class UretimUygunsuzlukService
    {
        SqlConnection _connection;
        public UretimUygunsuzlukService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<AdminProductionUnsuitabilityAnalysisChart> GetProductionUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action)
        {
            List<AdminProductionUnsuitabilityAnalysisChart> adminProductionUnsuitabilityChart = new List<AdminProductionUnsuitabilityAnalysisChart>();
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            if (action == 1) //Hurda
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN) ,
                        Percent = (decimal)((double)t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN)/ (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN) ,
                        Percent = (decimal)((double)t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
            }
            else if ( action == 2) // Düzeltme
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total =  t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN),
                        Percent = (decimal)((double)t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total =  t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN),
                        Percent = (decimal)((double)t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
            }
            else if ( action == 3) //Olduğu Gibi Kalacak
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN) ,
                        Percent = (decimal)((double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN),
                        Percent = (decimal)((double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
            }
            else if (action == 4) //Hepsini Göster
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Sum(t => t.OLCUKONTROLFORMBEYAN),
                        Percent = (decimal)((double)t.Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Sum(t => t.OLCUKONTROLFORMBEYAN),
                        Percent = (decimal)((double)t.Sum(t => t.OLCUKONTROLFORMBEYAN) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET))
                    }).ToList();
                    adminProductionUnsuitabilityChart = gList;
                }
            }



            return adminProductionUnsuitabilityChart;

        }

        public List<ProductionUnsuitabilityAnalysis> GetProductionUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ProductionUnsuitabilityAnalysis> productionUnsuitabilityAnalysis = new List<ProductionUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var list = unsuitabilityLines.Select(t => t.KOD).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in list)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    ProductionUnsuitabilityAnalysis analysis = new ProductionUnsuitabilityAnalysis
                    {
                        ProductionUnsuitabilityID = unsuitabilityLines.Where(t => t.KOD == unsuitability).Select(t => t.ID).FirstOrDefault(),
                        ScrapQuantity = scrap,
                        ToBeUsedAs = tobeused,
                        Correction = correction,
                        Total = scrap + tobeused + correction,
                        UnsuitabilityReason = unsuitabilityLines.Where(t=>t.KOD == unsuitability).Select(t=>t.HATAACIKLAMA).FirstOrDefault(),
                         Code = unsuitability

                    };
                    productionUnsuitabilityAnalysis.Add(analysis);
                }
            }
            return productionUnsuitabilityAnalysis;
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
    }
}
