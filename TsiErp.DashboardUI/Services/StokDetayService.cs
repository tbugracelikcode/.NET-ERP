using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class StokDetayService
    {
        SqlConnection _connection;
        public StokDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Hurda Analizi

        public List<ProductGroupDetailedChart> GetProductGroupDetailedtChart(int productgroupID, DateTime startDate, DateTime endDate, int products)
        {


            List<ProductGroupDetailedChart> productgroupDetailedChart = new List<ProductGroupDetailedChart>();

            var scrapLines = DBHelper.GetScrapLinesGroupedQuery(productgroupID, startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var causeList = DBHelper.GetScrapCauses();
            int totalProduction = (int)operationLines.Where(t => t.URUNGRPID == productgroupID).Sum(t => t.URETILENADET);
            foreach (var item in causeList)
            {
                int causeID = item.ID;
                int totalScrap = scrapLines.Where(t => t.HURDAID == causeID).Sum(t => t.HURDAADET);

                ProductGroupDetailedChart analysis = new ProductGroupDetailedChart
                {
                    ScrapCauseName = item.BASLIK,
                    TotalScrap = totalScrap,
                    TotalProduction = totalProduction,
                    PPM = (totalScrap > 0 && totalProduction > 0 ? ((Convert.ToDecimal(totalScrap) / Convert.ToDecimal(totalProduction)) * 1000000) : 0)/causeList.Count()
                };
                if (analysis.PPM > 0)
                {
                    productgroupDetailedChart.Add(analysis);
                }
                
            }
            return productgroupDetailedChart;
        }

        public List<ProductScrapAnalysis> GetProductScrapAnalysis(int groupID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //groupID = 55;

            List<ProductScrapAnalysis> productScrapAnalysis = new List<ProductScrapAnalysis>();

            var scrapLines = DBHelper.GetScrapLinesGroupedQuery(groupID, startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var causeList = DBHelper.GetScrapCauses();
            int totalProduction = (int)operationLines.Where(t => t.URUNGRPID == groupID).Sum(t => t.URETILENADET);
            if (causeList != null)
            {
                foreach (var item in causeList)
                {
                    int causeID = item.ID;
                    int totalScrap = scrapLines.Where(t => t.HURDAID == causeID).Sum(t => t.HURDAADET);

                    ProductScrapAnalysis analysis = new ProductScrapAnalysis
                    {
                        ScrapCauseID = causeID,
                        ScrapCauseName = item.BASLIK,
                        TotalScrap = totalScrap,
                        TotalProduction = totalProduction,
                        PPM = totalScrap > 0 && totalProduction > 0 ? ((Convert.ToDecimal(totalScrap) / Convert.ToDecimal(totalProduction)) * 1000000) : 0
                    };
                    productScrapAnalysis.Add(analysis);
                }
            }

            return productScrapAnalysis.OrderByDescending(t => t.PPM).ToList();
        }


        #endregion

    }
}
