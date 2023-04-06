namespace NBCC.Courses.DataAccess;

public interface ICourseRepository
{
    Task Create(string courseName);
}
