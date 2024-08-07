using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.StockManagement.Product.Reports;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.ReportDtos.EmployeeListReportDtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.ReportDtos.ProductListReportDtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.Employee.Reports
{
    [ServiceRegistration(typeof(IEmployeeReportsAppService), DependencyInjectionType.Scoped)]
    public class EmployeeReportsAppService : IEmployeeReportsAppService
    {
        private readonly IEmployeesAppService _employeesAppService;

        public EmployeeReportsAppService(IEmployeesAppService employeesAppService)
        {
            _employeesAppService = employeesAppService;
        }

        public async Task<List<EmployeeListReportDto>> GetEmployeeListReport(EmployeeListReportParameterDto filters, object localizer)
        {
            var loc = (IStringLocalizer)localizer;

            List<EmployeeListReportDto> reportDatasource = new List<EmployeeListReportDto>();

            var employees = (await _employeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.AsQueryable();

            if (filters.HiringStartDate.HasValue && filters.HiringEndDate.HasValue)
            {
                employees = employees.Where(t => t.HiringDate >= filters.HiringStartDate.Value && t.HiringDate <= filters.HiringEndDate).AsQueryable();
            }

            if (filters.Departments.Count > 0)
            {
                employees = employees.Where(t => filters.Departments.Contains(t.DepartmentID.Value)).AsQueryable();
            }

            if (filters.Seniorities.Count > 0)
            {
                employees = employees.Where(t => filters.Seniorities.Contains(t.SeniorityID.Value)).AsQueryable();
            }

            if (filters.EducationLevels.Count > 0)
            {
                employees = employees.Where(t => filters.EducationLevels.Contains(t.EducationLevelID.Value)).AsQueryable();
            }

            var employeeList = employees.ToList();

            int lineNr = 1;

            foreach (var employee in employeeList)
            {
                var locKey = loc[Enum.GetName(typeof(BloodTypeEnum), employee.BloodType)];

                reportDatasource.Add(new EmployeeListReportDto
                {
                    LineNr = lineNr,
                    BloodTypeName = loc[GetBloodTypeEnumStringKey(locKey)],
                    Birthday = employee.Birthday,
                    Code = employee.Code,
                    CurrentSalary = employee.CurrentSalary,
                    DepartmentName = employee.Department,
                    EducationLevelName = employee.EducationLevelName,
                    Email = employee.Email,
                    FullName = employee.Name + " " + employee.Surname,
                    HiringDate = employee.HiringDate,
                    IDnumber = employee.IDnumber,
                    SeniorityName = employee.SeniorityName,
                    TaskDefinition = employee.TaskDefinition
                });

                lineNr++;
            }

            return reportDatasource;
        }

        public static string GetBloodTypeEnumStringKey(LocalizedString stateString)
        {
            string result = "";

            switch (stateString)
            {
                case "1":
                    result = "0+";
                    break;
                case "2":
                    result = "0-";
                    break;
                case "3":
                    result = "AB+";
                    break;
                case "4":
                    result = "AB-";
                    break;
                case "5":
                    result = "A+";
                    break;
                case "6":
                    result = "A-";
                    break;
                case "7":
                    result = "B+";
                    break;
                case "8":
                    result = "B-";
                    break;
            }

            return result;
        }
    }
}
