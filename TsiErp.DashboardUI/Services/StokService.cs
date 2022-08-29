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

        public List<ProductGroupsAnalysis> GetProductGroupsAnalysis(DateTime startDate, DateTime endDate)
        {
            startDate = new DateTime(2022, 06, 01);
            endDate = new DateTime(2022, 08, 22);

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
    }
}
