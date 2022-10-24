using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using System.Globalization;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class StokService : IStokService
    {
        

        #region Chart

        public async Task< List<AdminProductChart>> GetProductChart(DateTime startDate, DateTime endDate, int frequency, int? productionSelection)
        {
            List<AdminProductChart> adminProductChart = new List<AdminProductChart>();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            Tuple<int, int> tuple = _PlanlananAdetHesapla(productionSelection, operationLines);

            #region Değişkenler

            double previousMonthScrapPercent = 0;
            int hurda = 0;
            decimal uretilenadet = 0;
            double scrappercent = 0;

            #endregion

            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.Where(t => t.URUNGRPID == productionSelection).OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                {

                    #region Değişkenler

                    previousMonthScrapPercent = scrappercent;

                    hurda = unsuitabilityLines.Where(x => x.URUNGRUPID == productionSelection && x.HURDA == true && x.TARIH.Month == t.Key.Ay).Sum(x => x.OLCUKONTROLFORMBEYAN);

                    uretilenadet = t.Sum(t => t.URETILENADET);

                    scrappercent = hurda / (double)uretilenadet;

                    decimal differenceScrapPercent = (decimal)(scrappercent - previousMonthScrapPercent);

                    #endregion



                    return new AdminProductChart
                    {
                        Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                        OEE = t.Average(x => x.OEE),
                        ScrapPercent = scrappercent,
                        DIFFSCRAPPERCENT = differenceScrapPercent,
                        PRODUCTION = uretilenadet,
                        SCRAP = hurda
                    };
                }).ToList();
                adminProductChart = gList;
            }

            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => 
                {

                    #region Değişkenler

                    previousMonthScrapPercent = scrappercent;

                    hurda = unsuitabilityLines.Where(x => x.URUNGRUPID == productionSelection && x.HURDA == true && x.TARIH.Date == t.Key.HAFTA).Sum(x => x.OLCUKONTROLFORMBEYAN);

                    uretilenadet = t.Sum(t => t.URETILENADET);

                    scrappercent = hurda / (double)uretilenadet;

                    decimal differenceScrapPercent = (decimal)(scrappercent - previousMonthScrapPercent);

                    #endregion

                    return new AdminProductChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                        OEE = t.Average(x => x.OEE),
                        ScrapPercent = scrappercent,
                        DIFFSCRAPPERCENT = differenceScrapPercent,
                        PRODUCTION = uretilenadet,
                        SCRAP = hurda
                    };

                    
                }).ToList();
                adminProductChart = gList;
            }

            return await Task.FromResult(adminProductChart);

        }

        #endregion

        #region Grid

        public async Task< List<ProductGroupsAnalysis>> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate)
        {

            List<ProductGroupsAnalysis> productGroupsAnalysis = new List<ProductGroupsAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var groupList = operationLines.Select(t => t.URUNGRPID).Distinct().ToList();

            if (groupList != null)
            {
                foreach (var groupID in groupList)
                {
                    var tempUnsuitability = unsuitabilityLines.Where(t => t.URUNGRUPID == groupID).ToList();
                    Tuple<int, int> tuple = _PlanlananAdetHesapla(groupID, operationLines);

                    #region Değişkenler

                    string productGroupName = operationLines.Where(t => t.URUNGRPID == groupID).Select(t => t.URUNGRUBU).FirstOrDefault();
                    int totalScrap = Convert.ToInt32(tempUnsuitability.Where(t=>t.HURDA==true).Sum(t => t.OLCUKONTROLFORMBEYAN));

                    #endregion

                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = productGroupName,
                        PlannedQuantity = tuple.Item1,
                        TotalProduction = tuple.Item2,
                        TotalScrap = totalScrap,
                        Quality = tuple.Item1 > 0 && tuple.Item2 > 0 ? ((double)tuple.Item2 / (double)tuple.Item1) : 0
                    };
                    productGroupsAnalysis.Add(analysis);
                }
            }
            return await Task.FromResult(productGroupsAnalysis);
        }

        #endregion

        #region Combobox

        public async Task< List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate)
        {

            List<ProductGroupsAnalysis> productGroupsAnalysis = new List<ProductGroupsAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate).Where(t=>t.STOKTURU == 12).ToList();

            var groupList = operationLines.Select(t => t.URUNGRPID).Distinct().ToList();

            if (groupList != null)
            {
                foreach (var groupID in groupList)
                {
                    Tuple<int, int> tuple = _PlanlananAdetHesapla(groupID, operationLines);

                    #region Değişkenler

                    string productGroupName = operationLines.Where(t => t.URUNGRPID == groupID).Select(t => t.URUNGRUBU).FirstOrDefault();
                    int totalScrap = Convert.ToInt32(operationLines.Where(t => t.URUNGRPID == groupID).Sum(t => t.HURDAADET));

                    #endregion

                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = productGroupName,
                        PlannedQuantity = tuple.Item1,
                        TotalProduction = tuple.Item2,
                        TotalScrap = totalScrap,
                        Quality = tuple.Item1 > 0 && tuple.Item2 > 0 ? ((double)tuple.Item2 / (double)tuple.Item1) : 0
                    };
                    productGroupsAnalysis.Add(analysis);
                }
            }
            return await Task.FromResult(productGroupsAnalysis);
        }

        #endregion

        private Tuple<int, int> _PlanlananAdetHesapla(int? urunGrupID, List<OperasyonSatir> opr)
        {

            int planlananAdet = 0;
            int gerceklesenAdet = 0;           

            var uretimEmriList = opr.Where(t=>t.URUNGRPID == urunGrupID).Select(t=>t.URETIMEMRIID).Distinct().ToList();

            for (int i = 0; i < uretimEmriList.Count; i++)
            {
                int ueEmriID = uretimEmriList[i];
                planlananAdet += Convert.ToInt32(opr.Where(t => t.URETIMEMRIID == ueEmriID).Select(t => t.PLNMIKTAR).FirstOrDefault());
                gerceklesenAdet += Convert.ToInt32(opr.Where(t => t.URETIMEMRIID == ueEmriID).Select(t => t.GRCMIKTAR).FirstOrDefault());
            }

            return Tuple.Create(planlananAdet, gerceklesenAdet);

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
