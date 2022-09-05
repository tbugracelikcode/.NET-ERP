using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class FasonUygunsuzlukService
    {
        SqlConnection _connection;
        public FasonUygunsuzlukService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<AdminContractUnsuitabilityAnalysisChart> GetContractUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminContractUnsuitabilityAnalysisChart> adminContractUnsuitabilityChart = new List<AdminContractUnsuitabilityAnalysisChart>();
            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);


            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminContractUnsuitabilityAnalysisChart
                {
                    Ay = GetMonth(t.Key.Ay),
                    Total = t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR) + t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) + t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR)
                }).ToList();
                adminContractUnsuitabilityChart = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminContractUnsuitabilityAnalysisChart
                {
                    Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                    Total = t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR) + t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) + t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR)
                }).ToList();
                adminContractUnsuitabilityChart = gList;
            }

            return adminContractUnsuitabilityChart;

        }

        public List<ContractUnsuitabilityAnalysis> GetContractUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ContractUnsuitabilityAnalysis> contractUnsuitabilityAnalysis = new List<ContractUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);
            var list = unsuitabilityLines.Select(t => t.HATAID).Distinct().ToList();
            //var deneme = DBHelper.GetContractCauses();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in list)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    ContractUnsuitabilityAnalysis analysis = new ContractUnsuitabilityAnalysis
                    {
                        ContractUnsuitabilityID = unsuitabilityLines.Where(t => t.HATAID == unsuitability).Select(t=>t.ID).FirstOrDefault(),
                        ScrapQuantity = scrap,
                        ToBeUsedAs = tobeused,
                        Correction = correction,
                        Total = scrap + tobeused + correction,
                        UnsuitabilityReason = unsuitabilityLines.Where(t=>t.HATAID == unsuitability).Select(t=>t.HATAACIKLAMA).FirstOrDefault(),
                        ErrorID = unsuitability

                    };
                    contractUnsuitabilityAnalysis.Add(analysis);
                }
            }
            return contractUnsuitabilityAnalysis;
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
