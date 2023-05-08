﻿using NBCC.Courses.Properties;

namespace NBCC.Courses.DataAccess;

public sealed class CourseRepository : ICourseRepository
{
    Connection Connection { get; }
    public CourseRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));

    public async Task Create(string courseName)
    {
        await using SqlConnection connection = new(Connection.Value);
        await connection.ExecuteAsync(SqlScript.INSERT_Courses, new { courseName });
    }
}
