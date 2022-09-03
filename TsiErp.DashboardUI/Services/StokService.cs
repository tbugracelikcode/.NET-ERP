using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class StokService
    {
        SqlConnection _connection;
        public StokService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<AdminProductChart> GetProductChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminProductChart> adminProductChart = new List<AdminProductChart>();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);



            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminProductChart
                {
                    Ay = GetMonth(t.Key.Ay),
                    Quality = t.Average(x => x.KALITE)
                }).ToList();
                adminProductChart = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminProductChart
                {
                    Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                    Quality = t.Average(x => x.KALITE)
                }).ToList();
                adminProductChart = gList;
            }

            return adminProductChart;

        }

        public List<ProductGroupsAnalysis> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate)
        {

            List<ProductGroupsAnalysis> productGroupsAnalysis = new List<ProductGroupsAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            var groupList = operationLines.Select(t => t.URUNGRPID).Distinct().ToList();
            if (groupList != null)
            {
                foreach (var groupID in groupList)
                {
                    Tuple<int, int> tuple = _PlanlananAdetHesapla(groupID, operationLines);
                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = operationLines.Where(t => t.URUNGRPID == groupID).Select(t => t.URUNGRUBU).FirstOrDefault(),
                        PlannedQuantity = tuple.Item1,
                        TotalProduction = tuple.Item2,
                        TotalScrap = Convert.ToInt32(operationLines.Where(t => t.URUNGRPID == groupID).Sum(t => t.HURDAADET)),
                        Quality = tuple.Item1 > 0 && tuple.Item2 > 0 ? ((double)tuple.Item2 / (double)tuple.Item1) : 0
                };
                    productGroupsAnalysis.Add(analysis);
                }
            }
            return productGroupsAnalysis;
        }

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
