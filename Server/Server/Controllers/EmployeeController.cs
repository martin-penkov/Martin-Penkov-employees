using Microsoft.AspNetCore.Mvc;
using Server.Requests;
using Server.Services.CsvReader;
using Server.Types;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {

        private readonly ILogger<EmployeeController> _logger;

        private readonly ICsvReader _csvReader;

        public EmployeeController(ILogger<EmployeeController> logger, ICsvReader csvReader)
        {
            _logger = logger;
            _csvReader = csvReader;
        }

        [HttpPost("processCsvDataSingleOutput")]
        public async Task<IActionResult> ProcessCsvDataSingleOutput(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            List<Employee> projects;
            using (var stream = file.OpenReadStream())
            {
                projects = await _csvReader.ReadEmployeeProjects(stream);
            }

            List<EmployeePair> employeePairs = GetAllEmployeePairs(projects);

            return Ok(employeePairs.MaxBy(employeePair => employeePair.DaysWorkedTogether));
        }

        [HttpPost("processCsvDataAllEmployeePairs")]
        public async Task<IActionResult> ProcessCsvDataAllEmployeePairs(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            List<Employee> projects;
            using (var stream = file.OpenReadStream())
            {
                projects = await _csvReader.ReadEmployeeProjects(stream);
            }

            List<EmployeePair> employeePairs = GetAllEmployeePairs(projects);

            return Ok(employeePairs);
        }

        private List<EmployeePair> GetAllEmployeePairs(List<Employee> projects)
        {
            List<EmployeePair> employeePairs = new List<EmployeePair>();

            projects.GroupBy(employeeProject => employeeProject.ProjectId).ToList().ForEach(projectWithAllEmployees => {
                List<Employee> employeesInProject = [.. projectWithAllEmployees];

                foreach (Employee? lookupEmployee in employeesInProject)
                {
                    foreach (Employee? otherEmployee in employeesInProject)
                    {
                        int emp1 = lookupEmployee.Id;
                        int emp2 = otherEmployee.Id;
                        bool pairExists = employeePairs.Any(p => (p.FirstEmpId == emp1 && p.SecondEmpId == emp2) || (p.FirstEmpId == emp2 && p.SecondEmpId == emp1));

                        if (lookupEmployee.Id != otherEmployee.Id && !pairExists)
                        {
                            employeePairs.Add(new EmployeePair
                            {
                                FirstEmpId = lookupEmployee.Id,
                                SecondEmpId = otherEmployee.Id,
                                ProjectId = lookupEmployee.ProjectId,
                                DaysWorkedTogether = GetOverlapDays(lookupEmployee.DateFrom, lookupEmployee.DateTo, otherEmployee.DateFrom, otherEmployee.DateTo)
                            });
                        }
                    }
                }
            });

            return employeePairs;
        }

        private int GetOverlapDays(DateTime emp1From, DateTime emp1To, DateTime emp2From, DateTime emp2To)
        {
            DateTime start = emp1From > emp2From ? emp1From : emp2From;

            DateTime end = emp1To < emp2To ? emp1To : emp2To;

            TimeSpan overlap = end - start;

            return overlap.TotalDays >= 0 ? (int)overlap.TotalDays + 1 : 0;
        }
    }
}
