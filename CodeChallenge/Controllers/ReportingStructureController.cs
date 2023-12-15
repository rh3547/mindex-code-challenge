using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

/*
    I debated whether to add the GetReportingStructureByEmployeeId endpoint to the employee controller/service
    or create new ones for it.
    
    On one hand it is only a single endpoint that directly deals with employees, and the underlying
    ReportingStructure doesn't need to be persisted. So it could be argued that it should be in the employee
    controller/service.

    I decided to create a new controller/service for the sake of compartmentalization and future proofing.
    Should we want to add more reporting structure related endpoints, or add a repository to persist them
    down the road, the separation will make it easier to do so.
*/
namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reporting-structure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{employeeId}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetReportingStructureByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received reporting structure get request for employee '{employeeId}'");

            ReportingStructure report = _reportingStructureService.GetReportingStructureByEmployeeId(employeeId);
            if (report == null) return NotFound();

            return Ok(report);
        }
    }
}
