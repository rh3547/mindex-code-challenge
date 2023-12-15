using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _dbContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _dbContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _dbContext.Compensations.Add(compensation);
            _logger.LogDebug($"Added compensation for employee '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");
            return compensation;
        }

        public Compensation GetById(string id)
        {
            _logger.LogDebug($"Retrieving compensation by id '{id}'");
            return _dbContext.Compensations
                .SingleOrDefault(c => c.CompensationId == id);
        }

        /*
            I created a separate repository function for this to leave the base "GetById" intact
            assuming we ever want to get a compensation by id in the future.

            Ideally we would have system that allows us to add includes and filters to queries
            without writing new repository functions in simple cases like this.
        */
        public Compensation GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Retrieving compensation by employee id '{employeeId}'");
            return _dbContext.Compensations
                .Include(c => c.Employee)
                .SingleOrDefault(c => c.Employee.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
