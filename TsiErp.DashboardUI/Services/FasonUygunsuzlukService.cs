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

        public List<ContractUnsuitabilityAnalysis> GetContractUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ContractUnsuitabilityAnalysis> contractUnsuitabilityAnalysis = new List<ContractUnsuitabilityAnalysis>();

            var generalList = DBHelper.GetContractUnsuitabilityQueryGeneral(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);
            var list = unsuitabilityLines.Select(t => t.HATAID).Distinct().ToList();
            var fasonList = unsuitabilityLines.Select(t => t.CARIID).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var fasontedarikci in fasonList)
                {
                    var total = generalList.Where(t => t.CariID == fasontedarikci).Sum(t => t.Miktar);
                    var receipt = generalList.Where(t => t.CariID == fasontedarikci).Sum(t => t.FasonFisiAdeti);

                    ContractUnsuitabilityAnalysis analysis = new ContractUnsuitabilityAnalysis
                    {
                        ContractSupplierID = fasontedarikci,
                        ContractSupplier = unsuitabilityLines.Where(t => t.CARIID == fasontedarikci).Select(t => t.CARIUNVAN).FirstOrDefault(),
                        Total =total,
                        ContractReceiptQuantity = receipt,
                        ProductionOrderID = unsuitabilityLines.Where(t => t.CARIID == fasontedarikci).Select(t => t.URETIMEMRIID).FirstOrDefault(),
                        Percent = (double)total/ (double)receipt
                    };
                    if(analysis.Total > 0)
                    {
                        contractUnsuitabilityAnalysis.Add(analysis);
                    }
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
