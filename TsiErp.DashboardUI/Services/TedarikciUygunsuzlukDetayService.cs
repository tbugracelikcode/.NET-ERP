using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class TedarikciUygunsuzlukDetayService
    {
        SqlConnection _connection;
        public TedarikciUygunsuzlukDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Müşteriye Göre Analiz

        #region Chart-Grid

        public List<SupplierUnsuitabilityDetailedCustomer> GetSupplierUnsuitabilityDetailedCustomerAnalysis(int errorID, DateTime startDate, DateTime endDate)
        {

            List<SupplierUnsuitabilityDetailedCustomer> supplierUnsuitabilityDetailedCustomerAnalysis = new List<SupplierUnsuitabilityDetailedCustomer>();

            var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate).Where(t => t.HATAID == errorID).ToList();
            var customerList = unsuitabilityLines.Select(t => t.CARIID).Distinct().ToList();
            int total = unsuitabilityLines.Sum(t => t.UYGUNOLMAYANMIKTAR);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in customerList)
                {
                    #region Değişkenler

                    int refuse = unsuitabilityLines.Where(t => t.RED == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int contact = unsuitabilityLines.Where(t => t.TEDARIKCIIRTIBAT == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    string customer = unsuitabilityLines.Where(t => t.CARIID == unsuitability).Select(t => t.CARIUNVAN).FirstOrDefault();

                    #endregion

                    SupplierUnsuitabilityDetailedCustomer analysis = new SupplierUnsuitabilityDetailedCustomer
                    {
                        Customer = customer,
                        Quantity = contact + tobeused + correction+refuse,
                        Percent = (double)(contact + tobeused + correction + refuse) / (double)total
                    };
                    if (analysis.Quantity > 0)
                    {
                        supplierUnsuitabilityDetailedCustomerAnalysis.Add(analysis);
                    }
                }
            }
            return supplierUnsuitabilityDetailedCustomerAnalysis;
        }

        #endregion

        #endregion

        #region Stoğa Göre Analiz

        #region Chart-Grid
        public List<SupplierUnsuitabilityDetailedProduct> GetSupplierUnsuitabilityDetailedProductAnalysis(int errorID, DateTime startDate, DateTime endDate)
        {

            List<SupplierUnsuitabilityDetailedProduct> supplierUnsuitabilityDetailedProductAnalysis = new List<SupplierUnsuitabilityDetailedProduct>();

            var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate).Where(t => t.HATAID == errorID).ToList();
            var productList = unsuitabilityLines.Select(t => t.STOKID).Distinct().ToList();
            int total = unsuitabilityLines.Sum(t => t.UYGUNOLMAYANMIKTAR);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in productList)
                {
                    #region Değişkenler

                    int refuse = unsuitabilityLines.Where(t => t.RED == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int contact = unsuitabilityLines.Where(t => t.TEDARIKCIIRTIBAT == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    string productCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault();

                    #endregion

                    SupplierUnsuitabilityDetailedProduct analysis = new SupplierUnsuitabilityDetailedProduct
                    {
                        ProductCode = productCode,
                        Quantity = contact + tobeused + correction,
                        Percent = (double)(contact + tobeused + correction + refuse) / (double)total
                    };
                    if (analysis.Quantity > 0)
                    {
                        supplierUnsuitabilityDetailedProductAnalysis.Add(analysis);
                    }
                }
            }
            return supplierUnsuitabilityDetailedProductAnalysis;
        }

        #endregion

        #endregion
    }
}
