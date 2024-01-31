using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.ReportDtos.StationsListReportDtos
{
    public class StationListReportParameterDto
    {
        public List<Guid> StationGroups { get; set; }

        public StationListReportParameterDto()
        {
            StationGroups = new List<Guid>();
        }

    }
}
