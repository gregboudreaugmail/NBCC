using NBCC.Courses.Models;
using NBCC.Courses.Properties;

namespace NBCC.Courses.DataAccess;

public sealed class CourseRepository : ICourseRepository
{
    Connection Connection { get; }
    public CourseRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
    /*
     * Note 20
     * Data access: Dapper as a 3rd party
     * As we saw, the make course controller called the make course command, was handled
     * by the make course handler and finally we're going to access the database with the
     * 'Make' function.  The idea of this 3rd party to access SQL is pretty simple.
     * It accepts a connection string and will either execute the SQL code provided or produces
     * a return value.  During the 'return' process, you specify the return type and dapper will
     * attempt to transform your data into the type provided.  This saves you the tedious task
     * of doing "Name = return.Name" and so on.
     */
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

    public async Task<IEnumerable<Course>> Get()
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QueryAsync<Course>(SqlScript.SELECT_Courses);
    }
}
