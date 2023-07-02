namespace NBCC.Logging.Models;

public sealed record Interaction(string AssemblyName, string Command, string Parameters)
{
    public static implicit operator string(Interaction interaction) => JsonSerializer.Serialize(interaction);
}