using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.ReportDtos.StationsListReportDtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.Station.Reports
{
    public interface IStationReportsAppService
    {
        Task<List<StationListReportDto>> GetStationListReport(StationListReportParameterDto filters, object localizer);
    }
}
