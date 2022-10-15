using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using System.Globalization;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class UrunGrubuPerformansService : IUrunGrubuPerformansService
    {
        #region Chart

        public async Task<List<AdminProductGroupPerformanceAnalysisChart>> GetProductGroupPerformanceAnalysisChart(DateTime startDate, DateTime endDate, int frequency, int? productionSelection)
        {
            List<AdminProductGroupPerformanceAnalysisChart> adminProductGroupPerformanceAnalysisChart = new List<AdminProductGroupPerformanceAnalysisChart>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            #region Değişkenler

            decimal previousMonthPerformance = 0;
            decimal performans = 0;
            int planlananbirimsure = 0;
            decimal gerceklesenbirimsure = 0;

            #endregion


            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                {

                    #region Değişkenler

                    #region Performans

                    previousMonthPerformance = performans;

                    gerceklesenbirimsure = t.Where(t => t.URUNGRPID == productionSelection).Sum(t => t.BIRIMSURE);

                    planlananbirimsure = t.Where(t => t.URUNGRPID == productionSelection).Sum(t => t.PLANLANANOPRSURESI);

                    performans = gerceklesenbirimsure > 0 ? (planlananbirimsure / gerceklesenbirimsure) : 0;
                    #endregion

                    decimal differencePerformance = performans - previousMonthPerformance;

                    #endregion

                    return new AdminProductGroupPerformanceAnalysisChart
                    {
                        Month = GetMonth(t.Key.AY) + " " + t.Key.YIL.ToString(),
                        THmonth = t.Key.AY,
                        Year = t.Key.YIL,
                        Performance = performans,
                        DIFFPER = differencePerformance,
                        OCCUREDUNITTIME = gerceklesenbirimsure,
                        PLANNEDUNITTIME = planlananbirimsure
                    };
                }).ToList();

                adminProductGroupPerformanceAnalysisChart = gList;
            }

            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t =>
                {

                    #region Değişkenler

                    #region Performans

                    previousMonthPerformance = performans;

                    gerceklesenbirimsure = t.Where(t => t.URUNGRPID == productionSelection).Sum(t => t.BIRIMSURE);

                    planlananbirimsure = t.Where(t => t.URUNGRPID == productionSelection).Sum(t => t.PLANLANANOPRSURESI);

                    performans = gerceklesenbirimsure > 0 ? (planlananbirimsure / gerceklesenbirimsure) : 0;
                    #endregion

                    decimal differencePerformance = performans - previousMonthPerformance;

                    #endregion

                    return new AdminProductGroupPerformanceAnalysisChart
                    {
                        Month = t.Key.HAFTA.ToString("dd MMM", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
                        THmonth = t.Key.HAFTA.Day,
                        Year = t.Key.YIL,
                        Performance = performans,
                        DIFFPER = differencePerformance,
                        OCCUREDUNITTIME = gerceklesenbirimsure,
                        PLANNEDUNITTIME = planlananbirimsure
                    };
                }).ToList();

                adminProductGroupPerformanceAnalysisChart = gList;
            }

            return await Task.FromResult(adminProductGroupPerformanceAnalysisChart);
        }

        #endregion

        #region Grid

        public async Task<List<ProductGroupPerformanceAnalysis>> GetProductGroupPerformanceAnalysis(DateTime startDate, DateTime endDate, int? productionSelection, int frequency)
        {
            List<ProductGroupPerformanceAnalysis> productGroupPerformanceAnalysis = new List<ProductGroupPerformanceAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var stationList = operationLines.Where(t => t.URUNGRPID == productionSelection).Select(t => t.ISTASYONID).Distinct().ToList();

            #region Değişkenler

            string stationCode = string.Empty;
            decimal perf = 0;

            #endregion

            if (stationList != null)
            {
                foreach (var stationID in stationList)
                {
                    var tempOperationLines = operationLines.Where(t => t.ISTASYONID == stationID && t.URUNGRPID == productionSelection).ToList();

                    stationCode = tempOperationLines.Where(t => t.ISTASYONID == stationID).Select(t => t.MAKINEKODU).FirstOrDefault();




                    if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                    {
                        var gList = tempOperationLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t =>
                        {
                            perf = t.Sum(t => t.BIRIMSURE) == 0 ? 0 : t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE);

                            return new ProductGroupPerformanceAnalysis
                            {
                                StationCode = stationCode,

                                Performance = perf
                            };
                        }
                            ).ToList();

                        productGroupPerformanceAnalysis.AddRange(gList);

                    }


                    else if (frequency == 5 || frequency == 6)
                    {
                        var gList = tempOperationLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t =>
                         {
                             perf = t.Sum(t => t.BIRIMSURE) == 0 ? 0 : t.Sum(t => t.PLANLANANOPRSURESI) / t.Sum(t => t.BIRIMSURE);

                             return new ProductGroupPerformanceAnalysis
                             {
                                 StationCode = stationCode,

                                 Performance = perf
                             };
                         }).ToList();

                        productGroupPerformanceAnalysis = gList;
                    }


                }
            }

            productGroupPerformanceAnalysis = productGroupPerformanceAnalysis.OrderByDescending(t => t.Performance).ToList();

            return await Task.FromResult(productGroupPerformanceAnalysis);
        }

        #endregion

        #region Combobox

        public async Task<List<ProductGroupsAnalysis>> GetProductGroupsComboboxAnalysis(DateTime startDate, DateTime endDate)
        {

            List<ProductGroupsAnalysis> productGroupsAnalysis = new List<ProductGroupsAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate).Where(t => t.STOKTURU == 11).ToList(); //Yarı Mamül

            var groupList = operationLines.Select(t => t.URUNGRPID).Distinct().ToList();

            if (groupList != null)
            {
                foreach (var groupID in groupList)
                {

                    #region Değişkenler

                    string productGroupName = operationLines.Where(t => t.URUNGRPID == groupID).Select(t => t.URUNGRUBU).FirstOrDefault();

                    #endregion

                    ProductGroupsAnalysis analysis = new ProductGroupsAnalysis
                    {
                        ProductGroupID = groupID,
                        ProductGroupName = productGroupName
                    };
                    productGroupsAnalysis.Add(analysis);
                }
            }
            return await Task.FromResult(productGroupsAnalysis);
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
