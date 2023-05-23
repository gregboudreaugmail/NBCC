namespace NBCC.Courses.Commands;

public sealed record MakeCoursesCommand(string CourseName, int? InstructorId);