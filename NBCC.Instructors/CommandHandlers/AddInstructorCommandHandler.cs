using NBCC.Instructors.Commands;
using NBCC.Instructors.DataAccess;

namespace NBCC.Instructors.CommandHandlers;

public sealed class AddInstructorCommandHandler : ICommandHandler<AddInstructorCommand, int>
{
    IInstructorRepository InstructorRepository { get; }
    public AddInstructorCommandHandler(IInstructorRepository instructorRepository) => InstructorRepository = instructorRepository;

    public async Task<int> Handle(AddInstructorCommand command) => await InstructorRepository.Add(command.FirstName, command.LastName, command.Email);
}