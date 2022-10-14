using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;
using System.Globalization;

namespace TsiErp.DashboardUI.Services
{
    public class UretimUygunsuzlukService : IUretimUygunsuzlukService
    {
        SqlConnection _connection;
        public UretimUygunsuzlukService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        #region Chart

        public async Task< List<AdminProductionUnsuitabilityAnalysisChart>> GetProductionUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action)
        {
            List<AdminProductionUnsuitabilityAnalysisChart> adminProductionUnsuitabilityChart = new List<AdminProductionUnsuitabilityAnalysisChart>();
            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            #region Değişkenler
            decimal previousMonthUnsuitabilityPercent = 0;
            int uygunsuzluk = 0;
            decimal uretilenadet = 0;
            decimal unsuitabilityPercent = 0;
            decimal differenceUnsuitability = 0;
            #endregion

            switch (action)
            {
                #region Hurda

                case 1:

                    if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                    {
                        var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    else if (frequency == 5 || frequency == 6)
                    {
                        var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t =>
                        {
                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.HURDA == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                        
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    break;

                #endregion

                #region Düzeltme

                case 2:

                    if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                    {
                        var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
                        {
                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    else if (frequency == 5 || frequency == 6)
                    {
                        var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t =>
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.DUZELTME == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                        {
                            Ay = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                        
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    break;

                #endregion

                #region Olduğu Gibi Kullanılacak

                case 3:

                    if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                    {
                        var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                           
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }

                    else if (frequency == 5 || frequency == 6)
                    {
                        var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => 
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    break;

                #endregion

                #region Toplam Uygunsuzluk

                case 4:

                    if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                    {
                        var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => 
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }

                    else if (frequency == 5 || frequency == 6)
                    {
                        var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => 
                        {

                            #region Değişkenler

                            previousMonthUnsuitabilityPercent = unsuitabilityPercent;

                            uygunsuzluk = t.Sum(t => t.OLCUKONTROLFORMBEYAN);

                            uretilenadet = operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET);

                            unsuitabilityPercent = (decimal)(uygunsuzluk / (double)uretilenadet);

                            differenceUnsuitability = unsuitabilityPercent - previousMonthUnsuitabilityPercent;

                            #endregion

                            return new AdminProductionUnsuitabilityAnalysisChart
                            {
                                Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                                Total = uygunsuzluk,
                                Percent = unsuitabilityPercent,
                                PRODUCTION = uretilenadet,
                                UNSUITABILITY = uygunsuzluk,
                                DIFFUNS = differenceUnsuitability
                            };
                            
                        }).ToList();

                        adminProductionUnsuitabilityChart = gList;
                    }
                    break;

                #endregion

                default: break;
            }



            return await Task.FromResult(adminProductionUnsuitabilityChart);

        }


        #endregion

        #region Grid

        public async Task< List<ProductionUnsuitabilityAnalysis>> GetProductionUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<ProductionUnsuitabilityAnalysis> productionUnsuitabilityAnalysis = new List<ProductionUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetUnsuitabilityQuery(startDate, endDate);
            var list = unsuitabilityLines.Select(t => t.KOD).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in list)
                {
                    #region Değişkenler

                    int scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    int tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    int correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.KOD == unsuitability).Sum(t => t.OLCUKONTROLFORMBEYAN);
                    string unsuitabilityReason = unsuitabilityLines.Where(t => t.KOD == unsuitability).Select(t => t.HATAACIKLAMA).FirstOrDefault();
                    int productionUnsuitabilityID = unsuitabilityLines.Where(t => t.KOD == unsuitability).Select(t => t.ID).FirstOrDefault();

                    #endregion

                    ProductionUnsuitabilityAnalysis analysis = new ProductionUnsuitabilityAnalysis
                    {
                        ProductionUnsuitabilityID = productionUnsuitabilityID,
                        ScrapQuantity = scrap,
                        ToBeUsedAs = tobeused,
                        Correction = correction,
                        Total = scrap + tobeused + correction,
                        UnsuitabilityReason = unsuitabilityReason,
                         Code = unsuitability

                    };
                    productionUnsuitabilityAnalysis.Add(analysis);
                }
            }
            return await Task.FromResult(productionUnsuitabilityAnalysis);
        }

        #endregion

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
