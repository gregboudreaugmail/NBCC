namespace NBCC.Courses.DataAccess;

public interface ICourseRepository
{
    Task<int> Make(string courseName);
    Task Archive(int courseId);
}
