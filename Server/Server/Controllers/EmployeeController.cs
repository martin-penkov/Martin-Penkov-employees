using Microsoft.AspNetCore.Mvc;
using Server.Services.CsvReader;
using Server.Types;
using System.Formats.Asn1;

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

        [HttpGet(Name = "GetEmployeesData")]
        public IEnumerable<Employee> Get()
        {
            return [];
        }


        [HttpPost(Name = "ProcessCsvData")]
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            List<Employee> projects;
            using (var stream = file.OpenReadStream())
            {
                projects = await _csvReader.ReadEmployeeProjects(stream);
            }

            return Ok(projects);
        }
    }
}
