namespace NBCC.Courses.Commands;

public sealed class CoursesCommand
{
    internal string CourseName { get; }
    public CoursesCommand(string courseName) => CourseName = courseName;
}
