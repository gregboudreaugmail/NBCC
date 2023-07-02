namespace NBCC.Instructors.Models;

public sealed class Assignment
{
    public Assignment(){ }
    public Assignment(int assignmentId, Course course, Instructor instructor)
    {
        AssignmentId = assignmentId;
        Course = course;
        Instructor = instructor;
    }

    public int AssignmentId { get; init; }
    public Course Course { get; set; } = new();
    public Instructor Instructor { get; set; } = new();

}
