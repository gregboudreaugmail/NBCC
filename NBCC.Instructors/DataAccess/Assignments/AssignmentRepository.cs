using NBCC.Instructors.DataAccess.Assignments;
using NBCC.Instructors.Models;
using NBCC.Instructors.Properties;

namespace NBCC.Instructors.DataAccess;

public sealed class AssignmentRepository : IAssignmentRepository
{
    Connection Connection { get; }
    public AssignmentRepository(Connection connection) =>
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));


    public async Task Archive(int instructorId, int courseId)
    {
        await using SqlConnection connection = new(Connection.Value);
        await connection.ExecuteAsync(SqlScript.ARCHIVE_Assignments, new { instructorId, courseId });
    }

    public async Task<IEnumerable<Assignment>> Get(int instructorId)
    {
        var assignments = new Dictionary<int, Assignment>();
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QueryAsync<Assignment, Course, Instructor, Assignment>(SqlScript.SELECT_Assignments, 
            map: (assignment, course, instructor) =>
            {
                if (!assignments.TryGetValue(assignment.AssignmentId, out var u))
                {
                    u = assignment;
                    u.Course = course;
                    u.Instructor = instructor;
                    assignments.Add(u.AssignmentId,u);
                }

                u.Course = course;
                u.Instructor = instructor;
                return u;
            }, splitOn: "CourseId,InstructorId", param: new{ instructorId });
    }

    public async Task<int> Add(int instructorId, int courseId)
    {
        await using SqlConnection connection = new(Connection.Value);
        return await connection.QuerySingleAsync<int>(SqlScript.INSERT_Assignments, new { instructorId, courseId });
    }
}
