using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class FasonUygunsuzlukDetayService
    {
        SqlConnection _connection;
        public FasonUygunsuzlukDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Müşteriye Göre Analiz
        public List<ContractUnsuitabilityDetailedCustomer> GetContractUnsuitabilityDetailedCustomerAnalysis(int errorID, DateTime startDate, DateTime endDate)
        {

            List<ContractUnsuitabilityDetailedCustomer> contractUnsuitabilityDetailedCustomerAnalysis = new List<ContractUnsuitabilityDetailedCustomer>();

            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate).Where(t => t.HATAID == errorID).ToList();
            var customerList = unsuitabilityLines.Select(t => t.CARIID).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in customerList)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);

                    ContractUnsuitabilityDetailedCustomer analysis = new ContractUnsuitabilityDetailedCustomer
                    {
                        Customer = unsuitabilityLines.Where(t => t.CARIID == unsuitability).Select(t => t.CARIUNVAN).FirstOrDefault(),
                        Quantity = scrap + tobeused + correction
                    };
                    if (analysis.Quantity > 0)
                    {
                        contractUnsuitabilityDetailedCustomerAnalysis.Add(analysis);
                    }
                }
            }
            return contractUnsuitabilityDetailedCustomerAnalysis;
        }
        #endregion

        #region Stoğa Göre Analiz
        public List<ContractUnsuitabilityDetailedProduct> GetContractUnsuitabilityDetailedProductAnalysis(int errorID, DateTime startDate, DateTime endDate)
        {

            List<ContractUnsuitabilityDetailedProduct> contractUnsuitabilityDetailedProductAnalysis = new List<ContractUnsuitabilityDetailedProduct>();

            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate).Where(t => t.HATAID == errorID).ToList();
            var productList = unsuitabilityLines.Select(t => t.STOKID).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in productList)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);

                    ContractUnsuitabilityDetailedProduct analysis = new ContractUnsuitabilityDetailedProduct
                    {
                        ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
                        Quantity = scrap + tobeused + correction
                    };
                    if (analysis.Quantity > 0)
                    {
                        contractUnsuitabilityDetailedProductAnalysis.Add(analysis);
                    }
                }
            }
            return contractUnsuitabilityDetailedProductAnalysis;
        }
        #endregion
    }
}
