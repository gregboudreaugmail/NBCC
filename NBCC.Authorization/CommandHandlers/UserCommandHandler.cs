using NBCC.Authorization.Commands;
using NBCC.Authorization.DataAccess;

namespace NBCC.Authorization.CommandHandlers;

public sealed class UserCommandHandler : ICommandHandler<UserCommand>
{
    IUserRepository AuthenticationRepository { get; }
    public UserCommandHandler(IUserRepository authenticationRepository) => AuthenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
    public async Task Handle(UserCommand command) => await AuthenticationRepository.Create(command.UserName, command.Password, command.Email);
}

