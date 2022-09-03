using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class UretimUygunsuzlukDetayService
    {
        SqlConnection _connection;
        public UretimUygunsuzlukDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region İstasyona Göre Analiz
        public List<ProductionUnsuitabilityDetailedStation> GetProductionUnsuitabilityDetailedStationAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate)
        {

            List<ProductionUnsuitabilityDetailedStation> productionUnsuitabilityDetailedStationAnalysis = new List<ProductionUnsuitabilityDetailedStation>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery( startDate, endDate).Where(t=>t.KOD == unsuitabilityCode).ToList();
            var stationList = DBHelper.GetStations();

            if(unsuitabilityLines != null)
            {
                foreach (var station in stationList)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                    ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                    {
                         Station = station.MAKINEKODU,
                          Quantity = scrap + tobeused+ correction
                    };
                    if (analysis.Quantity > 0 )
                    {
                        productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                    }
                }
            }
            return productionUnsuitabilityDetailedStationAnalysis;
        }
        #endregion

        #region Çalışana Göre Analiz
        public List<ProductionUnsuitabilityDetailedEmployee> GetProductionUnsuitabilityDetailedEmployeeAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate)
        {

            List<ProductionUnsuitabilityDetailedEmployee> productionUnsuitabilityDetailedEmployeeAnalysis = new List<ProductionUnsuitabilityDetailedEmployee>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in unsuitabilityLines)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CALISANID == unsuitability.CALISANID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CALISANID == unsuitability.CALISANID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CALISANID == unsuitability.CALISANID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                    ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                    {
                        EmployeeName = unsuitability.CALISANAD,
                        Quantity = scrap + tobeused + correction
                    };
                    if (analysis.Quantity > 0)
                    {
                        productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                    }
                }
            }
            return productionUnsuitabilityDetailedEmployeeAnalysis;
        }
        #endregion

        #region Stoğa Göre Analiz
        public List<ProductionUnsuitabilityDetailedProduct> GetProductionUnsuitabilityDetailedProductAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate)
        {

            List<ProductionUnsuitabilityDetailedProduct> productionUnsuitabilityDetailedProductAnalysis = new List<ProductionUnsuitabilityDetailedProduct>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in unsuitabilityLines)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability.STOKID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability.STOKID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability.STOKID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                    ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                    {
                        ProductCode = unsuitability.STOKACIKLAMASI,
                        Quantity = scrap + tobeused + correction
                    };
                    if (analysis.Quantity > 0)
                    {
                        productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                    }
                }
            }
            return productionUnsuitabilityDetailedProductAnalysis;
        }
        #endregion

    }
}
