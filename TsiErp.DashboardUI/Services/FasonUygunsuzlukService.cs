using Microsoft.Data.SqlClient;
using Syncfusion.Blazor.Kanban.Internal;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class FasonUygunsuzlukService : IFasonUygunsuzlukService
    {


        #region Grid-Chart

        public async Task<List<ContractUnsuitabilityAnalysis>> GetContractUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ContractUnsuitabilityAnalysis> contractUnsuitabilityAnalysis = new List<ContractUnsuitabilityAnalysis>();

            var purchaseList = DBHelper.GetContractorsQuery(startDate, endDate).Where(t => t.DURUM == 3 || t.DURUM == 4 || t.DURUM == 6);
            var purchaseLinesList = DBHelper.GetPurchaseLinesQuery(startDate, endDate);
            var generalList = DBHelper.GetContractUnsuitabilityQueryGeneral(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);
            var contractList = purchaseList.Select(t => t.CARIID).Distinct().ToList();

            if (purchaseList != null)
            {
                foreach (var contractSupplierID in contractList)
                {
                    #region Değişkenler
                    var tempPurchaseList = purchaseList.Where(t => t.CARIID == contractSupplierID).ToList();
                    int receipt = Convert.ToInt32(tempPurchaseList.Sum(t => t.ADET));
                    var tempPurchaseLinesList = purchaseLinesList.Where(t => t.CARIUNVAN == tempPurchaseList.Where(t => t.CARIID == contractSupplierID).Select(t => t.CARIUNVAN).FirstOrDefault()).ToList();

                    int total = generalList.Where(t => t.CariID == contractSupplierID).Sum(t => t.Miktar);
                    //int receipt = generalList.Where(t => t.CariID == contractSupplierID).Sum(t => t.FasonFisiAdeti);
                    string supplierTitle = tempPurchaseList.Where(t => t.CARIID == contractSupplierID).Select(t => t.CARIUNVAN).FirstOrDefault();
                    int productionOrderID = unsuitabilityLines.Where(t => t.CARIID == contractSupplierID).Select(t => t.URETIMEMRIID).FirstOrDefault();

                    #endregion

                    ContractUnsuitabilityAnalysis analysis = new ContractUnsuitabilityAnalysis
                    {
                        ContractSupplierID = contractSupplierID,
                        ContractSupplier = supplierTitle,
                        Total = total,
                        ContractReceiptQuantity = receipt,
                        ProductionOrderID = productionOrderID,
                        Percent = (double)total / (double)receipt
                    };
                    //if(analysis.Total > 0)
                    //{
                    contractUnsuitabilityAnalysis.Add(analysis);
                    //}
                }
            }

            contractUnsuitabilityAnalysis = contractUnsuitabilityAnalysis.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(contractUnsuitabilityAnalysis);
        }

        #endregion

    }
}
