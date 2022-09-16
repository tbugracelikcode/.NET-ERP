using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class FasonUygunsuzlukService : IFasonUygunsuzlukService
    {
        

        #region Grid-Chart

        public async Task< List<ContractUnsuitabilityAnalysis>> GetContractUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ContractUnsuitabilityAnalysis> contractUnsuitabilityAnalysis = new List<ContractUnsuitabilityAnalysis>();

            var generalList = DBHelper.GetContractUnsuitabilityQueryGeneral(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);
            var contractList = unsuitabilityLines.Select(t => t.CARIID).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var contractSupplierID in contractList)
                {
                    #region Değişkenler

                    int total = generalList.Where(t => t.CariID == contractSupplierID).Sum(t => t.Miktar);
                    int receipt = generalList.Where(t => t.CariID == contractSupplierID).Sum(t => t.FasonFisiAdeti);
                    string supplierTitle = unsuitabilityLines.Where(t => t.CARIID == contractSupplierID).Select(t => t.CARIUNVAN).FirstOrDefault();
                    int productionOrderID = unsuitabilityLines.Where(t => t.CARIID == contractSupplierID).Select(t => t.URETIMEMRIID).FirstOrDefault();

                    #endregion

                    ContractUnsuitabilityAnalysis analysis = new ContractUnsuitabilityAnalysis
                    {
                        ContractSupplierID = contractSupplierID,
                        ContractSupplier = supplierTitle,
                        Total =total,
                        ContractReceiptQuantity = receipt,
                        ProductionOrderID = productionOrderID,
                        Percent = (double)total/ (double)receipt
                    };
                    if(analysis.Total > 0)
                    {
                        contractUnsuitabilityAnalysis.Add(analysis);
                    }
                }
            }
            return await Task.FromResult(contractUnsuitabilityAnalysis);
        }

        #endregion

    }
}
