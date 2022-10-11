using Microsoft.Data.SqlClient;
using TsiErp.DashboardUI.Helpers;
using TsiErp.DashboardUI.Helpers.HelperModels;
using TsiErp.DashboardUI.Models;
using TsiErp.DashboardUI.Services.Interfaces;

namespace TsiErp.DashboardUI.Services
{
    public class PersonelDetayService : IPersonelDetayService
    {

        #region Chart

        public async Task<List<EmployeeDetailedChart>> GetEmployeeDetailedtChart(int calisanID, DateTime startDate, DateTime endDate)
        {


            List<EmployeeDetailedChart> employeeDetailedChart = new List<EmployeeDetailedChart>();

            var haltCodes = DBHelper.GetHaltCodes();
            var haltLines = DBHelper.GetHaltQueryEmployee(calisanID, startDate, endDate).Where(t=>t.PKD==true).ToList();
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

            employeeDetailedChart = employeeDetailedChart.OrderByDescending(t => t.Percent).ToList();
            return await Task.FromResult(employeeDetailedChart);
        }

        #endregion

        #region Grid

        public async Task<List<EmployeeDetailedHaltAnalysis>> GetStationDetailedHaltAnalysis(int calisanID, DateTime startDate, DateTime endDate)
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
            return await Task.FromResult(employeeDetailedHaltAnalysis);
        }

        #endregion
    }
}
