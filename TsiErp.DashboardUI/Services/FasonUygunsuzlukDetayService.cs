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

        #region Chart

        public List<ContractUnsuitabilityAnalysis> GetContractUnsuitabilityDetailedChart(DateTime startDate, DateTime endDate, int frequency, int? action, int cariID,int total)
        {
            List<ContractUnsuitabilityAnalysis> adminContractUnsuitabilityDetailedChart = new List<ContractUnsuitabilityAnalysis>();
            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate).Where(t => t.CARIID == cariID);
            var operationLines = DBHelper.GetContractUnsuitabilityQueryGeneral(startDate, endDate).Where(t => t.CariID == cariID);

            #region Hurda

            if (action == 1) //Hurda
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month }).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Total = t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Ay = GetMonth(t.Key.AY),
                        Percent = ((double)t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total) 

                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = ((double)t.Where(t => t.HURDA == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)
                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
            }

            #endregion

            #region Red

            else if (action == 2) //Red
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month }).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Ay = GetMonth(t.Key.AY),
                        Percent = ((double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)

                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = ((double)t.Where(t => t.RED == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)
                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }

            }

            #endregion

            #region Olduğu Gibi Kullanılacak

            else if (action == 3) //Olduğu Gibi
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month }).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Ay = GetMonth(t.Key.AY),
                        Percent = ((double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)

                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = ((double)t.Where(t => t.OLDUGUGIBIKULLANILACAK == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total) 
                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }

            }

            #endregion

            #region Düzeltme

            else if (action == 4) //Düzeltme
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month }).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Ay = GetMonth(t.Key.AY),
                        Percent = ((double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)

                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = ((double)t.Where(t => t.DUZELTME == true).Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)
                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
            }

            #endregion

            #region Toplam Uygunsuzluk

            else if (action == 5) //Toplam
            {
                if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
                {
                    var gList = unsuitabilityLines.OrderBy(t => t.TARIH).GroupBy(t => new { AY = t.TARIH.Month }).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Ay = GetMonth(t.Key.AY),
                        Percent = ((double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)

                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
                else if (frequency == 5 || frequency == 6)
                {
                    var gList = unsuitabilityLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new ContractUnsuitabilityAnalysis
                    {
                        Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                        Total = t.Sum(t => t.UYGUNOLMAYANMIKTAR),
                        Percent = ((double)t.Sum(t => t.UYGUNOLMAYANMIKTAR) / (double)total)
                    }).ToList();
                    adminContractUnsuitabilityDetailedChart = gList;
                }
            }

            #endregion

            return adminContractUnsuitabilityDetailedChart;

        }

        #endregion

        #region Grid

        public List<ContractUnsuitabilityAnalysis> GetContractUnsuitabilityDetailed(DateTime startDate, DateTime endDate, int cariID)
        {

            List<ContractUnsuitabilityAnalysis> contractUnsuitabilityAnalysis = new List<ContractUnsuitabilityAnalysis>();

            var unsuitabilityLines = DBHelper.GetContractUnsuitabilityQuery(startDate, endDate);
            var theList = unsuitabilityLines.Where(t => t.CARIID == cariID).Select(t => t.CARIID).Distinct().ToList();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            if (unsuitabilityLines != null)
            {
                foreach (var unsuitability in theList)
                {
                    var scrap = unsuitabilityLines.Where(t => t.HURDA == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var refuse = unsuitabilityLines.Where(t => t.RED == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var tobeused = unsuitabilityLines.Where(t => t.OLDUGUGIBIKULLANILACAK == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);
                    var correction = unsuitabilityLines.Where(t => t.DUZELTME == true && t.CARIID == unsuitability).Sum(t => t.UYGUNOLMAYANMIKTAR);

                    ContractUnsuitabilityAnalysis analysis = new ContractUnsuitabilityAnalysis
                    {
                        ContractSupplierID = unsuitability,
                        ContractSupplier = unsuitabilityLines.Where(t=>t.CARIID == unsuitability).Select(t => t.CARIUNVAN).FirstOrDefault(),
                        ScrapQuantity = scrap,
                        RefuseQuantity = refuse,
                        Correction = correction,
                        ToBeUsedAs = tobeused,
                        Total = scrap + refuse + tobeused + correction

                    };
                    contractUnsuitabilityAnalysis.Add(analysis);
                }
            }
            return contractUnsuitabilityAnalysis;
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
