using NBCC.Authorizaion.Commands;
using NBCC.Authorizaion.DataAccess;
using NBCC.Courses.Commands;

namespace NBCC.Authorizaion.CommandHandlers
{
    public sealed class UserCommandHandler : ICommandHandler<UserCommand>
    {
        IUserRepository AuthenticationRepository { get; }
        public UserCommandHandler(IUserRepository authenticationRepository) => AuthenticationRepository = authenticationRepository;

        public async Task Handle(UserCommand command) => await AuthenticationRepository.Create(command.UserName, command.Password, command.Email);
    }
}
