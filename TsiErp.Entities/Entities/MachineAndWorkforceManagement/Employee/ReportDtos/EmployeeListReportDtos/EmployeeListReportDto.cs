using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Enums;

namespace TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos
{
    public class EmployeeListReportDto
    {
        public int LineNr { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string DepartmentName { get; set; }

        public string IDnumber { get; set; }

        public DateTime? Birthday { get; set; }

        public string BloodTypeName { get; set; }

        public string Email { get; set; }

        public string IsActive { get; set; }

        public string SeniorityName { get; set; }

        public string EducationLevelName { get; set; }

        public decimal CurrentSalary { get; set; }

        public string TaskDefinition { get; set; }

        public DateTime? HiringDate { get; set; }
    }
}
