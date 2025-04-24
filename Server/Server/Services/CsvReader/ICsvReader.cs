using Server.Types;

namespace Server.Services.CsvReader
{
    public interface ICsvReader
    {
        Task<List<Employee>> ReadEmployeeProjects(Stream stream);
    }
}
