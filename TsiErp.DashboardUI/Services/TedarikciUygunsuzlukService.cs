using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class TedarikciUygunsuzlukService
    {
        SqlConnection _connection;
        public TedarikciUygunsuzlukService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<AdminSupplierUnsuitabilityAnalysisChart> GetSupplierUnsuitabilityChart(DateTime startDate, DateTime endDate, int frequency, int? action)
        {
            List<AdminSupplierUnsuitabilityAnalysisChart> adminSupplierUnsuitabilityChart = new List<AdminSupplierUnsuitabilityAnalysisChart>();
            var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate);
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var percenttotal = (double)operationLines.Sum(t => t.PLNMIKTAR);

            if (action ==1) // Tedarikçi ile İrtibat
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR) ,
                        Percent = (double)t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.TEDARIKCIIRTIBAT == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
            }
            else if (action == 2) //Düzeltme
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
            }
            else if (action ==3) //Olduğu Gibi Kullanılacak
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total =  t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) ,
                        Percent = (double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total =  t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
            }
            else if (action == 4) //Red
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
            }
            else if (action == 5) //Hepsini Göster
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = GetMonth(t.Key.Ay),
                        Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Month == t.Key.Ay).Sum(x => x.URETILENADET) * 100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminSupplierUnsuitabilityAnalysisChart
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = (double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)operationLines.Where(x => x.TARIH.Date == t.Key.HAFTA).Sum(x => x.URETILENADET)*100
                    }).ToList();
                    adminSupplierUnsuitabilityChart = gList;
                }
            }


            return adminSupplierUnsuitabilityChart;

        }

        public List<SupplierUnsuitabilityAnalysis> GetSupplierUnsuitabilityAnalysis(DateTime startDate, DateTime endDate)
        {
            List<SupplierUnsuitabilityAnalysis> supplierUnsuitabilityAnalysis = new List<SupplierUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetSuppliertUnsuitabilityQuery(startDate, endDate);
            var list = unsuitabilityLines.Select(t => t.HATAID).Distinct().ToList();

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in list)
                {
                    var contact = unsuitabilityLines.Where(t => t.TEDARIKCIIRTIBAT == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var refuse = unsuitabilityLines.Where(t => t.RED == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.HATAID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    SupplierUnsuitabilityAnalysis analysis = new SupplierUnsuitabilityAnalysis
                    {
                        SupplierUnsuitabilityID = unsuitabilityLines.Where(t => t.HATAID == unsuitability).Select(t => t.ID).FirstOrDefault(),
                        ContactWithSupplier = contact,
                        ToBeUsedAs = tobeused,
                        Correction = correction,
                        Total = contact + tobeused + correction+refuse,
                        UnsuitabilityReason = unsuitabilityLines.Where(t => t.HATAID == unsuitability).Select(t => t.HATAACIKLAMA).FirstOrDefault(),
                        ErrorID = unsuitability,
                        RefuseQuantity = refuse

                    };
                    supplierUnsuitabilityAnalysis.Add(analysis);
                }
            }
            return supplierUnsuitabilityAnalysis;
        }

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
