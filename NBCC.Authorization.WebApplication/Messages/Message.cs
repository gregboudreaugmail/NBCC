using NBCC.WebApplication.Messages;

namespace NBCC.Authorization.WebApplication.Messages
{
    public sealed record Message : IErrorMessage
    {
        public string PersistanceError { get; init; } = string.Empty;
        public string GeneralError { get; init; } = string.Empty;
    }
}
