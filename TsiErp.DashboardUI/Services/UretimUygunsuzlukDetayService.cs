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
        public List<ProductionUnsuitabilityDetailedStation> GetProductionUnsuitabilityDetailedStationAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedStation> productionUnsuitabilityDetailedStationAnalysis = new List<ProductionUnsuitabilityDetailedStation>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery( startDate, endDate).Where(t=>t.KOD == unsuitabilityCode).ToList();
            var stationList = DBHelper.GetStations();
            var total = (int)unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if(unsuitabilityLines != null)
            {
                if(selectedActionID == 1)
                {
                    foreach (var station in stationList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                        {
                            Station = station.MAKINEKODU,
                            Quantity = scrap,
                            Percent = (double)(scrap) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                        }
                    }
                }
                else if (selectedActionID == 2)
                {
                    foreach (var station in stationList)
                    {
                        
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                        {
                            Station = station.MAKINEKODU,
                            Quantity = correction,
                            Percent = (double)(correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                        }
                    }
                }
                else if (selectedActionID == 3)
                {
                    foreach (var station in stationList)
                    {
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                        {
                            Station = station.MAKINEKODU,
                            Quantity = tobeused ,
                            Percent = (double)(tobeused) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                        }
                    }
                }
                else if (selectedActionID == 4)
                {
                    foreach (var station in stationList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                        {
                            Station = station.MAKINEKODU,
                            Quantity = scrap + tobeused + correction,
                            Percent = (double)(scrap + tobeused + correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                        }
                    }
                }

            }
            return productionUnsuitabilityDetailedStationAnalysis;
        }
        #endregion

        #region Çalışana Göre Analiz
        public List<ProductionUnsuitabilityDetailedEmployee> GetProductionUnsuitabilityDetailedEmployeeAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedEmployee> productionUnsuitabilityDetailedEmployeeAnalysis = new List<ProductionUnsuitabilityDetailedEmployee>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode).ToList();
            var employeeList = unsuitabilityLines.Select(t => t.CALISANID).Distinct().ToList();
            var total = unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if (unsuitabilityLines != null)
            {
                if(selectedActionID == 1)
                {
                    foreach (var unsuitability in employeeList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                        {
                            EmployeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault(),
                            Quantity = scrap,
                            Percent = (double)(scrap) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                        }
                    }
                }
                else if (selectedActionID == 2)
                {
                    foreach (var unsuitability in employeeList)
                    {
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                        {
                            EmployeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault(),
                            Quantity =  correction,
                            Percent = (double)(correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                        }
                    }
                }
                if (selectedActionID == 3)
                {
                    foreach (var unsuitability in employeeList)
                    {
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                        {
                            EmployeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault(),
                            Quantity = tobeused ,
                            Percent = (double)(tobeused) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                        }
                    }
                }
                if (selectedActionID == 4)
                {
                    foreach (var unsuitability in employeeList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                        {
                            EmployeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault(),
                            Quantity = scrap + tobeused + correction,
                            Percent = (double)(scrap + tobeused + correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                        }
                    }
                }

            }
            return productionUnsuitabilityDetailedEmployeeAnalysis;
        }
        #endregion

        #region Stoğa Göre Analiz
        public List<ProductionUnsuitabilityDetailedProduct> GetProductionUnsuitabilityDetailedProductAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedProduct> productionUnsuitabilityDetailedProductAnalysis = new List<ProductionUnsuitabilityDetailedProduct>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode).Distinct().ToList();
            var productList = unsuitabilityLines.Select(t => t.STOKID).Distinct().ToList();
            var total = unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if (unsuitabilityLines != null)
            {
                if(selectedActionID == 1)
                {
                    foreach (var unsuitability in productList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                        {
                            ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
                            Quantity = scrap ,
                            Percent = (double)(scrap) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                        }
                    }
                }
                if (selectedActionID == 2)
                {
                    foreach (var unsuitability in productList)
                    {
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                        {
                            ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
                            Quantity =  correction,
                            Percent = (double)(correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                        }
                    }
                }
                if (selectedActionID == 3)
                {
                    foreach (var unsuitability in productList)
                    {
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                        {
                            ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
                            Quantity =  tobeused ,
                            Percent = (double)(tobeused) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                        }
                    }
                }
                if (selectedActionID == 4)
                {
                    foreach (var unsuitability in productList)
                    {
                        var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                        var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);

                        ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                        {
                            ProductCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault(),
                            Quantity = scrap + tobeused + correction,
                            Percent = (double)(scrap + tobeused + correction) / (double)total
                        };
                        if (analysis.Quantity > 0)
                        {
                            productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                        }
                    }
                }

            }
            return productionUnsuitabilityDetailedProductAnalysis;
        }
        #endregion

    }
}
