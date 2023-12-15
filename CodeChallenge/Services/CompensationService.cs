using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using CodeChallenge.DTOs;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeService employeeService)
        {
            _compensationRepository = compensationRepository;
            _employeeService = employeeService;
            _logger = logger;
        }

        public Compensation Create(CompensationAddDTO compensationAddDTO)
        {
            /*
                    NOTE: This project could use some more robust error handling, but for timesake and to follow
                    suit with the rest of the project I'm going to keep it simple and mention it instead.

                    The big problem we currently have is that there is no way to indicate the specific error to
                    whoever called this endpoint. Everything is just returning null/NotFound at the moment, but
                    we should really be returning descriptive messages along with varrying response codes for
                    more specific cases.
                */
            if (compensationAddDTO != null)
            {
                Employee Employee = _employeeService.GetById(compensationAddDTO.EmployeeId);
                if (Employee == null) return null;

                Compensation compensation = new Compensation()
                {
                    Employee = Employee,
                    Salary = compensationAddDTO.Salary,
                    EffectiveDate = compensationAddDTO.EffectiveDate
                };
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
                return compensation;
            }

            return null;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetByEmployeeId(employeeId);
            }

            return null;
        }

        public Compensation GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetById(id);
            }

            return null;
        }
    }
}
