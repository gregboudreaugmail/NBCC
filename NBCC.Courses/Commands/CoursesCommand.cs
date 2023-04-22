namespace NBCC.Courses.Commands;

public sealed record CoursesCommand
{
    internal string CourseName { get; }
    public CoursesCommand(string courseName) => CourseName = courseName;
}
