namespace NBCC.Courses.DataAccess;

public interface ICourseRepository
{
    Task Make(string courseName);
    Task Archive(int courseId);
}
