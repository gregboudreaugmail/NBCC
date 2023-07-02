using NBCC.Instructors.Models;

namespace NBCC.Instructors.DataAccess.Assignments;

public interface IAssignmentRepository
{
    Task<int> Add(int instructorId, int courseId);
    Task Archive(int instructorId, int courseId);
    Task<IEnumerable<Assignment>> Get(int instructorId);
}