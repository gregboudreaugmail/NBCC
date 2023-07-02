using NBCC.Instructors.Models;

namespace NBCC.Instructors.DataAccess;

public interface IInstructorRepository
{
    Task<int> Add(string firstName, string lastName, string email);
    Task Archive(int instructorId);
    Task<IEnumerable<Instructor>> Get();
    Task<IEnumerable<Instructor>> GetByCourse(int courseId);
}