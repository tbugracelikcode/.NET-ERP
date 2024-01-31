using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.ErpUI.Reports.MachineAndWorkforceManagement;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.ReportPages
{
    public partial class EmployeeListReportPage : IDisposable
    {

        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        public DateTime? StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 



        protected override async void OnInitialized()
        {
            await GetDepartments();
            await GetSeniorities();
            await GetEducationLevelScores();
        }

        #region Departments

        List<ListDepartmentsDto> Departments = new List<ListDepartmentsDto>();
        List<Guid> BindingDepartments = new List<Guid>();

        private async Task GetDepartments()
        {
            Departments = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
        }

        #endregion

        #region Seniorities

        List<ListEmployeeSenioritiesDto> Seniorities = new List<ListEmployeeSenioritiesDto>();
        List<Guid> BindingSeniorities = new List<Guid>();

        private async Task GetSeniorities()
        {
            Seniorities = (await EmployeeSenioritiesService.GetListAsync(new ListEmployeeSenioritiesParameterDto())).Data.ToList();
        }

        #endregion

        #region Education Level Scores

        List<ListEducationLevelScoresDto> EducationLevelScores = new List<ListEducationLevelScoresDto>();
        List<Guid> BindingEducationLevelScores = new List<Guid>();

        private async Task GetEducationLevelScores()
        {
            EducationLevelScores = (await EducationLevelScoresService.GetListAsync(new ListEducationLevelScoresParameterDto())).Data.ToList();
        }

        #endregion

        private async void CreateReport()
        {
            if (BindingDepartments == null)
            {
                BindingDepartments = new List<Guid>();
            }

            if (BindingSeniorities == null)
            {
                BindingSeniorities = new List<Guid>();
            }

            if (BindingEducationLevelScores == null)
            {
                BindingEducationLevelScores = new List<Guid>();
            }

            EmployeeListReportParameterDto filters = new EmployeeListReportParameterDto();
            filters.Departments = BindingDepartments;
            filters.Seniorities = BindingSeniorities;
            filters.EducationLevels = BindingEducationLevelScores;
            filters.HiringStartDate = StartDate;
            filters.HiringEndDate = EndDate;

            var report = (await EmployeeReportsAppService.GetEmployeeListReport(filters, EmployeesLocalizer)).ToList();
            Report = new EmployeeListReport();
            Report.DataSource = report;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
