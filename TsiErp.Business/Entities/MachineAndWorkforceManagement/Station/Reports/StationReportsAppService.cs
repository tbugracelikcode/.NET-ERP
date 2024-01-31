using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.MachineAndWorkforceManagement.Employee.Reports;
using TsiErp.Business.Entities.Station.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.ReportDtos.StationsListReportDtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.Station.Reports
{
    [ServiceRegistration(typeof(IStationReportsAppService), DependencyInjectionType.Scoped)]
    public class StationReportsAppService : IStationReportsAppService
    {
        private readonly IStationsAppService _stationsAppService;

        public StationReportsAppService(IStationsAppService stationsAppService)
        {
            _stationsAppService = stationsAppService;
        }

        public async Task<List<StationListReportDto>> GetStationListReport(StationListReportParameterDto filters, object localizer)
        {
            List<StationListReportDto> reportDatasource = new List<StationListReportDto>();

            var stations = (await _stationsAppService.GetListAsync(new ListStationsParameterDto())).Data.AsQueryable();

            if (filters.StationGroups.Count > 0)
            {
                stations = stations.Where(t => filters.StationGroups.Contains(t.GroupID)).AsQueryable();
            }

            var stationList = stations.ToList();

            int lineNr = 1;

            foreach (var station in stationList)
            {
                reportDatasource.Add(new StationListReportDto
                {
                    Brand = station.Brand,
                    Code = station.Code,
                    LineNr = lineNr,
                    Model = station.Model,
                    Name = station.Name,
                    StationGroup = station.StationGroup
                });

                lineNr++;
            }

            return reportDatasource;
        }
    }
}
