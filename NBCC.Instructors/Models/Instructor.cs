namespace NBCC.Instructors.Models;

public sealed class Instructor
{
    public Instructor() { }
    public Instructor(int instructorId, string firstName, string lastName, string email)
    {
        InstructorId = instructorId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public int InstructorId { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
}