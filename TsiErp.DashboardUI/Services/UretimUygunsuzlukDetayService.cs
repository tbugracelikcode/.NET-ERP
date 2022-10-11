using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class UretimUygunsuzlukDetayService : IUretimUygunsuzlukDetayService
    {
        SqlConnection _connection;
        public UretimUygunsuzlukDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region İstasyona Göre Analiz

        #region Chart-Grid
        public async Task< List<ProductionUnsuitabilityDetailedStation>> GetProductionUnsuitabilityDetailedStationAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedStation> productionUnsuitabilityDetailedStationAnalysis = new List<ProductionUnsuitabilityDetailedStation>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery( startDate, endDate).Where(t=>t.KOD == unsuitabilityCode && t.ISTVERIMLILIIKANALIZI == true).ToList();
            var stationList = DBHelper.GetStations();
            int total = (int)unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if(unsuitabilityLines != null)
            {
                switch(selectedActionID)
                {
                    #region Hurda

                    case 1:

                        foreach (var station in stationList)
                        {
                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

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
                        break;

                    #endregion

                    #region Düzeltme

                    case 2:

                        foreach (var station in stationList)
                        {

                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

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
                        break;

                    #endregion

                    #region Olduğu Gibi Kullanılacak

                    case 3:

                        foreach (var station in stationList)
                        {
                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            ProductionUnsuitabilityDetailedStation analysis = new ProductionUnsuitabilityDetailedStation
                            {
                                Station = station.MAKINEKODU,
                                Quantity = tobeused,
                                Percent = (double)(tobeused) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedStationAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Toplam Uygunsuzluk

                    case 4:

                        foreach (var station in stationList)
                        {
                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.ISTASYONID == station.ID).Sum(t => t.OLCUKONTROLFORMBEYAN);

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
                        break;

                    #endregion

                    default: break;
                }

            }

            productionUnsuitabilityDetailedStationAnalysis = productionUnsuitabilityDetailedStationAnalysis.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(productionUnsuitabilityDetailedStationAnalysis);
        }

        #endregion

        #endregion

        #region Çalışana Göre Analiz

        #region Chart-Grid
        public async Task< List<ProductionUnsuitabilityDetailedEmployee>> GetProductionUnsuitabilityDetailedEmployeeAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedEmployee> productionUnsuitabilityDetailedEmployeeAnalysis = new List<ProductionUnsuitabilityDetailedEmployee>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode && t.PERVERIMLILIKANALIZI == true).ToList();
            var employeeList = unsuitabilityLines.Select(t => t.CALISANID).Distinct().ToList();
            int total = unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if (unsuitabilityLines != null)
            {
                switch(selectedActionID)
                {
                    #region Hurda

                    case 1:

                        foreach (var unsuitability in employeeList)
                        {
                            #region Değişkenler

                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string employeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                            {
                                EmployeeName = employeeName,
                                Quantity = scrap,
                                Percent = (double)(scrap) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Düzeltme

                    case 2:

                        foreach (var unsuitability in employeeList)
                        {
                            #region Değişkenler

                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string employeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                            {
                                EmployeeName = employeeName,
                                Quantity = correction,
                                Percent = (double)(correction) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Olduğu Gibi Kullanılacak

                    case 3:

                        foreach (var unsuitability in employeeList)
                        {
                            #region Değişkenler

                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string employeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                            {
                                EmployeeName = employeeName,
                                Quantity = tobeused,
                                Percent = (double)(tobeused) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Toplam Uygunsuzluk

                    case 4:

                        foreach (var unsuitability in employeeList)
                        {
                            #region Değişkenler

                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CALISANID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string employeeName = unsuitabilityLines.Where(t => t.CALISANID == unsuitability).Select(t => t.CALISANAD).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedEmployee analysis = new ProductionUnsuitabilityDetailedEmployee
                            {
                                EmployeeName = employeeName,
                                Quantity = scrap + tobeused + correction,
                                Percent = (double)(scrap + tobeused + correction) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedEmployeeAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    default: break;
                }
            }

            productionUnsuitabilityDetailedEmployeeAnalysis = productionUnsuitabilityDetailedEmployeeAnalysis.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(productionUnsuitabilityDetailedEmployeeAnalysis);
        }

        #endregion

        #endregion

        #region Stoğa Göre Analiz

        #region Chart-Grid

        public async Task< List<ProductionUnsuitabilityDetailedProduct>> GetProductionUnsuitabilityDetailedProductAnalysis(string unsuitabilityCode, DateTime startDate, DateTime endDate, int selectedActionID)
        {

            List<ProductionUnsuitabilityDetailedProduct> productionUnsuitabilityDetailedProductAnalysis = new List<ProductionUnsuitabilityDetailedProduct>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate).Where(t => t.KOD == unsuitabilityCode).Distinct().ToList();
            var productList = unsuitabilityLines.Select(t => t.STOKID).Distinct().ToList();
            int total = unsuitabilityLines.Sum(t => t.OLCUKONTROLFORMBEYAN);

            if (unsuitabilityLines != null)
            {
                switch (selectedActionID)
                {
                    #region Hurda

                    case 1:

                        foreach (var unsuitability in productList)
                        {
                            #region Değişkenler

                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string productCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                            {
                                ProductCode = productCode,
                                Quantity = scrap,
                                Percent = (double)(scrap) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Düzeltme

                    case 2:

                        foreach (var unsuitability in productList)
                        {
                            #region Değişkenler

                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string productCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                            {
                                ProductCode = productCode,
                                Quantity = correction,
                                Percent = (double)(correction) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Olduğu Gibi Kullanılacak

                    case 3:

                        foreach (var unsuitability in productList)
                        {
                            #region Değişkenler

                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string productCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                            {
                                ProductCode = productCode,
                                Quantity = tobeused,
                                Percent = (double)(tobeused) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    #region Toplam Uygunsuzluk

                    case 4:

                        foreach (var unsuitability in productList)
                        {
                            #region Değişkenler

                            int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.STOKID == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                            string productCode = unsuitabilityLines.Where(t => t.STOKID == unsuitability).Select(t => t.STOKACIKLAMASI).FirstOrDefault();

                            #endregion

                            ProductionUnsuitabilityDetailedProduct analysis = new ProductionUnsuitabilityDetailedProduct
                            {
                                ProductCode = productCode,
                                Quantity = scrap + tobeused + correction,
                                Percent = (double)(scrap + tobeused + correction) / (double)total
                            };
                            if (analysis.Quantity > 0)
                            {
                                productionUnsuitabilityDetailedProductAnalysis.Add(analysis);
                            }
                        }
                        break;

                    #endregion

                    default: break;
                }

            }

            productionUnsuitabilityDetailedProductAnalysis = productionUnsuitabilityDetailedProductAnalysis.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(productionUnsuitabilityDetailedProductAnalysis);
        }

        #endregion

        #endregion

    }
}
