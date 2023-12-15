using Microsoft.Extensions.Logging;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using System;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public ReportingStructureService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /*
            Determine the number of employees that report to the given employee both directly and indirectly.
            i.e. Employees who don't report directly to the given employee but do report to employees who
            report directly to the given employee should count towards this metric.
        */
        public ReportingStructure GetReportingStructureByEmployeeId(string employeeId)
        {
            if (String.IsNullOrEmpty(employeeId)) return null;
            
            Employee employee = _employeeRepository.GetById(employeeId);
            if (employee == null) return null;

            return new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = GetNumberOfReports(employee)
            };
        }

        // Recursively determine the number of employees that report to the given employee.
        private int GetNumberOfReports(Employee employee)
        {
            int numberOfReports = 0;

            if (employee.DirectReports != null)
            {
                foreach (Employee directReport in employee.DirectReports)
                {
                    Employee nextEmployee = _employeeRepository.GetById(directReport.EmployeeId);
                    numberOfReports += 1 + GetNumberOfReports(nextEmployee);
                }
            }

            return numberOfReports;
        }
    }
}
