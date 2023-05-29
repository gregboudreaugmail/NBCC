using NBCC.Instructors.Models;
using NBCC.Instructors.Properties;

namespace NBCC.Instructors.DataAccess;

public sealed class InstructorRepository : IInstructorRepository
{
    Connection Connection { get; }
    public InstructorRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task<int> Add(string firstName, string lastName, string email)
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QuerySingleAsync<int>(SqlScript.INSERT_Instructors, new { firstName, lastName, email });
    }

    public async Task Archive(int instructorId)
    {
        await using SqlConnection connection = new(Connection.Value);
        await connection.ExecuteAsync(SqlScript.ARCHIVE_Instructors, new { instructorId });
    }

    public async Task<IEnumerable<Instructor>> Get()
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QueryAsync<Instructor>(SqlScript.SELECT_Instructors);
    }
}
