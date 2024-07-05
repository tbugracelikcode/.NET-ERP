using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.ReportDtos.StationsListReportDtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.ErpUI.Reports.MachineAndWorkforceManagement;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.ReportPages
{
    public partial class StationListReportPage : IDisposable
    {

        [Inject]
        ModalManager ModalManager { get; set; }

        DxReportViewer reportViewer { get; set; }
        XtraReport Report { get; set; }

        protected override async void OnInitialized()
        {
            await GetStationGroups();
        }

        #region Station Groups

        List<ListStationGroupsDto> StationGroups = new List<ListStationGroupsDto>();
        List<Guid> BindingStationGroups = new List<Guid>();

        private async Task GetStationGroups()
        {
            StationGroups = (await StationGroupsAppService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
        }

        #endregion

        private async void CreateReport()
        {
            if (BindingStationGroups == null)
            {
                BindingStationGroups = new List<Guid>();
            }

            StationListReportParameterDto filters = new StationListReportParameterDto();
            filters.StationGroups = BindingStationGroups;

            var report = (await StationReportsAppService.GetStationListReport(filters, StationsLocalizer)).ToList();

            if (report.Count > 0)
            {
                Report = new StationListReport();
                Report.DataSource = report;
            }
            else
            {
                Report = new StationListReport();
                Report.DataSource = null;
                await ModalManager.MessagePopupAsync(ReportLocalizer["ReportMessageTitle"], ReportLocalizer["ReportRecordNotFound"]);
            }
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
