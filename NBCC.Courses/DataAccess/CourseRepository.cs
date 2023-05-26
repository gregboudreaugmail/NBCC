using NBCC.Courses.Models;
using NBCC.Courses.Properties;

namespace NBCC.Courses.DataAccess;

public sealed class CourseRepository : ICourseRepository
{
    Connection Connection { get; }
    public CourseRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task<int> Make(string courseName)
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QuerySingleAsync<int>(SqlScript.INSERT_Courses, new { courseName });
    }

    public async Task Archive(int courseId)
    {
        await using SqlConnection connection = new(Connection.Value);
        await connection.ExecuteAsync(SqlScript.ARCHIVE_Courses, new { courseId });
    }

    public async Task<IEnumerable<Course>> GetCourses()
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QueryAsync<Course>(SqlScript.SELECT_Courses);
    }
}
