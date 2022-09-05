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

        public List<AdminProductionUnsuitabilityAnalysisChart> GetProductionUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminProductionUnsuitabilityAnalysisChart> adminProductionUnsuitabilityChart = new List<AdminProductionUnsuitabilityAnalysisChart>();
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);


            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                {
                    Ay = GetMonth(t.Key.Ay),
                    Total = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN) + t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN) + t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN)
                }).ToList();
                adminProductionUnsuitabilityChart = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductionUnsuitabilityAnalysisChart
                {
                    Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                    Total = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN) + t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN) + t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN)
                }).ToList();
                adminProductionUnsuitabilityChart = gList;
            }

            return adminProductionUnsuitabilityChart;

        }

        public List<ProductionUnsuitabilityAnalysis> GetProductionUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ProductionUnsuitabilityAnalysis> productionUnsuitabilityAnalysis = new List<ProductionUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in unsuitabilityLines)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.HATAACIKLAMA == unsuitability.HATAACIKLAMA).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.HATAACIKLAMA == unsuitability.HATAACIKLAMA).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.HATAACIKLAMA == unsuitability.HATAACIKLAMA).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    ProductionUnsuitabilityAnalysis analysis = new ProductionUnsuitabilityAnalysis
                    {
                        ProductionUnsuitabilityID = unsuitability.ID,
                        ScrapQuantity = scrap,
                        ToBeUsedAs = tobeused,
                        Correction = correction,
                        Total = scrap + tobeused + correction,
                        UnsuitabilityReason = unsuitability.HATAACIKLAMA,
                         Code = unsuitability.KOD

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
