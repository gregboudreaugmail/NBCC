namespace NBCC.Instructors.Commands.Assignments;

public sealed record AddAssignmentCommand(int InstructorId, int CourseId);
