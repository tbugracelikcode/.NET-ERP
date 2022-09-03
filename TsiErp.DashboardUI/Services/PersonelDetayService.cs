using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;

namespace TsiErp.DashboardUI.Services
{
    public class PersonelDetayService
    {
        SqlConnection _connection;
        public PersonelDetayService()
        {
            _connection = DBHelper.GetSqlConnection();
        }

        public List<EmployeeDetailedHaltAnalysis> GetStationDetailedHaltAnalysis(int calisanID, DateTime startDate, DateTime endDate)
        {
            //startDate = new DateTime(2022, 06, 01);
            //endDate = new DateTime(2022, 08, 22);
            //calisanID = 109;
            List<EmployeeDetailedHaltAnalysis> employeeDetailedHaltAnalysis = new List<EmployeeDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryEmployee(calisanID, startDate, endDate);

            foreach (var code in haltCodes)
            {
                int durusID = code.ID;

                EmployeeDetailedHaltAnalysis analysis = new EmployeeDetailedHaltAnalysis
                {
                    HaltName = code.KOD,
                    HaltID = durusID,
                    HaltTimeSecond = haltLines.Where(t => t.DURUSID == durusID).Sum(t => t.DURUSSURE),
                    HaltTimeMinute = haltLines.Where(t => t.DURUSID == durusID).Sum(t => t.DURUSSURE) / 60
                };

                employeeDetailedHaltAnalysis.Add(analysis);
            }
            return employeeDetailedHaltAnalysis;
        }
    }
}
