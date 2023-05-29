using NBCC.Instructors.Commands;
using NBCC.Instructors.DataAccess;

namespace NBCC.Instructors.CommandHandlers;

public sealed class ArchiveInstructorCommandHandler : ICommandHandler<ArchiveInstructorCommand>
{
    IInstructorRepository InstructorRepository { get; }

    public ArchiveInstructorCommandHandler(IInstructorRepository instructorRepository) => InstructorRepository = instructorRepository;

    public async Task Handle(ArchiveInstructorCommand command) => await InstructorRepository.Archive(command.InstructorId);
}

