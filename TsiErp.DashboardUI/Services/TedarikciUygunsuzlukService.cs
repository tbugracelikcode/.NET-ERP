using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using System.Globalization;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class TedarikciUygunsuzlukService : ITedarikciUygunsuzlukService
    {


        #region Chart

        //public async Task< List<AdminSupplierUnsuitabilityAnalysisChart>> GetSupplierUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action)
        //{
        //    List<AdminSupplierUnsuitabilityAnalysisChart> adminSupplierUnsuitabilityChart = new List<AdminSupplierUnsuitabilityAnalysisChart>();
        //    var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate);
        //    var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

        //    switch(action)
        //    {
        //        #region Tedarikçi ile İrtibat

        //        case 1:

        //            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
        //            {
        //                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            else if (frequency == 5 || frequency == 6)
        //            {
        //                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            break;

        //        #endregion

        //        #region Düzeltme

        //        case 2:

        //            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
        //            {
        //                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            else if (frequency == 5 || frequency == 6)
        //            {
        //                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            break;

        //        #endregion

        //        #region Olduğu Gibi Kullanılacak

        //        case 3:

        //            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
        //            {
        //                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            else if (frequency == 5 || frequency == 6)
        //            {
        //                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            break;

        //        #endregion

        //        #region Red

        //        case 4:

        //            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
        //            {
        //                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            else if (frequency == 5 || frequency == 6)
        //            {
        //                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            break;

        //        #endregion

        //        #region Toplam Uygunsuzluk

        //        case 5:

        //            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
        //            {
        //                var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month, YIL = t.TARIH.Year }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = GetMonth(t.Key.Ay) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            else if (frequency == 5 || frequency == 6)
        //            {
        //                var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date, YIL = t.TARIH.Year }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
        //                {
        //                    Ay = t.Key.HAFTA.ToString("dd MMM yy", new CultureInfo("tr-TR")) + " " + t.Key.YIL.ToString(),
        //                    Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
        //                    Percent = (double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
        //                }).ToList();

        //                adminSupplierUnsuitabilityChart = gList;
        //            }
        //            break;

        //        #endregion

        //        default: break;
        //    }

        //    return await Task.FromResult(adminSupplierUnsuitabilityChart);

        //}

        #endregion

        #region Grid

        public async Task<List<SupplierUnsuitabilityAnalysis>> GetSupplierUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<SupplierUnsuitabilityAnalysis> supplierUnsuitabilityAnalysis = new List<SupplierUnsuitabilityAnalysis>();

            var purchaseList = DBHelper.GetPurchaseQuery(startDate, endDate).Where(t => t.DURUM == 1 || t.DURUM == 2);
            var purchaseLinesList = DBHelper.GetPurchaseLinesQuery(startDate, endDate);
            var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate);
            var Carilist = purchaseList.Select(t => t.CARIID).Distinct().ToList();

            if (purchaseList != null)
            {
                foreach (var cari in Carilist)
                {
                    var tempPurchaseList = purchaseList.Where(t => t.CARIID == cari).ToList();
                    var tempPurchaseLinesList = purchaseLinesList.Where(t => t.CARIUNVAN == tempPurchaseList.Where(t => t.CARIID == cari).Select(t => t.CARIUNVAN).FirstOrDefault()).ToList();
                    var tempList = unsuitabilityLines.Where(t => t.CARIID == cari).ToList();
                    var orderList = tempList.Select(t => t.SIPARISID).Distinct().ToList();

                    #region Değişkenler

                    int totalOrder = 0;
                    int total = tempList.Sum(t => t.UYGUNOLMAYANMIKTAR);
                    string supplierName = tempPurchaseList.Where(t => t.CARIID == cari).Select(t => t.CARIUNVAN).FirstOrDefault();

                    #endregion

                    //foreach (var orderID in orderList)
                    //{
                    //    totalOrder += (int)DBHelper.GetSuppliertUnsuitabilityLinesQuery().Where(t => t.SIPARISID == orderID).Sum(t => t.ADET);
                    //}
                    foreach (var Purchase in tempPurchaseList)
                    {
                        totalOrder += Convert.ToInt32(tempPurchaseLinesList.Where(t=>t.FISNO == Purchase.FISNO).Sum(t=>t.ADET));
                    }

                    SupplierUnsuitabilityAnalysis analysis = new SupplierUnsuitabilityAnalysis
                    {
                        SupplierID = cari,
                        SupplierName = supplierName,
                        Total = total,
                        TotalOrder = (int)totalOrder,
                        Percent = (double)total / (double)totalOrder

                    };
                    supplierUnsuitabilityAnalysis.Add(analysis);
                }
            }

            supplierUnsuitabilityAnalysis = supplierUnsuitabilityAnalysis.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(supplierUnsuitabilityAnalysis);
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
