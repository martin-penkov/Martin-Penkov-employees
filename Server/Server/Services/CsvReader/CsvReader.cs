using Server.Types;
using System.Globalization;
using System.IO;

namespace Server.Services.CsvReader
{
    public class CsvReader : ICsvReader
    {
        public async Task<List<Employee>> ReadEmployeeProjects(Stream stream)
        {
            List<Employee> employeeProjects = new List<Employee>();

            using var reader = new StreamReader(stream);
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                string[] values = line.Split(',');

                Employee employee = new Employee
                {
                    Id = int.Parse(values[0].Trim()),
                    ProjectId = int.Parse(values[1].Trim()),
                    DateFrom = DateTime.ParseExact(values[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    DateTo = ParseNullableDate(values[3].Trim())
                };

                employeeProjects.Add(employee);
            }

            return employeeProjects;
        }

        private DateTime ParseNullableDate(string value)
        {
            if (string.Equals(value, "NULL", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.UtcNow;
            }

            return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }
}
