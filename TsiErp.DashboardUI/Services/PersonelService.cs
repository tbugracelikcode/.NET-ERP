using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class PersonelService
    {
        SqlConnection _connection;

        public PersonelService()
        {
            _connection = DBHelper.GetSqlConnection();
        } 

        public List<AdminEmployeeChart> GetEmployeeChart(DateTime startDate, DateTime endDate, int frequency)
        {
            List<AdminEmployeeChart> adminEmployeeChart = new List<AdminEmployeeChart>();
            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);
            var calenderLines = DBHelper.GetCalendarQuery(startDate, endDate);



            if (frequency == 0 || frequency == 1 || frequency == 2 || frequency == 3 || frequency == 4)
            {
                var gList = operationLines.OrderBy(t => t.TARIH).GroupBy(t => new { Ay = t.TARIH.Month }).Select(t => new AdminEmployeeChart
                {
                    Ay = GetMonth(t.Key.Ay),
                    PlannedQuantity = t.Select(x => x.PLNMIKTAR).FirstOrDefault(),
                    TotalProduction = t.Select(x => x.URETILENADET).FirstOrDefault(),
                    TotalScrap = t.Select(x => x.HURDAADET).FirstOrDefault(),
                    Productivity = (t.Average(x => x.URETILENADET) - t.Average(x => x.HURDAADET)) / t.Average(x => x.PLNMIKTAR)
                }).ToList();
                adminEmployeeChart = gList;
            }
            else if (frequency == 5 || frequency == 6)
            {
                var gList = operationLines.GroupBy(t => new { HAFTA = t.TARIH.Date }).OrderBy(t => t.Key.HAFTA).Select(t => new AdminEmployeeChart
                {
                    Ay = t.Key.HAFTA.ToString("dd MMM yy"),
                    PlannedQuantity = t.Select(x => x.PLNMIKTAR).FirstOrDefault(),
                    TotalProduction = t.Select(x => x.URETILENADET).FirstOrDefault(),
                    TotalScrap = t.Select(x => x.HURDAADET).FirstOrDefault(),
                    Productivity =( t.Average(x => x.URETILENADET) - t.Average(x => x.HURDAADET) )/ t.Average(x => x.PLNMIKTAR)
                }).ToList();
                adminEmployeeChart = gList;
            }

            return adminEmployeeChart;

        }

        public List<EmployeeGeneralAnalysis> GetEmployeeGeneralAnalysis(DateTime startDate, DateTime endDate)
        {
           

            List<EmployeeGeneralAnalysis> employeeGeneralAnalysis = new List<EmployeeGeneralAnalysis>();

            var operationLines = DBHelper.GetOperationLinesQuery(startDate, endDate);

            var employeeList = operationLines.Select(t => t.CALISANID).Distinct().ToList();
            if (employeeList != null)
            {
                foreach (var employeeID in employeeList)
                {
                    EmployeeGeneralAnalysis analysis = new EmployeeGeneralAnalysis
                    {
                        EmployeeID = employeeID,
                        EmployeeName = operationLines.Where(t => t.CALISANID == employeeID).Select(t => t.CALISAN).FirstOrDefault(),
                        OperationTime = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.OPERASYONSURESI),
                        HaltTime = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.ATILSURE),
                        TotalProduction = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.URETILENADET),
                        TotalScrap = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.HURDAADET),
                        PlannedQuantity = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.PLNMIKTAR)
                    };
                    employeeGeneralAnalysis.Add(analysis);
                }
            }
            return employeeGeneralAnalysis;
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