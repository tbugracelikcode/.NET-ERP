using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class StokDetayService : IStokDetayService
    {
       

        #region Hurda Analizi

        #region Chart

        public async Task< List<ProductGroupDetailedChart>> GetProductGroupDetailedtChart(int productgroupID, DateTime startDate, DateTime endDate, int products)
        {


            List<ProductGroupDetailedChart> productgroupDetailedChart = new List<ProductGroupDetailedChart>();

            var scrapLines = DBHelper.GetScrapLinesGroupedQuery(productgroupID, startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var causeList = DBHelper.GetScrapCauses();
            int totalProduction = (int)operationLines.Where(t => t.URUNGRPID == productgroupID).Sum(t => t.URETILENADET);

            foreach (var item in causeList)
            {
                #region Değişkenler

                int causeID = item.ID;
                int totalScrap = scrapLines.Where(t => t.HURDAID == causeID).Sum(t => t.HURDAADET);

                #endregion

                ProductGroupDetailedChart analysis = new ProductGroupDetailedChart
                {
                    ScrapCauseName = item.BASLIK,
                    TotalScrap = totalScrap,
                    TotalProduction = totalProduction,
                    PPM = (totalScrap > 0 && totalProduction > 0 ? ((Convert.ToDecimal(totalScrap) / Convert.ToDecimal(totalProduction)) * 1000000) : 0)
                };
                if (analysis.PPM > 0)
                {
                    productgroupDetailedChart.Add(analysis);
                }
                
            }

            productgroupDetailedChart = productgroupDetailedChart.OrderByDescending(t => t.PPM).ToList();
            return await Task.FromResult(productgroupDetailedChart);
        }

        #endregion

        #region Grid

        public async Task< List<ProductScrapAnalysis>> GetProductScrapAnalysis(int groupID, DateTime startDate, DateTime endDate)
        {
            List<ProductScrapAnalysis> productScrapAnalysis = new List<ProductScrapAnalysis>();

            var scrapLines = DBHelper.GetScrapLinesGroupedQuery(groupID, startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var causeList = DBHelper.GetScrapCauses();
            int totalProduction = (int)operationLines.Where(t => t.URUNGRPID == groupID).Sum(t => t.URETILENADET);

            if (causeList != null)
            {
                foreach (var item in causeList)
                {
                    #region Değişkenler

                    int causeID = item.ID;
                    int totalScrap = scrapLines.Where(t => t.HURDAID == causeID).Sum(t => t.HURDAADET);
                    string productGroupName = scrapLines.Where(t => t.HURDAID == causeID).Select(t => t.URUNGRUBU).FirstOrDefault();

                    #endregion

                    ProductScrapAnalysis analysis = new ProductScrapAnalysis
                    {
                        ScrapCauseID = causeID,
                        ScrapCauseName = item.BASLIK,
                        TotalScrap = totalScrap,
                        TotalProduction = totalProduction,
                        PPM = totalScrap > 0 && totalProduction > 0 ? ((Convert.ToDecimal(totalScrap) / Convert.ToDecimal(totalProduction)) * 1000000) : 0,
                        ProductGroupName = productGroupName
                    };
                    productScrapAnalysis.Add(analysis);
                }
            }

            return await Task.FromResult(productScrapAnalysis.OrderByDescending(t => t.PPM).ToList());
        }

        #endregion


        #endregion

    }
}
