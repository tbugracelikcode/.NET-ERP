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

        #region Chart

        public List<EmployeeDetailedChart> GetEmployeeDetailedtChart(int calisanID, DateTime startDate, DateTime endDate)
        {


            List<EmployeeDetailedChart> employeeDetailedChart = new List<EmployeeDetailedChart>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryEmployee(calisanID, startDate, endDate);
            int totaltime = haltLines.Sum(t => t.DURUSSURE);

            foreach (var code in haltCodes)
            {
                #region Değişkenler

                int haltID = code.ID;
                int time = haltLines.Where(t => t.DURUSID == haltID).Sum(t => t.DURUSSURE);

                #endregion

                EmployeeDetailedChart analysis = new EmployeeDetailedChart
                {
                    HaltName = code.KOD,
                    HaltTimeSecond = time,
                    Percent = (double)time / (double)totaltime
                };
                
                if (analysis.HaltTimeSecond > 0)
                {
                    employeeDetailedChart.Add(analysis);
                }

            };
            return employeeDetailedChart;
        }

        #endregion

        #region Grid

        public List<EmployeeDetailedHaltAnalysis> GetStationDetailedHaltAnalysis(int calisanID, DateTime startDate, DateTime endDate)
        {
            List<EmployeeDetailedHaltAnalysis> employeeDetailedHaltAnalysis = new List<EmployeeDetailedHaltAnalysis>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryEmployee(calisanID, startDate, endDate);

            foreach (var code in haltCodes)
            {
                #region Değişkenler

                int haltID = code.ID;
                int haltTimeSecond = haltLines.Where(t => t.DURUSID == haltID).Sum(t => t.DURUSSURE);
                int haltTimeMinute = haltTimeSecond / 60;
                string employeeName = haltLines.Where(t => t.DURUSID == haltID).Select(t => t.CALISAN).FirstOrDefault();

                #endregion

                EmployeeDetailedHaltAnalysis analysis = new EmployeeDetailedHaltAnalysis
                {
                    HaltName = code.KOD,
                    HaltID = haltID,
                    HaltTimeSecond = haltTimeSecond,
                    HaltTimeMinute = haltTimeMinute,
                    EmployeeName = employeeName
                };

                employeeDetailedHaltAnalysis.Add(analysis);
            }
            return employeeDetailedHaltAnalysis;
        }

        #endregion
    }
}
