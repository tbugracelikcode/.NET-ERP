using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos
{
    public class EmployeeListReportParameterDto
    {
        public List<Guid> Departments { get; set; }

        public List<Guid> Seniorities { get; set; }

        public List<Guid> EducationLevels { get; set; }


        public DateTime? HiringStartDate { get; set; }

        public DateTime? HiringEndDate { get; set; }

        public EmployeeListReportParameterDto()
        {
            Departments = new List<Guid>();
            Seniorities = new List<Guid>();
            EducationLevels = new List<Guid>();
        }
    }
}
