namespace NBCC.Instructors.Models;

public sealed class Course
{
    public Course() { }
    public Course(int courseId, string courseName)
    {
        CourseId = courseId;
        CourseName = courseName;
    }

    public int CourseId { get; init; }
    public string CourseName { get; init; } = string.Empty;
}

