using NBCC.Instructors.Commands.Assignments;
using NBCC.Instructors.DataAccess.Assignments;

namespace NBCC.Instructors.CommandHandlers.Assignments;

public class ArchiveAssignmentsCommandHandler : ICommandHandler<ArchiveAssignmentsCommand>
{
    IAssignmentRepository AssignmentRepository { get; }
    public ArchiveAssignmentsCommandHandler(IAssignmentRepository assignmentRepository) => AssignmentRepository = assignmentRepository;

    public async Task Handle(ArchiveAssignmentsCommand command) => await AssignmentRepository.Archive(command.InstructorId, command.CourseId);
}

