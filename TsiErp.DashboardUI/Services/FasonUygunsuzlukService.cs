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

    }
}
