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
            var total = unsuitabilityLines.Sum(t => t.UYGUNOLMAYANMIKTAR);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in customerList)
                {
                    var refuse = unsuitabilityLines.Where(t => t.RED == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var contact = unsuitabilityLines.Where(t => t.TEDARIKCIIRTIBAT == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);

                    SupplierUnsuitabilityDetailedCustomer analysis = new SupplierUnsuitabilityDetailedCustomer
                    {
                        Customer = unsuitabilityLines.Where(t => t.CARIID == unsuitability).Select(t => t.CARIUNVAN).FirstOrDefault(),
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
            var total = unsuitabilityLines.Sum(t => t.UYGUNOLMAYANMIKTAR);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in productList)
                {
                    var refuse = unsuitabilityLines.Where(t => t.RED == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var contact = unsuitabilityLines.Where(t => t.TEDARIKCIIRTIBAT == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);

                    SupplierUnsuitabilityDetailedProduct analysis = new SupplierUnsuitabilityDetailedProduct
                    {
                        ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
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
