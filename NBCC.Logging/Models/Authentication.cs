using System.Text.Json;

namespace NBCC.Logging.Models;

public sealed class Authentication
{
    public int UserId { get; }

    public Authentication() { }

    public Authentication(int userId) => UserId = userId;

    public static implicit operator string(Authentication authentication) => JsonSerializer.Serialize(authentication);
}