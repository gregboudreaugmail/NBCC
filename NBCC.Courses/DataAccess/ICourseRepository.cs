using NBCC.Courses.Models;

namespace NBCC.Courses.DataAccess;

public interface ICourseRepository
{
    Task<int> Make(string courseName);
    Task Archive(int courseId);
    Task<IEnumerable<Course>> GetCourses();
}
