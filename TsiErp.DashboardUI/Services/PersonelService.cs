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

        public List<EmployeeGeneralAnalysis> GetEmployeeGeneralAnalysis(DateTime startDate, DateTime endDate)
        {
            startDate = new DateTime(2022, 06, 01);
            endDate = new DateTime(2022, 08, 22);

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
                        TotalScrap = operationLines.Where(t => t.CALISANID == employeeID).Sum(t => t.HURDAADET)
                    };
                    employeeGeneralAnalysis.Add(analysis);
                }
            }
            return employeeGeneralAnalysis;
        }
    }
}