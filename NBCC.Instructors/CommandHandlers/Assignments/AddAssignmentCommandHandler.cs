using NBCC.Instructors.Commands.Assignments;
using NBCC.Instructors.DataAccess.Assignments;

namespace NBCC.Instructors.CommandHandlers.Assignments;

public class AddAssignmentCommandHandler : ICommandHandler<AddAssignmentCommand, int>
{
    IAssignmentRepository AssignmentRepository { get; }
    public AddAssignmentCommandHandler(IAssignmentRepository assignmentRepository) => AssignmentRepository = assignmentRepository;

    public async Task<int> Handle(AddAssignmentCommand command) => await AssignmentRepository.Add(command.InstructorId, command.CourseId);
}

